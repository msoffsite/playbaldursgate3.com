namespace PlayBaldursGate3.Models
{
	public class Playlist : Base
    {
        public string MetaDescription { get; set; } = string.Empty;
        public string DescriptionTruncated { get; set; } = string.Empty;
        public string HtmlTitle { get; set; } = string.Empty;
		public List<Video> Videos { get; set; } = new List<Video>();
	}
}
