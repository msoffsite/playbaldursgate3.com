using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EclecticPlayList = EclecticXnet.Models.Playlist;
using EclecticVideo = EclecticXnet.Models.Video;

namespace EclecticXnet.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

			var apiKey = "AIzaSyCTQ_Mh9vqFtUkCAGQpfXOme3lHMpDJKyY";
			var apiAppName = "youtube-eclecticx-net-api-key";

			var youtubeService = new YouTubeService(new Google.Apis.Services.BaseClientService.Initializer()
				{
					ApiKey = apiKey,
					ApplicationName = apiAppName
				});

			var channelsListRequest = youtubeService.Channels.List("contentDetails");
			channelsListRequest.Id = "UCoSGwhk4Ql18r4WNb-p5JLQ";

			List<EclecticPlayList> eclecticPlaylists = new List<EclecticPlayList>();

			var channelsListResponse = channelsListRequest.Execute();
			foreach (var channel in channelsListResponse.Items)
			{	
				
				var requestPlaylists = youtubeService.Playlists.List("snippet");
				requestPlaylists.MaxResults = int.MaxValue;
				requestPlaylists.ChannelId = "UCoSGwhk4Ql18r4WNb-p5JLQ";
				var responsePlaylists = requestPlaylists.Execute();
				foreach (var playListResult in responsePlaylists.Items)
				{
					var playListId = playListResult.Id;

					List<EclecticVideo> playListVideos = new List<EclecticVideo>();

					var nextPageToken = "";
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

							if (videoSnippet.Thumbnails.Maxres != null)
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
						}

						nextPageToken = responsePlayListVideos.NextPageToken;
					}

					EclecticPlayList eclecticPlayList = new EclecticPlayList
					{
						Id = playListId,
						Title = playListResult.Snippet.Title,
						Description = playListResult.Snippet.Description,
						ThumbnailUrl = playListResult.Snippet.Thumbnails.Maxres.Url,
						Videos = playListVideos
					};

					if (!eclecticPlaylists.Contains(eclecticPlayList))
					{
						eclecticPlaylists.Add(eclecticPlayList);
					}
				}
			}
		}
	}
}
