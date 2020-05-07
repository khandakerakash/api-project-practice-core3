using System.Collections.Generic;
using DLL.Model;

namespace BLL.Response
{
    public class DepartmentReportResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}