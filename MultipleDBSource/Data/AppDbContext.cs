using Microsoft.EntityFrameworkCore;
using MultipleDBSourceSolution.Models;

namespace MultipleDBSourceSolution.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :  base(options)
    {
        
    }

    public DbSet<Person> Persons { get; set; }
}
