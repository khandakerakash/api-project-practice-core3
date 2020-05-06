using System.Threading.Tasks;
using BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility.Routes;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/student-report")]
    public class StudentReportV1Controller : RootController
    {
        private readonly IStudentService _studentService;

        public StudentReportV1Controller(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet(ApiRoutes.StudentReport.StudentDepartmentInfo)]
        [AllowAnonymous]
        public async Task<IActionResult> StudentDepartmentInfoListAsync()
        {
            return Ok(await _studentService.StudentDepartmentInfoListAsync());
        }
    }
}