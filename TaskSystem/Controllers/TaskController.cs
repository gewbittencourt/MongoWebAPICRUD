using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskSystem.Domain.Entities;
using TaskSystem.Service.DTO;
using TaskSystem.Service.Interface;

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


		// Utilizar verbos http para referenciar o que você quer fazer
		//Feito
		[HttpPost]
		[Route("")]



		//NÃO PRECISA TER EXCEPTION NA CONTROLLER. ELA DEVE SER BURRA.
		public async Task<IActionResult> Create([FromBody] TasksDTO taskDto, CancellationToken cancellationToken)
		{
			// var meu DTOouINPUT = _mapper.Map<TaskDto>(request)
			// var result = await _taskService.CreateNewTask(DTOouINPUT, cancellationToken);
			// if (result not is valido?) return MEUMETODOQUECRIAUMACTIONRESULT_DE_ERROR(RESULT)
			// return MEUMETODOQUECRIAUMACTIONRESULT_DE_SUCESSO(RESULT)



			try
			{
				var result = await _taskService.CreateNewTask(taskDto, cancellationToken);
				if (result != null)
				{
					return new JsonResult(result);
				}
				return BadRequest(new
				{
					message = "Não foi possível completar a tarefa."
				});

			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(new { message = "Dados inválidos: " + ex.Message, details = ex.StackTrace });
			}
			catch (OperationCanceledException ex)
			{
				return StatusCode(StatusCodes.Status408RequestTimeout, new { message = "Operação cancelada.", details = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new
				{
					message = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
					details = ex.Message,
					exceptionType = ex.GetType().Name
				});
			}
		}

		[HttpGet]
		[Route("")]
		public async Task<IActionResult> GetAllTasks([FromQuery] Guid? id, CancellationToken cancellationToken)
		{
			try
			{
				if (id.HasValue)
				{
					var result = await _taskService.GetDetailedTask(id.Value, cancellationToken);
					if (result != null)
					{
						return new JsonResult(result);
					}
					return new JsonResult("Não foi encontrado a tarefa desejada.");

				}
				else
				{
					var result = await _taskService.GetAllTasks(cancellationToken);
					if (result.Count() >= 1)
					{
						return new JsonResult(result);
					}
					return new JsonResult("Não existem tarefas cadastradas.");

				}
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(new { message = "Dados inválidos: " + ex.Message, details = ex.StackTrace });
			}
			catch (OperationCanceledException ex)
			{
				return StatusCode(StatusCodes.Status408RequestTimeout, new { message = "Operação cancelada.", details = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new
				{
					message = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
					details = ex.Message,
					exceptionType = ex.GetType().Name
				});
			}
		}



		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> DeleteTask([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _taskService.DeleteTask(id, cancellationToken);
				if (!result)
				{
					return BadRequest(new
					{
						message = "Não foi possível completar a tarefa.",
						taskId = id
					});
				}
				return Ok(new { message = "Tarefa completada com sucesso.", taskId = id });

			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(new { message = "Dados inválidos: " + ex.Message, details = ex.StackTrace });
			}
			catch (OperationCanceledException ex)
			{
				return StatusCode(StatusCodes.Status408RequestTimeout, new { message = "Operação cancelada.", details = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new
				{
					message = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
					details = ex.Message,
					exceptionType = ex.GetType().Name
				});
			}

		}


		[HttpPut]
		[Route("{id}")]

		public async Task<IActionResult> UpdateTask([FromRoute] Guid id, [FromBody]TasksDTO taskDto, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _taskService.UpdateTask(id, taskDto, cancellationToken);
				if (!result)
				{
					return BadRequest(new
					{
						message = "Não foi possível atualizar a tarefa.",
						taskId = id
					});
				}
				return Ok(new { message = "Tarefa atualizada com sucesso.", taskId = id });
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(new { message = "Dados inválidos: " + ex.Message, details = ex.StackTrace });
			}
			catch (OperationCanceledException ex)
			{
				return StatusCode(StatusCodes.Status408RequestTimeout, new { message = "Operação cancelada.", details = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new
				{
					message = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
					details = ex.Message,
					exceptionType = ex.GetType().Name
				});
			}

		}


		[HttpPut]
		[Route("{id}/Complete")]
		public async Task<IActionResult> CompleteTask([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _taskService.CompleteTask(id, cancellationToken);

				if (!result)
				{
					return BadRequest(new
					{
						message = "Não foi possível completar a tarefa.",
						taskId = id
					});
				}

				return Ok(new { message = "Tarefa completada com sucesso.", taskId = id });
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(new { message = "Dados inválidos: " + ex.Message, details = ex.StackTrace });
			}
			catch (OperationCanceledException ex)
			{
				return StatusCode(StatusCodes.Status408RequestTimeout, new { message = "Operação cancelada.", details = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new
				{
					message = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
					details = ex.Message,
					exceptionType = ex.GetType().Name
				});
			}
		}

	}
}
