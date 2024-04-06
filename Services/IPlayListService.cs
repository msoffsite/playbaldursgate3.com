using EclecticXnet.Models;

namespace EclecticXnet.Services
{
	public interface IPlayListService
	{
		Task<List<Playlist>> GetPlaylists();
	}
}
