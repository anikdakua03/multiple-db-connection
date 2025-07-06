using Microsoft.EntityFrameworkCore;
using MultipleDBSourceSolution.Data;
using MultipleDBSourceSolution.Models;

namespace MultipleDBSourceSolution.Services;

public class PersonService : IPersonService
{
    private readonly AppDbContext _appDbContext;

    public PersonService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Person>> GetAllPersonsAsync(CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Persons.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Person?> GetPersonByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.Persons.FirstOrDefaultAsync(a => a.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<Person?> CreatePersonAsync(Person newPerson, CancellationToken cancellationToken = default)
    {
        Person? existingUser = await _appDbContext.Persons.FirstOrDefaultAsync(a => a.Email == newPerson.Email, cancellationToken: cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException($"Person with same email already exists.");
        }

        await _appDbContext.Persons.AddAsync(newPerson, cancellationToken);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return newPerson;
    }

    public async Task<Person?> DeletePersonByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Person? existingUser = await _appDbContext.Persons.FirstOrDefaultAsync(a => a.Id == id, cancellationToken: cancellationToken);

        if (existingUser is null)
        {
            throw new InvalidOperationException($"Person doesn't exists.");
        }

        _appDbContext.Persons.Remove(existingUser);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return existingUser;
    }
}
