using System.Threading.Tasks;
using BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility.Routes;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/course-report")]
    public class CourseReportV1Controller : RootController
    {
        private readonly ICourseService _courseService;

        public CourseReportV1Controller(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet(ApiRoutes.CourseReport.CourseStudentList)]
        [AllowAnonymous]
        public async Task<IActionResult> CourseStudentListAsync()
        {
            return Ok(await _courseService.CourseStudentListAsync());
        }
    }
}