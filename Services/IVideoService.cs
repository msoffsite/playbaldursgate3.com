using PlayBaldursGate3.Models;

namespace PlayBaldursGate3.Services
{
	public interface IVideoService
	{
		Task<List<Video>> GetVideosForPlayListId(string playListId);
	}
}
