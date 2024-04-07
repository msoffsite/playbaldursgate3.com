using EclecticXnet.Models;

namespace EclecticXnet.Services
{
	public interface IPlayListService
	{
		Task<List<Playlist>> GetPlaylists();
        Task<Playlist> GetPlaylistById(string playListId);

    }
}
