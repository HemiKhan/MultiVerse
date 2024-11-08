using Data.DataAccess;
using Data.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AccountServices;
using Services.NFTServices;

namespace MultiVerse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFTController : ControllerBase
    {
        private readonly INFTService srv;

        public NFTController(INFTService srv)
        {
            this.srv = srv;
        }

        [HttpPost(nameof(test))]
        [AllowAnonymous]
        public async Task<IActionResult> test()
        {
            var result = "await accountServices.LoginAsync(req, accountServices.GenerateToken, cancellationToken);";
            string ResponseJson = Globals.GetResponseJson(result);
            return Globals.GetContentResult(ResponseJson, 200);
        }
    }
}
