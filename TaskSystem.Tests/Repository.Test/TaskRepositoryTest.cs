using AutoMapper;
using MongoDB.Driver;
using Moq;
using TaskSystem.Domain.Entities;
using TaskSystem.Infrastructure.MongoDb.Collection;
using TaskSystem.Infrastructure.MongoDb.Repository;


namespace TaskSystem.Tests.Repository.Test
{
	public class TaskRepositoryTest
	{

		private readonly TaskRepository _taskRepository;
		private readonly Mock<IMongoClient> _mockMongoClient;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<IAsyncCursor<TaskCollection>> _mockCursor;
		private readonly Mock<IMongoDatabase> _mockDatabase;
		private readonly Mock<IMongoCollection<TaskCollection>> _mockMongoCollection;

		public TaskRepositoryTest()
		{
			_mockMongoClient = new Mock<IMongoClient>();
			_mockMapper = new Mock<IMapper>();
			_mockCursor = new Mock<IAsyncCursor<TaskCollection>>();
			_mockDatabase = new Mock<IMongoDatabase>();
			_mockMongoCollection = new Mock<IMongoCollection<TaskCollection>>();

			// Configurações para retornar a coleção mockada
			_mockMongoClient.Setup(client => client.GetDatabase("TaskSystem", null))
							.Returns(_mockDatabase.Object);
			_mockDatabase.Setup(db => db.GetCollection<TaskCollection>(nameof(TaskCollection), null))
						 .Returns(_mockMongoCollection.Object);

			// Inicializa o TaskRepository com os mocks
			_taskRepository = new TaskRepository(_mockMongoCollection.Object, _mockMapper.Object);
		}


		[Fact]
		public async Task CreateNewTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var task = new Tasks(title: "title", description: "description");
			var taskCollection = new TaskCollection();
			_mockMapper.Setup(mapper => mapper.Map<TaskCollection>(task)).Returns(taskCollection);
			_mockMongoCollection.Setup(mongo => mongo.InsertOneAsync(taskCollection, It.IsAny<CancellationToken>()));

			//Act
			await _taskRepository.CreateNewTask(task, It.IsAny<CancellationToken>());

			// Assert
			_mockMapper.Verify(mapper => mapper.Map<TaskCollection>(task), Times.Once);
			_mockMongoCollection.Verify(repository => repository.InsertOneAsync(
				taskCollection,
				It.IsAny<InsertOneOptions>(),
				It.IsAny<CancellationToken>()),
				Times.Once);
		}

		[Fact]
		public async Task DeleteTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var id = Guid.NewGuid();
			var mockResult = new DeleteResult.Acknowledged(1);
			_mockMongoCollection.Setup(mongo => mongo.DeleteOneAsync(It.IsAny<FilterDefinition<TaskCollection>>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockResult);


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
			var tasksList = new List<TaskCollection>
		{
			new TaskCollection {Title = "teste", Description = "teste"},
			new TaskCollection {Title = "teste1", Description = "teste1"},
		};

			//SIMULAÇÃO DO FIND
			_mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
			_mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
			_mockCursor.Setup(x => x.Current).Returns(tasksList);

			_mockMongoCollection.Setup(x => x.FindAsync(
				It.IsAny<FilterDefinition<TaskCollection>>(),
				It.IsAny<FindOptions<TaskCollection, TaskCollection>>(),
				It.IsAny<CancellationToken>())).ReturnsAsync(_mockCursor.Object);

			// Act
			var result = await _taskRepository.GetAllTasks(cancellationToken);

			// Assert
			Assert.IsType<Tasks[]>(result);
		}

		[Fact]
		public async Task GetDetailedTask_ReturnsTask_WhenTaskExists()
		{
			// Arrange
			var taskId = Guid.NewGuid();
			var taskCollection = new TaskCollection { TaskID = taskId, Title = "teste", Description = "teste" };
			var task = new Tasks(title: taskCollection.Title, description: taskCollection.Description);

			// SIMULAÇÃO DO FIND que encontra um item
			var mockCursor = new Mock<IAsyncCursor<TaskCollection>>();
			mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
			mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
			mockCursor.Setup(x => x.Current).Returns(new List<TaskCollection> { taskCollection });

			_mockMongoCollection.Setup(x => x.FindAsync(
				It.IsAny<FilterDefinition<TaskCollection>>(),
				It.IsAny<FindOptions<TaskCollection, TaskCollection>>(),
				It.IsAny<CancellationToken>())).ReturnsAsync(mockCursor.Object);

			_mockMapper.Setup(m => m.Map<Tasks>(It.IsAny<TaskCollection>()))
					   .Returns(task);

			// Act
			var result = await _taskRepository.GetDetailedTask(task.Id, It.IsAny<CancellationToken>());

			// Assert
			Assert.NotNull(result);
			Assert.IsType<Tasks>(result);
			Assert.Equal(taskCollection.Title, result.Title);
		}
		
		[Fact]

		public async Task UpdateTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var taskId = Guid.NewGuid();
			var task = new Tasks(title: "title", description: "description");
			var updatedTask = new Tasks(title: "title1", description: "description1");
			var mockResult = new UpdateResult.Acknowledged(1, 1, null);

			var mockCursor = new Mock<IAsyncCursor<Tasks>>();
			mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
			mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
			mockCursor.Setup(x => x.Current).Returns(new List<Tasks> { task });

			_mockMongoCollection.Setup(x => x.FindAsync(
				It.IsAny<FilterDefinition<TaskCollection>>(),
				It.IsAny<FindOptions<TaskCollection, Tasks>>(),
				It.IsAny<CancellationToken>())).ReturnsAsync(mockCursor.Object);


			_mockMongoCollection.Setup(x => x.UpdateOneAsync(
			It.IsAny<FilterDefinition<TaskCollection>>(),
			It.IsAny<UpdateDefinition<TaskCollection>>(),
			null,
			It.IsAny<CancellationToken>())).ReturnsAsync(mockResult);



			//Act
			var result = await _taskRepository.UpdateTask(updatedTask, It.IsAny<CancellationToken>());

			//Asserts
			Assert.NotNull(result);
			Assert.True(result);

		}
		
	}

}
