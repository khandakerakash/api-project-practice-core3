﻿using System;
using System.Collections.Generic;
using DLL.Model.Interfaces;

namespace DLL.Model
{
    public class Student : ITrackable, ISoftDeletable
    {
        public long StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RollNo { get; set; }
        public long DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<CourseStudent> CourseStudents { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}