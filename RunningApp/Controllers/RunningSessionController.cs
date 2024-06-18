using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunningApp.Data;
using RunningApp.Models;

namespace RunningApp.Controllers
{

    public class RunningSessionController :BaseApiController
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public RunningSessionController(DataContext context, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

       
        [HttpPost]


        [HttpPost]
        [Authorize(Roles = "User")] // Ensure the user has the 'User' role
        public async Task<IActionResult> Create([FromForm] RunningSessionDTO model)
        {
            try
            {
                byte[] imageData = null;
                if (model.Image != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        imageData = memoryStream.ToArray();
                    }
                }

                // Extract UserId from authenticated user
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return Unauthorized("User is not authenticated.");
                }

                var runningSession = new RunningSession
                {
                    Date = model.Date,
                    Kilometers = model.Kilometers,
                    Minutes = model.Minutes,
                    Description = model.Description,
                    Image = imageData,
                    UserId = userId // Correctly set the UserId from the authenticated user
                };

                _context.RunningSessions.Add(runningSession);
                await _context.SaveChangesAsync();
                return Ok("Running session created.");
            }
            catch (Exception ex)
            {
                // Log the full exception
                var innerException = ex.InnerException?.ToString();
                return BadRequest($"Error: {ex.Message}, Inner Exception: {innerException}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var runningSession = await _context.RunningSessions.FindAsync(id);
            if (runningSession == null)
            {
                return NotFound("Running session not found.");
            }
            return Ok(runningSession);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var runningSessions = await _context.RunningSessions.ToListAsync();
            return Ok(runningSessions);
        }

        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] RunningSessionDTO model)
        {
            var runningSession = await _context.RunningSessions.FindAsync(id);
            if (runningSession == null)
            {
                return NotFound("Running session not found.");
            }

            runningSession.Date = model.Date;
            runningSession.Kilometers = model.Kilometers;
            runningSession.Minutes = model.Minutes;
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

        [HttpDelete("delete/{id:int}")]
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

        [HttpDelete("deleteImage/{id:int}")]
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

        [HttpGet("last")]
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

        [HttpGet("debug/claims")]
        public IActionResult GetClaims()
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims;
            var claimsDictionary = claims.ToDictionary(c => c.Type, c => c.Value);
            return Ok(claimsDictionary);
        }
    }
}
