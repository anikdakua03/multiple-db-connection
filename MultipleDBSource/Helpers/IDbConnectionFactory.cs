using MultipleDBSource.Data;

namespace MultipleDBSource.Helpers;

public interface IDbConnectionFactory
{
    AppDbContext CreateDBContext(string database);
}
