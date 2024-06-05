
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System;
using Newtonsoft.Json.Linq;
using System.IO;
public class CinemaHall
{
    private const string EmptyRowSeparator = "|                                                    |";
    private const string EmptyRowSeparator2 = " |";
    private const string EmptyRowSeparator3 = "         |";
    private const string EmptyRowSeparator4 = "      |";
    private const string EmptyRowSeparator5 = "|                                   |";
    private const string EmptyRowSeparator6 = "        |";
    private const string EmptyRowSeparator7 = "      |";
    private static int currentRow = 2;
    private static int cursorPosition = 5;

    private bool running = true;

    // public void NavigateGrid()
    // {
    //     Console.SetCursorPosition(0, 30);
    //     Console.Write("> (Q) Terug naar het hoofdmenu <");
    //     while (running)
    //     {
    //         SetInitialCursorPosition();

    //         Console.SetCursorPosition(cursorPosition, currentRow);
    //         ConsoleKeyInfo keyInfo = Console.ReadKey(true);

    //         if (keyInfo.Key == ConsoleKey.UpArrow && currentRow > 1)
    //         {
    //             currentRow -= 2;
    //             GlobalVariables.GlobalCurrentRow--;
    //         }
    //         else if (keyInfo.Key == ConsoleKey.DownArrow && currentRow < 19)
    //         {
    //             currentRow += 2;
    //             GlobalVariables.GlobalCurrentRow++;

    //         }
    //         else if (keyInfo.Key == ConsoleKey.LeftArrow && cursorPosition > 4)
    //         {
    //             if (currentRow == 4 || currentRow == 8 || currentRow == 10 || currentRow == 14 || currentRow == 18 || currentRow == 20 || currentRow == 24 || currentRow == 28)
    //             {
    //                 cursorPosition -= 6;
    //             }
    //             else
    //             {
    //                 cursorPosition -= 2;
    //             }
    //             GlobalVariables.GlobalCollum--;

    //         }
    //         else if (keyInfo.Key == ConsoleKey.RightArrow && cursorPosition < 45)
    //         {
    //             if (currentRow == 4 || currentRow == 8 || currentRow == 10 || currentRow == 14 || currentRow == 18 || currentRow == 20 || currentRow == 24 || currentRow == 24)
    //             {
    //                 cursorPosition += 2;
    //             }
    //             else
    //             {
    //                 cursorPosition += 6;
    //             }
    //             GlobalVariables.GlobalCollum++;
    //         }
    //         else if (keyInfo.Key == ConsoleKey.Enter)
    //         {
    //             PrintMessageAtDifferentLocation();
    //             cursorPosition += 2;
    //             GlobalVariables.GlobalList.Add(GlobalVariables.GlobalCollum);
    //             GlobalVariables.GlobalList.Add(GlobalVariables.GlobalCurrentRow);
    //         }
    //         else if (keyInfo.Key == ConsoleKey.Escape || keyInfo.Key == ConsoleKey.Q)
    //         {
    //             running = false;
    //             break;
    //         }
    //     }
    // }

