using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace RunningApp.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("RunningApp")]
    public class BaseApiController:ControllerBase
    {
    }
}
