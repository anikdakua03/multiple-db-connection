namespace MultipleDBSourceSolution.Models;

public class Person
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName => FirstName + " " + LastName;

    public required string Email { get; set; }

    public string Address { get; set; } = string.Empty;

    public DateTimeOffset CreatedTimestamp { get; set; }

    public DateTimeOffset UpdatedTimestamp { get; set; }
}