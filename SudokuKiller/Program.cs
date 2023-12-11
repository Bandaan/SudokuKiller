using System;
using System.Diagnostics;

namespace SudokuKiller
{
    class Program
    {
        static void Main()
        {
            string[] input = Console.ReadLine().Split(" ");
            Sudoku sudoku = ParseHelper.ParseSudoku(input);
            Algoritme algorithm = new Algoritme(sudoku, 5, 2, "best");
            Console.WriteLine(algorithm.RunAlgoritme().Result.Item2);

            // GetTestResults();
        }

        static void GetTestResults()
        {
            var directory = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(directory, @"..\..\..\..\TestFiles\tests.txt"));

            string[] lines = File.ReadAllLines(newPath);
            for (int i = 0; i < lines.Length; i += 2)
            {
                GenerateTestResults(lines[i].Replace(" ", string.Empty), lines[i + 1].Split(" "), newPath);
            }
        }
        
        static async Task RunTest(string[] test, string newPath)
        {
            string[] improvement = new[] { "best", "first" };
            var tasks = new List<Task<Tuple<long, string, int, int, string>>>();

            int index = 0;
            Parallel.ForEach(Enumerable.Range(1, 40), i =>
            {
                Parallel.ForEach(Enumerable.Range(1, 40), j =>
                {
                    foreach (var type in improvement)
                    {
                        tasks.Add(new Algoritme(ParseHelper.ParseSudoku(test), i, j, type).RunAlgoritme());
                    }
                });
            });
            
            using (StreamWriter sw = new StreamWriter(newPath))
            {
                string headers = "RunTime,RandomWalkLength,RandomWalkStart,Improvement";
                sw.WriteLine(headers);
                
                foreach (var task in await Task.WhenAll(tasks))
                {
                    if (task.Item1 > 60000)
                    {
                        sw.WriteLine($"N/E,{task.Item3},{task.Item4},{task.Item5}");
                    }
                    else
                    {
                        sw.WriteLine($"{task.Item1},{task.Item3},{task.Item4},{task.Item5}");
                    }
                }

            }

        }

        static async void GenerateTestResults(string testName, string[] test, string path)
        {
            string newPath = Path.Combine(path, $@"..\{testName}.csv");

            InstatiateFile(newPath);
            await RunTest(test, newPath);
        }
        

        private static void InstatiateFile(string newPath)
        {
            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            File.Create(newPath).Close();

            string headers = "RunTime,RandomWalkLength,RandomWalkStart,Improvement";
        }
    }
}