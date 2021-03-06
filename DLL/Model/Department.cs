﻿using System;
using System.Collections.Generic;
using DLL.Model.Interfaces;

namespace DLL.Model
{
    public class Department : ITrackable, ISoftDeletable
    {
        public long DepartmentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Student> Students { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}