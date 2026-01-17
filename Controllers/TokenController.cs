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
        public IActionResult Get(string userName,string password)
        {
            // this should be validated against database
            if(userName == "user@123" || password == "password@123")
            {
                var accessToken = tokenGeneratorService.GenerateToken(userName, password);

                return Ok(accessToken); 

            }

            return Unauthorized(""); // 401
        }

    }
}
