using System.ComponentModel.DataAnnotations;

namespace employeeManagement.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public int Bio { get; set; }
        public User? User { get; set; }
        public int userId { get; set; }
    }
}
