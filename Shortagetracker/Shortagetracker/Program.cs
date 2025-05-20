using Shortagetracker.Models;
using Shortagetracker.Tasks;

namespace Shortagetracker;

class Program
{
    static void Main(string[] args)
    {
        //default users for testing
        User admin = new User("Admin", Role.Admin);
        User user = new User("User", Role.User);
        
        string? command;
        string? title;
        string room;
        string category;
        int priority;
        int indexToDelete;
        while (true)
        {
            Console.WriteLine("Enter which command you want to execute: (d - delete shortage) (c - create shortage) (l - list all shortages by filter) ");
            command = Console.ReadLine();
            if (command == "l")
            {
                Console.WriteLine("Enter title if you want to filter: ");
                //for now just filter with title for simplicity, there is operation to do with other filters
               string filterTitle = Console.ReadLine();
               var shoragesList = ShortageTasks.GetFilteredShortages(admin, filterTitle);
               for (int i = 0; i < shoragesList.Count; i++)
               {
                   Console.WriteLine($"{i + 1}. Title: {shoragesList[i].Title} Priority: {shoragesList[i].Priority}");
               }
               break;
            }

            if (command == "c")
            {
                break;
            }

            if (command == "d")
            {
                var shoragesList = ShortageTasks.GetFilteredShortages(admin);
                for (int i = 0; i < shoragesList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Title: {shoragesList[i].Title} Priority: {shoragesList[i].Priority}");
                }
                Console.WriteLine("Enter number to delete: ");
                indexToDelete = int.Parse(Console.ReadLine());
                if (indexToDelete > 0 && indexToDelete - 1 < shoragesList.Count)
                {
                    var deleted = ShortageTasks.RemoveShortage(shoragesList[indexToDelete - 1], admin);
                    if (deleted)
                    {
                        Console.WriteLine($"{indexToDelete} removed");
                        break;
                    }
                }
                
                
            }
            
        }
        Console.WriteLine("Enter correct values in order to create a new shortage");
        while (true)
        {
           
            Console.WriteLine("Enter shortage title: ");
            title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Must enter shortage title");
                continue;
            }

            break;
        }

        while (true)
        {
            Console.WriteLine("Select room (m - metting room) (k- kitchen) (b - bathroom): ");
            room = Console.ReadLine().ToLower();
            if (string.IsNullOrWhiteSpace(room) || (room != "m" && room != "b" && room != "k"))
            {
                Console.WriteLine("Invalid room");
                continue;
            }
            Console.WriteLine("Successfully selected room");
            break;
        }
        while (true)
        {
            Console.WriteLine("Select category (e - electronics) (f- food) (o - other): ");
            category = Console.ReadLine().ToLower();
            if (string.IsNullOrWhiteSpace(category) || (category != "e" && category != "f" && category != "o"))
            {
                Console.WriteLine("Invalid category");
                continue;
            }
            Console.WriteLine("Successfully selected category");
            break;
        }

        while (true)
        {
            Console.WriteLine("Enter shortage priority 1-10: ");
            priority = int.Parse(Console.ReadLine() ?? string.Empty);
            if (priority < 1 || priority > 10)
            {
                Console.WriteLine("Must be between 0 and 10");
                continue;
            }

            break;
        }

        Category categoryEnum = Category.Electronics;
        Room roomEnum = Room.MeetingRoom;
        switch (room)
        {
            case "m":
                roomEnum = Room.MeetingRoom;
                break;
            case "b":
                roomEnum = Room.Bathroom;
                break;
            case "k":
                roomEnum = Room.Kitchen;
                break;
            
        }
        switch (category)
        {
            case "e":
                categoryEnum = Category.Electronics;
                break;
            case "f":
                categoryEnum = Category.Food;
                break;
            case "o":
                categoryEnum = Category.Other;
                break;
            
        }
        Shortage shortage = new Shortage(title, admin, roomEnum, categoryEnum, priority, DateTime.Now);
        ShortageTasks.AddShortage(shortage);
    }
}