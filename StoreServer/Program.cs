using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StoreServer.Services;

namespace StoreServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                var connectionString = "Server = localhost; Database = Store; User = root";
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            scope.ServiceProvider.GetRequiredService<StoreDbContext>().Database.EnsureCreated();

            builder.Services.AddControllers();
            builder.Services.AddScoped<StorageDbService>();
            builder.Services.AddScoped<OrderDbService>();
            builder.Services.AddScoped<UsersDbService>();
            builder.Services.AddScoped<SessionsDbService>();

            var app = builder.Build();
            app.MapControllers();

            app.Run();
        }
    }
}