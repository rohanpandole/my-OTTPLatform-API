using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTTMyPlatform.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OTTPlatformController : ControllerBase
    {

        [HttpGet("GetTvShow")]
        public IEnumerable<Tvshow> GetTvShow()
        {
            using (var context = new OttplatformContext())
            {
                return context.Tvshows.ToList();
            }
        }

        [HttpGet("{id}")]
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

        [HttpPut("{id}")]
        public async Task<ActionResult<Tvshow>> UpdateTvShowById(int id, Tvshow tvshow)
        {
            if(id != tvshow.ShowId)
            {
                return BadRequest();
            }
            using (var context = new OttplatformContext())
            {
                context.Entry(tvshow).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Tvshow>> SaveTvShow(Tvshow tvshow)
        {
            if (tvshow == null)
            {
                return BadRequest();
            }
            using (var context = new OttplatformContext())
            {

                context.Tvshows.AddAsync(tvshow);
                await context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<Tvshow>> DeleteTvShowById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            using (var context = new OttplatformContext())
            {
                var existingTvShow = await context.Tvshows.Where(a => a.ShowId == id).FirstAsync();

                if (existingTvShow != null) {
                    context.Entry(existingTvShow).State = EntityState.Deleted;

                    context.SaveChanges();
                }
            }
            return Ok();
        }
    }
}
