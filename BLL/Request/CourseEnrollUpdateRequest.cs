using DLL.Model;

namespace BLL.Request
{
    public class CourseEnrollUpdateRequest
    {
        public long CourseId { get; set; }
        public long StudentId { get; set; }
    }
}