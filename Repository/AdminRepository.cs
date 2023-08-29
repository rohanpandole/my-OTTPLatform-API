using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OTTMyPlatform.Models;
using OTTMyPlatform.Repository.Interface;
using OTTMyPlatform.Repository.Interface.Context;
using System.Data;

namespace OTTMyPlatform.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDBContext _context;
        private readonly ITVShowImageProcessRepository _processTvShowImage;
        public AdminRepository(IConfiguration configuration, IDBContext dBContext, ITVShowImageProcessRepository tVShowImageProcess)
        {
            _configuration = configuration;
            _context = dBContext;
            _processTvShowImage = tVShowImageProcess;
        }

        public async Task<IEnumerable<Tvshow>> GetTVshowByName(string showName)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@TVShowName", showName, DbType.String);
            string searchTVShow = "SP_SearchTVShow";
            IEnumerable<Tvshow> aa = (IEnumerable<Tvshow>)await _context.DbConnection().QueryAsync(searchTVShow, param, commandType: CommandType.StoredProcedure);

            return aa;
        }
        public async Task<bool> DeleteTVshowRecodsById(int showId)
        {
            using (SqlConnection connection = (SqlConnection)_context.DbConnection())
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd1 = new SqlCommand("DELETE FROM UserShowWatchList WHERE ShowID= " + showId + ";", connection);
                    SqlCommand cmd2 = new SqlCommand("DELETE FROM Episode WHERE ShowID= " + showId + ";", connection);
                    SqlCommand cmd3 = new SqlCommand("DELETE FROM Tvshow WHERE ShowID= " + showId + ";", connection);

                    await cmd1.ExecuteNonQueryAsync();
                    await cmd2.ExecuteNonQueryAsync();
                    _processTvShowImage.RemoveTVShowImage(showId);
                    await cmd3.ExecuteNonQueryAsync();

                    return true;
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
        }
        public async Task<bool> AddTVshow(Tvshow tvshow, int episodeTimeDuration, IFormCollection uploadeFile)
        {
            try
            {
                using (var context = new OttplatformContext())
                {

                    await context.Tvshows.AddAsync(tvshow);
                    await context.SaveChangesAsync();

                    var newShowID = await context.Tvshows.Where(x => x.Title == tvshow.Title).FirstAsync();

                    Episode episode = new Episode()
                    {
                        EpisodeTimeDuration = episodeTimeDuration,
                        ShowId = newShowID.ShowId
                    };
                    await context.Episodes.AddAsync(episode);
                    await context.SaveChangesAsync();

                    return _processTvShowImage.UploadeFiles(uploadeFile.Files);
                }
            }

            catch (SqlException e)
            {
                throw e;
            }
        }
        public async Task<bool> UpdateTVshow(int showId, TvShowModel tvShowModel, IFormFileCollection updateFile)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = _configuration.GetConnectionString("DbConnection").ToString();
                    SqlCommand cmd = new SqlCommand("UPDATE [dbo].[TVShow] SET [Title] = '" + tvShowModel.Title + "',[Description] = '" + tvShowModel.Description + "' WHERE ShowID = " + showId + ";", connection);
                    connection.Open();
                    int result = await cmd.ExecuteNonQueryAsync();

                    var isImageUpdated = _processTvShowImage.UploadeFiles(updateFile);
                    return isImageUpdated;
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
        }
    }
}
