using LabThree.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace LabThree.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AccordionEntity> Accordions { get; set; }
}