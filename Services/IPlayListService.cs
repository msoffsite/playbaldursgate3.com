using PlayBaldursGate3.Models;

namespace PlayBaldursGate3.Services
{
	public interface IPlayListService
	{
		Task<List<Playlist>> GetPlayLists();

    }
}