    public void NavigateGrid()
    {
        Console.SetCursorPosition(0, 30);
        Console.Write("> (Q) Terug naar het hoofdmenu <");
        while (running)
        {
            //SetInitialCursorPosition();

            Console.SetCursorPosition(cursorPosition, currentRow);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.UpArrow && currentRow > 1)
            {
                currentRow --;
                GlobalVariables.GlobalCurrentRow--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && currentRow < 19)
            {
                currentRow ++;
                GlobalVariables.GlobalCurrentRow++;

            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow && cursorPosition > 4)
            {
                cursorPosition -= 4;
                GlobalVariables.GlobalCollum--;

            }
            else if (keyInfo.Key == ConsoleKey.RightArrow && cursorPosition < 45)
            {
               
                cursorPosition += 4;
                GlobalVariables.GlobalCollum++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.SetCursorPosition(0, 35);
                Console.Write($"Je stoel is ({GlobalVariables.GlobalCurrentRow}, {GlobalVariables.GlobalCollum})\n> (K) Bevestging zitplekken <");
                if (keyInfo.Key == ConsoleKey.K)
                {
                    //vincent zijn code moet hier met betalen
                    // hier worden de coordinaten opgeslagen. bij het betalen wordt de json geupdate
                    // denk dat je tuple moet vergelijken met de id en dan als die de id heeft gevonden isavialable op false zetten
                    // en dan moet de list weer geleegt worden
                    var myTuple = Tuple.Create(GlobalVariables.GlobalCollum, GlobalVariables.GlobalCurrentRow);
                    GlobalVariables.GlobalList.Add(myTuple);
                }
            }
            else if (keyInfo.Key == ConsoleKey.Escape || keyInfo.Key == ConsoleKey.Q)
            {
                running = false;
            }
        }
    }
    // public static void PrintMessageAtDifferentLocation()
    // {
    //     int originalCursorLeft = Console.CursorLeft;
    //     int originalCursorTop = Console.CursorTop;

    //     Console.SetCursorPosition(0, 40);
    //     Console.Write($"Je stoel is ({currentRow}, {cursorPosition})\n> (K) Bevestging zitplekken <");

    //     Console.CursorLeft = originalCursorLeft;
    //     Console.CursorTop = originalCursorTop;
    // }
    // public static void SetInitialCursorPosition()
    // {

    //     if (currentRow == 4 || currentRow == 8 || currentRow == 10 || currentRow == 14 || currentRow == 18 || currentRow == 20)
    //     {
    //         cursorPosition = cursorPosition + 2;
    //     }
    //     else
    //     {
    //         cursorPosition = cursorPosition - 2;
    //     }

    // }
    
    public static void PrintGridGroteZaal()
    {
        Console.WriteLine("_____________________________________________________");
        Console.WriteLine("|												    |");
        for (char c = 'A'; c <= 'J'; c++)
        {
            Console.Write("|  " + c + "");

           for (int i = 1; i <= 11; i++)
            {
                
                if (Seat.IsAvailable == true)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green; 
                    Console.Write("x");
                    Console.ResetColor();
                    Console.Write("] ");
                }
                else
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("x");
                    Console.ResetColor();
                    Console.Write("] ");
                }
            }
            if (c != 'J')
            {
                Console.WriteLine(EmptyRowSeparator);
            }
        }
        Console.WriteLine("|                                                    |");
        Console.WriteLine("|                   filmdoek                         |");
        Console.WriteLine("|____________________________________________________|");
    }
        
    


    public static void PrintGridMediumZaal()
    {
        Console.WriteLine("_____________________________________________________");
        Console.WriteLine("|												    |");

        for (char c = 'A'; c <= 'I'; c++)
        {
            Console.Write("|  " + c + "");
            int maxSeats2 = (c - 'A') % 2 == 0 ? 10 : 9;
            for (int i = 1; i <= maxSeats2; i++)
            {
                if (i == 1 && maxSeats2 == 9)
                    Console.Write("  [" + i + "] ");

                else if (i <= 9)
                {
                    Console.Write("[" + i + "] ");
                }
                else
                {
                    Console.Write("[" + i + "] ");
                }
            }
            if (maxSeats2 == 10)
            {
                Console.WriteLine(EmptyRowSeparator4);
            }
            else
            {
                Console.WriteLine(EmptyRowSeparator3);
            }
            if (c != 'J')
            {
                Console.WriteLine(EmptyRowSeparator);
            }
        }

        // Print the bottom part
        Console.WriteLine("|                                                    |");
        Console.WriteLine("|                   filmdoek                         |");
        Console.WriteLine("|____________________________________________________|");
    }

    public static void PrintGridKleineZaal()
    {
        Console.WriteLine("____________________________________");
        Console.WriteLine("|								   |");

        for (char c = 'A'; c <= 'J'; c++)
        {
            Console.Write("|  " + c + "");
            int maxSeats3 = (c - 'A') % 2 == 0 ? 6 : 5;
            for (int i = 1; i <= maxSeats3; i++)
            {
                if (i == 1 && maxSeats3 == 5)
                Console.Write("  ["+ i + "] ");
                else
                Console.Write("[" + i + "] ");
            }
            if (maxSeats3 == 5)
            {
                Console.WriteLine(EmptyRowSeparator6);
            }
            else
            {
                Console.WriteLine(EmptyRowSeparator7);
            }
            if (c != 'J')
            {
            Console.WriteLine(EmptyRowSeparator5);
            }
        }

        Console.WriteLine("|                                   |");
        Console.WriteLine("|           filmdoek                |");
        Console.WriteLine("|___________________________________|");
    }
    // static void DisplayCinemaShowInfo()
    // {
    //     string filePath = "schedule.json";
    //     // Open the file for reading
    //     using (FileStream fs = File.OpenRead(filePath))
    //     {
    //         // Deserialize the JSON content into a Schedule object
    //         Schedule cinemaShow = JsonSerializer.DeserializeAsync<Schedule>(fs).Result;

    //         // Iterate through each seat in the cinema show
    //         foreach (var seat in cinemaShow.Seats)
    //         {
    //             // Extract the first and third characters from the seat ID
    //             char firstChar = seat.ID[0];
    //             char thirdChar = seat.ID[2];
    //             int intFirstChar = (int)char.GetNumericValue(firstChar);
    //             int intThirdChar = (int)char.GetNumericValue(thirdChar);

    //             // Display or use the extracted information
    //             Console.WriteLine($"First character: {firstChar}, Third character: {thirdChar}");
    //             Console.WriteLine($"Numeric value of first character: {intFirstChar}, Numeric value of third character: {intThirdChar}");
    //         }
    //     }
    // }

}



