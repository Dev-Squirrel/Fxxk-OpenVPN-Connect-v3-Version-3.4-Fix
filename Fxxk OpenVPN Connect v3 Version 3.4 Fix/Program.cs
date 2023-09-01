using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter source folder path: ");
        string sourceFolderPath = Console.ReadLine();

        Console.Write("Enter destination folder path: ");
        string destinationFolderPath = Console.ReadLine();

        string[] ovpnFiles = Directory.GetFiles(sourceFolderPath, "*.ovpn");

        foreach (string ovpnFile in ovpnFiles)
        {
            ProcessAndSaveOvpnFile(ovpnFile, destinationFolderPath);
        }

        Console.WriteLine("Processing and saving completed.");
    }

    static void ProcessAndSaveOvpnFile(string sourceFilePath, string destinationFolderPath)
    {
        string[] lines = File.ReadAllLines(sourceFilePath);

        string[] unsupportedOptions = {
            "resolv-retry", "persist-key", "persist-tun",
            "tun-mtu-extra", "pull", "block-outside-dns"
        };

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (IsUnsupportedOptionLine(line, unsupportedOptions))
            {
                lines[i] = "#" + lines[i];
            }
        }

        string originalFileName = Path.GetFileNameWithoutExtension(sourceFilePath);
        string destinationFileName = $"{originalFileName}_Version_3.4.ovpn";
        string destinationFilePath = Path.Combine(destinationFolderPath, destinationFileName);

        File.WriteAllLines(destinationFilePath, lines);
        Console.WriteLine($"Processed and saved: {destinationFilePath}");
    }

    static bool IsUnsupportedOptionLine(string line, string[] unsupportedOptions)
    {
        foreach (string option in unsupportedOptions)
        {
            if (line.StartsWith(option))
            {
                return true;
            }
        }

        return false;
    }
}
