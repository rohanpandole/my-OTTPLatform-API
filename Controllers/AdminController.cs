using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OTTMyPlatform.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("employees")]
        public IEnumerable<string> Get()
        {
            return new List<string> { "rohan", "Kishor", "Pandole" };
        }

        [AllowAnonymous]
        [HttpPost("postemployees")]
        public IEnumerable<string> PostMy()
        {
            return new List<string> { "rohan", "Kishor", "Pandole" };
        }


        [HttpGet("GetAllTvShow")]
        public IEnumerable<Tvshow> GetAllTvShow()
        {
            using (var context = new OttplatformContext())
            {
                return context.Tvshows.ToList();
            }
        }

        [HttpGet("GetTvShowById")]
        public async Task<ActionResult<Tvshow>> GetTvShowById(int id)
        {
            Tvshow tvshow = new Tvshow();
            using (var context = new OttplatformContext())
            {
                var getDataByID = await context.Tvshows.FindAsync(id);
                if (getDataByID != null)
                {
                    tvshow.Description = getDataByID.Description;
                    tvshow.ShowId = getDataByID.ShowId;
                    tvshow.Title = getDataByID.Title;
                }
            }
            return tvshow;
        }

        [HttpGet("GetTvShowByName")]
        public async Task<ActionResult<Tvshow>> GetTvShowById(string tvShowName)
        {
            Tvshow tvshow = new Tvshow();
            using (var context = new OttplatformContext())
            {
                var getDataByID = await context.Tvshows.Where(x=> x.Title == tvShowName).FirstAsync();
                if (getDataByID != null)
                {
                    tvshow.Description = getDataByID.Description;
                    tvshow.ShowId = getDataByID.ShowId;
                    tvshow.Title = getDataByID.Title;
                }
            }
            return tvshow;
        }

        [HttpPut("UpdateTvShowById")]
        public async Task<ActionResult> UpdateTvShowById(int showId, TvShowModel tvShowModel)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    //SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString());
                    connection.ConnectionString = _configuration.GetConnectionString("DbConnection").ToString();
                    SqlCommand cmd = new SqlCommand("UPDATE [dbo].[TVShow] SET [Title] = '"+tvShowModel.Title+"',[Description] = '" + tvShowModel.Description + "' WHERE ShowID = " + showId + ";", connection);
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

        [HttpPost("AddTvShow")]
        public async Task<ActionResult> AddTvShow(TvShowModel tvShowModel)
        {
            if (tvShowModel == null)
            {
                return BadRequest();
            }
            Tvshow tvshow = new Tvshow()
            {
                Description = tvShowModel.Description,
                Title = tvShowModel.Title
            };
            using (var context = new OttplatformContext())
            {

                await context.Tvshows.AddAsync(tvshow);
                await context.SaveChangesAsync();

                var newShowID = await context.Tvshows.Where(x=> x.Title == tvShowModel.Title).FirstAsync();

                foreach ( var a in tvShowModel.episodes)
                {
                    Episode episode = new Episode()
                    {
                        EpisodeTimeDuration = a.EpisodeTimeDuration,
                        ShowId = newShowID.ShowId
                    };
                    await context.Episodes.AddAsync(episode);
                    await context.SaveChangesAsync();

                }
            }
            return CreatedAtAction("GetTvShowById", new { id = tvshow.ShowId }, tvshow);
        }

        [HttpDelete("DeleteTvShowById")]
        public async Task<ActionResult> DeleteTvShowById(int showId)
        {
            if (showId <= 0)
            {
                return BadRequest();
            }
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    //SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DbConnection").ToString());
                    connection.ConnectionString = _configuration.GetConnectionString("DbConnection").ToString();
                    connection.Open();

                    SqlCommand cmd1 = new SqlCommand("DELETE FROM UserShowWatchList WHERE ShowID= " + showId + ";", connection);
                    SqlCommand cmd2 = new SqlCommand("DELETE FROM Episode WHERE ShowID= " + showId + ";", connection);
                    SqlCommand cmd3 = new SqlCommand("DELETE FROM Tvshow WHERE ShowID= " + showId + ";", connection);

                    await cmd1.ExecuteNonQueryAsync();
                    await cmd2.ExecuteNonQueryAsync();
                    await cmd3.ExecuteNonQueryAsync();
                    return Ok();
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
        }
        }

}

