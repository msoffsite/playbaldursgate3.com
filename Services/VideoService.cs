using EclecticXnet.Models;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
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

			var playListVideos = new List<EclecticVideo>();

			var nextPageToken = string.Empty;
			while (nextPageToken != null)
			{
				PlaylistItemsResource.ListRequest? requestPlayListVideos = youtubeService.PlaylistItems.List("snippet");
				requestPlayListVideos.PlaylistId = playListId;
				requestPlayListVideos.MaxResults = int.MaxValue;
				requestPlayListVideos.PageToken = nextPageToken;

				PlaylistItemListResponse? responsePlayListVideos = await requestPlayListVideos.ExecuteAsync();
				
                foreach (PlaylistItem? responsePlayListVideo in responsePlayListVideos.Items)
				{
					PlaylistItemSnippet? videoSnippet = responsePlayListVideo.Snippet;

                    if (videoSnippet.Title.Equals("private video", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
					
                    try
					{
						var playListVideo = new EclecticVideo
						{
							Id = videoSnippet.ResourceId.VideoId,
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
