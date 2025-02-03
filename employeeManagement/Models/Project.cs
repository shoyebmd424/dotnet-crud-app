using System.ComponentModel.DataAnnotations;

namespace employeeManagement.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String? Title { get; set; }
        [Required]
        public String? ProjectDesc { get; set; }
        public DateTime Deadline { get; set; }
        public ICollection<UserProject>? UserProjects  { get; set; }
    }
}
