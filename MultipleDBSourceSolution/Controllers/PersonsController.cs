using Microsoft.AspNetCore.Mvc;
using MultipleDBSourceSolution.Models;
using MultipleDBSourceSolution.Services;

namespace MultipleDBSourceSolution.Controllers;

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
        var res = await _personService.GetAllPersonsAsync(cancellationToken);

        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePersonAsync(Person newPerson, CancellationToken cancellationToken)
    {
        var res = await _personService.CreatePersonAsync(newPerson, cancellationToken);

        return Ok(res);
    }
}
