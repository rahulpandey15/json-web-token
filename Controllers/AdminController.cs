using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToAPI.Controllers
{
    
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class AdminController: Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                UserName="admin",
                Access="allowed"
            });
        }
    }
}