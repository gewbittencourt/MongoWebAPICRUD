using Microsoft.AspNetCore.Mvc;
using TaskSystem.API.BaseResponse;
using TaskSystem.Application.Input;
using TaskSystem.Application.Interface;

namespace TaskSystem.API.Controllers
{
	[ApiController]
	[ApiVersion("1")]
	[Route("api/v1/[controller]")]
	public class TaskController : ControllerBase
	{
		private readonly ITaskService _taskService;

		public TaskController(ITaskService taskService)
		{
			_taskService = taskService;
		}


		[HttpPost]
		[Route("")]
		public async Task<IActionResult> Create([FromBody] CreateTaskInput taskInput, CancellationToken cancellationToken)
		{
			var result = await _taskService.CreateNewTask(taskInput, cancellationToken);
			if (result.IsValid)
			{
				return Ok(BaseResponseController.CreateSuccessResponse(result));
			}
			return (IActionResult)BaseResponseController.ErrorResponse(result.Errors);

		}

		[HttpGet]
		[Route("")]
		public async Task<IActionResult> GetAllTasks([FromQuery] Guid? id, CancellationToken cancellationToken)
		{
			if (id.HasValue)
			{
				var result = await _taskService.GetDetailedTask(id.Value, cancellationToken);
				if (result.IsValid)
				{
					return Ok(BaseResponseController.GetTaskSuccessResponse(result));
				}
				return (IActionResult)BaseResponseController.ErrorResponse(result.Errors);

			}
			else
			{
				var result = await _taskService.GetAllTasks(cancellationToken);
				if (result.IsValid)
				{
					return Ok(BaseResponseController.GetAllSuccessResponse(result));
				}
				return (IActionResult)BaseResponseController.ErrorResponse(result.Errors);

			}
		}



		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> DeleteTask([FromRoute] Guid id, CancellationToken cancellationToken)
		{

			var result = await _taskService.DeleteTask(id, cancellationToken);
			if (result.IsValid)
			{
				return Ok(BaseResponseController.DeleteSuccessResponse(result));
			}
			return (IActionResult)BaseResponseController.ErrorResponse(result.Errors);

		}


		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> UpdateTask([FromRoute] Guid id, [FromBody] CreateTaskInput taskInput, CancellationToken cancellationToken)
		{

			var result = await _taskService.UpdateTask(id, taskInput, cancellationToken);
			if (result.IsValid)
			{

				return Ok(BaseResponseController.UpdateTaskSuccessResponse(result));
			}
			return (IActionResult)BaseResponseController.ErrorResponse(result.Errors);
		}


		[HttpPut]
		[Route("{id}/Complete")]
		public async Task<IActionResult> CompleteTask([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			var result = await _taskService.CompleteTask(id, cancellationToken);

			if (result.IsValid)
			{
				return Ok(BaseResponseController.CompleteTaskSuccessResponse(result));
			}
			return (IActionResult)BaseResponseController.ErrorResponse(result.Errors);

		}


	}
}
