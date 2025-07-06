using Microsoft.EntityFrameworkCore;
using MultipleDBSource.Data;

namespace MultipleDBSource.Helpers;

public static class DatabaseConnectionHelper
{
    public static void UpdateConnectionString(string database, AppDbContext context, IConfiguration configuration)
    {
        string? updatedConnectionString = configuration.GetConnectionString(database);

        if(string.IsNullOrEmpty(updatedConnectionString) )
        {
            throw new InvalidProgramException("No databse connection string found.");
        }

        // remove previous context
        context.ChangeTracker.Clear();

        // update connection
        context.Database.SetConnectionString(updatedConnectionString);
    }
}
