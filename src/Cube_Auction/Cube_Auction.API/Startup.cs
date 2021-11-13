using Cube_Auction.Core.Repositories;
using Cube_Auction.Core.Repositories.Base;
using Cube_Auction.Infrastructure.Data;
using Cube_Auction.Infrastructure.Repository;
using Cube_Auction.Infrastructure.Repository.Base;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Cube_Auction.API
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

            #region SqlServer Dependencies

            //// use in-memory database
            //services.AddDbContext<OrderContext>(c =>
            //    c.UseInMemoryDatabase("OrderConnection"));

            // use real database
            services.AddDbContext<AuctionContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("OrderConnection")), ServiceLifetime.Singleton); 

            #endregion

            #region Project Dependencies

            // Add Infrastructure Layer
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IAuctionRepository), typeof(AuctionRepository));
            //services.AddScoped(typeof(IAuctionRepository), typeof(AuctionRepository));
      
            // Add AutoMapper
            services.AddAutoMapper(typeof(Startup));

            //Add MediatR
            //services.AddMediatR(typeof(CheckoutOrderHandler).GetTypeInfo().Assembly);

            //Domain Level Validation
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

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

            services.AddSingleton<EventBusRabbitMQProducer>();

            //LD Note, this will not work if repositories are not declared before "AutoMaper"
            // in this "ConfigureServices" method
            //services.AddSingleton<EventBusRabbitMQConsumer>();

            #endregion

            #region Swagger Dependencies

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });
            });
            
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
            });

        }
    }
}
