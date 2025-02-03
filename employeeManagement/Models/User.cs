using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace employeeManagement.Models
{
    public class User : IdentityUser
    {
        [Key]
        public int Id { get; set; }
        public Profile? Profile { get; set; }
        public ICollection<UserProject>? UserProjects { get; set; }
        public ICollection<Department>? departments { get; set; }
    }
}