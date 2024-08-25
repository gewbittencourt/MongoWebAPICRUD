namespace TaskSystem.Application.Output
{
	public class GetTaskOutput
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime CompletationDate { get; set; }
	}
}
