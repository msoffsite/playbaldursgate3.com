using EclecticXnet.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc.RazorPages;

using EclecticPlayList = EclecticXnet.Models.Playlist;

namespace EclecticXnet.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IPlayListService _playListService;

		public List<EclecticPlayList> PlayLists { get; set; }


		public IndexModel(ILogger<IndexModel> logger, IPlayListService playListService)
		{
			_logger = logger;
			_playListService = playListService;
		}

		public void OnGet()
		{
			List<EclecticPlayList> playListServiceResult = _playListService.GetPlaylists().Result;
			PlayLists = playListServiceResult;
		}
	}
}
