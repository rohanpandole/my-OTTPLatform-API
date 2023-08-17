namespace OTTMyPlatform.Models
{
    public class TvShowModel
    {
        //public int ShowId { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public List<EpisodModel> episodes { get; set; }

        // add episode collection
    }
}
