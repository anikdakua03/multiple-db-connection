using Microsoft.EntityFrameworkCore;
using MultipleDBSource.Data;
using MultipleDBSource.Helpers;
using MultipleDBSource.Models;

namespace MultipleDBSource.Services;

public class PersonService : IPersonService
{
    private readonly AppDbContext _appDbContext;
    private readonly IConfiguration _configuration;

    public PersonService(AppDbContext appDbContext, IConfiguration configuration)
    {
        _appDbContext = appDbContext;
        _configuration = configuration;
    }

    public async Task<List<Person>> GetAllPersonsAsync(string database, CancellationToken cancellationToken = default)
    {
        // before fetching update the connection
        DatabaseConnectionHelper.UpdateConnectionString(database, _appDbContext, _configuration);

        return await _appDbContext.Persons.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Person?> GetPersonByIdAsync(Guid id, string database, CancellationToken cancellationToken = default)
    {
        // before fetching update the connection
        DatabaseConnectionHelper.UpdateConnectionString(database, _appDbContext, _configuration);

        return await _appDbContext.Persons.FirstOrDefaultAsync(a => a.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<Person?> CreatePersonAsync(Person newPerson, string database, CancellationToken cancellationToken = default)
    {
        // before fetching update the connection
        DatabaseConnectionHelper.UpdateConnectionString(database, _appDbContext, _configuration);

        Person? existingUser = await _appDbContext.Persons.FirstOrDefaultAsync(a => a.Email == newPerson.Email, cancellationToken: cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException($"Person with same email already exists.");
        }

        _ = await _appDbContext.Persons.AddAsync(newPerson, cancellationToken);

        _ = await _appDbContext.SaveChangesAsync(cancellationToken);

        return newPerson;
    }

    public async Task<Person?> DeletePersonByIdAsync(Guid id, string database, CancellationToken cancellationToken = default)
    {
        // before fetching update the connection
        DatabaseConnectionHelper.UpdateConnectionString(database, _appDbContext, _configuration);

        Person? existingUser = await _appDbContext.Persons.FirstOrDefaultAsync(a => a.Id == id, cancellationToken: cancellationToken);

        if (existingUser is null)
        {
            throw new InvalidOperationException($"Person doesn't exists.");
        }

        _ = _appDbContext.Persons.Remove(existingUser);

        _ = await _appDbContext.SaveChangesAsync(cancellationToken);

        return existingUser;
    }
}
