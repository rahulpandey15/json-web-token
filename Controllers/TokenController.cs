using IntroductionToAPI.Models.Request;
using IntroductionToAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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


        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> Get([FromBody] TokenRequestDto request)
        {
            var accessToken
                = await tokenGeneratorService.GenerateToken(request.UserName, request.Password);

            if (accessToken is not null)
            {
                return Ok(accessToken);
            }
            return Unauthorized(""); // 401
        }

    }
}
