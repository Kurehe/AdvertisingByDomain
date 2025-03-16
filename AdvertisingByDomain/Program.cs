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

            // �������� ��������� ������� ����� � ������� (in-memory collection)
            services.AddSingleton<DomainRepository>();  // ���� ������� � ������
            
            // �������� ����� ��� �������� ������
            services.AddScoped<DomainServices>();       // ���� ������ ������...

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
