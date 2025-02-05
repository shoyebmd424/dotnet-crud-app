using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmplyeeSystem.Model.employee
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string UserId { get; set; }
        public User? User { get; set; }
    }
}
