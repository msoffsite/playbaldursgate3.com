using EclecticXnet.Models;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Options;

using EclecticPlayList = EclecticXnet.Models.Playlist;
using EclecticVideo = EclecticXnet.Models.Video;
using Playlist = Google.Apis.YouTube.v3.Data.Playlist;

namespace EclecticXnet.Services
{
	public class PlayListService : IPlayListService
	{
		private readonly IOptions<GoogleSettings> _googleSettings;

		public PlayListService(IOptions<GoogleSettings> googleSettings)
		{
			_googleSettings = googleSettings;
		}

		public async Task<List<EclecticPlayList>> GetPlayLists()
		{
			var youtubeService = new YouTubeService(new Google.Apis.Services.BaseClientService.Initializer()
			{
				ApiKey = _googleSettings.Value.Key,
				ApplicationName = _googleSettings.Value.AppName
			});

			ChannelsResource.ListRequest? channelsListRequest = youtubeService.Channels.List("contentDetails");
			channelsListRequest.Id = _googleSettings.Value.YouTubeChannelId;

			List<EclecticPlayList> eclecticPlayLists = new();

			PlaylistsResource.ListRequest? requestPlayLists = youtubeService.Playlists.List("snippet");
			requestPlayLists.MaxResults = int.MaxValue;
			requestPlayLists.ChannelId = _googleSettings.Value.YouTubeChannelId;
			
            PlaylistListResponse? responsePlayLists = await requestPlayLists.ExecuteAsync();
			foreach (Playlist? playListResult in responsePlayLists.Items)
			{
				PlaylistSnippet? snippet = playListResult.Snippet;
                string? description = snippet.Description;

                if (description.IndexOf("{featured-playlist}", StringComparison.Ordinal) < 1)
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
                var lengthMax = 500;
                if (truncatedDescription.Length > lengthMax)
                {
                    truncatedDescription = truncatedDescription[..lengthMax];
                    int lastSpace = truncatedDescription.LastIndexOf(" ", StringComparison.Ordinal);
                    truncatedDescription = truncatedDescription[..lastSpace] + "...";
                }

                string metaDescription = truncatedDescription;
                metaDescription = metaDescription.Replace(Environment.NewLine, " ",
                    StringComparison.InvariantCultureIgnoreCase);
                lengthMax = 198;
                if (metaDescription.Length > lengthMax)
                {
					metaDescription = metaDescription[..lengthMax];
                    int lastSpace = metaDescription.LastIndexOf(" ", StringComparison.Ordinal);
                    metaDescription = metaDescription[..lastSpace];
                }

                try
				{
					EclecticPlayList eclecticPlayList = new()
                    {
						Id = playListResult.Id,
						Title = title.Trim(),
						HtmlTitle = htmlTitle.Trim(),
						Description = description.Trim(),
						MetaDescription = metaDescription.Trim(),
						DescriptionTruncated = truncatedDescription.Trim(),
						ThumbnailUrl = snippet.Thumbnails.Maxres.Url,
						Videos = new()
					};

					if (!eclecticPlayLists.Contains(eclecticPlayList) &&
						snippet.Title.ToLowerInvariant().IndexOf("act", StringComparison.Ordinal) < 1)
					{
						eclecticPlayLists.Add(eclecticPlayList);
					}
				}
				catch (NullReferenceException) {}
			}

            eclecticPlayLists = eclecticPlayLists.OrderBy(x => x.Title).ToList();
            
            return eclecticPlayLists;
		}
    }
}
