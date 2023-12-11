using System;
using System.Diagnostics;

namespace SudokuKiller
{
    class Program
    {
        static void Main()
        {
            // string[] input = Console.ReadLine().Split(" ");
            // Sudoku sudoku = ParseHelper.ParseSudoku(input);
            // Algoritme algorithm = new Algoritme(sudoku, 0, 0, "best", 0);
            // Console.WriteLine(algorithm.RunAlgoritme());

            GetTestResults();
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
        
        static async Task<Tuple<long, string>>[] Run(string[] test)
        {
            string[] improvement = new[] { "best", "first" };
            var tasks = new List<Task<Tuple<long, string>>>();

            int index = 0;
            Parallel.ForEach(Enumerable.Range(1, 1), i =>
            {
                Parallel.ForEach(Enumerable.Range(1, 1), j =>
                {
                    foreach (var type in improvement)
                    {
                        tasks.Add(new Algoritme(ParseHelper.ParseSudoku(test), i, j, type, index).RunAlgoritme());
                        index++;
                    }
                });
            });
            
            return await Task.WhenAll(tasks);
            
        }

        static async void GenerateTestResults(string testName, string[] test, string path)
        {
            string newPath = Path.Combine(path, $@"..\{testName}.csv");

            InstatiateFile(newPath);
            await Task<Tuple<long, string>>[] tasks = Run(test);
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