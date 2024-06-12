using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
public class Schedule : ProgramFunctions
{
    [JsonPropertyName("movieTitle")]
    public string JsonMovieTitle { set => MovieTitle = value; }

    private int _cinemaHallSerialNumber;
    [JsonPropertyName("cinemaHallSerialNumber")]
    public int CinemaHallSerialNumber
    {
        get => _cinemaHallSerialNumber;
        set
        {
            _cinemaHallSerialNumber = Convert.ToInt32(value);
            Hall = AdminFunctions.ReadFromCinemaHall().Find(x => x.SerialNumber == _cinemaHallSerialNumber);
        }
    }

    private int _serialNumber;
    [JsonPropertyName("serialNumber")]
    public int SerialNumber
    {
        get => _serialNumber;
        set
        {
            _serialNumber = value;
        }
    }
    [JsonPropertyName("date")]
    public string JsonDate { get => Date.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture); set => Date = DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture); }

    public string MovieTitle { get; set; }

    [JsonIgnore]
    public DateTime Date { get; set; }

    [JsonIgnore]
    public AdminFunctions? Hall { get; set; }

    [JsonPropertyName("seats")]
    public List<Seat> Seats { get; set; }

    public Schedule(string movieTitle, string cinemaHallSerialNumber, string date) : this(movieTitle, Convert.ToInt32(cinemaHallSerialNumber), DateTime.Parse(date))
    {

    }

    public Schedule(string movieTitle, int cinemaHallSerialNumber, DateTime date)
    {
        MovieTitle = movieTitle;
        CinemaHallSerialNumber = cinemaHallSerialNumber;
        Date = date;
        SerialNumber = ReadScheduleJson().OrderByDescending(x => x.SerialNumber).First().SerialNumber + 1;
        GenerateSeats();
    }

    public Schedule() { }

    // Als schedule.json niet goed gaat kan dit gerunned worden
    public static void ReserializeExistingSchedule()
    {
        List<Schedule> AccountList = ReadScheduleJson();

        foreach (Schedule schedule in AccountList)
        {
            if (schedule.Seats is null)
                schedule.GenerateSeats();
        }

        JsonSerializerOptions options = new() { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(AccountList, options);
        File.WriteAllText("schedule.json", jsonString);
    }

    private void GenerateSeats()
    {
        if (Hall is null)
            return;

        Seats = new List<Seat>();
        int rowLength;
        switch (Hall.Size)
        {
            case 1:
                for (int i = 0; i < 8; i++)
                {
                    rowLength = 7;
                    for (int j = 0; j < rowLength; j++)
                    {
                        Seat seat = j == 0 || j == rowLength - 1
                            ? new CheapSeat($"{i + 1}-{j + 1}")
                            : new Seat($"{i + 1}-{j + 1}");
                        Seats.Add(seat);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < 11; i++)
                {
                    rowLength = 8;
                    for (int j = 0; j < rowLength; j++)
                    {
                        Seat seat = j == 0 || j == rowLength - 1
                            ? new CheapSeat($"{i + 1}-{j + 1}")
                            : new Seat($"{i + 1}-{j + 1}");
                        Seats.Add(seat);
                    }
                }
                break;
            case 3:
                for (int i = 0; i < 10; i++)
                {
                    rowLength = 10;
                    for (int j = 0; j < rowLength; j++)
                    {
                        Seat seat = j == 0 || j == rowLength - 1
                            ? new CheapSeat($"{i + 1}-{j + 1}")
                            : new Seat($"{i + 1}-{j + 1}");
                        Seats.Add(seat);
                    }
                }
                break;
        }
    }

    public static List<Schedule> ReadScheduleJson()
    {
        string jsonString = File.ReadAllText("schedule.json");

        return JsonSerializer.Deserialize<List<Schedule>>(jsonString) ?? new();
    }

    public static void OpenGeneralMenu()
    {
        List<Schedule> schedules = ReadScheduleJson();
        schedules = schedules.Where(x => x.Date > DateTime.Now).OrderBy(x => x.Date).ToList();

        if (Authentication.User is not null && schedules.Count != 0)
        {
            List<Movie> movies = JsonSerializer.Deserialize<List<Movie>>(File.ReadAllText("movies.json"));

            DateTime birthDate = DateTime.ParseExact(Authentication.User.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            // Get the current date
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthDate.Year;
            schedules = schedules.Where(schedule => movies.Find(m => m.Title == schedule.MovieTitle).AgeRestricted <= age).ToList();
        }
        OpenScheduleMenu(schedules);
    }

    public static void OpenSpecificMenu<T>(T movie)
    {
        string movieTitle = (movie is Movie) ? (movie as Movie).Title : movie as string;

        List<Schedule> schedules = ReadScheduleJson();
        schedules = schedules.Where(x => x.MovieTitle == movieTitle).Where(x => x.Date > DateTime.Now).OrderBy(x => x.Date).ToList();
        OpenScheduleMenu(schedules);
    }

    private static void OpenScheduleMenu(List<Schedule> schedules)
    {
        int currentIndex = 0;
        const int stepSize = 5;
        Schedule pickedSchedule;

        while (true)
        {
            int moviesShownAmount = Math.Min(stepSize, schedules.Count - currentIndex);
            List<string> movies = new();
            for (int i = 0; i < moviesShownAmount; i++)
            {
                var schedule = schedules[currentIndex + i];
                movies.Add($"{i + 1 + (currentIndex != 0 ? 1 : 0)}.{schedule.MovieTitle}\n{schedule.Date}\n{schedule.Hall.Name}");
            }

            List<string> options = new();
            if (currentIndex != 0) options.Add("1.Terug");
            movies.ForEach(options.Add);

            if (currentIndex == 0)
            {
                if (currentIndex + stepSize < schedules.Count)
                {
                    options.Add($"{moviesShownAmount + 1}.Volgende");
                    options.Add($"{moviesShownAmount + 2}.Terug naar hoofdmenu");
                }
                else
                {
                    options.Add($"{moviesShownAmount + 1}.Terug naar hoofdmenu");
                }
            }
            else
            {
                if (currentIndex + stepSize < schedules.Count)
                {
                    options.Add($"{moviesShownAmount + 2}.Volgende");
                    options.Add($"{moviesShownAmount + 3}.Terug naar hoofdmenu");
                }
                else
                {
                    options.Add($"{moviesShownAmount + 2}.Terug naar hoofdmenu");
                }
            }

            Console.Clear();
            AsciiArtPrinter.AsciiArtPrinterRooster();

            int userAction = ShowMenuInline(options.ToArray());

            if (currentIndex != 0 && userAction == 0)
            {
                currentIndex = currentIndex - 5 < 0 ? 0 : currentIndex - 5;
            }
            else if ((currentIndex == 0 && userAction == movies.Count && moviesShownAmount != 0 && currentIndex + stepSize < schedules.Count)
                || (currentIndex != 0 && userAction == movies.Count + 1 && currentIndex + stepSize < schedules.Count))
            {
                currentIndex += stepSize;
            }
            else if (currentIndex == 0 && userAction < movies.Count)
            {
                pickedSchedule = schedules[currentIndex + userAction];
                Console.Clear();

                if (Authentication.User is not null)
                {
                    TheaterSeatingPrinter seatingPrinter = new TheaterSeatingPrinter();
                    seatingPrinter.PrintTheaterSeating(ReadScheduleJson(), pickedSchedule.SerialNumber);
                    return;
                }
                else if (Authentication.User is null)
                {
                    AsciiArtPrinter.PrintAsciiInlog();
                    Console.WriteLine();
                    PrintTextCentered("U bent nog niet ingelogd");
                    Authentication.Start();
                }
                else if (Authentication.User.IsAdmin)
                {
                    PrintTextCentered("U bent admin");
                    Program.Main();
                }
            }
            else if (currentIndex > 0 && userAction < movies.Count + 1)
            {
                pickedSchedule = schedules[currentIndex + (userAction - 1)];
                Console.Clear();

                if (Authentication.User is not null)
                {
                    TheaterSeatingPrinter seatingPrinter = new TheaterSeatingPrinter();
                    seatingPrinter.PrintTheaterSeating(ReadScheduleJson(), pickedSchedule.SerialNumber);
                    return;
                }
                else if (Authentication.User is null)
                {
                    PrintTextCentered("U bent nog niet ingelogd");
                    Authentication.Start();

                }
            }
            else if (
                (currentIndex == 0 && userAction == movies.Count + 0 && currentIndex <= schedules.Count) ||
                (currentIndex == 0 && userAction == movies.Count + 1 && currentIndex + stepSize < schedules.Count) ||
                (currentIndex != 0 && userAction == movies.Count + 2 && currentIndex < schedules.Count) ||
                (currentIndex != 0 && userAction == movies.Count + 1 && currentIndex + stepSize >= schedules.Count) ||
                (currentIndex == 0 && userAction == 0 && moviesShownAmount == 0)
            )
            {
                return;
            }

            Console.Clear();
        }
    }

    // Modified ShowMenuInline for multiline options
    private static int ShowMenuInline(string[] options)
    {
        int selectedOption = 0;
        int optionsLength = 0;
        int longestLineLength = 0;

        foreach (string option in options)
        {
            optionsLength += option.Split('\n').Length;

            foreach (string line in option.Split('\n'))
            {
                longestLineLength = longestLineLength < line.Length ? line.Length : longestLineLength;
            }
        }
        PrintTextCentered("     ┌" + new string('─', longestLineLength + 3) + "┐");

        do
        {
            for (int i = 0; i < options.Length; i++)
            {

                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    PrintTextCentered($"{options[i]}", longestLineLength);
                    Console.ResetColor();
                }
                else
                {
                    PrintTextCentered($"{options[i]}", longestLineLength);
                }
            }

            PrintTextCentered("     └" + new string('─', longestLineLength + 3) + "┘");

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow && selectedOption > 0)
            {
                selectedOption--;
            }
            else if (key.Key == ConsoleKey.DownArrow && selectedOption < options.Length - 1)
            {
                selectedOption++;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                break;
            }

            // Erase previous options display
            Console.CursorTop -= optionsLength + 1;
        } while (true);

        return selectedOption;
    }

    // Modified PrintTextCentered for multiline options
    private static void PrintTextCentered(string text, int longestLongestLineLength)
    {
        string[] textArray = text.Split('\n');
        int windowWidth;
        int leftPadding;
        int longestLineLength = 0;
        foreach (string line in textArray)
        {
            longestLineLength = longestLineLength < line.Length ? line.Length : longestLineLength;
        }

        for (int i = 0; i < textArray.Length; i++)
        {
            windowWidth = Console.WindowWidth;
            leftPadding = (windowWidth - longestLineLength + longestLineLength - longestLongestLineLength) / 2;
            Console.CursorLeft = leftPadding;

            Console.SetCursorPosition(leftPadding, Console.CursorTop);
            Console.WriteLine($"│ {textArray[i]}" + new string(' ', longestLongestLineLength + 3 - textArray[i].Length - 1) + "│");
        }
    }
}
