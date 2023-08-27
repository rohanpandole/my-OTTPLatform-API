using Dapper;
using Microsoft.EntityFrameworkCore;
using OTTMyPlatform.Models;
using OTTMyPlatform.Repository.Interface;
using OTTMyPlatform.Repository.Interface.Context;
using System.Data;

namespace OTTMyPlatform.Repository.InterfaceImplementation
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDBContext _context;
        private readonly ITVShowImageProcess _processTvShowImage;
        public UserRepository(IConfiguration configuration, IDBContext dBContext, ITVShowImageProcess tVShowImageProcess)
        {
            _configuration = configuration;
            _context = dBContext;
            _processTvShowImage = tVShowImageProcess;
        }
        public List<Tvshow> SerachTVshowByName(string tvShowName)
        {
            List<Tvshow>tvshows = new List<Tvshow>();
            Tvshow tvshow = null;
            DynamicParameters param = new DynamicParameters();

            param.Add("@TVShowName", tvShowName, DbType.String);
            string sp_searchTVShow = "SP_SearchTVShow";
            var searchData = _context.DbConnection().Query<Tvshow>(sp_searchTVShow, param, commandType: CommandType.StoredProcedure).ToList();
            if (searchData != null)
            {
                foreach (var item in searchData)
                {
                    tvshow = new Tvshow();
                    tvshow.Title = item.Title;
                    tvshow.Description = item.Description;
                    tvshow.tvShowImage = _processTvShowImage.GetTVShowImage(item.tvShowImage);
                    tvshows.Add(tvshow);
                }
            }
            return tvshows;
        }


        public List<Tvshow> GetAllTvShow()
        {
            using (var context = new OttplatformContext())
            {
                var data = context.Tvshows.ToList();
                if (data != null && data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        item.tvShowImage = _processTvShowImage.GetTVShowImage(item.tvShowImage);
                    }
                }
                else
                {
                    return data;
                }
                return data;
            }
        }

        public List<Tvshow> GetMyAllWatchedEpisods(int userId)
        {
            List<Tvshow> Tvshows = new List<Tvshow>();
            Tvshow Tvshow = new Tvshow();
            using (var context = new OttplatformContext())
            {
                var watchList = context.UserShowWatchLists.Where(x => x.UserId == userId).ToList();
                foreach (var data in watchList)
                {
                    var showData = context.Tvshows.Where(x => x.ShowId == data.ShowId).SingleOrDefault();
                    showData.tvShowImage = _processTvShowImage.GetTVShowImage(showData.tvShowImage);
                    Tvshows.Add(showData);
                }

                return Tvshows;
            }
        }
        public async Task<bool> MarkTvShowById(Watched watched)
        {
            UserShowWatchList watchlist = new UserShowWatchList();
            using (var context = new OttplatformContext())
            {
                var getDataByID = await context.Episodes.Where(x => x.ShowId == watched.showId).FirstAsync();
                if (getDataByID != null)
                {
                    watchlist.ShowId = getDataByID.ShowId;
                    watchlist.EpisodeId = getDataByID.EpisodeId;
                    watchlist.UserId = watched.UserID;
                    watchlist.Watched = 1;
                }
                try
                {
                    await context.UserShowWatchLists.AddAsync(watchlist);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    throw;
                }
            }

        }
        public Task<bool> AddTVshow(Tvshow tvshow, int episodeTimeDuration, IFormCollection uploadeFile)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTVshowRecodsById(int shoeId)
        {
            throw new NotImplementedException();
        }
    }

}
