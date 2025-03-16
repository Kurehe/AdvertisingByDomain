using BusinessLogicLayer;
using DataAccessLayer;

namespace AdvertisingByDomain
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            // Add services to the container.
            services.AddControllers();

            // Добавить постоянно живущий класс с данными (in-memory collection)
            services.AddSingleton<DomainRepository>();  // Слой доступа к данным
            
            // Добавить класс для парсинга данных
            services.AddScoped<DomainServices>();       // Слой бизнес логики...

            services.AddSwaggerGen();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            
            app.MapControllers();
            app.Run();
        }
    }
}
