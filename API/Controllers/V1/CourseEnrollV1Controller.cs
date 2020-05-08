using System.Threading.Tasks;
using BLL.Request;
using BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility.Routes;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/course-enroll")]
    public class CourseEnrollV1Controller : RootController
    {
        private readonly ICourseEnrollService _courseEnrollService;

        public CourseEnrollV1Controller(ICourseEnrollService courseEnrollService)
        {
            _courseEnrollService = courseEnrollService;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _courseEnrollService.FindAllAsync());
        }
        
        [HttpPost(ApiRoutes.CourseEnroll.Create)]
        [Authorize(Roles = "teacher, staff", Policy = "AtToken")]
        public async Task<IActionResult> Create(CourseEnrollCreateRequest request)
        {
            return Ok(await _courseEnrollService.CreateAsync(request));
        }
    }
}