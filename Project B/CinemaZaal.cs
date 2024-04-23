public  class CinemaZaal
{   
    private const string EmptyRowSeparator = "|                                                    |";
	private const string EmptyRowSeparator2 = " |";
	private const string EmptyRowSeparator3 = "         |";
    private const string EmptyRowSeparator4 = "      |";
    private const string EmptyRowSeparator5 = "|                                   |";
	private const string EmptyRowSeparator6 = "        |";
	private const string EmptyRowSeparator7 = "      |";


    public void PrintGridGroteZaal()
    {
        Console.WriteLine("_____________________________________________________");
        Console.WriteLine("|												    |");


        for (char c = 'A'; c <= 'J'; c++)
        {
            Console.Write("|  " + c + "  ");
            int maxSeats = (c - 'A') % 2 == 0 ? 11 : 9;
            // hierboven staat een bereking van hoe het wordt berekent 
            // (c - 'A') hier wordt met ascii table gebruikt gemaakt dus character - a.
            // als c B is en B is 2 in ascii table dus het resutlaat zal dan 1 zijn etc.
            // als (c - 'A') % 2 en even getal is zal er een row met 11 stoelen geprint worden en als het oneven is dan een row met 9 stoelen.
            // zelfde is met de andere functies maar dan met andere getallen
            for (int i = 1; i <= maxSeats; i++)
            {
                if (i == 1 && maxSeats == 9)
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
            if (maxSeats == 11)
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
            Console.Write("|  " + c + "  ");
            int maxSeats = (c - 'A') % 2 == 0 ? 10 : 9;
            for (int i = 1; i <= maxSeats; i++)
            {
                if (i == 1 && maxSeats == 9)
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
            if (maxSeats == 10)
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
            int maxSeats = (c - 'A') % 2 == 0 ? 6 : 5;
            for (int i = 1; i <= maxSeats; i++)
            {
                if (i == 1 && maxSeats == 5)
                Console.Write("  ["+ i + "] ");
                else
                Console.Write("[" + i + "] ");
            }
            if (maxSeats == 5)
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


