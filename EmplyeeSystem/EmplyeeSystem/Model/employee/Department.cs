using System.ComponentModel.DataAnnotations;

namespace EmplyeeSystem.Model.employee
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentName { get; set; }

        public virtual ICollection<UserDepartment>? UserDepartments { get; set; }
    }
}
