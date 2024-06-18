using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunningApp.Data;
using RunningApp.Models;
using RunningApp.Repository.Interfaces;
using RunningApp.DTO_s;

namespace RunningApp.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        public AccountController(DataContext context, ITokenService tokenService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            try
            {
                var user = new User
                {
                    UserName = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return new BadRequestObjectResult(result.Errors);
                }

                var assignRoleResult = await _userManager.AddToRoleAsync(user, Roles.User);

                if (!assignRoleResult.Succeeded)
                {
                    return new BadRequestObjectResult(assignRoleResult.Errors);
                }

                return Ok("Registration successful.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return BadRequest(new { message = "Incorrect email or password." });
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "Incorrect email or password." });
                }

                var token = await _tokenService.GenerateJwtToken(user, TimeSpan.FromMinutes(600));

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        }
    }

