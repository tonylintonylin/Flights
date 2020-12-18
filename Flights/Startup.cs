using AutoMapper;
using Flights.Application._Related;
using Flights.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace Flights
{
    public class Startup
    {
        private IConfiguration _config { get; }
        private IWebHostEnvironment _env { get; }

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            // Caching 
            services.AddScoped<ICache, Cache>();
            services.AddScoped<ILookup, Lookup>();
            services.AddScoped<ITypeahead, Typeahead>();
            services.AddSingleton<IFilter, Filter>();

            // Support services
            services.AddScoped<IExcel, Excel>();
            services.AddScoped<IEmail, Email>();
            services.AddScoped<IRollup, Rollup>();
            services.AddSingleton<IRelated, Related>();

            // Applications services
            services.AddScoped<IViewedService, ViewedService>();
            services.AddScoped<IIssueService, IssueService>();

            // Identity support
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, ClaimsPrincipalFactory>();
            services.AddScoped<IIdentityService, IdentityService>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/");
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            });

            // Database contexts: For localdb connectionString's path is calculated
            var connectionString = _config.GetConnectionString("Flights")
                                          .Replace("{Path}", _env.ContentRootPath);

            services.AddDbContext<FlightsContext>(options =>
                                     options.UseSqlServer(connectionString));
            services.AddDbContext<ApplicationDbContext>(options =>
                                     options.UseSqlServer(connectionString));

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(typeof(ControllerAccessorFilter)); // required for ultra-clean architecture

            }).AddFlatAreas(new FlatAreaOptions())
              .AddRazorRuntimeCompilation();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                              IHttpContextAccessor httpContextAccessor, FlightsContext db)
        {
            ServiceLocator.Register(httpContextAccessor);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            db.Database.ExecuteSqlRaw("SELECT 1"); // Warmup localDb for better user experience 
        }
    }
}
