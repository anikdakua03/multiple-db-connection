using Microsoft.EntityFrameworkCore.Migrations;

namespace MultipleDBSource.Extensions;

public static class MigrationExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="migrationBuilder"></param>
    /// <param name="category"></param>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public static void ApplySqlScripts(this MigrationBuilder migrationBuilder, string category)
    {
        // Path logic - works during 'dotnet ef database update'
        // Try to find the path in the output directory (bin folder)
        string sqlPath = Path.Combine(AppContext.BaseDirectory,"RawSQLs", category);

        //  NOTE : If not found in bin, try to look in the project source (useful for local development)
        if (Directory.Exists(sqlPath) == false)
        {
            sqlPath = Path.Combine(Directory.GetCurrentDirectory(), "RawSQLs", category);
        }

        if (Directory.Exists(sqlPath) == false)
        {
            throw new DirectoryNotFoundException($"Could not find SQL scripts at: {sqlPath}");
        }

        string[] files = Directory.GetFiles(sqlPath, "*.sql");

        foreach (string file in files)
        {
            string sql = File.ReadAllText(file);

            if (string.IsNullOrWhiteSpace(sql) == false)
            {
                migrationBuilder.Sql(sql);
            }
        }
    }
}
