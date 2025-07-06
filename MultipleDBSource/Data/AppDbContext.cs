using Microsoft.EntityFrameworkCore;
using MultipleDBSource.Models;

namespace MultipleDBSource.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Person> Persons { get; set; }
}
