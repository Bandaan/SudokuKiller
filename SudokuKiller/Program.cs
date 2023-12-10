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
            Algoritme algorithm = new Algoritme(sudoku, 10000000, 2, "best");
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

        static async Task GenerateTestResults(string testName, string[] test, string path)
        {
            string newPath = Path.Combine(path, $@"..\{testName}.csv");
            
            InstatiateFile(newPath);
            Sudoku sudoku = ParseHelper.ParseSudoku(test);
            
            string[] improvement = new[] { "best", "first" };
            int MaxWalkLength = 20;
            int MaxWalkStart = 20;

            Task<string>[] tasksArray = new Task<string>[MaxWalkLength * MaxWalkStart * 2 + 1];

            int index = 0;
            for (int i = 5; i < MaxWalkStart; i++)
            {
                for (int j = 2; j < MaxWalkLength; j++)
                {
                    foreach (var type in improvement)
                    {
                        tasksArray[index] = Task.Run(() => RunTask(sudoku, i, j, type));
                        index++;
                    }
                }
            }
            
            //string[] results = Task.WaitAll(tasksArray);
        }

        private static string RunTask(Sudoku sudoku, int i, int j, string type)
        {
            Stopwatch sw = new Stopwatch();
            Algoritme algorithm = new Algoritme(sudoku, i, j, type);
            
            sw.Start();
            algorithm.RunAlgoritme();
            sw.Stop();

            return $"{sw.ElapsedMilliseconds},{i},{j},{type}";

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