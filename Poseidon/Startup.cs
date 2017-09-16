using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poseidon.Configuration;
using Poseidon.Repositories;
using Poseidon.Models;
using Poseidon.Services;

namespace Poseidon
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            services.AddScoped<MongoDbContext>();

            services.AddScoped<IRepository<Alarm>, MongoDbAlarmsRepository>();
            services.AddScoped<IRepository<Measure>, MongoDbMeasuresRepository>();
            services.AddScoped<IRepository<Pool>, MongoDbPoolRespository>();
            services.AddScoped<IRepository<User>, MongoDbUsersRepository>();

            services.AddScoped<UserPermissionService>();

            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Poseidon",
                    Version = "1.0.0",
                    Description = "API for connected swimmingpool",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "AlexisMtr",
                        Url = "http://github.com/AlexisMtr"
                    }
                });
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Poseidon V1");
            });

            app.UseMvc();
        }
    }
}
