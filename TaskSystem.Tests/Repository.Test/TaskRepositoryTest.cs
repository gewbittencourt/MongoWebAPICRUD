using AutoMapper;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Domain.Entities;
using TaskSystem.Infrastructure.MongoDb.Repository;
using TaskSystem.Service.DTO;
using TaskSystem.Service.Interface;

namespace TaskSystem.Tests.Repository.Test
{
	public class TaskRepositoryTest
	{

		private readonly TaskRepository _taskRepository;
		private readonly Mock<IMongoClient> _mockMongoClient;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<IAsyncCursor<Tasks>> _mockCursor;
		private readonly Mock<IMongoDatabase> _mockDatabase;
		private readonly Mock<IMongoCollection<Tasks>> _mockMongoCollection;

		public TaskRepositoryTest()
		{
			// Inicializa os mocks
			_mockMongoClient = new Mock<IMongoClient>();
			_mockMapper = new Mock<IMapper>();
			_mockCursor = new Mock<IAsyncCursor<Tasks>>();
			_mockDatabase = new Mock<IMongoDatabase>();
			_mockMongoCollection = new Mock<IMongoCollection<Tasks>>();

			// Configurações para retornar a coleção mockada
			_mockMongoClient.Setup(client => client.GetDatabase(It.IsAny<string>(), null))
							.Returns(_mockDatabase.Object);
			_mockDatabase.Setup(db => db.GetCollection<Tasks>(It.IsAny<string>(), null))
						 .Returns(_mockMongoCollection.Object);

			// Inicializa o TaskRepository com os mocks
			_taskRepository = new TaskRepository(_mockMapper.Object, _mockMongoClient.Object);
		}


		[Fact]
		public async Task CreateNewTask_ReturnTask_WhenSuccessfull()
		{
			//Arrange
			var task = new Tasks(title: "title", description: "description");
			task.NewTask(Guid.NewGuid());
			_mockMongoCollection.Setup(mongo => mongo.InsertOneAsync(task, It.IsAny<CancellationToken>()));

			//Act
			var result = await _taskRepository.CreateNewTask(task, It.IsAny<CancellationToken>());

			//Asserts
			Assert.IsType<Tasks>(result);
			Assert.Equal(task.Title, result.Title);
		}

		[Fact]
		public async Task DeleteTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var id = Guid.NewGuid();
			var mockResult = new DeleteResult.Acknowledged(1);
			_mockMongoCollection.Setup(mongo => mongo.DeleteOneAsync(It.IsAny<FilterDefinition<Tasks>>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockResult);


			//Act
			var result = await _taskRepository.DeleteTask(id, It.IsAny<CancellationToken>());


			//Asserts
			Assert.True(result);
		}

		[Fact]
		public async Task GetAllTasks_ReturnsListOfTasks_WhenSuccessful()
		{
			// Arrange
			var cancellationToken = new CancellationToken();
			var tasksList = new List<Tasks>
		{
			new Tasks (title:"teste", description:"description"),
			new Tasks (title:"teste", description:"description"),
		};

			//SIMULAÇÃO DO FIND
			_mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
			_mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
			_mockCursor.Setup(x => x.Current).Returns(tasksList);

			_mockMongoCollection.Setup(x => x.FindAsync(
				It.IsAny<FilterDefinition<Tasks>>(),
				It.IsAny<FindOptions<Tasks, Tasks>>(),
				It.IsAny<CancellationToken>())).ReturnsAsync(_mockCursor.Object);

			// Act
			var result = await _taskRepository.GetAllTasks(cancellationToken);

			// Assert
			Assert.Equal(tasksList.Count, result.Count());
			Assert.Equal(tasksList, result);
		}

		[Fact]
		public async Task GetDetailedTask_ReturnsNull_WhenTaskDoesNotExist()
		{
			// Arrange
			var taskId = Guid.NewGuid();
			var task = new Tasks(title: "title", description: "description");
			task.NewTask(taskId);

			//SIMULAÇÃO DO FIND
			var mockCursor = new Mock<IAsyncCursor<Tasks>>();
			mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
			mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
			mockCursor.Setup(x => x.Current).Returns(new List<Tasks> { task });

			_mockMongoCollection.Setup(x => x.FindAsync(
				It.IsAny<FilterDefinition<Tasks>>(),
				It.IsAny<FindOptions<Tasks, Tasks>>(),
				It.IsAny<CancellationToken>())).ReturnsAsync(mockCursor.Object);

			// Act
			var result = await _taskRepository.GetDetailedTask(taskId, It.IsAny<CancellationToken>());

			// Assert
			Assert.NotNull(result);
			Assert.IsType<Tasks>(result);
			Assert.Equal(task.Id, result.Id);
			Assert.Equal(task.Title, result.Title);
		}

		[Fact]

		public async Task UpdateTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var taskId = Guid.NewGuid();
			var task = new Tasks(title: "title", description: "description");
			var updatedTask = new Tasks(title: "title1", description: "description1");
			task.NewTask(taskId);
			var mockResult = new UpdateResult.Acknowledged(1, 1, null);

			var mockCursor = new Mock<IAsyncCursor<Tasks>>();
			mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
			mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
			mockCursor.Setup(x => x.Current).Returns(new List<Tasks> { task });

			_mockMongoCollection.Setup(x => x.FindAsync(
				It.IsAny<FilterDefinition<Tasks>>(),
				It.IsAny<FindOptions<Tasks, Tasks>>(),
				It.IsAny<CancellationToken>())).ReturnsAsync(mockCursor.Object);


			_mockMongoCollection.Setup(x => x.UpdateOneAsync(
			It.IsAny<FilterDefinition<Tasks>>(),
			It.IsAny<UpdateDefinition<Tasks>>(),
			null,
			It.IsAny<CancellationToken>())).ReturnsAsync(mockResult);



			//Act
			var result = await _taskRepository.UpdateTask(taskId,updatedTask, It.IsAny<CancellationToken>());

			//Asserts
			Assert.NotNull(result);
			Assert.True(result);

		}



		[Fact]

		public async Task CompleteTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var taskId = Guid.NewGuid();
			var task = new Tasks(title: "title", description: "description");
			task.NewTask(taskId);
			var mockResult = new UpdateResult.Acknowledged(1, 1, null);

			var mockCursor = new Mock<IAsyncCursor<Tasks>>();
			mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
			mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
			mockCursor.Setup(x => x.Current).Returns(new List<Tasks> { task });

			_mockMongoCollection.Setup(x => x.FindAsync(
				It.IsAny<FilterDefinition<Tasks>>(),
				It.IsAny<FindOptions<Tasks, Tasks>>(),
				It.IsAny<CancellationToken>())).ReturnsAsync(mockCursor.Object);


			_mockMongoCollection.Setup(x => x.UpdateOneAsync(
			It.IsAny<FilterDefinition<Tasks>>(),
			It.IsAny<UpdateDefinition<Tasks>>(),
			null,
			It.IsAny<CancellationToken>())).ReturnsAsync(mockResult);


			//Act
			var result = await _taskRepository.CompletedTask(taskId, It.IsAny<CancellationToken>());


			//Assert
			Assert.NotNull(result);
			Assert.True(result);

		}

	}
}
