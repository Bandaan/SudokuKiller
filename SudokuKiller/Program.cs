using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;

namespace SudokuKiller
{
    class Program
    {
        /// <summary>
        /// Main class where you can solve sudoku
        /// </summary>
        
        static void Main()
        {
            string[] input = Console.ReadLine().Split(" ");
            Sudoku sudoku = ParseHelper.ParseSudoku(input);
            Algorithm algorithm = new Algorithm(sudoku, 3, 3, "best", 60000, true);
            Console.WriteLine(algorithm.RunAlgorithm().Result.Item2);

            GetTestResults();
            CalculateAverageRuntimes();
        }
        
        /// <summary>
        /// Get the test results
        /// </summary>
        static async void GetTestResults()
        {
            // Get the current path directory
            var directory = Directory.GetCurrentDirectory();
            // Calculate the path for the txt file where the testcases are in
            string newPath = Path.GetFullPath(Path.Combine(directory, @"..\..\..\..\TestFiles\tests.txt"));
            // Read all the testcases
            string[] lines = File.ReadAllLines(newPath);
            // Initialize task list
            var tasks = new List<Task>();
            
            // For each testcase calculate tests
            Parallel.ForEach(Enumerable.Range(0, lines.Length / 2), i =>
            {
                int index = i * 2;
                tasks.Add(GenerateTestResults(lines[index].Replace(" ", string.Empty), lines[index + 1].Split(" "), newPath));
            });
            
            // Wait till all the tests are done
            await Task.WhenAll(tasks);
        }
        
        /// <summary>
        /// Runs test cases 
        /// </summary>
        /// <param name="test">Array of testcase (sudoku)</param>
        /// /// <param name="newPath">Path of file where the test results needs to be add in</param>
        static async Task RunTest(string[] test, string newPath)
        {
            // Declare test features and initialize task list
            string[] improvement = new[] { "best", "first" };
            var tasks = new List<Task<Tuple<long, string, int, int, string>>>();
            
            // Compare each feature value and create a task for it
            int index = 0;
            Parallel.ForEach(Enumerable.Range(1, 40), i =>
            {
                Parallel.ForEach(Enumerable.Range(1, 40), j =>
                {
                    foreach (var type in improvement)
                    {
                        tasks.Add(new Algorithm(ParseHelper.ParseSudoku(test), i, j, type, 6000,false).RunAlgorithm());
                    }
                });
            });
            
            // Write the testresults to a csv file
            using (StreamWriter sw = new StreamWriter(newPath))
            {
                // Add headers to csv file
                string headers = "RunTime,RandomWalkLength,RandomWalkStart,Improvement";
                sw.WriteLine(headers);
                
                foreach (var task in await Task.WhenAll(tasks))
                {
                    if (task.Item1 > 60000)
                    {
                        // Task is not finished write N/E (not executed)
                        sw.WriteLine($"N/E,{task.Item3},{task.Item4},{task.Item5}");
                    }
                    else
                    {
                        // Task is executed write all the feature values
                        sw.WriteLine($"{task.Item1},{task.Item3},{task.Item4},{task.Item5}");
                    }
                }

            }

        }
        
        /// <summary>
        /// Create file for each sudoku testcase and run the test
        /// </summary>
        /// <param name="testName">Name of the testcase</param>
        /// /// <param name="test">Sudoku array of the testcase</param>
        /// /// <param name="path">Path of the testcase</param>
        static async Task GenerateTestResults(string testName, string[] test, string path)
        {
            // Calculate new path for testfile
            string newPath = Path.Combine(path, $@"..\{testName}.csv");
            
            // Initialize file
            InstantiateFile(newPath);
            // Run test
            await RunTest(test, newPath);
        }
        
        /// <summary>
        /// If file exists delete and add it again otherwise just add it</summary>
        /// <param name="newPath">Path of the file</param>
        private static void InstantiateFile(string newPath)
        {
            if (File.Exists(newPath))
            {
                // File exists so delete it
                File.Delete(newPath);
            }
            // Create file
            File.Create(newPath).Close();
        }

        /// <summary>
        /// Gets all csv files in the TestFiles folder and calls ProcessFiles and GenerateAverageTestResults to process the data and write new data in a new file
        /// </summary>
        private static void CalculateAverageRuntimes()
        {
            // Get the current path directory
            var directory = Directory.GetCurrentDirectory();

            // Get path to TestFiles folder
            string newPath = Path.GetFullPath(Path.Combine(directory, @"..\..\..\..\TestFiles"));

            // Get all CSV files in the directory
            string[] csvFiles = Directory.GetFiles(newPath, "*.csv");

            // Call ProcessFiles and store the dictionary
            Dictionary<string, Tuple<double, int>> averageRuntimes = ProcessFiles(csvFiles);

            // Call GenerateAverageTestResults to write the new data
            GenerateAverageTestResults(averageRuntimes, directory);
        }

    /// <summary>
    /// Creates a dictionary with keys as the settings of the algorithm and values as new calculated averages of the run time skipping all N/E
    /// </summary>
    /// <param name="csvFiles">All csvFiles in the TestFiles folder</param>
    /// <returns>A dictionary with the keys being the different settings of the algorithm and the values being the average run time</returns>
    private static Dictionary<string, Tuple<double, int>> ProcessFiles(string[] csvFiles)
    {
        // Creates a dictionary to store average runtimes for each combination of RandomWalkLength, RandomWalkStart, and Algorithm type
        Dictionary<string, Tuple<double, int>> averageRuntimes = new Dictionary<string, Tuple<double, int>>();

        // Loop through each CSV file
        foreach (string csvFile in csvFiles)
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(csvFile);

            // Skip the headers
            var dataLines = lines.Skip(1);

            // Parse each line
            foreach (var line in dataLines)
            {
                // Split the line into words
                string[] words = line.Split(',');

                // Get values
                string runtimeString = words[0];
                int randomWalkLength = int.Parse(words[1]);
                int randomWalkStart = int.Parse(words[2]);
                string algorithm = words[3];

                if (runtimeString == "N/E")
                {
                    continue;
                }

                double runtime = double.Parse(runtimeString);

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
                    // If the key has not been added to the dictionary, add it with the first runtime
                    averageRuntimes[key] = new Tuple<double, int>(runtime, 1);
                }
            }
        }

        return averageRuntimes;
    }

    /// <summary>
    /// Writes info of the given dictionary into a new file
    /// </summary>
    /// <param name="averageRuntimes">A dictionary with keys being the settings of the algorithms and values being the average run times</param>
    /// <param name="directory">The directory path</param>
    private static void GenerateAverageTestResults(Dictionary<string, Tuple<double, int>> averageRuntimes, string directory)
    {
        // Writing path
        string newPath = Path.GetFullPath(Path.Combine(directory, @"..\..\..\..\TestFiles\tests.txt"));
        string path = Path.Combine(newPath, $@"..\Average_RunTime.csv");

        // Create a new CSV file
        InstantiateFile(path);

        using (StreamWriter sw = new StreamWriter(path))
        {
            // Add headers to csv file
            string headers = "Average RunTime,RandomWalkLength,RandomWalkStart,Improvement";
            sw.WriteLine(headers);

            // Write the outputs in the file
            foreach (var output in averageRuntimes)
            {
                sw.WriteLine($"{output.Value.Item1}ms,{output.Key}");
            }
        }

        Console.WriteLine($"Results written to {path}");
    }
    }
}