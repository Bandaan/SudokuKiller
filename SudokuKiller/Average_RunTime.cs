using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Average_RunTime
{
    class Program
{
    static void Main()
    {
        // Get the current path directory
        var directory = Directory.GetCurrentDirectory();
        // Calculate the path for the txt file where the testcases are in
        string newPath = Path.GetFullPath(Path.Combine(directory, @"..\..\..\..\TestFiles"));

        // Get all CSV files in the directory
        string[] csvFiles = Directory.GetFiles(newPath, "*.csv");

        // Creates a dictionary to store average runtimes for each combination of RandomWalkLength, RandomWalkStart, and Algorithm type
        Dictionary<string, Tuple<double, int>> averageRuntimes = new Dictionary<string, Tuple<double, int>>();

        // Loop through each CSV file
        foreach (string csvFile in csvFiles)
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(csvFile);

            // Skip the first line
            var dataLines = lines.Skip(1);

            // Parse each line
            foreach (var line in dataLines)
            {
                // Split the line into words
                string[] words = line.Split(',');

                // Get values
                double runtime = double.Parse(words[0]);
                int randomWalkLength = int.Parse(words[1]);
                int randomWalkStart = int.Parse(words[2]);
                string algorithm = words[3];

                // Create a key for our dictionary which is the combination of values
                string key = $"{randomWalkLength},{randomWalkStart},{algorithm}";

                // Update or add the runtime value to the dictionary
                if (averageRuntimes.ContainsKey(key))
                {
                    Tuple<double, int> currentAverage = averageRuntimes[key];
                    double currentTotal = currentAverage.Item1;
                    int count = currentAverage.Item2;

                    // Update the average runtime and count
                    averageRuntimes[key] = new Tuple<double, int>((currentTotal * count + runtime) / (count + 1), count + 1);
                }
                else
                {
                    // If the key has not been added to the dictionary add it with the first runtime
                    averageRuntimes[key] = new Tuple<double, int>(runtime, 1);
                }
            }
        }

        // Write results to output CSV file
        using (StreamWriter sw = new StreamWriter(newPath))
            {
                // Add headers to csv file
                string headers = "Average RunTime,RandomWalkLength,RandomWalkStart,Improvement";
                sw.WriteLine(headers);
                
                // Write the outputs
                foreach (var output in averageRuntimes)
                {
                    sw.WriteLine($"{output.Value.Item1},{output.Key}");
                }

            }

        Console.WriteLine($"Results written to {newPath}");
    }
}
}