namespace OTTMyPlatform.Models
{
    public class TvShowModel
    { 

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public List<EpisodModel> episodes { get; set; }
        
    }
}
