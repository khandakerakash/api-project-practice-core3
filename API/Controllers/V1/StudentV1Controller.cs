using System.Threading.Tasks;
using Utility.Routes;
using BLL.Request;
using BLL.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/student")]
    public class StudentV1Controller : RootController
    {
        private readonly IStudentService _studentService;

        public StudentV1Controller(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _studentService.FindAllAsync());
        }

        [HttpGet(ApiRoutes.Student.GetOne)]
        public async Task<IActionResult> GetOne(long id)
        {
            return Ok(await _studentService.FindSingleAsync(id));
        }

        [HttpPost(ApiRoutes.Student.Create)]
        public async Task<IActionResult> Create(StudentCreateRequest request)
        {
            return Ok(await _studentService.CreateAsync(request));
        }

        [HttpPut(ApiRoutes.Student.Update)]
        public async Task<IActionResult> Update(long id, StudentUpdateRequest request)
        {
            return Ok(await _studentService.UpdateAsync(id, request));
        }

        [HttpDelete(ApiRoutes.Student.Delete)]
        public async Task<IActionResult> Delete(long id)
        {
            return Ok(await _studentService.DeleteAsync(id));
        }
    }
}