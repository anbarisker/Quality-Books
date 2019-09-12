using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QualityBooks.Data;
using QualityBooks.Models;
using QualityBooks.Services;
using Microsoft.AspNetCore.Identity;

namespace QualityBooks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<QualityBooksContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            //Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<QualityBooksContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.CookieHttpOnly = true;
            });

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            await CreateRoles(serviceProvider);
        }


        public async Task CreateRoles(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //create database schema if none exists
                var apContext = serviceScope.ServiceProvider.GetService<QualityBooksContext>();
                apContext.Database.EnsureCreated();

                //If there is already an Administrator role, abort
                var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                //var _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                string[] roleNames = { "Admin", "Member" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {

                    bool roleExist = _roleManager.RoleExistsAsync(roleName).Result;
                    if (!roleExist)
                    {
                        roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }

                }

                var poweruser = new ApplicationUser
                {
                    UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                    Email = Configuration.GetSection("UserSettings")["UserEmail"],
                    Address = "Addmin Address",
                    Enabled = true
                };
                var _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var test = _userManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);
                if (test.Result == null)
                {
                    string UserPassword = Configuration.GetSection("UserSettings")["UserPassword"];
                    poweruser.EmailConfirmed = true;
                    var createPowerUser = await _userManager.CreateAsync(poweruser, UserPassword);
                    if (createPowerUser.Succeeded)
                    {
                        //here we tie the new user to the "Admin" role 
                        await _userManager.AddToRoleAsync(poweruser, "Admin");
                    }
                }

                //string path = Environment.CurrentDirectory + @"/wwwroot/images/Temp/defaultBook.jpg";
                //byte[] image = System.IO.File.ReadAllBytes(path);

               
                var normalUser = new ApplicationUser
                {
                    UserName = Configuration.GetSection("CustomerSettings")["UserEmail"],
                    Email = Configuration.GetSection("CustomerSettings")["UserEmail"],
                    Address = "Customer Address",
                    Enabled = true,
                   // BirthDate = DateTime.Parse("24/08/1992"),
                    City = "Singapore",
                    Country = "Singapore",
                    Created_At = DateTime.Now,
                    Gender = "Male",
                    LastName = "Rasan",
                    EmailConfirmed = true,
                    MobileNo = 2115425344,
                    HomeNo = 34424234,
                    PostalCode = 4232344,
                    Updated_At = DateTime.Now
                    //Photo = image

                };
                var _userManagerCustomer = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var customer_set = _userManager.FindByEmailAsync(Configuration.GetSection("CustomerSettings")["UserEmail"]);
                if (customer_set.Result == null)
                {
                    string UserPassword = Configuration.GetSection("CustomerSettings")["UserPassword"];
                    normalUser.EmailConfirmed = true;
                    var createPowerUser = await _userManager.CreateAsync(normalUser, UserPassword);
                    if (createPowerUser.Succeeded)
                    {
                        //here we tie the new user to the "Admin" role 
                        await _userManager.AddToRoleAsync(normalUser, "Member");
                    }
                }

            }


        }
    }
}
