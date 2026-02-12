using IntroductionToAPI.Filters;
using IntroductionToAPI.Models.Request;
using IntroductionToAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController(ITokenGeneratorService tokenGeneratorService) : Controller
    {
        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> Generate([FromBody] TokenRequestDto request)
        {
            var accessToken
                = await tokenGeneratorService.GenerateToken(request.UserName, request.Password);

            return accessToken != null ? Ok(accessToken) : Unauthorized("");
        }
        
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var response
                 = await tokenGeneratorService.GenerateRefreshTokenAsync(request);

            return Ok(response);
        }

    }
}
