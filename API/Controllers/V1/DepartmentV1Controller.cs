using System.Threading.Tasks;
using API.Utility;
using BLL.Request;
using BLL.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/department")]
    public class DepartmentV1Controller : RootController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentV1Controller(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _departmentService.FindAllAsync());
        }

        [HttpGet(ApiRoutes.Department.GetOne)]
        public async Task<IActionResult> GetOne(long id)
        {
            return Ok(await _departmentService.FindOneAsync(id));
        }

        [HttpPost(ApiRoutes.Department.Create)]
        public async Task<IActionResult> Create(DepartmentCreateRequest request)
        {
            return Ok(await _departmentService.CreateAsync(request));
        }
        
        [HttpPut(ApiRoutes.Department.Update)]
        public IActionResult Update()
        {
            return Ok("Done");
        }

        [HttpDelete(ApiRoutes.Department.Delete)]
        public IActionResult Delete()
        {
            return Ok("Done");
        }
    }
}