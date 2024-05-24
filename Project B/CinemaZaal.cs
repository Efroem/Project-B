public  class CinemaZaal
{   
    private const string EmptyRowSeparator = "|                                                    |";
	private const string EmptyRowSeparator2 = " |";
	private const string EmptyRowSeparator3 = "         |";
    private const string EmptyRowSeparator4 = "      |";
    private const string EmptyRowSeparator5 = "|                                   |";
	private const string EmptyRowSeparator6 = "        |";
	private const string EmptyRowSeparator7 = "      |";
    private HashSet<int> executedRows = new HashSet<int>();
    private int currentRow = 2; 
    private int cursorPosition = 5;
    public void NavigateGrid()
    {
        while (true)
        {   
            SetInitialCursorPosition();
            
            Console.SetCursorPosition(cursorPosition, currentRow);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.UpArrow && currentRow > 1)
            {
                currentRow -= 2;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && currentRow < 19)
            {
                currentRow += 2;
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow )// && cursorPosition > 4)
            {
                if(currentRow == 4 ||currentRow == 8 || currentRow == 10)
                {
                    cursorPosition -= 6;
                }
                else
                {
                    cursorPosition -= 4;
                }
            }
            // else if (keyInfo.Key == ConsoleKey.LeftArrow|| currentRow == 4 ||currentRow == 8 || currentRow == 10)// && cursorPosition > 4)
            // {
            //     cursorPosition -= 6;
            // }
            else if (keyInfo.Key == ConsoleKey.RightArrow )//&& cursorPosition < 45)
            {
                if(currentRow == 4 ||currentRow == 8 || currentRow == 10)
                {
                    cursorPosition +=2 ;
                }
                else
                {
                    cursorPosition += 4;
                }
            }
            else if (keyInfo.Key == ConsoleKey.Escape)
            {
                break;
            }
        }
    }

    // /*
    // private void SetInitialCursorPosition()
    // {
    //     if(!isCursorPositionSet)
    //         {
    //         if (currentRow == 4 ||currentRow == 8 || currentRow == 10 )
    //             {
    //                 cursorPosition = cursorPosition + 2;
    //                 isCursorPositionSet = true;

    //             }
    //         }
    // }
    // */
    //currentRow % 2 == 1
     private void SetInitialCursorPosition()
    {
        
        if (currentRow == 4 ||currentRow == 8 || currentRow == 10|| currentRow == 14 || currentRow == 18 || currentRow == 20)
        {
            cursorPosition = cursorPosition + 2;
        }
        else
        {
            cursorPosition = cursorPosition - 2;
        }

    }
    public void PrintGridGroteZaal()
    {
        Console.WriteLine("_____________________________________________________");
        Console.WriteLine("|												    |");


        for (char c = 'A'; c <= 'J'; c++)
        {
            Console.Write("|  " + c + "");
            GlobalVariables.maxSeats1 = (c - 'A') % 2 == 0 ? 11 : 9;            
            // hierboven staat een bereking van hoe het wordt berekent 
            // (c - 'A') hier wordt met ascii table gebruikt gemaakt dus character - a.
            // als c B is en B is 2 in ascii table dus het resutlaat zal dan 1 zijn etc.
            // als (c - 'A') % 2 en even getal is zal er een row met 11 stoelen geprint worden en als het oneven is dan een row met 9 stoelen.
            // zelfde is met de andere functies maar dan met andere getallen
            for (int i = 1; i <= GlobalVariables.maxSeats1; i++)
            {
                if (i == 1 && GlobalVariables.maxSeats1 == 9)
                Console.Write("  ["+ i + "] ");
				
				else if (i <= 9)
                {
                    Console.Write("["+ i +"] ");
                }
                else
                {
                    Console.Write("[" + i +  "] ");
                }
            }
            if (GlobalVariables.maxSeats1 == 11)
            {
                Console.WriteLine(EmptyRowSeparator2);
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

        Console.WriteLine("|                                                    |");
        Console.WriteLine("|                   filmdoek                         |");
        Console.WriteLine("|____________________________________________________|");
    }



public void PrintGridMediumZaal()
    {
        Console.WriteLine("_____________________________________________________");
        Console.WriteLine("|												    |");

        for (char c = 'A'; c <= 'I'; c++)
        {
            Console.Write("|  " + c + "");
            GlobalVariables.maxSeats2 = (c - 'A') % 2 == 0 ? 10 : 9;
            for (int i = 1; i <= GlobalVariables.maxSeats2; i++)
            {
                if (i == 1 && GlobalVariables.maxSeats2 == 9)
                Console.Write("  ["+ i + "] ");
				
				else if (i <= 9)
                {
                    Console.Write("["+ i +"] ");
                }
                else
                {
                    Console.Write("[" + i +  "] ");
                }
            }
            if (GlobalVariables.maxSeats2 == 10)
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

    public void PrintGridKleineZaal()
    {
        Console.WriteLine("____________________________________");
        Console.WriteLine("|								   |");

        for (char c = 'A'; c <= 'J'; c++)
        {
            Console.Write("|  " + c + "  ");
            GlobalVariables.maxSeats3 = (c - 'A') % 2 == 0 ? 6 : 5;
            for (int i = 1; i <= GlobalVariables.maxSeats3; i++)
            {
                if (i == 1 && GlobalVariables.maxSeats3 == 5)
                Console.Write("  ["+ i + "] ");
                else
                Console.Write("[" + i + "] ");
            }
            if (GlobalVariables.maxSeats3 == 5)
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
}


