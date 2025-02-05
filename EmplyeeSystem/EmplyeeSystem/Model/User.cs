using EmplyeeSystem.Model.employee;
using Microsoft.AspNetCore.Identity;

namespace EmplyeeSystem.Model
{
    public class User: IdentityUser
    {
        // 1 : 1
        public virtual Profile? Profile { get; set; }
        // 1:m
        public virtual ICollection<Project>? Projects { get; set; }
        //m:m
        public virtual ICollection<UserDepartment>? UserDepartments { get; set; }
    }
}