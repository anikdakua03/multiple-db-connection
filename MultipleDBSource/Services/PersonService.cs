using Microsoft.EntityFrameworkCore;
using MultipleDBSource.Data;
using MultipleDBSource.Helpers;
using MultipleDBSource.Models;

namespace MultipleDBSource.Services;

public class PersonService : IPersonService
{
    //private readonly AppDbContext _appDbContext;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IConfiguration _configuration;

    public PersonService(/* AppDbContext appDbContext,*/ IConfiguration configuration, IDbConnectionFactory dbConnectionFactory)
    {
        //_appDbContext = appDbContext;
        _configuration = configuration;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<List<Person>> GetAllPersonsAsync(string database, CancellationToken cancellationToken = default)
    {
        // before fetching update the connection
        //DatabaseConnectionHelper.UpdateConnectionString(database, _appDbContext, _configuration);

        using var appDbContext = _dbConnectionFactory.CreateDBContext(database);

        return await appDbContext.Persons.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Person?> GetPersonByIdAsync(Guid id, string database, CancellationToken cancellationToken = default)
    {
        // before fetching update the connection
        //DatabaseConnectionHelper.UpdateConnectionString(database, _appDbContext, _configuration);

        using var appDbContext = _dbConnectionFactory.CreateDBContext(database);

        return await appDbContext.Persons.FirstOrDefaultAsync(a => a.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<Person?> CreatePersonAsync(Person newPerson, string database, CancellationToken cancellationToken = default)
    {
        // before fetching update the connection
        //DatabaseConnectionHelper.UpdateConnectionString(database, _appDbContext, _configuration);

        using var appDbContext = _dbConnectionFactory.CreateDBContext(database);

        Person? existingUser = await appDbContext.Persons.FirstOrDefaultAsync(a => a.Email == newPerson.Email, cancellationToken: cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException($"Person with same email already exists.");
        }

        await appDbContext.Persons.AddAsync(newPerson, cancellationToken);

        await appDbContext.SaveChangesAsync(cancellationToken);

        return newPerson;
    }

    public async Task<Person?> UpdatePersonAsync(Guid id, Person updatedPerson, string database, CancellationToken cancellationToken = default)
    {
        // before fetching update the connection
        //DatabaseConnectionHelper.UpdateConnectionString(database, _appDbContext, _configuration);

        using var appDbContext = _dbConnectionFactory.CreateDBContext(database);

        var existingPerson = await appDbContext.Persons.FindAsync(id);

        if (existingPerson is null)
        {
            throw new InvalidOperationException($"Person doesn't exists.");
        }

        // Update updatedPerson fields (excluding CreatedAt, it should remain unchanged)
        existingPerson.FirstName = updatedPerson.FirstName;
        existingPerson.LastName = updatedPerson.LastName;
        existingPerson.Email = updatedPerson.Email;
        existingPerson.Address = updatedPerson.Address;

        // Automatically update UpdatedAt in the database
        existingPerson.UpdatedTimestamp = DateTimeOffset.UtcNow;

        appDbContext.Persons.Update(existingPerson);

        await appDbContext.SaveChangesAsync(cancellationToken);

        return existingPerson;
    }

    public async Task<Person?> DeletePersonByIdAsync(Guid id, string database, CancellationToken cancellationToken = default)
    {
        // before fetching update the connection
        //DatabaseConnectionHelper.UpdateConnectionString(database, _appDbContext, _configuration);

        using var appDbContext = _dbConnectionFactory.CreateDBContext(database);

        Person? existingUser = await appDbContext.Persons.FirstOrDefaultAsync(a => a.Id == id, cancellationToken: cancellationToken);

        if (existingUser is null)
        {
            throw new InvalidOperationException($"Person doesn't exists.");
        }

        appDbContext.Persons.Remove(existingUser);

        _ = await appDbContext.SaveChangesAsync(cancellationToken);

        return existingUser;
    }
}
