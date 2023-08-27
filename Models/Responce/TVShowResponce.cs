namespace OTTMyPlatform.Models.Responce
{
    public class TVShowResponce
    {
        public List<Tvshow> Tvshows { get; set; }
        public TVShowResponce()
        {
            Tvshows = new List<Tvshow>();
        }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
    }
}
