using OFXCodingTest.Services.Quotes;
using OFXCodingTest.Services.Rates;
using OFXCodingTest.Services.Repository;
using OFXCodingTest.Services.Transfers;

namespace OFXCodingTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IQuotesService, QuotesService>();
            builder.Services.AddSingleton<ITransfersService, TransfersService>();
            builder.Services.AddSingleton<IRatesService, PublicRatesService>();
            builder.Services.AddSingleton<IRepositoryService, InMemoryRepositoryService>();

            builder.Services.AddMemoryCache();

            builder.Services.AddHttpClient();

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
