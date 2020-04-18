using System.Threading.Tasks;
using Utility.Routes;
using BLL.Request;
using BLL.Service;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "staff", Policy = "AtToken")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _departmentService.FindAllAsync());
        }

        [HttpGet(ApiRoutes.Department.GetOne)]
        [AllowAnonymous]
        public async Task<IActionResult> GetOne(long id)
        {
            return Ok(await _departmentService.FindSingleAsync(id));
        }

        [HttpPost(ApiRoutes.Department.Create)]
        [Authorize(Roles = "staff", Policy = "AtToken")]
        public async Task<IActionResult> Create(DepartmentCreateRequest request)
        {
            return Ok(await _departmentService.CreateAsync(request));
        }
        
        [HttpPut(ApiRoutes.Department.Update)]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> Update(long id, DepartmentUpdateRequest request)
        {
            return Ok(await _departmentService.UpdateAsync(id, request));
        }

        [HttpDelete(ApiRoutes.Department.Delete)]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> Delete(long id)
        {
            return Ok(await _departmentService.DeleteAsync(id));
        }
    }
}