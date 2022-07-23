using Cube_Bid.API.Entities;
using Cube_Bid.API.Repositories;
using Cube_Bid.API.Repositories.Interfaces;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cube_Bid.API.RabbitMq
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IAuctionsHistoryRepositoryRedis _auctionsHistoryRepositoryRedis;
        private readonly IBidRepositoryMongo _bidRepositoryMongo;
        private readonly IBidValidator _bidValidator;
        private readonly EventBusRabbitMQProducer _eventBus;

        public EventBusRabbitMQConsumer(IRabbitMQConnection connection, 
                                        IBidRepositoryRedis bidRepositoryRedis, 
                                        IBidRepositoryMongo bidRepositoryMongo, 
                                        IBidValidator bidValidator, 
                                        IAuctionsHistoryRepositoryRedis auctionsHistoryRepositoryRedis,
                                        EventBusRabbitMQProducer eventBus)//, IMediator mediator, IMapper mapper, IOrderRepository repository)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _auctionsHistoryRepositoryRedis = auctionsHistoryRepositoryRedis ?? throw new ArgumentNullException(nameof(_auctionsHistoryRepositoryRedis));
            _bidRepositoryMongo = bidRepositoryMongo ?? throw new ArgumentNullException(nameof(_bidRepositoryMongo));
            _bidValidator = bidValidator ?? throw new ArgumentNullException(nameof(_bidValidator));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }


        public void Consume()
        {

            //LD consuming the queue "AuctionEventQueue" ------------------------------------
            var channelTwo = _connection.CreateModel();
            channelTwo.QueueDeclare(queue: EventBusConstants.QUEUE_AuctionEvent, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumerTwo = new EventingBasicConsumer(channelTwo);
            //Create event when something receive
            consumerTwo.Received += ReceivedEvent;
            channelTwo.BasicConsume(queue: EventBusConstants.QUEUE_AuctionEvent, autoAck: true, consumer: consumerTwo);

            //LD consuming the queue "BidCreationQueue" ------------------------------------
            var channelThree = _connection.CreateModel();
            channelThree.QueueDeclare(queue: EventBusConstants.QUEUE_BidCreation, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumerThree = new EventingBasicConsumer(channelThree);
            //Create event when something receive
            consumerThree.Received += ReceivedEvent;
            channelThree.BasicConsume(queue: EventBusConstants.QUEUE_BidCreation, autoAck: true, consumer: consumerThree);


        }

        //ORDERS APPLICATION
        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
        //LD STORE AUCTION EVENT IN REDIS    
            if (e.RoutingKey == EventBusConstants.QUEUE_AuctionEvent)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var Event = JsonConvert.DeserializeObject<AuctionEvent>(message);

                Debug.WriteLine("REDIS STORE ->" + message);
                //LD NOTE -> AUCTION and AUCTION EVENTS are stored with same hardcoded time in both DB 
                _auctionsHistoryRepositoryRedis.InsertAuctionEvent(Event.Id.ToString()+ " " + Event.EventCode.ToString(),  
                                                                   Event.EventDateTime.ToString() + " " + Event.EventDateTimeMilliseconds.ToString());
            }


        //LD STORE BID in mongo and ASYNC VALIDATION  
            if (e.RoutingKey == EventBusConstants.QUEUE_BidCreation)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var Event = JsonConvert.DeserializeObject<BidCreationEvent>(message);
                //TEMP FOR DEBUG
                Debug.WriteLine("CONSUME QUEUE_BidCreation ->" + message);
                //TEMP FOR DEBUG (END)


                //LD STEP ONE -> store bid in mongo
                Bid aBid = new Bid();
                try
                {
                    aBid.BidName = ("n." + Event.IncrementalId + " - Created at " + DateTime.UtcNow + " by thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
                    aBid.AuctionId = Event.AuctionId;
                    aBid.Amount = Event.Amount;
                    aBid.confirmed = 0; //LD by default is in "Pending status"
                    aBid.DateTime = DateTime.UtcNow;
                    aBid.DateTimeMilliseconds = aBid.DateTime.Millisecond;
                    await _bidRepositoryMongo.Create(aBid);
                    Debug.WriteLine("MONGO CREATE ->" + aBid.BidName);
                }
                catch (Exception ex)
                {
                    throw;
                }

                //LD STEP TWO -> validate bid already stored in mongo (parallel threads)
                var t = Task.Run(() => 
                {
                    var validationResponse = _bidValidator.ValidateInputBid(aBid);//validation by creation date. REDIS is used as source for auction data events comparison
                    aBid.confirmed = validationResponse;
                    aBid.BidName = aBid.BidName + (" - Updated at " + DateTime.UtcNow + " by thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
                    _bidRepositoryMongo.Update(aBid);
                    Debug.WriteLine("MONGO UPDATE ->" + aBid.BidName);


                    //LD STEP THREE: create finalization event and update queue
                    try
                    {
                        BidFinalizationEvent eventMessage = new BidFinalizationEvent();
                        eventMessage.BasicLog = aBid.BidName;
                        eventMessage.Status = aBid.confirmed;
                        Debug.WriteLine("PUSH in QUEUE: QUEUE_BidFinalization" + " - message" + aBid.BidName);
                        _eventBus.PublishBidStatusFinalization(EventBusConstants.QUEUE_BidFinalization, eventMessage); //need to create event object
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                });


            }

        }

        public void Disconnect()
        {
            _connection.Dispose();
        }

        public void writeFile()
        {
            string path = @"C:\Users\Luca\Desktop\MyTest.txt";

            try
            {
                // Create the file, or overwrite if the file exists.
                //using (FileStream fs = File.OpenWrite(path))
                using (var fs = new FileStream(path, FileMode.Append))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(" ORDERING API consumed an event ->" + DateTimeOffset.UtcNow);
                    // Add some information to the file.
                    
                    fs.Write(info, 0, info.Length);
                 
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
