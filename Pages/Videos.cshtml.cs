using EclecticXnet.Services;
using Microsoft.AspNetCore.Mvc;
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

        public EclecticPlaylist PlayList { get; set; } = null;
        public List<EclecticVideo> Videos { get; set; } = new List<EclecticVideo>();

        public string PlayListId { get; set; }

        public VideosModel(ILogger<IndexModel> logger, IPlayListService playListService, IVideoService videoService)
        {
            _logger = logger;
            _playListService = playListService;
            _videoService = videoService;
        }

        public void OnGet(string playListId)
        {
            PlayList = _playListService.GetPlaylistById(playListId).Result;
            List<EclecticVideo> playListVideos = _videoService.GetVideosForPlayListId(playListId).Result;
            Videos = playListVideos;
        }
    }
}
