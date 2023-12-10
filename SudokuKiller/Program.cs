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
            // Algoritme algorithm = new Algoritme(sudoku, 10, 10, "best");
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

        static void GenerateTestResults(string testName, string[] test, string path)
        {
            string newPath = Path.Combine(path, $@"..\{testName}.csv");

            InstatiateFile(newPath);

            string[] improvement = new[] { "best", "first" };

            using (StreamWriter sw = new StreamWriter(newPath))
            {
                string headers = "RunTime,RandomWalkLength,RandomWalkStart,Improvement";
                sw.WriteLine(headers);
                
                for (int i = 5; i < 6; i++)
                {
                    for (int j = 5; j < 6; j++)
                    {
                        foreach (var type in improvement)
                        {
                            long time = GetResult(new Algoritme(ParseHelper.ParseSudoku(test), i, j, type));

                            string text = $"{time},{j},{i},{type}";

                            sw.WriteLine(text);
                        }
                    }
                }
            }
        }

        private static long GetResult(Algoritme algorithm)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            algorithm.RunAlgoritme();
            stopwatch.Stop();
            
            return stopwatch.ElapsedMilliseconds;
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