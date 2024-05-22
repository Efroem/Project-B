using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
public class Schedule
{
    [JsonPropertyName("movieTitle")]
    public string JsonMovieTitle { set => MovieTitle = value; }
    [JsonPropertyName("serialNumber")]
    public string JsonSerialNumber { set {
            SerialNumber = Convert.ToInt32(value);
            Hall = CinemaHall.ReadFromCinemaHall().Find(x => x.SerialNumber == SerialNumber);
        }
    }
    [JsonPropertyName("date")]
    public string JsonDate { set => Date = DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture); }


    public string MovieTitle { get; set; }
    public int SerialNumber { get; set; }
    public DateTime Date { get; set; }

    public CinemaHall Hall {get; set;}

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
        List<Schedule> schedules = ReadScheduleJson();
        schedules = schedules.Where(x => x.Date > DateTime.Now).OrderBy(x => x.Date).ToList();
        OpenScheduleMenu(schedules);
    }

    public static void OpenSpecificMenu(string movieTitle)
    {
        List<Schedule> schedules = ReadScheduleJson();
        schedules = schedules.Where(x => x.MovieTitle == movieTitle).Where(x => x.Date > DateTime.Now).OrderBy(x => x.Date).ToList();
        OpenScheduleMenu(schedules);
    }

    private static void OpenScheduleMenu(List<Schedule> schedules)
    {
        int currentIndex = 0;
        const int stepSize = 5;

        while (true)
        {
            int previousIndex = 0;
            if (currentIndex != 0)
            {
                previousIndex = 1;
                Console.WriteLine($"{previousIndex}. Vorige");
            }

            int moviesShownAmount = Math.Min(stepSize, schedules.Count - currentIndex);
            for (int i = 0; i < moviesShownAmount; i++)
            {
                Console.WriteLine($"{i + 1 + previousIndex}. {schedules[currentIndex + i].MovieTitle}");
                Console.WriteLine(schedules[currentIndex + i].Date);
                Console.WriteLine(schedules[currentIndex + i].Hall.Name);
                Console.WriteLine();
            }

            string nextNumber;
            string backNumber;
            if (currentIndex + 5 < schedules.Count)
            {
                nextNumber = Convert.ToString(moviesShownAmount + 1 + previousIndex);
                backNumber = Convert.ToString(moviesShownAmount + 2 + previousIndex);
                Console.WriteLine($"{nextNumber}. Volgende");
            }
            else
            {
                nextNumber = "_";
                backNumber = Convert.ToString(moviesShownAmount + 1 + previousIndex);
            }

            Console.WriteLine($"{backNumber}. Terug naar menu");

            string userAction = (Console.ReadLine() ?? "").ToLower();
            if (userAction == "1" || userAction == "vorige")
            {
                currentIndex = currentIndex - 5 < 0 ? 0 : currentIndex - 5;
            }
            else if (userAction == nextNumber || userAction == "volgende")
            {
                currentIndex = schedules.Count - currentIndex > stepSize
                        ? currentIndex + stepSize > schedules.Count - currentIndex
                            ? schedules.Count - stepSize
                            : currentIndex + stepSize
                        : currentIndex;
            }
            else if (userAction == backNumber || userAction.Contains("terug") || userAction.Contains("menu"))
            {
                return;
            }

            for (int i = 0; i < Math.Min(stepSize, schedules.Count - currentIndex); i++)
            {
                if (userAction == Convert.ToString(i + 1 + previousIndex) || userAction == schedules[currentIndex + i].MovieTitle)
                {
                    Console.WriteLine($"You picked {i + 1 + previousIndex}. {schedules[currentIndex + i].MovieTitle}");
                    Console.ReadLine();
                }
            }

            Console.Clear();
        }
    }
}