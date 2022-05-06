using Cube_BidsSignalR.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cube_BidsSignalR
{
    public static class ApplicationBuilderExtentions
    {
        public static Cube_BidsSignalR_RabbitMQConsumer Listener { get; set; }


        //LD "UseRabbitListener" called at startup 
        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<Cube_BidsSignalR_RabbitMQConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}