public class TheaterSeatingPrinter
{
    public void PrintTheaterSeating(string filePath, int scheduleSerialNumber)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                JArray jsonArray = JArray.Parse(json);
                
                if (scheduleSerialNumber >= 0 && scheduleSerialNumber < jsonArray.Count)
                {
                    JObject schedule = (JObject)jsonArray[scheduleSerialNumber];
                    JArray seats = (JArray)schedule["seats"];

                    int rows = GetMaxRow(seats);
                    int columns = GetMaxColumn(seats);

                    PrintGridGroteZaal(rows, columns, seats);
                }
                else
                {
                    Console.WriteLine("Invalid schedule serial number.");
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private int GetMaxRow(JArray seats)
    {
        // LINQ Max functie om de maximale rij te vinden
        // Extract row number from each seat ID and find the maximum value among them
        // haalt het rij nummer uit elke ID en dan vind die de 
        return seats.Max(s => 
        {
            // hier haalt de id eruit van de stoel eruit
            string seatId = (string)s["id"];
            
            // split de id van de stoel tussen de - en pakt het eerste getal
            string[] parts = seatId.Split('-');
            string rowPart = parts[0];
            
            // van string naar int
            return int.Parse(rowPart);
        });
    }

    private int GetMaxColumn(JArray seats)
    {
        // zelfde logica net als hierboven
        return seats.Max(s => 
        {
            string seatId = (string)s["id"];
            
            string[] parts = seatId.Split('-');
            string columnPart = parts[1];
            
            return int.Parse(columnPart);
        });
    }

    
    public static void PrintGridGroteZaal(int rows, int columns, JArray seats)
    {
        Console.WriteLine("_____________________________________________________");
        Console.WriteLine("|                                                    |");

        for (int i = 1; i <= rows; i++)
        {
            Console.Write("|  " + (char)(i + 64) + "");

            for (int j = 1; j <= columns; j++)
            {
                string seatId = i + "-" + j;
                bool isAvailable = seats.Any(s =>
                {
                    string id = (string)s["id"];
                    bool available = (bool)s["isAvailable"];
                    return id == seatId && available;
                });

                if (isAvailable)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("x");
                    Console.ResetColor();
                    Console.Write("] ");
                }
                else
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("x");
                    Console.ResetColor();
                    Console.Write("] ");
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine("|                                                    |");
        Console.WriteLine("|                   filmdoek                         |");
        Console.WriteLine("|____________________________________________________|");
    }
}
