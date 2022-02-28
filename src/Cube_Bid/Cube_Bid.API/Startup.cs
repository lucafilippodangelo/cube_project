using Cube_Bid.API.Data;
using Cube_Bid.API.Data.Interfaces;
using Cube_Bid.API.Extentions;
using Cube_Bid.API.RabbitMq;
using Cube_Bid.API.Repositories;
using Cube_Bid.API.Repositories.Interfaces;
using Cube_Bid.API.Settings;
using EventBusRabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace Cube_Bid.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();


            #region Mongo Dependencies

            services.Configure<BidMongoDatabaseSettings>(Configuration.GetSection(nameof(BidMongoDatabaseSettings)));

            services.AddSingleton<IBidMongoDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BidMongoDatabaseSettings>>().Value);

            #endregion

            #region Project Dependencies

            services.AddTransient<IBidValidator, BidValidator>();

            services.AddTransient<IAuctionContext, AuctionContext>();
            services.AddTransient<IAuctionReposirory, AuctionReposirory>();

            services.AddTransient<IBidContextRedis, BidContextRedis>();
            services.AddTransient<IBidRepositoryRedis, BidReposiroryRedis>();

            services.AddTransient<IBidContextMongo, BidContextMongo>();
            services.AddTransient<IBidRepositoryMongo, BidRepositoryMongo>();

            #endregion

            #region Redis Dependencies

            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            #endregion

            #region RabbitMQ Dependencies

            services.AddSingleton<IRabbitMQConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBus:HostName"]
                };

                if (!string.IsNullOrEmpty(Configuration["EventBus:UserName"]))
                {
                    factory.UserName = Configuration["EventBus:UserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBus:Password"]))
                {
                    factory.Password = Configuration["EventBus:Password"];
                }

                return new RabbitMQConnection(factory);
            });

            //LD Note, this will not work if repositories are not declared before "AutoMaper"
            // in this "ConfigureServices" method
            services.AddSingleton<EventBusRabbitMQConsumer>();

            #endregion

            #region Swagger Dependencies

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bid API", Version = "v1" });
            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cube_Bid.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Initilize Rabbit Listener in ApplicationBuilderExtentions
            app.UseRabbitListener();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
