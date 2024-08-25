namespace TaskSystem.Application.BaseResponse
{
	public class BaseOutputApplication
	{
		public bool IsValid { get; private set; }

		public object Data {  get; private set; } 

		public Exception Errors { get; private set; }

		public static BaseOutputApplication Success(Object data)
		{
			return new BaseOutputApplication { IsValid = true, Data = data };
		}

		public static BaseOutputApplication Success()
		{
			return new BaseOutputApplication { IsValid = true};
		}

		public static BaseOutputApplication Failure(Exception ex)
		{
			return new BaseOutputApplication { Errors = ex };
		}
	}
}
