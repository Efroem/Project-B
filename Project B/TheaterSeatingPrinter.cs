
using System;

using System.Collections.Generic;

using System.IO;

using System.Text.Json;

using System;

using Newtonsoft.Json.Linq;

using System.IO;

using System.Security.Cryptography.X509Certificates;

using System.Text.Json;
using System.Text.Json.Serialization;

public class TheaterSeatingPrinter
{
    public static bool navigateGridBool;
    private static int userXPosition = 1;
    private static int userYPosition = 1;
    private static List<(int x, int y)> userPositions = new List<(int x, int y)>();

    public void PrintTheaterSeating(List<Schedule> schedules, int scheduleSerialNumber)
    {
        try
        {
            var schedule = schedules.FirstOrDefault(s => s.SerialNumber == scheduleSerialNumber);

            if (schedule == null)
            {
                Console.WriteLine("Invalid schedule serial number.");
                return;
            }

            var seats = schedule.Seats;
            int rows = GetMaxRow(seats);
            int columns = GetMaxColumn(seats);
            bool running = true;

            while (running)
            {
                Console.Clear();
                PrintGridGroteZaal(rows, columns, seats);
                running = HandleUserInput(rows, columns, schedules, scheduleSerialNumber, this);
            }

            Console.WriteLine("Back to menu");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private int GetMaxRow(List<Seat> seats)
    {
        return seats.Max(s =>
        {
            string[] parts = s.ID.Split('-');
            return int.Parse(parts[1]);
        });
    }

    private int GetMaxColumn(List<Seat> seats)
    {
        return seats.Max(s =>
        {
            string[] parts = s.ID.Split('-');
            return int.Parse(parts[1]);
        });
    }

    public static void PrintGridGroteZaal(int rows, int columns, List<Seat> seats)
    {
        Console.WriteLine("_____________________________________________________");
        Console.WriteLine("|                                                    |");

        for (int i = 1; i <= rows; i++)
        {
            Console.Write("|  " + (char)(i + 64) + "");

            for (int j = 1; j <= columns; j++)
            {
                string seatId = i + "-" + j;
                var seat = seats.FirstOrDefault(s => s.ID == seatId);

                if (i == userYPosition && j == userXPosition)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("@");
                    Console.ResetColor();
                    Console.Write("] ");
                }
                else if (userPositions.Contains((j, i)))
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("O");
                    Console.ResetColor();
                    Console.Write("] ");
                }
                else if (seat != null && seat.IsAvailable)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("O");
                    Console.ResetColor();
                    Console.Write("] ");
                }
                else
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("X");
                    Console.ResetColor();
                    Console.Write("] ");
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine("|                                                    |");
        Console.WriteLine("|                   filmdoek                         |");
        Console.WriteLine("|____________________________________________________|");
        Console.WriteLine("Klik Q om naar het hoofdmenu terug te gaan");
        Console.WriteLine("Klik K om te stoelen te bevestigen");
    }

    private static bool HandleUserInput(int rows, int columns, List<Schedule> schedules, int scheduleSerialNumber, TheaterSeatingPrinter printer)
    {
        var keyInfo = Console.ReadKey(true);

        switch (keyInfo.Key)
        {
            case ConsoleKey.LeftArrow:
                if (userXPosition > 1) userXPosition--;
                break;

            case ConsoleKey.RightArrow:
                if (userXPosition < columns) userXPosition++;
                break;

            case ConsoleKey.UpArrow:
                if (userYPosition > 1) userYPosition--;
                break;

            case ConsoleKey.DownArrow:
                if (userYPosition < rows) userYPosition++;
                break;

            case ConsoleKey.Enter:
                var userPosition = (userXPosition, userYPosition);
                if (!userPositions.Contains(userPosition))
                {
                    var schedule = schedules.FirstOrDefault(s => s.SerialNumber == scheduleSerialNumber);
                    if (schedule != null)
                    {
                        var seat = schedule.Seats.FirstOrDefault(s => s.ID == $"{userYPosition}-{userXPosition}");
                        if (seat != null && seat.IsAvailable)
                        {
                            userPositions.Add(userPosition);
                        }
                    }
                }
                break;

            case ConsoleKey.Backspace:
                if (userPositions.Count > 0)
                {
                    userPositions.RemoveAt(userPositions.Count - 1);
                }
                break;

            case ConsoleKey.K:
                double seatPrice = printer.ZettenVanTuppleInListNaarJson(schedules, scheduleSerialNumber);
                Console.Clear();
                Payment.AddSeatPrice(seatPrice);
                Payment.AddSelectedSeats(userPositions); // Toegevoegd
                Payment.scheduleSerialNumber = scheduleSerialNumber;
                Payment.BestelMenu();
                break;

            case ConsoleKey.Q:
                return false;
        }

        return true;
    }

    public double ZettenVanTuppleInListNaarJson(List<Schedule> schedules, int scheduleSerialNumber)
    {
        var schedule = schedules.FirstOrDefault(s => s.SerialNumber == scheduleSerialNumber);
        if (schedule == null)
        {
            return 0;
        }

        double GeldteBetalen = 0;
        foreach (var position in userPositions)
        {
            foreach (var seat in schedule.Seats)
            {
                string[] parts = seat.ID.Split('-');
                int seatRow = int.Parse(parts[1]);
                int seatColumn = int.Parse(parts[0]);

                if (position.x == seatRow && position.y == seatColumn && seat.IsAvailable)
                {
                    GeldteBetalen += seat.Price;
                    seat.IsAvailable = false;
                }
            }
        }

        JsonSerializerOptions options = new() { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(schedules, options);
        File.WriteAllText("schedule.json", jsonString);

        // Add selected seats before clearing the list
        Payment.AddSelectedSeats(userPositions);

        userPositions.Clear();
        return GeldteBetalen;
    }
}
