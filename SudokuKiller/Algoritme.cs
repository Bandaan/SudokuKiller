using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices.JavaScript;

namespace SudokuKiller
{
    public class Algoritme
    {
        Sudoku sudoku;
        int evalSudoku = -1;
        int[] evalColumns = new int[9];
        int[] evalRows = new int[9];
        int randomWalkLength;
        int counter = 0;
        int randomWalkStart;
        bool improvement;
        Stopwatch stopwatch = new Stopwatch();
        private int number;

        public Algoritme(Sudoku sudoku, int randomwalkstart, int randomwalklength, string type, int poep)
        {
            this.sudoku = sudoku;
            randomWalkStart = randomwalkstart;
            randomWalkLength = randomwalklength;
            improvement = (type == "best") ? true : false;
            number = poep;
        }
        public async Task<Tuple<long, string>> RunAlgoritme()
        {
            Console.WriteLine($"runt met {randomWalkLength} en {randomWalkStart}");
            
            stopwatch.Start();
            evalSudoku = InstantiateEval();
            
            while (evalSudoku != 0)
            {
                if (stopwatch.ElapsedMilliseconds > 60000)
                {
                    Console.WriteLine("duurt langer dan 60");
                    return await Task.FromResult(new Tuple<long, string>(stopwatch.ElapsedMilliseconds, SudokuToString()));
                }
                
                MiniSudoku miniSudoku = sudoku.GetRandomMiniSudoku();
                Swap smallestSwap = SwapSuggest(miniSudoku);

                if (smallestSwap.eval <= evalSudoku)
                {
                    if (smallestSwap.eval == evalSudoku)
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }
                    
                    miniSudoku.Swap(smallestSwap.pos1, smallestSwap.pos2);
                    
                    
                    SetErrors(evalColumns, evalRows, smallestSwap.pos1.error, smallestSwap.pos2.error);
                    evalSudoku = smallestSwap.eval;
                }
                else
                {
                    counter++;
                }

                if (counter >= randomWalkStart)
                {
                    counter = 0;
                    RandomWalk();
                }
            }
            
            stopwatch.Stop();
            
            return await Task.FromResult(new Tuple<long, string>(stopwatch.ElapsedMilliseconds, SudokuToString()));
        }

        private void SetErrors(int[] columnError, int[] rowError, Error punt1, Error punt2)
        {
            columnError[punt1.columIndex] = punt1.columnError;
            columnError[punt2.columIndex] = punt2.columnError;

            rowError[punt1.rowIndex] = punt1.rowError;
            rowError[punt2.rowIndex] = punt2.rowError;
        }

        private void RandomWalk()
        {
            for (int i = 0; i < randomWalkLength; i++)
            {
                MiniSudoku miniSudoku = sudoku.GetRandomMiniSudoku();
                Swap swap = RandomSwap(miniSudoku);
                
                miniSudoku.Swap(swap.pos1, swap.pos2);
                
                SetErrors(evalColumns, evalRows, swap.pos1.error, swap.pos2.error);
                evalSudoku = swap.eval;
                
            }
        }

        private int InstantiateEval()
        {
            for (int i = 0; i < 9; i++)
            {
                evalColumns[i] = FindError(sudoku.GetColumn(i));
                evalRows[i] = FindError(sudoku.GetRow(i));
            }
            return CombineError(evalColumns, evalRows);
        }
        
        private int CombineError(int[] arrayColumns, int[] arrayRows)
        {
            int error = 0;
            for (int i = 0; i < arrayColumns.Length; i++)
            {
                error += arrayColumns[i];
                error += arrayRows[i];
            }

            return error;
        }

        private int FindEval(Coordinaat punt1, Coordinaat punt2, MiniSudoku miniSudoku)
        {
            int column1 = miniSudoku.column * 3 + punt1.column;
            
            int row1 = miniSudoku.row * 3 + punt1.row;
            
            punt1.error = new Error(row1, FindError(sudoku.GetColumn(row1)), column1, FindError(sudoku.GetRow(column1)));
            
            int column2 = miniSudoku.column * 3 + punt2.column;
            int row2 = miniSudoku.row * 3 + punt2.row;
            
            punt2.error = new Error(row2, FindError(sudoku.GetColumn(row2)), column2, FindError(sudoku.GetRow(column2)));

            int[] tempColumn = (int[])evalColumns.Clone();
            int[] tempRow = (int[])evalRows.Clone();
            
            SetErrors(tempColumn, tempRow, punt1.error, punt2.error);
            
            int result = CombineError(tempColumn, tempRow);
            
            
            return result;

        }

        public static int FindError(int[] array)
        {
            List<int> tempArray = new List<int>();

            for (int i = 0; i < array.Length; i++)
            {
                if (!tempArray.Contains(array[i]))
                {
                    tempArray.Add(array[i]);
                }
            }

            return 9-tempArray.Count;
        }
        
        private Swap SwapSuggest(MiniSudoku miniSudoku)
        {
            Swap smallestElement = new Swap(int.MaxValue, null, null);
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (miniSudoku.MiniSudokuList[j, i].vast)
                    {
                        continue;
                    }
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            if (miniSudoku.MiniSudokuList[j, i].number != miniSudoku.MiniSudokuList[l, k].number && !miniSudoku.MiniSudokuList[l, k].vast)
                            {
                                Coordinaat getal1 = new Coordinaat(l, k);
                                Coordinaat getal2 = new Coordinaat(j, i);

                                miniSudoku.Swap(getal1, getal2);
                                int newEval = FindEval(getal1, getal2, miniSudoku);
                                miniSudoku.Swap(getal2, getal1);

                                if (!improvement && newEval < evalSudoku)
                                {
                                    return new Swap(newEval, getal1, getal2);
                                }
                                
                                if (newEval < smallestElement.eval)
                                {
                                    smallestElement = new Swap(newEval, getal1, getal2);
                                }
                            }
                        }
                    }
                }
            }

            return smallestElement;
        }

        private Swap RandomSwap(MiniSudoku miniSudoku)
        {
            Tuple<Coordinaat, Coordinaat> swap = miniSudoku.GetRandomSwap();
            
            int eval = FindEval(swap.Item1, swap.Item2, miniSudoku);

            return new Swap(eval, swap.Item1, swap.Item2);

        }

        private string SudokuToString()
        {
            string sudokuString = "";

            for (int i = 0; i < 9; i++)
            {
                int[] row = sudoku.GetRow(i);
                for (int j = 0; j < row.Length; j++)
                {
                    if (j == 0)
                    {
                        sudokuString += row[j];
                    }
                    else
                    {
                        sudokuString += $" {row[j]}";
                    }
                }
                sudokuString += "\n";
            }

            return sudokuString;
        }
    }
}