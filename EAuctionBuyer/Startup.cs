using EAuctionBuyer.Data;
using EAuctionBuyer.Extensions;
using EAuctionBuyer.Interface;
using EAuctionBuyer.MessageBroker;
using EAuctionBuyer.Model;
using EAuctionBuyer.Repository;
using EAuctionSeller.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;

namespace EAuctionBuyer
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
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EAuctionSystem", Version = "v1" });
            });
            services.AddControllers();

            var serviceClientSettingsConfig = Configuration.GetSection("AzureServiceBus");
            services.Configure<AzureServiceBusConfiguration>(serviceClientSettingsConfig);
            bool.TryParse(Configuration["BaseServiceSettings:UserabbitMq"], out var useRabbitMq);

            if (useRabbitMq)
            {
                services.AddScoped<IRabbitMqListener, RabbitMqListener>();
                services.AddSingleton(service =>
                {
                    var _config = Configuration.GetSection("RabbitMQ");
                    return new ConnectionFactory()
                    {
                        HostName = _config["HostName"],
                        UserName = _config["UserName"],
                        Password = _config["Password"],
                        Port = Convert.ToInt32(_config["Port"]),
                        VirtualHost = _config["VirtualHost"],
                    };
                });
            }
            else
            {
                services.AddScoped<IBidUpdateSender, BidUpdateSenderServiceBus>();
            }

            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoDB"));
            services.AddSingleton<AuctionBuyerDBContext>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProductToBuyerRepository, ProductToBuyerRepository>();
            services.AddTransient<IProduct, BuyerProductService>();
            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EAuctionSystem V1");
                c.RoutePrefix = "swagger";
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
