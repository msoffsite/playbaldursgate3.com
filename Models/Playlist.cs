namespace EclecticXnet.Models
{
	public class Playlist : Base
	{
		internal List<Video> Videos { get; set; } = new List<Video>();
	}
}
