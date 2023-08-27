using Microsoft.AspNetCore.Mvc;
using OTTMyPlatform.Models;

namespace OTTMyPlatform.Repository.Interface
{
    public interface IUserRepository
    {
        List<Tvshow> SerachTVshowByName(string showName);
        List<Tvshow> GetAllTvShow();
        List<Tvshow> GetMyAllWatchedEpisods(int userId);
        Task<bool> MarkTvShowById(Watched watched);
    }
}
