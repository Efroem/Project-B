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
    public string JsonDate { get => Date.ToString("dd/MM/yyyy HH:mm:ss"); set => Date = DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture); }

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
                movies.Add($"{i + 1 + (currentIndex != 0 ? 1 : 0)}.{schedules[currentIndex + i].MovieTitle}\n{schedules[currentIndex + i].Date}\n{schedules[currentIndex + i].Hall.Name}");
            }
            List<string> options = new();
            if (currentIndex != 0)
                options.Add("1.Terug");
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
            string purple = "\u001b[35m";
            string reset = "\u001b[0m";

            AsciiArtPrinter.PrintAsciifilms();
            Program.PrintTextCentered($"\nGebruik de ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Program.PrintTextCentered("pijltjestoetsen");
            Console.ResetColor();
            Program.PrintTextCentered("om een optie te selecteren en druk op ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Program.PrintTextCentered("Enter\n");
            Console.ResetColor();
            int userAction = ShowMenuInline(options.ToArray());
            // Go back in list
            if (currentIndex != 0 && userAction == 0)
            {
                currentIndex = currentIndex - 5 < 0 ? 0 : currentIndex - 5;
            }
            // Go Forward in list
            else if ((currentIndex == 0 && userAction == movies.Count && moviesShownAmount != 0 && currentIndex + stepSize < schedules.Count)
                || (currentIndex > 0 && userAction == movies.Count + 1 && currentIndex + stepSize < schedules.Count))
            {
                currentIndex += stepSize;
            }
            // Movie picked
            else if (currentIndex == 0 && userAction < movies.Count)
            {
                pickedSchedule = schedules[currentIndex + userAction];
                Console.WriteLine($"You picked {userAction + 1}. {pickedSchedule.MovieTitle}");
                Console.ReadLine();
                Console.Clear();
                bool running = true;
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                while(running == true)
                {
                TheaterSeatingPrinter seatingPrinter = new TheaterSeatingPrinter();
                CinemaHall cinemaZaal = new CinemaHall();
                
                string filePath = "schedule.json";
                int scheduleSerialNumber = pickedSchedule.SerialNumber;

                seatingPrinter.PrintTheaterSeating(filePath, scheduleSerialNumber);
                cinemaZaal.NavigateGrid();
                if(keyInfo.Key == ConsoleKey.Escape || keyInfo.Key == ConsoleKey.Q)
                {
                    break;
                }
                }

                
                //HallAssignment.Callfunction2();
            }
            // Movie picked part 2
            else if (currentIndex > 0 && userAction < movies.Count + 1)
            {
                pickedSchedule = schedules[currentIndex + (userAction - 1)];
                Console.WriteLine($"You picked {userAction + 1}. {pickedSchedule.MovieTitle}");
                Console.ReadLine();
                Console.Clear();
                HallAssignment.Callfunction2();
            }
            // Complicated way to check if user wants to return
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
            Console.CursorTop -= optionsLength;
        } while (true);

        return selectedOption;
    }

    // Modified PrintTextCentered for multiline options
    private static void PrintTextCentered(string text, int longestLongestLineLength)
    {


        if (text.Contains('\n'))
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
                Console.WriteLine(textArray[i]);
            }
        }
        else
        {
            int windowWidth = Console.WindowWidth;
            int leftPadding = (windowWidth - text.Length) / 2;
            Console.CursorLeft = leftPadding;

            Console.SetCursorPosition(leftPadding, Console.CursorTop);
            Console.WriteLine(text);
        }


    }


}