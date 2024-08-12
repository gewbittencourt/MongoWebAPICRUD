using Microsoft.AspNetCore.Mvc;
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
            Console.WriteLine("teste");
            var id = await _taskService.CreateNewTask(taskDto, cancellationToken);
			return new JsonResult(id);
		}






	}
}
