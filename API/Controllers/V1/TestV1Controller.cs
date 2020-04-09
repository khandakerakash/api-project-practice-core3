using System.Threading.Tasks;
using BLL.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/test")]
    public class TestV1Controller : RootController
    {
        private readonly ITestService _testService;

        public TestV1Controller(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _testService.SaveTestData();
            return Ok("Hello, I'm from the test controller.");
        }
    }
}