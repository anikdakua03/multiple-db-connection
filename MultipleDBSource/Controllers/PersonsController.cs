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

    [HttpGet]
    public async Task<IActionResult> GetPersonsAsync(CancellationToken cancellationToken)
    {
        List<Person> res = await _personService.GetAllPersonsAsync(cancellationToken);

        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePersonAsync(Person newPerson, CancellationToken cancellationToken)
    {
        Person? res = await _personService.CreatePersonAsync(newPerson, cancellationToken);

        return Ok(res);
    }
}
