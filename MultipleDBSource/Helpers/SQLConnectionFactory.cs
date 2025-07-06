using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MultipleDBSource.Data;
using System.Data;

namespace MultipleDBSource.Helpers;

public class SQLConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SQLConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AppDbContext CreateDBContext(string database)
    {
        string? updatedConnectionString = _configuration.GetConnectionString(database);

        if (string.IsNullOrEmpty(updatedConnectionString))
        {
            throw new InvalidProgramException("No database connection string found.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(updatedConnectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
