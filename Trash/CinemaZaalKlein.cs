public class CinemaZaalKlein
{
    private const string EmptyRowSeparator5 = "|                                   |";
    private const string EmptyRowSeparator6 = "        |";
    private const string EmptyRowSeparator7 = "      |";
    private int currentRow = 2;
    private int cursorPosition = 2;

    private bool running = true;

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
                    Console.Write("  [" + i + "] ");
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

    public void NavigateGridKleineZaal()
    {
        Console.SetCursorPosition(0, 30);
        Console.Write("> (Q) Terug naar het hoofdmenu <");
        while (running)
        {
            CinemaZaal.SetInitialCursorPosition();

            Console.SetCursorPosition(cursorPosition, currentRow);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.UpArrow && currentRow > 1)
            {
                currentRow -= 2;
                GlobalVariables.GlobalCurrentRow--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && currentRow < 19)
            {
                currentRow += 2;
                GlobalVariables.GlobalCurrentRow++;

            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow && cursorPosition > 4)
            {
                if (currentRow == 4 || currentRow == 8 || currentRow == 10 || currentRow == 14 || currentRow == 18 || currentRow == 20 || currentRow == 24 || currentRow == 28)
                {
                    cursorPosition -= 6;
                }
                else
                {
                    cursorPosition -= 2;
                }
                GlobalVariables.GlobalCollum--;

            }
            else if (keyInfo.Key == ConsoleKey.RightArrow && cursorPosition < 45)
            {
                if (currentRow == 4 || currentRow == 8 || currentRow == 10 || currentRow == 14 || currentRow == 18 || currentRow == 20 || currentRow == 24 || currentRow == 24)
                {
                    cursorPosition += 2;
                }
                else
                {
                    cursorPosition += 6;
                }
                GlobalVariables.GlobalCollum++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                CinemaZaal.PrintMessageAtDifferentLocation();
                cursorPosition += 2;
                GlobalVariables.GlobalList.Add(GlobalVariables.GlobalCollum);
                GlobalVariables.GlobalList.Add(GlobalVariables.GlobalCurrentRow);
            }
            else if (keyInfo.Key == ConsoleKey.Escape || keyInfo.Key == ConsoleKey.Q)
            {
                running = false;
                break;
            }
        }
    }

}