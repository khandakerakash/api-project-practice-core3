using BLL.Service;
using Microsoft.AspNetCore.Mvc;
using Utility.Helpers;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/test")]
    public class TestV1Controller : RootController
    {
        private readonly ITestService _testService;
        private readonly TaposRSA _taposRsa;

        public TestV1Controller(ITestService testService, TaposRSA taposRsa)
        {
            _testService = testService;
            _taposRsa = taposRsa;
        }

        // [HttpGet]
        // public async Task<IActionResult> Index()
        // {
        //     await _testService.SaveTestData();
        //     return Ok("Hello, I'm from the test controller.");
        // }

        [HttpGet]
        public IActionResult Index()
        {
            _taposRsa.GenerateRsaKey("v1");
            return Ok("Hello, I'm RSA Key Tester.");
        }
    }
}