using System.Text.Json;
using System.Text.Json.Serialization;
using Shortagetracker.Models;

namespace Shortagetracker.Tasks;

public class ShortageTasks
{
    private const  string _fileToSave = "Shortage";
    public static void AddShortage(Shortage newShortage)
    {
        try
        {
            List<Shortage>? shortages = new List<Shortage>();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            if (File.Exists(_fileToSave) && new FileInfo(_fileToSave).Length > 0)
            {
                shortages = JsonSerializer.Deserialize<List<Shortage>>(File.ReadAllText(_fileToSave),options);
            }

            bool updated = false;

            if (shortages != null)
                for (int i = 0; i < shortages.Count; i++)
                {
                    if (shortages[i].Title == newShortage.Title && shortages[i].Room == newShortage.Room)
                    {
                        if (shortages[i].Priority < newShortage.Priority)
                        {
                            shortages[i] = newShortage;
                            Console.WriteLine("Shortage already exists, but overridden due to higher priority");
                            updated = true;
                            break;
                        }

                        Console.WriteLine("Shortage already exists");
                        return;
                    }
                }

            if (!updated)
            {
                shortages.Add(newShortage);
                Console.WriteLine("Shortage Added");
            }
            
            File.WriteAllText(_fileToSave, JsonSerializer.Serialize(shortages, options));
        }
        catch
        {
            Console.WriteLine("Error writing shortage");
        }
    }
    
    public static List<Shortage>? GetFilteredShortages(User name, string? filterTitle = null, Room? filterRoom = null, Category? filterCategory = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        List<Shortage>? shortages = new List<Shortage>();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        if (File.Exists(_fileToSave) && new FileInfo(_fileToSave).Length > 0)
        {
            shortages = JsonSerializer.Deserialize<List<Shortage>>(File.ReadAllText(_fileToSave),options);
        }

        if (name.Role == Role.User)
        {
            shortages = shortages.Where(x => x.Name.Name == name.Name).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filterTitle))
        {
            shortages= shortages.Where(s => s.Title.ToLower().Contains(filterTitle.ToLower())).ToList();
        }

        if (filterRoom.HasValue)
        {
            shortages= shortages.Where(x=> x.Room == filterRoom.Value).ToList();
        }
        if (filterCategory.HasValue)
        {
            shortages = shortages.Where(x=> x.Category == filterCategory.Value).ToList();
        }

        if (startDate.HasValue)
        {
            shortages = shortages.Where(r => r.CreatedOn >= startDate.Value) as List<Shortage>;
        }
        if (endDate.HasValue)
        {
            shortages = shortages.Where(r => r.CreatedOn < endDate.Value) as List<Shortage>;
        }

        return shortages.OrderByDescending(x => x.Priority).ToList();

    }

    public static bool RemoveShortage(Shortage shortage, User name)
    {
        List<Shortage>? shortages = new List<Shortage>();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        if (File.Exists(_fileToSave) && new FileInfo(_fileToSave).Length > 0)
        {
            shortages = JsonSerializer.Deserialize<List<Shortage>>(File.ReadAllText(_fileToSave),options);
        }

        if (shortage.Name.Name == name.Name || name.Role == Role.Admin)
        {
            var removed = shortages.Remove(shortage);
            if (removed)
            {
                File.WriteAllText(_fileToSave, JsonSerializer.Serialize(shortages, options));
                return true;
            }
            return removed;
        }
        
        return false;
    }
}