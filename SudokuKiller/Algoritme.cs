using System;
using System.Linq;
using System.Collections.Generic;

namespace SudokuKiller
{
    public class Algoritme
    {
        Sudoku sudoku;
        int evalSudoku;
        int[] evalColumns;
        int[] evalRows;
        Random rnd;

        public Algoritme(Sudoku sudoku)
        {
            this.sudoku = sudoku;
            this.evalSudoku = -1;
            this.evalColumns = new int[9];
            this.evalRows = new int[9];
            this.rnd = new Random();
        }

        //Runt het algoritme en verandert het naar een string om uit te printen
        public string RunAlgoritme()
        {
            evalSudoku = InstantiateEval();

            while (evalSudoku != 0)
            {
                //Select random miniSudoku
                MiniSudoku miniSudoku = this.sudoku.SudokuList[rnd.Next(3), rnd.Next(3)];

                //Do all permutations and suggest a swap
                Swap smallestSwap = SwapSuggest(miniSudoku);

                //Make swap if it is an improvement else we go on and increment a counter
                if (smallestSwap.eval <= this.evalSudoku)
                {
                    //Do the according swap
                    Getal temp_getal = miniSudoku.MiniSudokuList[smallestSwap.pos_2.Item1,smallestSwap.pos_2.Item2];
                    miniSudoku.MiniSudokuList[smallestSwap.pos_2.Item1,smallestSwap.pos_2.Item1] = miniSudoku.MiniSudokuList[smallestSwap.pos_1.Item1,smallestSwap.pos_1.Item2];
                    miniSudoku.MiniSudokuList[smallestSwap.pos_1.Item1,smallestSwap.pos_1.Item2] = temp_getal;

                    //Update evalSudoku to this smallest evalCell
                    this.evalSudoku = smallestSwap.eval;

                    //Also update the mistakes in evalColumns and evalRows
                    this.evalColumns[smallestSwap.error.column_1] = smallestSwap.error.errorColumn_1;
                    this.evalColumns[smallestSwap.error.column_2] = smallestSwap.error.errorColumn_2;
                    this.evalRows[smallestSwap.error.row_1] = smallestSwap.error.errorRow_1;
                    this.evalRows[smallestSwap.error.row_2] = smallestSwap.error.errorRow_2;

                    //If it is equal to evalSudoku we need to add to a counter so that we're not stuck on a plateau
                }
                else
                {
                    //In deze minisudoku is er geen swap die de toestand verbeterd dus random walk of een counter?
                }
            }

            //Nu is de fout 0 en willen we de sudoku omzetten in een string
            return SudokuToString();
        }

        private int InstantiateEval()
        {
            //Vind de fout van de hele sudoku

                //Loop door alle kolommen
                for (int i = 0; i < 9; i++)
                {
                    int fout = FindError(this.sudoku.GetColumn(i));
                    evalColumns[i] = fout;
                }

                //Loop door alle rows
                for (int i = 0; i < 9; i++)
                {
                    int fout = FindError(this.sudoku.GetRow(i));
                    evalRows[i] = fout;
                }

                return CombineError(evalColumns, evalRows);
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
            //Vind aantal missende getallen in de array en return die
            List<int> temp_array = new List<int>();

            for (int i = 0; i < array.Length; i++)
            {
                if (!temp_array.Contains(array[i]))
                {
                    temp_array.Add(array[i]);
                }
            }

            return 9-temp_array.Count;
        }

        private int CombineError(int[] arrayColumns, int[] arrayRows)
        {
            int error = 0;
            for (int i = 0; i < arrayColumns.Length; i++)
            {
                error += arrayColumns[i];
            }

            for (int j = 0; j < arrayRows.Length; j++)
            {
                error += arrayRows[j];
            }

            return error;
        }

        private Swap SwapSuggest(MiniSudoku miniSudoku)
        {
            //Doe alle swaps en als de fout kleniner is dan vorige dan voegen we deze fout toe als object
            Swap smallestElement = null;

            //Loop through all y's for 1st element
            for (int i = 0; i < 3; i++)
            {
                //Loop through all x's for 1st element
                for (int j = 0; j < 3; j++)
                {
                    //Check if it's not a fixed element
                    if (miniSudoku.MiniSudokuList[j,i].Fixed)
                    {
                        continue;
                    }
                    //Loop through all y's for 2nd element
                    for (int k = i; k < 3; k++)
                    {
                        //Loop through all x's for 2nd element
                        for (int l = j; l < 3; l++)
                        {
                            //Check if it's not the same element
                            if (miniSudoku.MiniSudokuList[j,i] == miniSudoku.MiniSudokuList[l,k])
                            {
                                continue;
                            }
                            //Check if it's not a fixed element
                            else if (miniSudoku.MiniSudokuList[l,k].Fixed)
                            {
                                continue;
                            }
                            else
                            {
                                //Swap the two their position in the miniSudoku
                                Getal temp_getal = miniSudoku.MiniSudokuList[l,k];
                                miniSudoku.MiniSudokuList[l,k] = miniSudoku.MiniSudokuList[j,i];
                                miniSudoku.MiniSudokuList[j,i] = temp_getal;

                                //Calculate the mistake
                                Tuple<int, Error> new_eval = FindEval(new Tuple<int, int>(j,i), new Tuple<int, int>(l,k), miniSudoku);

                                if (smallestElement == null)
                                {
                                    smallestElement = new Swap(new_eval.Item1, new_eval.Item2, new Tuple<int, int>(j,i), new Tuple<int, int>(l,k));
                                }
                                else if (new_eval.Item1 < smallestElement.eval)
                                {
                                    smallestElement = new Swap(new_eval.Item1, new_eval.Item2, new Tuple<int, int>(j,i), new Tuple<int, int>(l,k));
                                }

                                //Swap the two back to their old position in the miniSudoku
                                Getal temp_getal_2 = miniSudoku.MiniSudokuList[l,k];
                                miniSudoku.MiniSudokuList[l,k] = miniSudoku.MiniSudokuList[j,i];
                                miniSudoku.MiniSudokuList[j,i] = temp_getal;
                            }
                        }
                    }
                }
            }

            return smallestElement;
        }

        private string SudokuToString()
        {
            string sudokuString = "";

            for (int i = 0; i < 9; i++)
            {
                int[] row = this.sudoku.GetRow(i);
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