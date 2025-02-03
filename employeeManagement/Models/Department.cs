using System.ComponentModel.DataAnnotations;

namespace employeeManagement.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        public string name { get; set; }
        public string location { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
