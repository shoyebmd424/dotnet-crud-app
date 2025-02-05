using System.ComponentModel.DataAnnotations;

namespace EmplyeeSystem.Model.employee
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }  
    }
}
