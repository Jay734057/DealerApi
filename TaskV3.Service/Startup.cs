using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using TaskV3.Business;
using TaskV3.Core.Config;
using TaskV3.Core.Interfaces.Business;
using TaskV3.Core.Interfaces.Repositories;
using TaskV3.Core.Models;
using TaskV3.Repositories;

namespace TaskV3
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
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddSwaggerDocument();

            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("Database"));

            services.AddAutoMapper(c=>c.AddProfile<AutoMapperProfile>(), typeof(Startup));

            services.AddSingleton<IAuthenticationProvider, AuthenticationProvider>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IDealerContext, DealerContext>();

            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IDealerRepository, DealerRepository>();
            services.AddScoped<IStockRepository, StockRepository>();

            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IDealerService, DealerService>();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //setup authentication scheme
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        //verify the dealer does exist in the system
                        var dealerService = context.HttpContext.RequestServices
                                                 .GetRequiredService<IDealerService>();
                        var dealerId = Convert.ToInt32(context.Principal.Identity.Name);
                        var dealer = dealerService.GetDealerByIdAsync(dealerId);
                        if (dealer == null)
                        {
                            context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();

            //seed data for manual test purpose.
            var apiContext = serviceProvider.GetService<ApiContext>();
            var authenticationProvider = serviceProvider.GetService<IAuthenticationProvider>();
            
            SeedData(apiContext, authenticationProvider);


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SeedData(ApiContext context, IAuthenticationProvider authenticationProvider)
        {
            var password = "password";
            authenticationProvider.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var dealerA = new Dealer
            {
                Name = "A",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            var dealerB = new Dealer
            {
                Name = "B",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            context.Add(dealerA);
            context.Add(dealerB);

            var carAudi2021 = new Car
            {
                Make = "Audi",
                Model = "A4",
                Year = 2021
            };

            var carAudi2020 = new Car
            {
                Make = "Audi",
                Model = "A4",
                Year = 2020
            };

            context.Add(carAudi2021);
            context.Add(carAudi2020);

            var stockA = new Stock
            {
                CarId = carAudi2021.Id,
                DealerId = dealerA.Id,
                Amount = 5
            };

            var stockB = new Stock
            {
                CarId = carAudi2020.Id,
                DealerId = dealerA.Id,
                Amount = 3
            };

            context.Add(stockA);
            context.Add(stockB);

            context.SaveChanges();
        }
    }
}
