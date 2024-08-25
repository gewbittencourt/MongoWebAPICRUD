using AutoMapper;
using Moq;
using TaskSystem.Domain.Entities;
using TaskSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using TaskSystem.Application.Service;
using TaskSystem.Application.BaseResponse;
using TaskSystem.Application.Input;
using TaskSystem.Application.Output;

namespace TaskSystem.Tests.Service.Test
{
	public class TaskServiceTest
	{

		private readonly TaskServices _taskServices;
		private readonly Mock<ITaskRepository> _mockTaskRepository;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<ILogger<TaskServices>> _mockLogger;

		public TaskServiceTest()
		{
			_mockTaskRepository = new Mock<ITaskRepository>();
			_mockMapper = new Mock<IMapper>();
			_mockLogger = new Mock<ILogger<TaskServices>>();
			_taskServices = new TaskServices(_mockTaskRepository.Object, _mockMapper.Object, _mockLogger.Object);
		}

		[Fact]
		public async Task CompletedTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var id = Guid.NewGuid();
			_mockTaskRepository.Setup(repository => repository.GetDetailedTask(id, It.IsAny<CancellationToken>())).ReturnsAsync(new Tasks("teste", "teste"));
			_mockTaskRepository.Setup(repository => repository.UpdateTask(It.IsAny<Tasks>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

			//Act
			var result = await _taskServices.CompleteTask(id, It.IsAny<CancellationToken>());


			//Asserts
			Assert.IsType<BaseOutputApplication>(result);

		}

		[Fact]
		public async Task CreateNewTask_ReturnTaskDTO_WhenSuccessfull()
		{
			//Arrange
			var taskInput = new CreateTaskInput{Description = "teste", Title = "teste" };
			_mockMapper.Setup(mapper => mapper.Map<Tasks>(taskInput)).Returns(new Tasks(taskInput.Title, taskInput.Description));
			_mockTaskRepository.Setup(repository => repository.CreateNewTask(It.IsAny<Tasks>(), It.IsAny<CancellationToken>()));



			//Act
			var result = await _taskServices.CreateNewTask(taskInput, It.IsAny<CancellationToken>());


			//Asserts
			Assert.IsType<BaseOutputApplication>(result);
		}

		
		[Fact]
		public async Task DeleteTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var id = Guid.NewGuid();
			_mockTaskRepository.Setup(repository => repository.DeleteTask(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

			//Act
			var result = await _taskServices.DeleteTask(id, It.IsAny<CancellationToken>());

			//Asserts
			Assert.IsType<BaseOutputApplication>(result);

		}
		
		[Fact]
		public async Task GetAllTasks_ReturnIEnumerable_WhenSuccessfull()
		{
			//Arrange
			var taskInput = new List<CreateTaskInput>();
			var getOutput = new List<GetTaskOutput>();
			var task = new List<Tasks>();
			_mockTaskRepository.Setup(repository => repository.GetAllTasks(It.IsAny<CancellationToken>())).ReturnsAsync(task);
			_mockMapper.Setup(mapper => mapper.Map<IEnumerable<GetTaskOutput>>(It.IsAny<Tasks>())).Returns(getOutput);

			//Act
			var result = await _taskServices.GetAllTasks(It.IsAny<CancellationToken>());

			//Assert
			Assert.IsType<BaseOutputApplication>(result);

		}
		
		[Fact]
		public async Task GetDetailedTask_ReturnTaskDTO_WhenSuccessfull()
		{
			//Arrange
			var taskInput = new CreateTaskInput {Description = "teste", Title = "teste" };
			var taskOutput = new GetTaskOutput();
			var task = new Tasks(title: taskInput.Title, description: taskInput.Description);
			_mockTaskRepository.Setup(repository => repository.GetDetailedTask(task.Id, It.IsAny<CancellationToken>())).ReturnsAsync(task);
			_mockMapper.Setup(mapper => mapper.Map<GetTaskOutput>(task)).Returns(taskOutput);

			//Act
			var result = await _taskServices.GetDetailedTask(task.Id, It.IsAny<CancellationToken>());

			//Asserts
			Assert.IsType<BaseOutputApplication>(result);
		}
		[Fact]
		public async Task UpdateTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var taskInput = new CreateTaskInput {Description = "teste", Title = "teste" };
			var task = new Tasks(title: taskInput.Title, description: taskInput.Description);
			_mockTaskRepository.Setup(repository => repository.GetDetailedTask(task.Id, It.IsAny<CancellationToken>())).ReturnsAsync(task);
			_mockMapper.Setup(mapper => mapper.Map<Tasks>(taskInput)).Returns(task);
			_mockTaskRepository.Setup(repository => repository.UpdateTask(task, It.IsAny<CancellationToken>())).ReturnsAsync(true);

			//Act
			var result = await _taskServices.UpdateTask(task.Id, taskInput, It.IsAny<CancellationToken>());

			//Assert
			Assert.IsType<BaseOutputApplication>(result);

		}
	}
}
