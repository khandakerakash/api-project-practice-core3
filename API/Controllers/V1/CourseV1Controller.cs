using System.Threading.Tasks;
using BLL.Request;
using BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility.Routes;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/course")]
    public class CourseV1Controller : RootController
    {
        private readonly ICourseService _courseService;

        public CourseV1Controller(ICourseService courseService)
        {
            _courseService = courseService;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _courseService.FindAllAsync());
        }

        [HttpGet(ApiRoutes.Course.GetOne)]
        [AllowAnonymous]
        public async Task<IActionResult> GetOne(long id)
        {
            return Ok(await _courseService.FindSingleAsync(id));
        }

        [HttpPost(ApiRoutes.Course.Create)]
        [Authorize(Roles = "teacher, staff", Policy = "AtToken")]
        public async Task<IActionResult> Create(CourseCreateRequest request)
        {
            return Ok(await _courseService.CreateAsync(request));
        }
    }
}