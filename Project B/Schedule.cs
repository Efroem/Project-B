using System.Text.Json;
using System.Text.Json.Serialization;
public class Schedule
{
    [JsonPropertyName("movieTitle")]
    public string JsonMovieTitle { set => MovieTitle = value; }
    [JsonPropertyName("serialNumber")]
    public string JsonSerialNumber { set => SerialNumber = Convert.ToInt32(value); }
    [JsonPropertyName("date")]
    public string JsonDate { set => Date = DateTime.Parse(value); }


    public string MovieTitle { get; set; }
    public int SerialNumber { get; set; }
    public DateTime Date { get; set; }

    public Schedule(string movieTitle, string serialNumber, string date) : this(movieTitle, Convert.ToInt32(serialNumber), DateTime.Parse(date))
    {

    }

    public Schedule(string movieTitle, int serialNumber, DateTime date)
    {
        MovieTitle = movieTitle;
        SerialNumber = serialNumber;
        Date = date;
    }

    public Schedule() { }

    public static void Open()
    {
        Console.WriteLine("morgen is er een film om 18:00");
        Console.ReadLine();
    }

    public static List<Schedule> ReadScheduleJson()
    {
        // Read the JSON file as a string
        string jsonString = File.ReadAllText("schedule.json");

        // Deserialize the JSON string into an object
        return JsonSerializer.Deserialize<List<Schedule>>(jsonString) ?? new();
    }

    public static void OpenGeneralMenu()
    {
        List<Schedule> schedule = ReadScheduleJson();
        schedule = schedule.Where(x => x.Date > DateTime.Now).OrderBy(x => x.Date).ToList();
        int currentIndex = 0;
        const int stepSize = 5;

        while (true) {
            if (currentIndex != 0)
                Console.WriteLine("1. Vorige");
            for (int i = 0; i < Math.Min(stepSize, schedule.Count - currentIndex); i++)
            {
                Console.WriteLine($"{i+2}. {schedule[currentIndex + i].MovieTitle}");
                Console.WriteLine(schedule[currentIndex + i].Date);
                Console.WriteLine();
            }
            if (currentIndex + 5 < schedule.Count)
            Console.WriteLine("7. Volgende");
            Console.WriteLine("8. Terug naar menu");

            string userAction = (Console.ReadLine() ?? "").ToLower();
            switch (userAction)
            {   
                case "1":
                case "vorige":
                    currentIndex = currentIndex - 5 < 0 ? 0 : currentIndex - 5;
                    break; 
                case "7":
                case "volgende":
                    currentIndex = schedule.Count - currentIndex > stepSize 
                        ? currentIndex + stepSize > schedule.Count - currentIndex 
                            ? schedule.Count - stepSize 
                            : currentIndex + stepSize 
                        : currentIndex;
                    break;
                case "8":
                case "terug":
                case "terug naar menu":
                case "menu":
                    return;
            }

            for (int i = 0; i < Math.Min(stepSize, schedule.Count - currentIndex); i++)
            {
                if (userAction == Convert.ToString(i+2) || userAction == schedule[currentIndex + i].MovieTitle) {
                    Console.WriteLine($"You picked {i+2}. {schedule[currentIndex + i].MovieTitle}");
                }
            }

            Console.Clear();
        }
    }
}