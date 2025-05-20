using System.Text.Json.Serialization;

namespace Shortagetracker.Models;

public enum Role
{
    Admin, 
    User
}

public class User
{
    public string Name { get; set; }
    public Role Role { get; set; }

    [JsonConstructor]
    public User(string name, Role role)
    {
        Name = name;
        Role = role;
    }
}