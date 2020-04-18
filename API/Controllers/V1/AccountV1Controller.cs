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

        [HttpPost(ApiRoutes.Account.Logout)]
        [Authorize(Roles = "staff, teacher", Policy = "AtToken")]
        public async Task<IActionResult> Logout()
        {
            var cp = User;
            return Ok(await _accountService.Logout(cp));
        }

        [HttpGet("test1")]
        public IActionResult Test1()
        {
            return Ok("I'm from Test1");
        }
        
        [HttpGet("test2")]
        [Authorize(Roles = "staff")]
        public IActionResult Test2()
        {
            var cp = User;
            
            _accountService.Test(cp);
            return Ok("I'm from Test2");
        }
        
        [HttpGet("test3")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Test3()
        {
            var cp = User;
            
            await _accountService.Test(cp);
            return Ok("I'm from Test3");
        }
    }
}