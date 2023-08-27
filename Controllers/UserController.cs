using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OTTMyPlatform.Models;
using OTTMyPlatform.Models.Responce;
using OTTMyPlatform.Repository.Interface;
using OTTMyPlatform.Repository.Interface.Context;
using System.Data;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IDBContext _dBContext;
        private readonly IUserRepository _userRepository;
        public UserController(IConfiguration configuration, IWebHostEnvironment environment, IDBContext dBContext,IUserRepository userRepository)
        {
            _configuration = configuration;
            _environment = environment;
            _dBContext = dBContext;
            _userRepository = userRepository;
        }

        [HttpGet("GetAllTvShow")]
        public ActionResult<TVShowResponce> GetAllTvShow()
        {
            TVShowResponce responce = new TVShowResponce();
            responce.Tvshows = _userRepository.GetAllTvShow();
            if (responce.Tvshows.Count < 0)
            {
                responce.StatusCode = 500;
                responce.StatusMessage = "No TV show Available";
                return BadRequest(responce);
            }
            responce.StatusCode = 200;
            responce.StatusMessage = "Found TV Shows";
            return Ok(responce);

        }

        [HttpGet("SearchTvShowByName")]
        public ActionResult<TVShowResponce> SearchTvShowByName(string tvShowName)
        {
            TVShowResponce responce = new TVShowResponce();
            if (tvShowName.Length == 0 )
            {
                responce.StatusCode = 500;
                responce.StatusMessage = "Enter TV show name";
                return BadRequest(responce);
            }
            responce.Tvshows = _userRepository.SerachTVshowByName(tvShowName);
            responce.StatusCode = 200;
            responce.StatusMessage = "Found TV Show Search Result";
            return Ok(responce);
        }

        [HttpPost("MarkTvShowById")]
        public async Task<ActionResult> MarkTvShowById(Watched watched)
        {
            var IsWatched = await _userRepository.MarkTvShowById(watched);
            var responce = new { IsWatched = IsWatched };

            if (IsWatched)
            {
                return Ok(responce);
            }
            return BadRequest(responce);
        }

        [HttpGet("GetMyAllWatchedEpisods")]
        public ActionResult<TVShowResponce> GetMyAllWatchedEpisods(int userId)
        {
            TVShowResponce responce = new TVShowResponce();
            responce.Tvshows = _userRepository.GetMyAllWatchedEpisods(userId);
            if (responce.Tvshows.Count < 0)
            {
                responce.StatusCode = 500;
                responce.StatusMessage = "No TV show Watched Yet";
                return BadRequest(responce);
            }
            responce.StatusCode = 200;
            responce.StatusMessage = "Found My Watched TV Shows";
            return Ok(responce);

        }

        [HttpGet("GetNextEpisodByTvShowId")]
        public async Task<ActionResult> GetNextEpisodByTvShowId(int showID, int userID)
        {
            // TODO: Analyse Usage Of This API Then Consume In UI

            if (showID == 0 || 0 == userID)
            {
                return BadRequest();
            }
            Episode episode = new Episode();
            using (var context = new OttplatformContext())
            {
                var getDataByID = await context.UserShowWatchLists.Where(x => x.UserId == userID && x.ShowId == showID).FirstAsync();

                if (getDataByID != null)
                {
                    using (SqlConnection connection = new SqlConnection())
                    {
                        try
                        {
                            connection.ConnectionString = _configuration.GetConnectionString("DbConnection").ToString();
                            SqlCommand cmd = new SqlCommand("select top 1 usl.EpisodeID+1 as nextEpisode  from UserShowWatchList usl join Episode e on e.EpisodeID = usl.EpisodeID " +
                                "where usl.UserId = " + userID + " and usl.ShowID = " + showID + " order by usl.EpisodeID desc;", connection);
                            connection.Open();
                            //var resultData = await cmd.ExecuteNonQueryAsync();
                            SqlDataReader reader = cmd.ExecuteReader();
                            int nextEpisodeResult = 0;
                            var aa = reader.Read();
                            while (aa)
                            {
                                nextEpisodeResult = short.Parse(reader["nextEpisode"].ToString());
                                break;
                            }

                            var episodeResultData = await context.Episodes.Where(x => x.EpisodeId == nextEpisodeResult && x.ShowId == showID).AnyAsync();

                            if (episodeResultData)
                            {                              
                                var episodeDetails = await context.Episodes.FindAsync(nextEpisodeResult);
                                return Ok(episodeDetails);
                            }
                            else
                            {
                                return NotFound("It was last episod");
                            }
                        }
                        catch (SqlException e)
                        {
                            throw e;
                        }
                    }
                }
            }
            return Ok();
        }

        [HttpPut("UpdateMarkCurrentEpisodCompleted")]
        public async Task<ActionResult<Tvshow>> UpdateTvShowById(int showID, int episodID, int userID)
        {
            // TODO: Analyse Usage Of This API Then Remove In Future

            if (showID == 0 || 0 == episodID)
            {
                return BadRequest();
            }
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = _configuration.GetConnectionString("DbConnection").ToString();
                    SqlCommand cmd = new SqlCommand("UPDATE [dbo].[UserShowWatchList] SET Watched = 1 WHERE ShowID = " + showID + " AND EpisodeID = " + episodID + " AND UserId = " + userID + ";", connection);
                    connection.Open();
                    int result = await cmd.ExecuteNonQueryAsync();
                    return Ok(result);
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
        }
    }
}