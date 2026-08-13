using Microsoft.EntityFrameworkCore;
using SmartCityComplaint.Models;

namespace SmartCityComplaint.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Complaint> Complaints { get; set; }

    public DbSet<User> Users { get; set; }
}