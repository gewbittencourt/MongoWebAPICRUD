using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskSystem.Application.BaseResponse;

namespace TaskSystem.API.BaseResponse
{
	public class BaseResponseController
	{
		public static string CreateSuccessResponse(BaseResponseApplication result)
		{
			return "Tarefa inserida com sucesso";
		}

		public static Exception CreateErrorResponse(Exception errors)
		{
			return errors;
		}

	}
}
