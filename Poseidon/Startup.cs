using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poseidon.Configuration;
using Poseidon.Repositories;
using Poseidon.Models;
using Poseidon.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Poseidon.Hubs;
using Poseidon.Helpers;

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
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));
            services.Configure<IssuerSigningKeySettings>(Configuration.GetSection("IssuerSigningKey"));
            services.AddScoped<MongoDbContext>();
            
            services.AddScoped<IAlarmsRepository<Alarm>, MongoDbAlarmsRepository>();
            services.AddScoped<IMeasuresRepository<Measure>, MongoDbMeasuresRepository>();
            services.AddScoped<IPoolsRepository<Pool>, MongoDbPoolsRespository>();
            services.AddScoped<IUsersRepository<User>, MongoDbUsersRepository>();
            services.AddScoped<IPoolConfigurationsRepository<PoolConfiguration>, MongoDbPoolConfiguartionsRespository>();

            services.AddScoped<UserPermissionService>();

            services.AddSingleton<IConnectionMapper<string>, ConnectionMapping<string>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateActor = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("IssuerSigningKey")["SigningKey"]))
                };
            });

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
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });

            services.AddSignalR();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Poseidon V1");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<AlarmsHub>("alarms");
            });
            
            app.UseMvc();
        }
    }
}
