using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;

namespace RaceDay.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register the DbContext
            builder.Services.AddDbContext<RaceDayDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("RaceDayConnection")));

            // Add Session and Cookie Authentication services
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication("MyCookieAuth")
                .AddCookie("MyCookieAuth", options =>
                {
                    options.Cookie.Name = "RaceDayAuth";
                    options.LoginPath = "/api/auth/login";
                    options.AccessDeniedPath = "/api/auth/access-denied";
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}