using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KayakBackend.Configurations;
using KayakBackend.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace KayakBackend
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PersistenceConfiguration>(_configuration.GetSection(nameof(PersistenceConfiguration)));
            services.Configure<PredictorServiceConfiguration>(
                _configuration.GetSection(nameof(PredictorServiceConfiguration)));

            var persistenceConfig = _configuration.GetSection(nameof(PersistenceConfiguration))
                .Get<PersistenceConfiguration>();
            services.AddSingleton<IMongoClient>(new MongoClient(persistenceConfig.MongoClusterConnectionString));
            services.AddScoped<ITripRepository, TripRepository>();
            
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddMvc();
            
            services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();
            
           if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}