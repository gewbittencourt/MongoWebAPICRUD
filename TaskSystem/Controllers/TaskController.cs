using Microsoft.AspNetCore.Mvc;
using TaskSystem.Domain.Entities;
using TaskSystem.Service.DTO;
using TaskSystem.Service.Interface;

namespace TaskSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly ITaskService _taskService;

		public TaskController(ITaskService taskService)
		{
			_taskService = taskService;
		}

		[HttpPost]
		public async Task<IActionResult> Create(TasksDTO taskDto, CancellationToken cancellationToken)
		{

			var id = await _taskService.CreateNewTask(taskDto, cancellationToken);
			return new JsonResult(id);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllTasks([FromQuery] Guid? id, CancellationToken cancellationToken)
		{
			if (id.HasValue)
			{
				var result = await _taskService.GetDetailedTask(id.Value, cancellationToken);
				return new JsonResult(result);
			}
			else
			{
				var result = await _taskService.GetAllTasks(cancellationToken);
				return new JsonResult(result);

			}
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
		{
			var result = await _taskService.DeleteTask(id, cancellationToken);
			return new JsonResult(result);
		}


		[HttpPut]

		public async Task<IActionResult> UpdateTask(Guid id, TasksDTO taskDto, CancellationToken cancellationToken)
		{
			var result = await _taskService.UpdateTask(id, taskDto, cancellationToken);
			return new JsonResult(result);
		}

		[HttpPut("complete task")]
		public async Task<IActionResult> CompleteTask(Guid id, CancellationToken cancellationToken)
		{
			var result = await _taskService.CompletedTask(id, cancellationToken);
			return new JsonResult(result);
		}


	}
}
