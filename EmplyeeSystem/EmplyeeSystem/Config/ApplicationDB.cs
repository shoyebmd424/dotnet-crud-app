using EmplyeeSystem.Model;
using EmplyeeSystem.Model.employee;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmplyeeSystem.Config
{
    public class ApplicationDB : IdentityDbContext<User>
    {
      

        public ApplicationDB(DbContextOptions<ApplicationDB> options) : base(options)
        {
        }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserDepartment> UserDepartments { get; set; }

    }

}
