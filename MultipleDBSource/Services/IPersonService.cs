using MultipleDBSource.Models;

namespace MultipleDBSource.Services;

public interface IPersonService
{
    Task<List<Person>> GetAllPersonsAsync(string database, CancellationToken cancellationToken = default);

    Task<Person?> GetPersonByIdAsync(Guid id, string database, CancellationToken cancellationToken = default);

    Task<Person?> CreatePersonAsync(Person newPerson, string database, CancellationToken cancellationToken = default);

    Task<Person?> DeletePersonByIdAsync(Guid id, string database, CancellationToken cancellationToken = default);
}
