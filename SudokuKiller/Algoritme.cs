using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime;

namespace SudokuKiller
{
    public class Algoritme
    {
        Sudoku sudoku;
        int evalSudoku = -1;
        int[] evalColumns = new int[9];
        int[] evalRows = new int[9];
        Random rnd;
        int randomWalkLength;
        int counter = 0;
        int randomWalkStart;

        public Algoritme(Sudoku sudoku, int lengte)
        {
            this.sudoku = sudoku;
            randomWalkLength = lengte;
            randomWalkStart = 15;
        }
        public string RunAlgoritme()
        {
            evalSudoku = InstantiateEval();
            
            Console.WriteLine($"begin fout: {evalSudoku}");
            while (evalSudoku != 0)
            {
                MiniSudoku miniSudoku = sudoku.GetRandomMiniSudoku();
                Swap smallestSwap = SwapSuggest(miniSudoku);
                
                if (smallestSwap.eval <= evalSudoku)
                {
                    Console.WriteLine($"Kleiner swap ={smallestSwap.eval} evalsudoku = {evalSudoku}");

                    if (smallestSwap.eval == evalSudoku)
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }

                    evalSudoku = smallestSwap.eval;
                    miniSudoku.Swap(smallestSwap.pos1, smallestSwap.pos2);
                    SetErrors(smallestSwap.error);
                }
                else
                {
                    Console.WriteLine($"Groter swap ={smallestSwap.eval} evalsudoku = {evalSudoku}");

                    counter++;
                }

                if (counter >= randomWalkStart)
                {
                    Console.WriteLine("random swap");
                    counter = 0;
                    RandomWalk();
                }
            }
            
            return SudokuToString();
        }

        private void SetErrors(Error newEval)
        {
            SetColumnErrors(newEval.column1, newEval.errorColumn1);
            SetColumnErrors(newEval.column2, newEval.errorColumn2);
            
            SetRowErrors(newEval.row1, newEval.errorRow1);
            SetRowErrors(newEval.row2, newEval.errorRow2);
        }

        private void SetColumnErrors(int i, int error)
        {
            evalColumns[i] = error;
        }

        private void SetRowErrors(int i, int error)
        {
            evalRows[i] = error;
        }

        private void RandomWalk()
        {
            Console.WriteLine("walk");
            for (int i = 0; i < randomWalkLength; i++)
            {
                Console.WriteLine("doet nu random swap");
                MiniSudoku miniSudoku = sudoku.GetRandomMiniSudoku();
                RandomSwap(miniSudoku);
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

        private Error FindEval(Coordinaat punt1, Coordinaat punt2, MiniSudoku miniSudoku)
        {
            int[] tempArrayColumns = new int[evalColumns.Length];
            int[] tempArrayRows = new int[evalRows.Length];

            Array.Copy(evalColumns, tempArrayColumns, evalColumns.Length);
            Array.Copy(evalRows, tempArrayRows, evalRows.Length);

            int column1 = miniSudoku.x_pos * 3 + punt1.column;
            int column2 = miniSudoku.x_pos * 3 + punt2.column;
            int row1 = miniSudoku.y_pos * 3 + punt1.row;
            int row2 = miniSudoku.y_pos * 3 + punt2.row;

            // Update only if the columns are different
            if (punt1.column != punt2.column)
            {
                tempArrayColumns[column1] = FindError(sudoku.GetColumn(column1));
                tempArrayColumns[column2] = FindError(sudoku.GetColumn(column2));
            }

            // Update only if the rows are different
            if (punt1.row != punt2.row)
            {
                tempArrayRows[row1] = FindError(sudoku.GetRow(row1));
                tempArrayRows[row2] = FindError(sudoku.GetRow(row2));
            }

            return new Error(CombineError(tempArrayColumns, tempArrayRows), column1, column2, row1, row2, tempArrayColumns[column1], tempArrayColumns[column2], tempArrayRows[row1], tempArrayRows[row2]);
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
            
            Swap smallestElement = new Swap(int.MaxValue, null, null, null);
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            if (miniSudoku.MiniSudokuList[j, i].number != miniSudoku.MiniSudokuList[l, k].number && !miniSudoku.MiniSudokuList[l, k].vast && !miniSudoku.MiniSudokuList[j, i].vast)
                            {
                                Coordinaat getal1 = new Coordinaat(l, k);
                                Coordinaat getal2 = new Coordinaat(j, i);
                                
                                miniSudoku.Swap(getal1, getal2);
                                Error newEval = FindEval(getal1, getal2, miniSudoku);

                                if (newEval.eval < smallestElement.eval)
                                {
                                    smallestElement = new Swap(newEval.eval, newEval, getal1, getal2);
                                }
                                
                                miniSudoku.Swap(getal2, getal1);
                            }
                        }
                    }
                }
            }

            return smallestElement;
        }

        private void RandomSwap(MiniSudoku miniSudoku)
        {
            Tuple<Coordinaat, Coordinaat> swap = miniSudoku.GetRandomSwap();
            miniSudoku.Swap(swap.Item1, swap.Item2);

            Error newEval = FindEval(swap.Item1, swap.Item2, miniSudoku);
            evalSudoku = newEval.eval;
            SetErrors(newEval);
        }

        private string SudokuToString()
        {
            string sudokuString = "";

            for (int i = 0; i < 9; i++)
            {
                int[] row = sudoku.GetRow(i);
                foreach (int j in row)
                {
                    sudokuString += $" {j}";
                }
                sudokuString += "\n";
            }

            return sudokuString;
        }
    }
}