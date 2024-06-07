using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        // String to hash
        string inputString = "admin";

        // Compute SHA256 hash
        string hashedString = ComputeSHA256Hash(inputString);

        // Output the hash
        Console.WriteLine("SHA256 hash of 'admin': " + hashedString);
    }

    static string ComputeSHA256Hash(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
