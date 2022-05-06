using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Cube_BidsSignalR.RabbitMQ
{
    public class Cube_BidsSignalR_RabbitMQConsumer
    {
        private readonly IRabbitMQConnection _connection;

        public Cube_BidsSignalR_RabbitMQConsumer(IRabbitMQConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public void Consume()
        {

            //LD consuming the queue "QUEUE_BidFinalization" ------------------------------------
            var channelTwo = _connection.CreateModel();
            channelTwo.QueueDeclare(queue: EventBusConstants.QUEUE_BidFinalization, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumerTwo = new EventingBasicConsumer(channelTwo);

            //Create event when something received
            consumerTwo.Received += ReceivedEvent;
            channelTwo.BasicConsume(queue: EventBusConstants.QUEUE_BidFinalization, autoAck: true, consumer: consumerTwo);

        }

        //ORDERS APPLICATION
        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EventBusConstants.QUEUE_BidFinalization)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var Event = JsonConvert.DeserializeObject<BidFinalizationEvent>(message);

                var luca = 1;
            }
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }

    }
}
