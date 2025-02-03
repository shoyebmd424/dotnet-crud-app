using appCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace appCrud.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
            public DbSet<Client> Clients { get; set; }
    }
}
