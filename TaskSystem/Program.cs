using MongoDB.Driver;
using TaskSystem.Domain.Interfaces;
using TaskSystem.Service.Interface;
using TaskSystem.Service.Services;
using TaskSystem.Infrastructure.MongoDb.Repository;
using TaskSystem.Infrastructure.MongoDb.Mapper;
namespace TaskSystem

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
			builder.Services.AddAutoMapper(typeof(MappingClass));


			IConfiguration configuration = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.json")
				.AddJsonFile("appsettings.Development.json")
				.Build();

			var mongoclient = new MongoClient(configuration.GetConnectionString("MongoDb"));


			builder.Services.AddSingleton<IMongoClient>(mongoclient);
			builder.Services.AddScoped<ITaskRepository, TaskRepository>();
			builder.Services.AddScoped<ITaskService, TaskServices>();

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
