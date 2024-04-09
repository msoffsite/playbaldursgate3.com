using PlayBaldursGate3.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

using EclecticPlayList = PlayBaldursGate3.Models.Playlist;

namespace PlayBaldursGate3.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IPlayListService _playListService;

		public List<EclecticPlayList> PlayLists { get; set; } = new List<EclecticPlayList>();


		public IndexModel(ILogger<IndexModel> logger, IPlayListService playListService)
		{
			_logger = logger;
			_playListService = playListService;
		}

		public void OnGet()
		{
			List<EclecticPlayList> playListServiceResult = _playListService.GetPlayLists().Result;
			PlayLists = playListServiceResult;
		}
	}
}
