using Microsoft.AspNetCore.Mvc;
using MultipleDBSource.Models;
using MultipleDBSource.Services;

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

    [HttpGet("{database}")]
    public async Task<IActionResult> GetPersonsAsync(string database, CancellationToken cancellationToken)
    {
        List<Person> res = await _personService.GetAllPersonsAsync(database, cancellationToken);

        return Ok(res);
    }

    [HttpPost("{database}")]
    public async Task<IActionResult> CreatePersonAsync(Person newPerson, string database, CancellationToken cancellationToken)
    {
        Person? res = await _personService.CreatePersonAsync(newPerson, database, cancellationToken);

        return Ok(res);
    }
}
