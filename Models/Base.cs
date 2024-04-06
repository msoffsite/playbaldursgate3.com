namespace EclecticXnet.Models
{
	public class Base
	{
		public required string Id { get; set; } 
		public required string Title { get; set; }
		public required string Description { get; set; }
		public required string ThumbnailUrl { get; set; }
	}
}
