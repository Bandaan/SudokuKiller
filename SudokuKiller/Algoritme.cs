using System;
using System.Linq;
using System.Collections.Generic;

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

        public Algoritme(Sudoku sudoku, int randomWalkLength)
        {
            this.sudoku = sudoku;
            randomWalkLength = randomWalkLength;
            randomWalkStart = 300;
        }
        public string RunAlgoritme()
        {
            evalSudoku = InstantiateEval();

            while (evalSudoku != 0)
            {
                MiniSudoku miniSudoku = sudoku.GetRandomMiniSudoku();
                Swap smallestSwap = SwapSuggest(miniSudoku);

                if (smallestSwap.eval < evalSudoku)
                {
                    Console.WriteLine($"swap ={smallestSwap.eval} evalsudoku = {evalSudoku}");
                    evalSudoku = smallestSwap.eval;
                    
                    miniSudoku.Swap(smallestSwap.pos_1, smallestSwap.pos_2);

                    evalColumns[smallestSwap.error.column_1] = smallestSwap.error.errorColumn_1;
                    evalColumns[smallestSwap.error.column_2] = smallestSwap.error.errorColumn_2;
                    
                    evalRows[smallestSwap.error.row_1] = smallestSwap.error.errorRow_1;
                    evalRows[smallestSwap.error.row_2] = smallestSwap.error.errorRow_2;
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
            
            return SudokuToString();
        }

        private void RandomWalk()
        {
            Console.WriteLine("walk");
            for (int i = 0; i < randomWalkLength; i++)
            {
                MiniSudoku miniSudoku = sudoku.GetRandomMiniSudoku();
                randomSwap(miniSudoku);
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

        private Tuple<int, Error> FindEval(Tuple<int, int> cell1, Tuple<int, int> cell2, MiniSudoku miniSudoku)
        {
            //Update de fout

            int[] temp_array_columns = new int[evalColumns.Length];
            Array.Copy(evalColumns, temp_array_columns, evalColumns.Length);
            int[] temp_array_rows = new int[evalRows.Length];
            Array.Copy(evalRows, temp_array_rows, evalRows.Length);

            int column_1 = miniSudoku.x_pos*3 + cell1.Item1;
            int column_2 = miniSudoku.x_pos*3 + cell2.Item1;
            int row_1 = miniSudoku.y_pos*3 + cell1.Item2;
            int row_2 = miniSudoku.y_pos*3 + cell2.Item2;

            //X is hetzelfde dus 1 column en 2 rows checken
            if (cell1.Item1 == cell2.Item1)
            {
                temp_array_columns[column_1] = FindError(this.sudoku.GetColumn(column_1));
                temp_array_rows[row_1] = FindError(this.sudoku.GetColumn(row_1));
                temp_array_rows[row_2] = FindError(this.sudoku.GetColumn(row_2));
            }
            //Y is hetzelfde dus 2 columns en 1 row checken
            else if (cell1.Item2 == cell2.Item2)
            {
                temp_array_columns[column_1] = FindError(this.sudoku.GetColumn(column_1));
                temp_array_columns[column_2] = FindError(this.sudoku.GetColumn(column_2));
                temp_array_rows[row_1] = FindError(this.sudoku.GetColumn(row_1));
            }
            //Beide niet hetzelfde dus 2 columns en 2 rows checken
            else
            {
                temp_array_columns[column_1] = FindError(this.sudoku.GetColumn(column_1));
                temp_array_columns[column_2] = FindError(this.sudoku.GetColumn(column_2));
                temp_array_rows[row_1] = FindError(this.sudoku.GetColumn(row_1));
                temp_array_rows[row_2] = FindError(this.sudoku.GetColumn(row_2));
            }
            Error error = new Error(column_1,column_2,row_1,row_2,temp_array_columns[column_1],temp_array_columns[column_2],temp_array_rows[row_1],temp_array_rows[row_2]);
            return new Tuple<int, Error>(CombineError(temp_array_columns, temp_array_rows),error);
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
                    if (miniSudoku.MiniSudokuList[j,i].vast)
                    {
                        continue;
                    }
                    for (int k = i; k < 3; k++)
                    {
                        for (int l = j; l < 3; l++)
                        {
                            if (miniSudoku.MiniSudokuList[j, i] != miniSudoku.MiniSudokuList[l, k] || !miniSudoku.MiniSudokuList[l,k].vast)
                            {
                                miniSudoku.Swap(new Tuple<int, int>(l, k), new Tuple<int, int>(j, i));
                                
                                Tuple<int, Error> new_eval = FindEval(new Tuple<int, int>(j, i), new Tuple<int, int>(l, k), miniSudoku);
                                int error = InstantiateEval();
                                
                                if (error < smallestElement.eval)
                                {
                                    smallestElement = new Swap(error, new_eval.Item2, new Tuple<int, int>(l, k), new Tuple<int, int>(j, i));
                                }
                                // Swap the two back to their old position in the miniSudoku
                                miniSudoku.Swap(new Tuple<int, int>(l, k), new Tuple<int, int>(j, i));
                            }
                        }
                    }
                }
            }

            return smallestElement;
        }

        private void randomSwap(MiniSudoku miniSudoku)
        {
            int column_1 = 0, column_2 = 0, row_1 = 0, row_2 = 0;

            while (column_1 == column_2 && row_1 == row_2 || miniSudoku.MiniSudokuList[row_1, column_1].vast || miniSudoku.MiniSudokuList[row_2, column_2].vast)
            {
                column_1 = rnd.Next(3);
                column_2 = rnd.Next(3);
                row_1 = rnd.Next(3);
                row_2 = rnd.Next(3);
            }

            Getal temp_getal = miniSudoku.MiniSudokuList[row_2, column_2];
            miniSudoku.MiniSudokuList[row_2, column_2] = miniSudoku.MiniSudokuList[row_1,column_1];
            miniSudoku.MiniSudokuList[row_1, column_1] = temp_getal;

            Tuple<int, Error> new_eval = FindEval(new Tuple<int, int>(row_1, column_1), new Tuple<int, int>(row_2, column_2), miniSudoku);
            this.evalSudoku = new_eval.Item1;

            //Also update the mistakes in evalColumns and evalRows
            this.evalColumns[new_eval.Item2.column_1] = new_eval.Item2.errorColumn_1;
            this.evalColumns[new_eval.Item2.column_2] = new_eval.Item2.errorColumn_2;
            this.evalRows[new_eval.Item2.row_1] = new_eval.Item2.errorRow_1;
            this.evalRows[new_eval.Item2.row_2] = new_eval.Item2.errorRow_2;
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