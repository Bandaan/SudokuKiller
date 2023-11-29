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
            FindEval(null, null);

            while (evalSudoku != 0)
            {
                //Select random miniSudoku
                MiniSudoku miniSudoku = this.sudoku.SudokuList[rnd.Next(3), rnd.Next(3)];

                //Do all permutations and add them to evalCell
                //Get the (first) smallest mistake from evalCell and the according swap
                Tuple<int> smallestTuple = Swap(miniSudoku);

                //Do the according swap

                //Update evalSudoku to this smallest evalCell
                this.evalSudoku = smallestTuple.Item1;
            }

            //Nu is de fout 0 en willen we de sudoku omzetten in een string
            return SudokuToString();
        }

        private void FindEval(Tuple<int> cell1, Tuple<int> cell2)
        {
            if (cell1 != null && cell2 != null)
            {
                //Update de fout !!MAAR HOE????!!
            }
            else
            {
                //Vind de fout van de hele sudoku !!ALLEEN DE EERSTE KEER!!

                //Loop door alle kolommen
                for (int i = 0; i < 9; i++)
                {
                    //int fout = FindError(this.sudoku.GetCollumn(i));
                    //evalColumns[i] = fout;
                }

                //Loop door alle rows
                for (int i = 0; i < 9; i++)
                {
                    //int fout = FindError(this.sudoku.GetRow(i));
                    //evalRows[i] = fout;
                }
            }
        }

        public static int FindError(int[] array)
        {
            //Vind aantal dubbele in de array en return die
            int duplicates = 0;

            for (int i = 0; i < array.Length; i++)
            {
                int[] temp_array = array;
                temp_array[i] = 0;

                if (temp_array.Contains(array[i]))
                {
                    duplicates++;
                }
            }

            return duplicates;
        }

        private Tuple<int> Swap(MiniSudoku miniSudoku)
        {
            List<Tuple<int>> evalCell = new List<Tuple<int>>();

            //Doe alle swaps en voeg de fouten + swap toe aan evalCell


            //Daarna de kleinste fout vinden en die tuple sturen
            List<Tuple<int>> sortedList = evalCell.OrderBy(tuple => tuple.Item1).ToList();
            Tuple<int> smallestTuple = sortedList.First();

            return smallestTuple;
        }

        private string SudokuToString()
        {
            return "";
        }
    }
}