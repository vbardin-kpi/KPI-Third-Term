using LabThree.Api.Dal.Entities;

using Microsoft.EntityFrameworkCore;

namespace LabThree.Api.Dal
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Accordion> Accordions { get; set; }
    }
}
