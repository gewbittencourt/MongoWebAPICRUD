using TaskSystem.Application.BaseResponse;
using TaskSystem.Application.Output;

namespace TaskSystem.API.BaseResponse
{
	public class BaseResponseController
	{
		public static string CreateSuccessResponse(BaseOutputApplication result)
		{
			return "Tarefa inserida com sucesso!";
		}

		public static Exception ErrorResponse(Exception errors)
		{
			return errors;
		}

		public static List<GetTaskOutput> GetAllSuccessResponse(BaseOutputApplication result)
		{
			return (List<GetTaskOutput>)result.Data;
		}

		public static object GetTaskSuccessResponse(BaseOutputApplication result)
		{
			return result.Data;
		}

		public static string DeleteSuccessResponse(BaseOutputApplication result)
		{
			return "Tarefa excluida com sucesso!";
		}

		public static object UpdateTaskSuccessResponse(BaseOutputApplication result)
		{
			return "Tarefa atualizada com sucesso!";
		}

		public static object CompleteTaskSuccessResponse(BaseOutputApplication result)
		{
			return "Tarefa completada com sucesso!";
		}

	}
}
