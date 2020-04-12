using System.Threading.Tasks;
using BLL.Request;
using BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility.Routes;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/accounts")]
    public class AccountV1Controller : RootController
    {
        private readonly IAccountService _accountService;

        public AccountV1Controller(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost(ApiRoutes.Account.Login)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            return Ok(await _accountService.Login(request));
        }

        [HttpGet("test1")]
        public IActionResult Test1()
        {
            return Ok("I'm from Test1");
        }
        
        [HttpGet("test2")]
        [Authorize]
        public IActionResult Test2()
        {
            return Ok("I'm from Test2");
        }
    }
}