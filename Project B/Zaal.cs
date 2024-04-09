using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class Zaal
{
    public string Name { get; set; }
    public int Rows { get; set; }
    public int Columns { get; set; }
    public int SerialNumber { get; set; }

    public Zaal(string name, int rows, int columns, int serialNumber)
    {
        Name = name;
        Rows = rows;
        Columns = columns;
        SerialNumber = serialNumber;
    }


    public void WriteToZaal(List<Zaal> zalen)
    {
        string directory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(directory, "zaal.json");
        string json = JsonConvert.SerializeObject(zalen, Formatting.Indented);

        File.AppendAllText(filePath, json);
    }

    public static void ReadFromZaal()
    {
        string directory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(directory, "zaal.json");
        string json = File.ReadAllText(filePath);

        List<Zaal>? zalen = JsonConvert.DeserializeObject<List<Zaal>>(json);
        if (zalen != null)
        {
            foreach (var zaal in zalen)
            {
                Console.WriteLine($"Name: {zaal.Name}, Rows: {zaal.Rows}, Columns: {zaal.Columns}, SerialNumber: {zaal.SerialNumber}");
            }
        }
        else
        {
            Console.WriteLine("Failed to load JSON file or file is empty.");
        }

    }
}
