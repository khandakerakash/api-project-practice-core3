using System;
using DLL.Model.Interfaces;

namespace DLL.Model
{
    public class CourseStudent : ITrackable, ISoftDeletable
    {
        public long CourseStudentId { get; set; }
        public long CourseId { get; set; }
        public Course Course { get; set; }
        public long StudentId { get; set; }
        public Student Student { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}