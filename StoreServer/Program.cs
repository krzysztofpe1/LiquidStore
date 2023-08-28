using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StoreServer.Services;

namespace StoreServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ipAddress = "http://192.168.0.200:5000";
            #if (DEBUG)
            ipAddress = "http://127.0.0.1:5000";
            #endif
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                var connectionString = "Server = localhost; Database = Store; User = store; Password = P@ssWord123";
                #if (DEBUG)
                connectionString = "Server = localhost; Database = Store; User = root;";
                #endif
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

            app.Run(ipAddress);
        }
    }
}