using IntroductionToAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly ITokenGeneratorService tokenGeneratorService;

        public TokenController(ITokenGeneratorService tokenGeneratorService)
        {
            this.tokenGeneratorService = tokenGeneratorService;
        }


        [HttpGet]
        [Route("generate/{userName}/{password}")]
        public IActionResult Get(string userName, string password)
        {
            var accessToken 
                = tokenGeneratorService.GenerateToken(userName, password);

            if (accessToken is not null)
            {
                return Ok(accessToken);
            }
            return Unauthorized(""); // 401
        }

    }
}
