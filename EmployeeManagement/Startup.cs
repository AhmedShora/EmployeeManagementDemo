using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmployeeManagement
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            /*
             * to change password options
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 7;
            }).AddEntityFrameworkStores<AppDbContext>();*/

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            //I dont Know the use of this line
            //apply authhorize attribute globally
            services.AddMvc(confiq =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
                confiq.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();
            //Change Acess Denied Path
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });
            //claim based Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role"));

                //options.AddPolicy("EditRolePolicy",
                //   policy => policy.RequireClaim("Edit Role","true"));

                //Custom Auth and handler
                options.AddPolicy("EditRolePolicy", policy =>
                       policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                //custom authorization usind func
                /* options.AddPolicy("EditRolePolicy",
                    policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Admin") &&
                    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                    context.User.IsInRole("Super Admin")
                    ));*/


                options.AddPolicy("CreateRolePolicy",
                   policy => policy.RequireClaim("Create Role"));
                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));
            });

            services.AddMvcCore(options => options.EnableEndpointRouting = false);
            services.AddRazorPages();
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "38093762165-sm9e9k4oqgi144s99eodponba0maikdd.apps.googleusercontent.com";
                    options.ClientSecret = "zAUSiy5XQwFSHivPJfAajjBK";
                })
                .AddFacebook(options =>
                {
                    options.ClientId = "362066688274274";
                    options.ClientSecret = "1365309ffcb7619473810e6d02c029a5";
                });

            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Middle Wares
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            //app.UseRouting();    //(core 3.1)
            /*
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("foo.html");
            app.UseDefaultFiles(defaultFilesOptions);*/

            app.UseStaticFiles();
            app.UseAuthentication();
            //  app.UseAuthorization();
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes => routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"));

            /*app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World !");
            });*/

            /*app.UseEndpoints(endpoints =>      //(core 3.1)
              {
                  endpoints.MapGet("/", async context =>
                  {
                      await context.Response.WriteAsync("Hello World!");
                      //await context.Response.WriteAsync(_config["myKey"]);
                      //System.Diagnostics.Process.GetCurrentProcess().ProcessName
                  });
              });
            */

        }
    }
}
