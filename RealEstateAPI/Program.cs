using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateAPI.Configurations;
using RealEstateAPI.Data;
using RealEstateAPI.Interfaces;
using RealEstateAPI.Model;
using RealEstateAPI.Repository;
using System.Text;
using System.Text.Json.Serialization;

namespace RealEstateAPI
{

    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddControllers().AddJsonOptions(
                x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
                ); // this prevents error from happening when youre returning nested entities 
                   //after using include in the Repository class

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // repository services
            builder.Services.AddScoped<IListingsRepository, ListingRepository>();
            builder.Services.AddScoped<IUserOrRealtorRepository, UserOrRealtorRepository>();
            builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            builder.Services.AddScoped<IOffersRepository, OffersRepository>();

            // automapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Db config
            builder.Services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //jet config

            builder.Services.Configure<JwtConfig>(
                builder.Configuration.GetSection(key: "JwtConfig:Secret")
                );

            var key = Encoding.ASCII.GetBytes(
                builder.Configuration.GetSection(
                    key: "JwtConfig:Secret"
                    ).Value
                    );


            var tokenValidationParameter = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            builder.Services.AddAuthentication(configureOptions: options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParameter;
                
            });

            builder.Services.AddSingleton(tokenValidationParameter);

            builder.Services.AddIdentity <UsersOrRealtors, IdentityRole>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireNonAlphanumeric = false;

                }
                )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApiDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseHttpLogging();


            //  use the jwt authentication;
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Realtor", "Member" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));

                }
            }

            app.Run();
        }
    }
}