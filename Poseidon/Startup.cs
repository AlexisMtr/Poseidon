using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poseidon.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Poseidon.Repositories;
using Poseidon.Repositories.SQL;
using Poseidon.Services;
using Poseidon.Models;
using Microsoft.AspNetCore.Identity;

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
            services.Configure<IssuerSigningKeySettings>(Configuration.GetSection("IssuerSigningKey"));

            services.AddDbContext<PoseidonContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<PoseidonContext>();
            
            services.AddScoped<IAlarmRepository, AlarmRepository>();
            services.AddScoped<IPoolRepository, PoolRepository>();
            services.AddScoped<ITelemetryRepository, TelemetryRepository>();
            services.AddScoped<UserManager<User>>();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped<SignInManager<User>>();

            services.AddScoped<AlarmService>();
            services.AddScoped<PoolService>();
            services.AddScoped<TelemetryService>();
            services.AddScoped<UserService>();
            
            services.AddAuthentication(options => options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateActor = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
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

            services.AddCors();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();

            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Poseidon V1");
            });

            app.UseMvc();
        }
    }
}
