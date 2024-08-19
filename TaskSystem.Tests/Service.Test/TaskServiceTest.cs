using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSystem.Domain.Entities;
using TaskSystem.Domain.Interfaces;
using TaskSystem.Infrastructure.MongoDb.Mapper;
using TaskSystem.Service.DTO;
using TaskSystem.Service.Services;

namespace TaskSystem.Tests.Service.Test
{
	public class TaskServiceTest
	{

		private readonly TaskServices _taskServices;
		private readonly Mock<ITaskRepository> _mockTaskRepository;
		private readonly Mock<IMapper> _mockMapper;

		public TaskServiceTest()
		{
			_mockTaskRepository = new Mock<ITaskRepository>();
			_mockMapper = new Mock<IMapper>();
			_taskServices = new TaskServices(_mockTaskRepository.Object, _mockMapper.Object);
		}

		[Fact]
		public async Task CompletedTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var id = Guid.NewGuid();
			_mockTaskRepository.Setup(repository => repository.CompletedTask(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

			//Act
			var result = await _taskServices.CompletedTask(id, It.IsAny<CancellationToken>());


			//Asserts
			Assert.True(result);

		}

		[Fact]
		public async Task CreateNewTask_ReturnTaskDTO_WhenSuccessfull()
		{
			//Arrange
			var taskDTO = new TasksDTO { Id = new Guid(), CompletationDate = DateTime.Now, CreationDate = DateTime.Now, Description = "teste", Title = "teste" };
			var task = new Tasks(title: taskDTO.Title, description: taskDTO.Description);
			task.NewTask(Guid.NewGuid());
			_mockTaskRepository.Setup(repository => repository.CreateNewTask(It.IsAny<Tasks>(), It.IsAny<CancellationToken>())).ReturnsAsync(task);
			_mockMapper.Setup(mapper => mapper.Map<TasksDTO>(task)).Returns(taskDTO);


			//Act
			var result = await _taskServices.CreateNewTask(taskDTO, It.IsAny<CancellationToken>());


			//Asserts
			Assert.IsType<TasksDTO>(result);
			Assert.Equal(taskDTO.Title, result.Title);
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
			Assert.True(result);

		}

		[Fact]
		public async Task GetAllTasks_ReturnIEnumerable_WhenSuccessfull()
		{
			//Arrange
			var taskDTO = new List<TasksDTO>();
			var task = new List<Tasks>();
			_mockTaskRepository.Setup(repository => repository.GetAllTasks(It.IsAny<CancellationToken>())).ReturnsAsync(task);
			_mockMapper.Setup(mapper => mapper.Map<IEnumerable<TasksDTO>>(It.IsAny<Tasks>())).Returns(taskDTO);

			//Act
			var result = await _taskServices.GetAllTasks(It.IsAny<CancellationToken>());

			//Assert
			var returnValue = Assert.IsAssignableFrom<IEnumerable<TasksDTO>>(result);

		}

		[Fact]
		public async Task GetDetailedTask_ReturnTaskDTO_WhenSuccessfull()
		{
			//Arrange
			var taskDTO = new TasksDTO { Id = new Guid(), CompletationDate = DateTime.Now, CreationDate = DateTime.Now, Description = "teste", Title = "teste" };
			var task = new Tasks(title: taskDTO.Title, description: taskDTO.Description);
			task.NewTask(Guid.NewGuid());
			taskDTO.Id = task.Id;
			_mockTaskRepository.Setup(repository => repository.GetDetailedTask(task.Id, It.IsAny<CancellationToken>())).ReturnsAsync(task);
			_mockMapper.Setup(mapper => mapper.Map<TasksDTO>(It.IsAny<Tasks>())).Returns(taskDTO);

			//Act
			var result = await _taskServices.GetDetailedTask(taskDTO.Id, It.IsAny<CancellationToken>());

			//Asserts
			Assert.IsType<TasksDTO>(result);
		}

		[Fact]
		public async Task UpdateTask_ReturnBool_WhenSuccessfull()
		{
			//Arrange
			var id = Guid.NewGuid();
			var taskDTO = new TasksDTO { Id = new Guid(), CompletationDate = DateTime.Now, CreationDate = DateTime.Now, Description = "teste", Title = "teste" };
			var task = new Tasks(title: taskDTO.Title, description: taskDTO.Description);
			task.NewTask(Guid.NewGuid());
			_mockMapper.Setup(mapper => mapper.Map<Tasks>(taskDTO)).Returns(task);
			_mockTaskRepository.Setup(repository => repository.UpdateTask(id, task, It.IsAny<CancellationToken>())).ReturnsAsync(true);

			//Act
			var result = await _taskServices.UpdateTask(id, taskDTO, It.IsAny<CancellationToken>());

			//Assert
			Assert.True(result);

		}
	}
}
