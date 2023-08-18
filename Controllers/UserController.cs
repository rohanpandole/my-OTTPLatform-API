using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OTTMyPlatform.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        public UserController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;

        }

        [HttpGet("GetAllTvShow")]
        public IEnumerable<Tvshow> GetAllTvShow()
        {
            using (var context = new OttplatformContext())
            {
                var data = context.Tvshows.ToList();
                if(data!=null && data.Count>0)
                {
                    foreach (var item in data)
                    {
                        item.tvShowImage = GetTVShowImage(item.tvShowImage);
                    }
                }
                else
                {
                    return Enumerable.Empty<Tvshow>();
                }
                return data;
            }
        }

        [HttpGet("GetTvShowByName")]
        public async Task<ActionResult<Tvshow>> GetTvShowById(string tvShowName)
        {
            Tvshow tvshow = new Tvshow();
            using (var context = new OttplatformContext())
            {
                var getDataByID = await context.Tvshows.Where(x => x.Title == tvShowName).FirstAsync();
                if (getDataByID != null)
                {
                    tvshow.Description = getDataByID.Description;
                    tvshow.ShowId = getDataByID.ShowId;
                    tvshow.Title = getDataByID.Title;
                }
            }
            return tvshow;
        }


        [HttpGet("GetMyAllWatchedEpisods")]
        public List<UserShowWatchList> GetMyAllWatchedEpisods(int userId)
        {
            using (var context = new OttplatformContext())
            {
                var watchList = context.UserShowWatchLists.Where(x => x.UserId == userId).ToList();
                return watchList;
            }
        }

        [HttpGet("GetNextEpisodByTvShowId")]
        public async Task<ActionResult> GetNextEpisodByTvShowId(int showID, int userID)
        {
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

        [NonAction]
        private string GetImageFolderPath()
        {

            return this._environment.WebRootPath + "/Uploads/TVShowImages/";
        }

        [NonAction]
        private string GetTVShowImage(string tvShowImage)
        {
            string imageUrl = string.Empty;
            string hostUrl = "https://localhost:7005//";

            string imaheFolderPath = GetImageFolderPath();
            string imagePath = imaheFolderPath + tvShowImage;

            if (!System.IO.File.Exists(imagePath))
            {
                imageUrl = hostUrl + "//Uploads//TVShowCommanImage/defaultImage.jpg";
            }
            else
            {
                imageUrl = hostUrl + "/Uploads/TVShowImages/" + tvShowImage;
            }

            return imageUrl;

        }

    }
}
