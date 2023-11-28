using System;

namespace SudokuKiller
{
    class Program
    {
        static void Main()
        {
            string[] input = Console.ReadLine().Split(" ");
            Sudoku sudoku = ParseHelper.ParseSudoku(input);
            
            //int[] actual1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            //ParseHelper.FillNumbers(actual1);

            // int[] actual1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            // Getal[,] actualMiniSudokuList = ParseHelper.FillNumbers(actual1).MiniSudokuList;
            //
            // Getal[,] expected1 = {
            //     { new Getal(1, true) , new Getal(2, true), new Getal(3, true)},
            //     { new Getal(4, true) , new Getal(5, true), new Getal(6, true)},
            //     { new Getal(7, true) , new Getal(8, true), new Getal(9, true)}
            // };

            // for (int i = 0; i < 3; i++)
            // {
            //     for (int j = 0; j < 3; j++)
            //     {
            //         Console.WriteLine(actualMiniSudokuList[i, j].Fixed + "===" + expected1[i, j].Fixed);
            //         Console.WriteLine(actualMiniSudokuList[i, j].Number + "===" + expected1[i, j].Number);
            //     }
            // }
        }
    }
}