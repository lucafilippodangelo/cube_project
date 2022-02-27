using Cube_Bid.API.Entities;
using Cube_Bid.API.Repositories.Interfaces;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;

namespace Cube_Bid.API.RabbitMq
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IBidRepositoryRedis _bidRepositoryRedis;
        private readonly IBidRepositoryMongo _bidRepositoryMongo;

        public EventBusRabbitMQConsumer(IRabbitMQConnection connection, IBidRepositoryRedis bidRepositoryRedis, IBidRepositoryMongo bidRepositoryMongo)//, IMediator mediator, IMapper mapper, IOrderRepository repository)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _bidRepositoryRedis = bidRepositoryRedis ?? throw new ArgumentNullException(nameof(_bidRepositoryRedis));
            _bidRepositoryMongo = bidRepositoryMongo ?? throw new ArgumentNullException(nameof(_bidRepositoryMongo));
        }


        public void Consume()
        {

            //LD consuming the queue "AuctionEventQueue" ------------------------------------
            var channelTwo = _connection.CreateModel();
            channelTwo.QueueDeclare(queue: EventBusConstants.AuctionEventQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumerTwo = new EventingBasicConsumer(channelTwo);
            //Create event when something receive
            consumerTwo.Received += ReceivedEvent;
            channelTwo.BasicConsume(queue: EventBusConstants.AuctionEventQueue, autoAck: true, consumer: consumerTwo);

            //LD consuming the queue "BidCreationQueue" ------------------------------------
            var channelThree = _connection.CreateModel();
            channelThree.QueueDeclare(queue: EventBusConstants.BidCreationQueue_Mongo, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumerThree = new EventingBasicConsumer(channelThree);
            //Create event when something receive
            consumerThree.Received += ReceivedEvent;
            channelThree.BasicConsume(queue: EventBusConstants.BidCreationQueue_Mongo, autoAck: true, consumer: consumerThree);


        }

        //ORDERS APPLICATION
        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EventBusConstants.AuctionEventQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var Event = JsonConvert.DeserializeObject<AuctionEvent>(message);

                //_bidRepositoryRedis.InsertBid(Event.AuctionName + "-"+ Event.Id, Event.AuctionSubscriberName + "-" + Event.Amount + "-" + Event.DateTime);
            }

            if (e.RoutingKey == EventBusConstants.BidCreationQueue_Mongo)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var Event = JsonConvert.DeserializeObject<BidCreationEvent>(message);

                Bid aBid = new Bid();
                aBid.BidName = "Event Id: "+Event.Id;
                aBid.AuctionName = Event.AuctionName;
                aBid.Amount = Event.Amount;
                aBid.DateTime = Event.DateTime;

                await _bidRepositoryMongo.Create(aBid);
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
