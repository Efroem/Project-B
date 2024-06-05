using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
public class Schedule
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
    public string JsonDate { get => Date.ToString("dd/MM/yyyy HH:mm:ss"); set => Date = DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InstalledUICulture); }

    public string MovieTitle { get; set; }

    [JsonIgnore]
    public DateTime Date { get; set; }

    [JsonIgnore]
    public AdminFunctions? Hall { get; set; }

    [JsonPropertyName("seats")]
    public List<Seat> Seats { get; set; }

    private double _cheapSeatPrice = 6.99;
    private double _seatPrice = 8.99;

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
                            ? new Seat($"{i + 1}-{j + 1}", _cheapSeatPrice)
                            : new Seat($"{i + 1}-{j + 1}", _seatPrice);
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
                            ? new Seat($"{i + 1}-{j + 1}", _cheapSeatPrice)
                            : new Seat($"{i + 1}-{j + 1}", _seatPrice);
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
                            ? new Seat($"{i + 1}-{j + 1}", _cheapSeatPrice)
                            : new Seat($"{i + 1}-{j + 1}", _seatPrice);
                        Seats.Add(seat);
                    }
                }
                break;
        }
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

    public static void OpenSpecificMenu(Movie movie)
    {
        OpenSpecificMenu(movie.Title);
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
                movies.Add($"{i + 1 + (currentIndex != 0 ? 1 : 0)}.{schedule.MovieTitle}\n{schedule.Date}\n{schedule.CinemaHallSerialNumber}");
            }

            List<string> options = new();
            if (currentIndex != 0) options.Add("1.Terug");
            movies.ForEach(options.Add);

            if (currentIndex == 0)
            {
                if (currentIndex + stepSize <= schedules.Count)
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
                if (currentIndex + stepSize <= schedules.Count)
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
            // ASCII art and instructions...

            int userAction = ShowMenuInline(options.ToArray());

            if (currentIndex != 0 && userAction == 0)
            {
                currentIndex = currentIndex - 5 < 0 ? 0 : currentIndex - 5;
            }
            else if ((currentIndex == 0 && userAction == movies.Count && moviesShownAmount != 0 && currentIndex + stepSize < schedules.Count)
                || (currentIndex > 0 && userAction == movies.Count + 1 && currentIndex + stepSize < schedules.Count))
            {
                currentIndex += stepSize;
            }
            else if (currentIndex == 0 && userAction < movies.Count)
            {
                pickedSchedule = schedules[currentIndex + userAction];
                Console.WriteLine($"You picked {userAction + 1}. {pickedSchedule.MovieTitle}");
                Console.ReadLine();
                Console.Clear();


                TheaterSeatingPrinter seatingPrinter = new TheaterSeatingPrinter();
                seatingPrinter.PrintTheaterSeating(schedules, pickedSchedule.SerialNumber);
                //CinemaHall.NavigateGrid();
                Console.ReadLine();
                return;
            }
            else if (currentIndex > 0 && userAction < movies.Count + 1)
            {
                pickedSchedule = schedules[currentIndex + (userAction - 1)];
                Console.WriteLine($"You picked {userAction + 1}. {pickedSchedule.MovieTitle}");
                Console.ReadLine();
                Console.Clear();
                // HallAssignment.Callfunction2();
            }
            else if (
                (currentIndex == 0 && userAction == movies.Count + 1 && currentIndex <= schedules.Count) ||
                (currentIndex == 0 && userAction == movies.Count + 1 && currentIndex + stepSize > schedules.Count) ||
                (currentIndex != 0 && userAction == movies.Count + 2 && currentIndex <= schedules.Count) ||
                (currentIndex != 0 && userAction == movies.Count + 1 && currentIndex + stepSize > schedules.Count) ||
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
        Program.PrintTextCentered("     ┌" + new string('─', longestLineLength + 3) + "┐");

        // // Deel de prompt op rond de woorden die rood moeten worden
        // string[] promptParts = prompt.Split(new string[] { " pijltjestoetsen ", " Enter" }, StringSplitOptions.None);

        // // Schrijf het eerste deel van de prompt
        // AsciiArtPrinter.PrintCentered(promptParts[0]);
        // Console.ForegroundColor = ConsoleColor.Magenta;
        // AsciiArtPrinter.PrintCentered(" pijltjestoetsen ");
        // Console.ResetColor();

        // AsciiArtPrinter.PrintCentered(promptParts[1]);
        // Console.ForegroundColor = ConsoleColor.Magenta;
        // AsciiArtPrinter.PrintCentered(" Enter");
        // Console.ResetColor();

        // AsciiArtPrinter.PrintCentered(promptParts[2]);
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

            Program.PrintTextCentered("     └" + new string('─', longestLineLength + 3) + "┘");

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