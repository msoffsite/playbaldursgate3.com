using EclecticXnet.Models;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Options;

using EclecticPlayList = EclecticXnet.Models.Playlist;
using EclecticVideo = EclecticXnet.Models.Video;

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
                var description = snippet.Description;

                if (description.IndexOf("{featured-playlist}") < 1)
                {
					continue;
                }

                description = description.Replace("{featured-playlist}", string.Empty);

                string title = snippet.Title.ToUpperInvariant();
                title = title.Replace("ECLECTIC X * ", string.Empty);
                title = title.Replace("ECLECTIC X", string.Empty);
                title = title.Trim();
				string htmlTitle = title.Replace("*", "<br />");

                string truncatedDescription = description;
                int lengthMax = 500;
                if (truncatedDescription.Length > lengthMax)
                {
                    truncatedDescription = truncatedDescription.Substring(0, lengthMax);
                    int lastSpace = truncatedDescription.LastIndexOf(" ");
                    truncatedDescription = truncatedDescription.Substring(0, lastSpace) + "...";
                }

                try
				{
					EclecticPlayList eclecticPlayList = new EclecticPlayList
					{
						Id = playListResult.Id,
						Title = title,
						HtmlTitle = htmlTitle,
						Description = description,
						DescriptionTruncated = truncatedDescription,
						ThumbnailUrl = snippet.Thumbnails.Maxres.Url,
						Videos = new List<EclecticVideo>()
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

        public async Task<EclecticPlayList> GetPlaylistById(string playListId)
        {
            var eclecticPlaylists = await GetPlaylists();
			EclecticPlayList eclecticPlaylist = eclecticPlaylists.FirstOrDefault(x => x.Id == playListId);
			return eclecticPlaylist;
        }
    }
}
