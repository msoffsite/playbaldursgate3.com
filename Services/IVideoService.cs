using EclecticXnet.Models;

namespace EclecticXnet.Services
{
	public interface IVideoService
	{
		Task<List<Video>> GetVideosForPlayListId(string playListId);
	}
}
