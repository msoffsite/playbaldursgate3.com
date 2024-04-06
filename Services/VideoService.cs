using EclecticXnet.Models;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Options;

using EclecticVideo = EclecticXnet.Models.Video;

namespace EclecticXnet.Services
{
	public class VideoService : IVideoService
	{
		private readonly IOptions<GoogleSettings> _googleSettings;

		public VideoService(IOptions<GoogleSettings> googleSettings)
		{
			_googleSettings = googleSettings;
		}

		public async Task<List<EclecticVideo>> GetVideosForPlayListId(string playListId)
		{
			var youtubeService = new YouTubeService(new Google.Apis.Services.BaseClientService.Initializer()
			{
				ApiKey = _googleSettings.Value.Key,
				ApplicationName = _googleSettings.Value.AppName
			});

			List<EclecticVideo> playListVideos = new List<EclecticVideo>();

			var nextPageToken = string.Empty;
			while (nextPageToken != null)
			{
				var requestPlayListVideos = youtubeService.PlaylistItems.List("snippet");
				requestPlayListVideos.PlaylistId = playListId;
				requestPlayListVideos.MaxResults = int.MaxValue;
				requestPlayListVideos.PageToken = nextPageToken;

				var responsePlayListVideos = requestPlayListVideos.Execute();
				foreach (var responsePlayListVideo in responsePlayListVideos.Items)
				{
					var videoSnippet = responsePlayListVideo.Snippet;

					try
					{
						var playListVideo = new EclecticVideo
						{
							Id = responsePlayListVideo.Id,
							Description = videoSnippet.Description,
							ThumbnailUrl = videoSnippet.Thumbnails.Maxres.Url,
							Title = responsePlayListVideo.Snippet.Title
						};

						if (!playListVideos.Contains(playListVideo))
						{
							playListVideos.Add(playListVideo);
						}
					}
					catch (NullReferenceException) { }
				}

				nextPageToken = responsePlayListVideos.NextPageToken;
			}

			return playListVideos;
		}
	}
}
