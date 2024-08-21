using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.API.Controllers;
using TaskSystem.Domain.Entities;
using TaskSystem.Service.DTO;
using TaskSystem.Service.Interface;

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
			var listTask = new List<TasksDTO>();
			_mockService.Setup(services => services.GetAllTasks(It.IsAny<CancellationToken>())).ReturnsAsync(listTask);


			//Act
			var result = await _taskController.GetAllTasks(null, It.IsAny<CancellationToken>());


			//Asserts
			Assert.NotNull(result);
			Assert.IsType<JsonResult>(result);
		}

		[Fact]
		public async Task GetDetailedTask_ReturnJson_WhenSuccessfull()
		{

			//Arrange
			var task = new TasksDTO();
			_mockService.Setup(services => services.GetDetailedTask(task.Id, It.IsAny<CancellationToken>())).ReturnsAsync(task);


			//Act
			var result = await _taskController.GetAllTasks(new Guid(), It.IsAny<CancellationToken>());


			//Asserts
			Assert.NotNull(result);
			Assert.IsType<JsonResult>(result);
		}


		[Fact]
		public async Task CreateNewTask_ReturnJson_WhenSuccessfull()
		{
			//Arrange
			var task = new TasksDTO();
			_mockService.Setup(Service => Service.CreateNewTask(task, It.IsAny<CancellationToken>())).ReturnsAsync(task);


			//Act
			var result = await _taskController.Create(task, It.IsAny<CancellationToken>());

			//Asserts
			Assert.NotNull(result);
			Assert.IsType<JsonResult>(result);
		}


		[Fact]
		public async Task DeleteTask_ReturnOk_WhenSuccessfull()
		{
			//Arrange
			var id = Guid.NewGuid();
			_mockService.Setup(service => service.DeleteTask(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);


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
			var task = new TasksDTO();
			_mockService.Setup(service => service.UpdateTask(task.Id, task, It.IsAny<CancellationToken>())).ReturnsAsync(true);


			//Act
			var result = await _taskController.UpdateTask(task.Id, task, It.IsAny<CancellationToken>());


			//Asserts
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
		}

		[Fact]
		public async Task CompleteTask_ReturnOk_WhenSuccessfull()
		{
			//Arrange
			var id = new Guid();
			_mockService.Setup(service => service.CompleteTask(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);


			//Act
			var result = await _taskController.CompleteTask(id, It.IsAny<CancellationToken>());


			//Asserts
			Assert.NotNull(result);
			Assert.IsType<OkObjectResult>(result);
		}
	}
}
