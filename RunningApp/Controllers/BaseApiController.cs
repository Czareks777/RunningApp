using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace RunningApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAll")]
    public class BaseApiController:ControllerBase
    {
    }
}
