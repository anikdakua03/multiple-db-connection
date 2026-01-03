namespace MultipleDBSource.Models;

public class MoreInfo
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName
    {
        get => FirstName + " " + LastName;
        private set { } // EF can now "see" a setter
    }

    public string? Email { get; set; }

    public string Address { get; set; } = string.Empty;

    public Preferences? UserPrefs { get; set; } // Nested object inside JSON

    public List<EmergencyContact>? Contacts { get; set; }
}
