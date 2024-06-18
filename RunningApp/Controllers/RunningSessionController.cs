using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunningApp.Data;
using RunningApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RunningApp.Controllers
{
    public class RunningSessionController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        public RunningSessionController(DataContext context, IHttpContextAccessor httpContextAccessor,UserManager<User> userManager )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] RunningSessionDTO model)
        {
            try
            {
                string userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(userEmail);
                byte[] imageData = null;

                if (model.Image != null)
                {
                    
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        imageData = memoryStream.ToArray();
                    }
                }

                var runningSession = new RunningSession
                {
                    Date = model.Date,
                    Kilometers = model.Kilometers,
                    Minutes = model.Time,
                    UserId = model.UserId,
                    Description = model.Description,
                    Image = imageData
                };

                _context.RunningSessions.Add(runningSession);
                await _context.SaveChangesAsync();
                return Ok("Running session created.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var runningSession = await _context.RunningSessions.FindAsync(id);
            if (runningSession == null)
            {
                return NotFound("Running session not found.");
            }
            return Ok(runningSession);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            string userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            var runningSessions = await _context.RunningSessions.ToListAsync();
            return Ok(runningSessions);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] RunningSessionDTO model)
        {
            var runningSession = await _context.RunningSessions.FindAsync(id);
            if (runningSession == null)
            {
                return NotFound("Running session not found.");
            }

            runningSession.Date = model.Date;
            runningSession.Kilometers = model.Kilometers;
            runningSession.Minutes = model.Time;
            runningSession.Description = model.Description;

            if (model.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.Image.CopyToAsync(memoryStream);
                    runningSession.Image = memoryStream.ToArray();
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Running session updated.");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var runningSession = await _context.RunningSessions.FindAsync(id);
            if (runningSession == null)
            {
                return NotFound("Running session not found.");
            }

            _context.RunningSessions.Remove(runningSession);
            await _context.SaveChangesAsync();
            return Ok("Running session deleted.");
        }

        [HttpDelete]
        [Route("deleteImage/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var runningSession = await _context.RunningSessions.FindAsync(id);
            if (runningSession == null)
            {
                return NotFound("Running session not found.");
            }

            runningSession.Image = null;
            await _context.SaveChangesAsync();
            return Ok("Image deleted from running session.");
        }
        
        [HttpGet]
        [Route("last")]
        public async Task<IActionResult> GetLast()
        {
            var runningSession = await _context.RunningSessions
                .OrderByDescending(rs => rs.Date)
                .FirstOrDefaultAsync();

            if (runningSession == null)
            {
                return NotFound("No running sessions found.");
            }

            return Ok(runningSession);
        }
    }
}
