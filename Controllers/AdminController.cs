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
        private readonly IWebHostEnvironment _environment;
        public AdminController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _environment = webHostEnvironment;
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
                    tvshow.tvShowImage = getDataByID.tvShowImage;
                }
            }
            return tvshow;
        }

        [HttpGet("GetTvShowByName")]
        public async Task<ActionResult<Tvshow>> GetTvShowByName(string tvShowName)
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
                    connection.ConnectionString = _configuration.GetConnectionString("DbConnection").ToString();
                    connection.Open();

                    SqlCommand cmd1 = new SqlCommand("DELETE FROM UserShowWatchList WHERE ShowID= " + showId + ";", connection);
                    SqlCommand cmd2 = new SqlCommand("DELETE FROM Episode WHERE ShowID= " + showId + ";", connection);

                    string imagePath = "";
                    string imageFolderPath = GetImageFolderPath();

                    using (var context = new OttplatformContext())
                    {
                        var getData = await context.Tvshows.FindAsync(showId);
                        imagePath = imageFolderPath + getData.tvShowImage;
                        
                        if (System.IO.File.Exists(imagePath))
                        {
                            //if old image is there then it will delete it
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    SqlCommand cmd3 = new SqlCommand("DELETE FROM Tvshow WHERE ShowID= " + showId + ";", connection);

                    await cmd1.ExecuteNonQueryAsync();
                    await cmd2.ExecuteNonQueryAsync();
                    await cmd3.ExecuteNonQueryAsync();

                    return Ok("Record deleted successfuly");
                }
                catch (SqlException e)
                {
                    throw e;
                }
            }
        }

        [AllowAnonymous]
        [HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage(List<IFormFile> files)
        //public async Task<ActionResult> UploadImage()
        {
            bool result = false;
            try
            {
                //var uploadeFile = Request.Form.Files;
                var uploadeFile = files;

                foreach (IFormFile source in uploadeFile) {

                    string fileName = source.FileName;
                    //string fileFolderPath = GetImageFolderPath(fileName);
                    string fileFolderPath = GetImageFolderPath();

                    if (!System.IO.Directory.Exists(fileFolderPath))
                    {
                        System.IO.Directory.CreateDirectory(fileFolderPath);
                    }

                    string imagePath = fileFolderPath + fileName;

                    if (System.IO.File.Exists(imagePath))
                    { 
                        //if old image is there then it will delete it
                        System.IO.File.Delete(imagePath);
                    }

                    using(FileStream stream = System.IO.File.Create(imagePath))
                    { 
                        // it will uploade actual binary data into dummy file
                        await source.CopyToAsync(stream);
                        result = true;

                        // TODO: add code to save file name in tvshow table, later on that table column value will retrived from get TVshow api as image URL(with custome logic)
                    }
                }

            }catch (Exception e) {
                throw;
            }

            return Ok(result);

        }

        [NonAction]
        private string GetImageFolderPath()
        {

            return this._environment.WebRootPath + "/Uploads/TVShowImages/";
        }

    }

}

