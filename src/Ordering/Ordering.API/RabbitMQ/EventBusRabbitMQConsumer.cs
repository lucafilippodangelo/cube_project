using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands;
using Ordering.Core.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;

namespace Ordering.API.RabbitMQ
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository; // we added this in order to resolve in mediatR

        public EventBusRabbitMQConsumer(IRabbitMQConnection connection, IMediator mediator, IMapper mapper, IOrderRepository repository)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.SecondConsumerQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            //Create event when something receive
            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: EventBusConstants.SecondConsumerQueue, autoAck:true, consumer: consumer);
        }

        //ORDERS APPLICATION
        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EventBusConstants.SecondConsumerQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);

                // EXECUTION : Call Internal Checkout Operation
                var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);
                var result = await _mediator.Send(command); //for how mediator is implemented, this will call -> CheckoutOrderHandler, implementing the interface that specify in input the command "CheckoutOrderCommand" -> IRequestHandler<CheckoutOrderCommand, OrderResponse>

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
