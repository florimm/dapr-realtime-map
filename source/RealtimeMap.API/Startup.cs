using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealtimeMap.API.MQQT;
using Microsoft.OpenApi.Models;
using RealtimeMap.API.Actors;

namespace RealtimeMap.API
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
            services
                .AddMvc()
                .AddDapr(build => build.UseHttpEndpoint("http://localhost:3500").UseGrpcEndpoint("http://localhost:60001"));
            services.AddControllers();
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "RealTimeMap.Service.Api", Version = "v1" });
            // });
            services.AddActors(options => {
                options.Actors.RegisterActor<VehicleActor>();
                options.Actors.RegisterActor<SignalRActor>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RealTimeMap.Service.Api v1"));
            }

            // app.UseCors(builder => builder
            //     .WithOrigins("http://localhost:8080")
            //     .AllowAnyMethod()
            //     .AllowAnyHeader()
            //     .AllowCredentials()
            // );

            app.UseRouting();

            // app.UseAuthorization();

            app.UseCloudEvents();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapActorsHandlers();
                endpoints.MapSubscribeHandler();
                endpoints.MapControllers();
            });
        }
    }
}
