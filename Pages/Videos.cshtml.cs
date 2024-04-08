using System.Text;
using EclecticXnet.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

using EclecticPlaylist = EclecticXnet.Models.Playlist;
using EclecticVideo = EclecticXnet.Models.Video;

namespace EclecticXnet.Pages
{
    public class VideosModel : PageModel
    {

        private readonly ILogger<IndexModel> _logger;
        private readonly IPlayListService _playListService;
        private readonly IVideoService _videoService;

        public EclecticPlaylist PlayList { get; set; }
        public List<EclecticVideo> Videos { get; set; }

        public string HtmlForOtherPlayListLinks;

        public string PlayListId { get; set; } = null!;

        public VideosModel(ILogger<IndexModel> logger, IPlayListService playListService, IVideoService videoService)
        {
            _logger = logger;
            _playListService = playListService;
            _videoService = videoService;
        }

        public void OnGet(string playListId)
        {
            List<EclecticPlaylist> playLists = _playListService.GetPlayLists().Result;
            
            StringBuilder buildHtmlForOtherPlayListLinks = new();

            buildHtmlForOtherPlayListLinks.Append("<div class=\"list-group\">");

            foreach (EclecticPlaylist playlist in playLists)
            {
                if (playlist.Id.Equals(playListId, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                var playListLink = $"<a class=\"list-group-item list-group-item-action\" href=\"/Videos/{playlist.Id}\" title=\"{playlist.DescriptionTruncated}\">{playlist.Title}</a>";
                buildHtmlForOtherPlayListLinks.Append(playListLink);
            }

            buildHtmlForOtherPlayListLinks.Append("</div>");
            
            HtmlForOtherPlayListLinks = buildHtmlForOtherPlayListLinks.ToString();
            
            PlayList = playLists.First(x => x.Id == playListId);
            List<EclecticVideo> playListVideos = _videoService.GetVideosForPlayListId(playListId).Result;
            Videos = playListVideos;
        }
    }
}
