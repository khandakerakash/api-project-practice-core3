using System.Threading.Tasks;
using BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility.Routes;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/department-report")]
    public class DepartmentReportV1Controller : RootController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentReportV1Controller(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet(ApiRoutes.DepartmentReport.DepartmentWiseStudentList)]
        [AllowAnonymous]
        public async Task<IActionResult> DepartmentWiseStudentListAsync()
        {
            return Ok(await _departmentService.DepartmentWiseStudentListAsync());
        }
    }
}