using EclecticXnet.Models;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Options;
using EclecticPlayList = EclecticXnet.Models.Playlist;

namespace EclecticXnet.Services
{
	public class PlayListService : IPlayListService
	{
		private readonly IOptions<GoogleSettings> _googleSettings;

		public PlayListService(IOptions<GoogleSettings> googleSettings)
		{
			_googleSettings = googleSettings;
		}

		public async Task<List<EclecticPlayList>> GetPlaylists()
		{
			var youtubeService = new YouTubeService(new Google.Apis.Services.BaseClientService.Initializer()
			{
				ApiKey = _googleSettings.Value.Key,
				ApplicationName = _googleSettings.Value.AppName
			});

			var channelsListRequest = youtubeService.Channels.List("contentDetails");
			channelsListRequest.Id = _googleSettings.Value.YouTubeChannelId;

			List<EclecticPlayList> eclecticPlaylists = new List<EclecticPlayList>();

			var requestPlaylists = youtubeService.Playlists.List("snippet");
			requestPlaylists.MaxResults = int.MaxValue;
			requestPlaylists.ChannelId = _googleSettings.Value.YouTubeChannelId;
			var responsePlaylists = await requestPlaylists.ExecuteAsync();
			foreach (var playListResult in responsePlaylists.Items)
			{
				var snippet = playListResult.Snippet;

				try
				{
					EclecticPlayList eclecticPlayList = new EclecticPlayList
					{
						Id = playListResult.Id,
						Title = snippet.Title,
						Description = snippet.Description,
						ThumbnailUrl = snippet.Thumbnails.Maxres.Url,
						Videos = new List<Video>()
					};

					if ((!eclecticPlaylists.Contains(eclecticPlayList)) &&
						(snippet.Title.ToLowerInvariant().IndexOf("act") < 1))
					{
						eclecticPlaylists.Add(eclecticPlayList);
					}
				}
				catch (NullReferenceException) {}
			}

            eclecticPlaylists = eclecticPlaylists.OrderBy(x => x.Title).ToList();

            return eclecticPlaylists;
		}
	}
}
