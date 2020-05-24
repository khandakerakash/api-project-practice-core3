using System.Threading.Tasks;
using BLL.Service;
using DLL.MongoReport.Model;
using DLL.MongoReport.Repository;
using Microsoft.AspNetCore.Mvc;
using Utility.Helpers;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/test")]
    public class TestV1Controller : RootController
    {
        private readonly TaposRSA _taposRsa;
        private readonly ITestService _testService;
        private readonly IDepartmentStudentMongoRepository _departmentStudentMongoRepository;
        
        public TestV1Controller(ITestService testService, TaposRSA taposRsa, IDepartmentStudentMongoRepository departmentStudentMongoRepository)
        {
            _taposRsa = taposRsa;
            _testService = testService;
            _departmentStudentMongoRepository = departmentStudentMongoRepository;
        }

        // [HttpGet]
        // public async Task<IActionResult> Index()
        // {
        //     await _testService.SaveTestData();
        //     return Ok("Hello, I'm from the test controller.");
        // }

        // [HttpGet]
        // public IActionResult Index()
        // {
        //     _taposRsa.GenerateRsaKey("v1");
        //     return Ok("Hello, I'm RSA Key Tester.");
        // }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _testService.UpdateCustomerBalanceTest();
            return Ok("Hello, I'm 'UpdateCustomerBalanceTest' method from the TestController.");
        }
        
        [HttpGet("mongo-department-student-create")]
        public async Task<IActionResult> DepartmentStudentCreateAsync()
        {
            await _departmentStudentMongoRepository.CreateAsync(new DepartmentStudentMongoModel()
            {
                DepartmentCode = "CSE",
                DepartmentName = "Computer Science and Engineering",
                StudentName = "Khandaker Shiba",
                StudentEmail = "shiba@gmail.com",
                StudentRollNo = "CSE-121"
            });
            
            return Ok("Hello, I'm 'DepartmentStudentCreateAsync' Mongo method from the TestController.");
        }
        
        [HttpGet("mongo-department-student-list")]
        public async Task<IActionResult> DepartmentStudentListAsync()
        {
            return Ok(await _departmentStudentMongoRepository.GetAllAsync());
        }
    }
}