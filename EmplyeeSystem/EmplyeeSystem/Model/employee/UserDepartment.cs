﻿using System.ComponentModel.DataAnnotations;

namespace EmplyeeSystem.Model.employee
{
    public class UserDepartment
    {
        [Key]
        public int id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}