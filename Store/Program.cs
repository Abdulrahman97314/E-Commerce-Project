using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Store.APIs.Errors;
using Store.APIs.ExtensionsMethods;
using Store.Core.Entities;
using Store.Core.Entities.Identity;
using Store.Repository.Data;
using Store.Repository.Data.SeedData;
using Store.Repository.Identity;

namespace Store
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<StoreDbContext>(o =>
            {
                o.UseSqlServer(builder.Configuration.GetConnectionString("StoreDb"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(o =>
            {
                o.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDb"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(o =>
            {
                var configuration = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(configuration);
            });
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddControllers();
            builder.Services.AddSwaggerServices();
            builder.Services.AddApplicationService();
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().SetIsOriginAllowed(orjan => true);
                });
            });

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var storeContext=services.GetRequiredService<StoreDbContext>();
                await storeContext.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(storeContext);
                var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityContext.Database.MigrateAsync();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await AppIdentityDbContextSeed.CreateRolesAsync(roleManager);
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, ex.Message);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors("CorsPolicy");
            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}