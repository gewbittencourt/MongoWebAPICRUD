using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskSystem.API.Controllers;
using TaskSystem.Application.BaseResponse;
using TaskSystem.Application.Input;
using TaskSystem.Application.Interface;
namespace TaskSystem.Tests
{
	public class TaskControllerTest
	{
		private readonly TaskController _taskController;
		private readonly Mock<ITaskService> _mockService;


		public TaskControllerTest()
		{
			_mockService = new Mock<ITaskService>();
			_taskController = new TaskController(_mockService.Object);
		}


		[Fact]
		public async Task GetAllTask_ReturnIEnumerable_WhenSuccessfull()
		{
			//Arrange
			var output = BaseOutputApplication.Success();
			_mockService.Setup(services => services.GetAllTasks(It.IsAny<CancellationToken>())).ReturnsAsync(output);


			//Act
			var result = await _taskController.GetAllTasks(null, It.IsAny<CancellationToken>());


			//Asserts
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
		}

		[Fact]
		public async Task GetDetailedTask_ReturnJson_WhenSuccessfull()
		{

			//Arrange
			var task = BaseOutputApplication.Success();
			var id = new Guid();
			_mockService.Setup(services => services.GetDetailedTask(id, It.IsAny<CancellationToken>())).ReturnsAsync(task);


			//Act
			var result = await _taskController.GetAllTasks(new Guid(), It.IsAny<CancellationToken>());


			//Asserts
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
		}


		[Fact]
		public async Task CreateNewTask_ReturnJson_WhenSuccessfull()
		{
			//Arrange
			var task = BaseOutputApplication.Success();
			var inputTask = new CreateTaskInput();
			_mockService.Setup(Service => Service.CreateNewTask(inputTask, It.IsAny<CancellationToken>())).ReturnsAsync(task);


			//Act
			var result = await _taskController.Create(inputTask, It.IsAny<CancellationToken>());

			//Asserts
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
		}


		[Fact]
		public async Task DeleteTask_ReturnOk_WhenSuccessfull()
		{
			//Arrange
			var id = Guid.NewGuid();
			var task = BaseOutputApplication.Success();
			_mockService.Setup(service => service.DeleteTask(id, It.IsAny<CancellationToken>())).ReturnsAsync(task);


			//Act
			var result = await _taskController.DeleteTask(id, It.IsAny<CancellationToken>());


			//Asserts
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
		}


		[Fact]
		public async Task UpdateUser_ReturnOk_WhenSuccessfull()
		{
			//Arrange
			var task = new CreateTaskInput();
			var id = Guid.NewGuid();
			_mockService.Setup(service => service.UpdateTask(id, task, It.IsAny<CancellationToken>())).ReturnsAsync(BaseOutputApplication.Success());


			//Act
			var result = await _taskController.UpdateTask(id, task, It.IsAny<CancellationToken>());


			//Asserts
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
		}

		[Fact]
		public async Task CompleteTask_ReturnOk_WhenSuccessfull()
		{
			//Arrange
			var id = new Guid();
			_mockService.Setup(service => service.CompleteTask(id, It.IsAny<CancellationToken>())).ReturnsAsync(BaseOutputApplication.Success());


			//Act
			var result = await _taskController.CompleteTask(id, It.IsAny<CancellationToken>());


			//Asserts
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
		}
	}
}
