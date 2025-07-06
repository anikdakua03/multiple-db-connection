using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleDBSource.Models;
using MultipleDBSource.Services;
using System.Net.NetworkInformation;

namespace MultipleDBSource.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly ILogger<PersonsController> _logger;
    private readonly IPersonService _personService;

    public PersonsController(ILogger<PersonsController> logger, IPersonService personService)
    {
        _logger = logger;
        _personService = personService;
    }

    /// <summary>
    /// Gets all person
    /// </summary>
    /// <param name="database">Database name</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of all persons</returns>
    [HttpGet("{database}")]
    public async Task<IActionResult> GetPersonsAsync(string database, CancellationToken cancellationToken)
    {
        List<Person> res = await _personService.GetAllPersonsAsync(database, cancellationToken);

        return Ok(res);
    }

    /// <summary>
    /// Creates a person if it doesn't exist
    /// </summary>
    /// <param name="newPerson"></param>
    /// <param name="database">Database name</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created person</returns>
    [HttpPost("{database}")]
    public async Task<IActionResult> CreatePersonAsync(Person newPerson, string database, CancellationToken cancellationToken)
    {
        Person? res = await _personService.CreatePersonAsync(newPerson, database, cancellationToken);

        return Accepted(res);
    }

    /// <summary>
    /// Updates a person by id if it exists
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updatedPerson"></param>
    /// <param name="database">Database name</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Updated person</returns>
    [HttpPut("{id}/{database}")]
    public async Task<IActionResult> UpdatePersonAsync(Guid id, Person updatedPerson, string database, CancellationToken cancellationToken)
    {
        if (id != updatedPerson.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var updatedPersonRes = await _personService.UpdatePersonAsync(id, updatedPerson, database, cancellationToken);

        return Ok(updatedPersonRes);
    }

    /// <summary>
    /// Deletes a person by id if it exists
    /// </summary>
    /// <param name="id"></param>
    /// <param name="database">Database name</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted person</returns>
    [HttpDelete("{id}/{database}")]
    public async Task<IActionResult> DeletePersonAsync(Guid id, string database, CancellationToken cancellationToken)
    {
        var deletedPerson = await _personService.DeletePersonByIdAsync(id, database, cancellationToken);

        return Ok(deletedPerson);
    }
}
