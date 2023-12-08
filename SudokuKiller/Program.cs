using System;

namespace SudokuKiller
{
    class Program
    {
        static void Main()
        {
            string[] input = Console.ReadLine().Split(" ");
            Sudoku sudoku = ParseHelper.ParseSudoku(input);
            Algoritme algorithm = new Algoritme(sudoku, 3);
            Console.WriteLine(algorithm.RunAlgoritme());
        }
    }
}