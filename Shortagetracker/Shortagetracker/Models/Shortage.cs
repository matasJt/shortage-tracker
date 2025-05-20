using System.Text.Json.Serialization;

namespace Shortagetracker.Models;

public enum Room
{
    MeetingRoom,
    Kitchen,
    Bathroom
}

public enum Category
{
    Electronics,
    Food,
    Other
}


public class Shortage
{
    public string? Title { get; private set; }
    public User Name { get; private set; }
    public Room? Room { get; private set; }
    public Category? Category { get; private set; }
    public int Priority { get; private set; }
    public DateTime CreatedOn { get; private set; }
    
    [JsonConstructor]
    public Shortage(string? Title, User Name, Room? Room, Category? Category, int Priority, DateTime CreatedOn)
    {
        this.Title = Title;
        this.Name = Name;
        this.Room = Room;
        this.Category = Category;
        this.CreatedOn = CreatedOn;
        this.Priority = Priority;
    }
    public override bool Equals(object? obj)
    {
        if (obj is not Shortage other) return false;
        return Title == other.Title &&
               Name.Name == other.Name.Name &&
               Name.Role == other.Name.Role &&
               Room == other.Room &&
               Category == other.Category &&
               Priority == other.Priority &&
               CreatedOn == other.CreatedOn;
    }
}
