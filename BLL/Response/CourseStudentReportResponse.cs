using System.Collections.Generic;
using DLL.Model;

namespace BLL.Response
{
    public class CourseStudentReportResponse
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public ICollection<CourseStudent> CourseStudents { get; set; }
    }
}