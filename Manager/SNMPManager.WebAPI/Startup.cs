using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Interfaces;
using SNMPManager.Persistence;
using SNMPManager.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Hosting;

namespace SNMPManager
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
            // Configure Entity Framework with use of postgreSQL
            services.AddEntityFrameworkNpgsql()
                    //.AddDbContext<ManagerContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("DB"), b => b.MigrationsAssembly("SNMPManager.WebAPI")))
                    .AddDbContext<ManagerContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ManagerDatabase"), b => b.MigrationsAssembly("SNMPManager.WebAPI")))
                    .BuildServiceProvider();

            // Inject Custom logger and Databse handler services
            services.AddScoped<Core.Interfaces.ILogger, ManagerLogger>();
            services.AddScoped<IContextService, ContextService>();
            services.AddScoped<ISNMPManagerService, SNMPManagerService>();

            // Registrate and configure the TrapListener
            services.AddSingleton<IHostedService>(sp => new TrapListener(sp,
                                                                         "rsu",
                                                                         "trapauthpass01",
                                                                         "trapprivpass01",
                                                                         Configuration.GetValue<string>("TrapListener:IP"),
                                                                         Configuration.GetValue<int>("TrapListener:Port")));

            // Register Health checker service to chek rsus active states
            services.AddSingleton<IHostedService>(sp => new RSUHealthChecker(sp));

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register Swagger for API documention
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SNMPManager API", Version = "v1" });

                c.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SNMPManager API");
            });

            app.UseHttpsRedirection();
            app.UseMvc();

            CreateDefaultUsers(service).Wait();
        }

        private async Task CreateDefaultUsers(IServiceProvider serviceProvider)
        {
            var managerContext = serviceProvider.GetRequiredService<ManagerContext>();

            //managerContext.Database.Migrate();

            var admin = await managerContext.Users.SingleOrDefaultAsync(u => u.UserName == "admin");
            if (admin == null)
            {
                managerContext.Add(new User
                {
                    Id = 0,
                    UserName = Configuration.GetValue<string>("DefaultAdmin:UserName"),
                    Token = Configuration.GetValue<string>("DefaultAdmin:Token"),
                    Role = managerContext.Roles.Single(r => r.Name == "Admin"),
                    SNMPv3Auth = Configuration.GetValue<string>("DefaultAdmin:SNMPv3Auth"),
                    SNMPv3Priv = Configuration.GetValue<string>("DefaultAdmin:SNMPv3Priv")
                });

                await managerContext.SaveChangesAsync();
            }
        }
    }
}
