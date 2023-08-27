using OTTMyPlatform.Models;

namespace OTTMyPlatform.Repository.Interface
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Tvshow>> GetTVshowByName(string showName);
        Task<bool> DeleteTVshowRecodsById(int shoeId);
        Task<bool> AddTVshow(Tvshow tvshow, int episodeTimeDuration,IFormCollection uploadeFile);
        Task<bool> UpdateTVshow(int showId, TvShowModel tvShowModel, IFormFileCollection updateFile);
       
    }
}
