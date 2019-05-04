using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DashboardWebApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DashboardWebApp.WebApiClients;
using DashboardWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DashboardWebApp
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DashboardDatabase")))
                .BuildServiceProvider();

            services.AddDefaultIdentity<User>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Web API services
            services.AddHttpClient<RSUService>();
            services.AddHttpClient<UserService>();
            services.AddHttpClient<ManagerLogService>();
            services.AddHttpClient<TrapLogService>();
            services.AddHttpClient<SnmpService>();

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/");
                    /*options.Conventions.AuthorizeFolder("/Managers");
                    options.Conventions.AuthorizeFolder("/RSUs");
                    options.Conventions.AuthorizeFolder("/ManagerLogs");
                    options.Conventions.AuthorizeFolder("/Users");
                    options.Conventions.AuthorizeFolder("/Components");
                    options.Conventions.AuthorizeFolder("/TrapLogs");*/
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();

            CreateDefaultUsers(service).Wait();
        }

        private async Task CreateDefaultUsers(IServiceProvider serviceProvider)
        {
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var adminuser = await UserManager.FindByNameAsync("Admin");
            if (adminuser == null)
            {
                User user = new User()
                {
                    UserName = "Admin",
                    Email = "sturm.peti@gmail.com"
                };
                var result = await UserManager.CreateAsync(user, "Adminadmin_01");
            }

            /*var appDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (appDbContext.UserManagerUsers.Count() == 0)
            {
                var managerUser = appDbContext.ManagerUsers.Find(1, "sturm");

                appDbContext.UserManagerUsers.Add(new UserManagerUser
                { 
                    UserId = adminuser.Id,
                    User = adminuser,
                    ManagerUserManagerId = managerUser.ManagerId,
                    ManagerUserName = managerUser.Name,
                    ManagerUser = managerUser

                });

                appDbContext.SaveChanges();
            }*/
        }
    }
}
