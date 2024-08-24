using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSystem.Application.BaseResponse
{
	public class BaseResponseApplication
	{
		public bool IsValid { get; private set; }
		public object Data { get; private set; }
		public Exception Errors { get; private set; }

		public static BaseResponseApplication Success()
		{
			return new BaseResponseApplication { IsValid = true };
		}

		public static BaseResponseApplication Failure(Exception ex)
		{
			return new BaseResponseApplication { IsValid = false, Errors = ex };
		}
	}
}
