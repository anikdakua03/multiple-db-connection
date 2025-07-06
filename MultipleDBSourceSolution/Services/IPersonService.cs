using MultipleDBSourceSolution.Models;

namespace MultipleDBSourceSolution.Services;

public interface IPersonService
{
    Task<List<Person>> GetAllPersonsAsync(CancellationToken cancellationToken = default);

    Task<Person?> GetPersonByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Person?> CreatePersonAsync(Person newPerson, CancellationToken cancellationToken = default);

    Task<Person?> DeletePersonByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
