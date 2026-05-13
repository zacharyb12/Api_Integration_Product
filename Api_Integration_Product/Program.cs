
using Api_Integration_Product.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace Api_Integration_Product
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // configuration Entity 
            builder.Services.AddDbContext<MyAppContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("myConnection"))
            );

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
