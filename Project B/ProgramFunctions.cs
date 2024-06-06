using System;

public static class ProgramFunctions
{
    public static void PrintTextCentered(string text)
    {
        string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        int windowWidth = Console.WindowWidth;

        foreach (string line in lines)
        {
            int leftPadding = (windowWidth - line.Length) / 2;
            if (leftPadding < 0)
                leftPadding = 0;

            Console.SetCursorPosition(leftPadding, Console.CursorTop);
            Console.WriteLine(line);
        }
    }

    public static void PrintColoredTextCentered(string textPart1, ConsoleColor color1, string textPart2, ConsoleColor color2, string textPart3, ConsoleColor color3)
    {
        int windowWidth = Console.WindowWidth;
        string fullText = textPart1 + textPart2 + textPart3;
        int leftPadding = (windowWidth - fullText.Length) / 2;

        if (leftPadding < 0)
            leftPadding = 0;

        Console.SetCursorPosition(leftPadding, Console.CursorTop);
        Console.ForegroundColor = color1;
        Console.Write(textPart1);
        Console.ForegroundColor = color2;
        Console.Write(textPart2);
        Console.ForegroundColor = color3;
        Console.Write(textPart3);
        Console.ResetColor();
        Console.WriteLine();
    }

    public static void PrintColoredTextCentered(string textPart1, ConsoleColor color1, string textPart2, ConsoleColor color2, string textPart3, ConsoleColor color3, string textPart4, ConsoleColor color4)
    {
        int windowWidth = Console.WindowWidth;
        string fullText = textPart1 + textPart2 + textPart3 + textPart4;
        int leftPadding = (windowWidth - fullText.Length) / 2;

        if (leftPadding < 0)
            leftPadding = 0;

        Console.SetCursorPosition(leftPadding, Console.CursorTop);
        Console.ForegroundColor = color1;
        Console.Write(textPart1);
        Console.ForegroundColor = color2;
        Console.Write(textPart2);
        Console.ForegroundColor = color3;
        Console.Write(textPart3);
        Console.ForegroundColor = color4;
        Console.Write(textPart4);
        Console.ResetColor();
        Console.WriteLine();
    }

    public static int ShowMenuInline(string[] options)
    {
        int selectedOption = 0;
        int longestLineLength = 0;

        foreach (string option in options)
        {
            longestLineLength = longestLineLength < option.Length ? option.Length : longestLineLength;
        }
        longestLineLength += 3;
        PrintTextCentered("┌" + new string('─', longestLineLength) + "┐");

        do
        {
            for (int i = 0; i < options.Length; i++)
            {
                int leftPadding = (Console.WindowWidth - options[i].Length) / 2;
                Console.CursorLeft = leftPadding;
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    PrintTextCentered($"│ {options[i]}" + new string(' ', longestLineLength - options[i].Length - 1) + "│");
                    Console.ResetColor();
                }
                else
                {
                    PrintTextCentered($"│ {options[i]}" + new string(' ', longestLineLength - options[i].Length - 1) + "│");
                }
            }

            PrintTextCentered("┕" + new string('─', longestLineLength) + "┘");

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

            Console.CursorTop -= options.Length + 1;
        } while (true);

        return selectedOption;
    }

    public static void ShowIntro()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        AsciiArtPrinter.MegaBioscoop();
        Console.ResetColor();

        Console.WriteLine();
        ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);

        Console.ReadKey();
        Console.Clear();

        AsciiArtPrinter.PrintAscii("movies.json");
        Thread.Sleep(1000);
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
    }
}
