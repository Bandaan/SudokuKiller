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
            FindEval(null, null, null);

            while (evalSudoku != 0)
            {
                //Select random miniSudoku
                MiniSudoku miniSudoku = this.sudoku.SudokuList[rnd.Next(3), rnd.Next(3)];

                //Do all permutations and add them to evalCell
                //Get the (first) smallest mistake from evalCell and the according swap
                Swap smallestSwap = SwapSuggest(miniSudoku);

                if (smallestSwap.eval <= this.evalSudoku)
                {
                    //Do the according swap
                    //Update evalSudoku to this smallest evalCell
                    this.evalSudoku = smallestSwap.eval;

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

        private void FindEval(Tuple<int> cell1, Tuple<int> cell2, MiniSudoku miniSudoku)
        {
            if (cell1 != null && cell2 != null && miniSudoku != null)
            {
                //Update de fout

            }
            else
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

                this.evalSudoku = CombineError();
            }
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

        private int CombineError()
        {
            int error = 0;
            for (int i = 0; i < evalColumns.Length; i++)
            {
                error += evalColumns[i];
            }

            for (int j = 0; j < evalRows.Length; j++)
            {
                error += evalRows[j];
            }

            return error;
        }

        private Swap SwapSuggest(MiniSudoku miniSudoku)
        {
            //Doe alle swaps en als de fout kleniner is dan vorige dan voegen we deze fout toe als object
            Swap smallestTuple;

            //return smallestSwap;
            return new Swap(0, new Tuple<int, int>(0, 0), new Tuple<int, int>(0, 0));
        }

        private string SudokuToString()
        {
            string sudokuString = "";

            for (int i = 0; i < 9; i++)
            {
                int[] row = this.sudoku.GetRow(i);
                foreach (int j in row)
                {
                    sudokuString += $"{j} ";
                }
                sudokuString += "\n";
            }

            return sudokuString;
        }
    }
}