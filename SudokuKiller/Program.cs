using System;
using System.Diagnostics;

namespace SudokuKiller
{
    class Program
    {
        /// <summary>
        /// Main class where you can solve sudoku
        /// </summary>
        
        static void Main()
        {
            // string[] input = Console.ReadLine().Split(" ");
            // Sudoku sudoku = ParseHelper.ParseSudoku(input);
            // Algoritme algorithm = new Algoritme(sudoku, 3, 3, "best", 60000, true);
            // Console.WriteLine(algorithm.RunAlgoritme().Result.Item2);

            GetTestResults();
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
                        tasks.Add(new Algoritme(ParseHelper.ParseSudoku(test), i, j, type, 6000,false).RunAlgoritme());
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
            InstatiateFile(newPath);
            // Run test
            await RunTest(test, newPath);
        }
        
        /// <summary>
        /// If file exists delete and add it again otherwise just add it</summary>
        /// <param name="newPath">Path of the file</param>
        private static void InstatiateFile(string newPath)
        {
            if (File.Exists(newPath))
            {
                // File exists so delete it
                File.Delete(newPath);
            }
            // Create file
            File.Create(newPath).Close();
        }
    }
}