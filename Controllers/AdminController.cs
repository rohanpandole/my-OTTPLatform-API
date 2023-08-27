using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OTTMyPlatform.Models;
using OTTMyPlatform.Repository.Interface;
using OTTMyPlatform.Models.Responce;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpPut("UpdateTvShowById")]
        public async Task<ActionResult> UpdateTvShowById(int showId, TvShowModel tvShowModel)
        {
            //TODO: Analyse Usage Of Change TV Show Image As Admin Perspective
            var uploadeFile = Request.Form.Files;
            bool isTVShowUpdated = false;
            if (uploadeFile.Count == 0)
            {
                return BadRequest("Image not uploaded properly");
            }
            isTVShowUpdated = await _adminRepository.UpdateTVshow(showId,tvShowModel,uploadeFile);
            return Ok(isTVShowUpdated);
        }

        [HttpPost("AddTvShow")]
        public async Task<ActionResult> AddTvShow()
        {
            var uploadeFile = Request.Form;
            if (uploadeFile.Count == 0)
            {
                return BadRequest("Image not uploaded properly");
            }

            if (uploadeFile.Keys == null)
            {
                return BadRequest();
            }
          
            Tvshow tvshow = new Tvshow()
            {
                Description = uploadeFile["Description"],
                Title = uploadeFile["Title"],
                IsActive = "yes",
                tvShowImage = uploadeFile.Files[0].FileName
            };

            var isShowAdded = await _adminRepository.AddTVshow(tvshow, Int16.Parse(uploadeFile["episodes"].ToString()), uploadeFile);

            TVShowResponce response = new TVShowResponce();

            if(isShowAdded)
            {
                response.StatusMessage = "New record added successfully";
                response.StatusCode = 200;
                return Ok(response);
            }
            else
            {
                response.StatusMessage = "Failed to add new record";
                response.StatusCode = 500;
                return Ok(response);
            }
        }

        [HttpDelete("DeleteTvShowById")]
        public async Task<ActionResult> DeleteTvShowById(int showId)
        {
            if (showId <= 0)
            {
                return BadRequest();
            }
            
            if (await _adminRepository.DeleteTVshowRecodsById(showId))
            {
                return Ok("Record deleted successfully");
            }
            else
            {
                return BadRequest("failed to delete record");
            }
                
        }
    }
}

