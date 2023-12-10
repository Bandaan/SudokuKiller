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
            Console.WriteLine(algorithm.RunAlgoritme());

            //GetTestResults();
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
            Sudoku sudoku = ParseHelper.ParseSudoku(test);
            
            string[] improvement = new[] { "best", "first" };

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    foreach (var type in improvement)
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        Algoritme algorithm = new Algoritme(sudoku, i, j, type);
                        
                        stopwatch.Start();
                        algorithm.RunAlgoritme();
                        stopwatch.Stop();

                        string text = $"\n{stopwatch.ElapsedMilliseconds},{j},{i},{type}";
                        
                        File.AppendAllText(newPath, text);

                    }
                }
            }
        }

        private static void InstatiateFile(string newPath)
        {
            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            File.Create(newPath).Close();

            string headers = "RunTime,RandomWalkLength,RandomWalkStart,Improvement";
            File.WriteAllText(newPath, headers);
        }
    }
}