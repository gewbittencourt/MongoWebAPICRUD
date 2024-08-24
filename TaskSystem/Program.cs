using MongoDB.Driver;
using TaskSystem.Domain.Interfaces;
using TaskSystem.Infrastructure.MongoDb.Repository;
using TaskSystem.Domain.Entities;
using TaskSystem.Infrastructure.MongoDb.Collection;
using Microsoft.AspNetCore.Mvc;
using TaskSystem.Infrastructure.MongoDb.Mapper;
using TaskSystem.Application.Interface;
using TaskSystem.Application.Service;
using TaskSystem.Application.Mapping;
using TaskSystem.API.BaseResponse;
using TaskSystem.Application.BaseResponse;
namespace TaskSystem

{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddApiVersioning(x =>
			{
				x.DefaultApiVersion = new ApiVersion(1, 0);
				x.AssumeDefaultVersionWhenUnspecified = true;
				x.ReportApiVersions = true;
			});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddAutoMapper(typeof(MappingEntitie));
			builder.Services.AddAutoMapper(typeof(MappingCollection));


			IConfiguration configuration = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.json")
				.AddJsonFile("appsettings.Development.json")
				.Build();



			var mongoclient = new MongoClient(configuration.GetConnectionString("MongoDb"));

			builder.Services.AddSingleton<IMongoClient>(mongoclient);
			builder.Services.AddSingleton(sp =>
			{
				var database = mongoclient.GetDatabase(TaskCollection.CollectionName);
				return database.GetCollection<TaskCollection>(nameof(TaskCollection));
			});

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
