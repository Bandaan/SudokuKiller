using System;
using System.Linq;
using System.Collections.Generic;

namespace SudokuKiller
{
    public class Algoritme
    {
        int evalSudoku;
        Random rnd;

        public Algoritme()
        {
            evalSudoku = -1;
            rnd = new Random();
        }

        //Runt het algoritme en verandert het naar een string om uit te printen
        public string RunAlgoritme()
        {
            VindFout(null, null);

            while (evalSudoku != 0)
            {
                //Select random miniSudoku
                MiniSudoku miniSudoku = Sudoku.SudokuList[rnd.Next(3)][rnd.Next(3)];

                //Do all permutations and add them to evalCell
                //Get the (first) smallest mistake from evalCell and the according swap
                Tuple<int> smallestTuple = Swap(miniSudoku);

                //Do the according swap

                //Update evalSudoku to this smallest evalCell
                evalSudoku = smallestTuple.Item1;
            }

            //Nu is de fout 0 en willen we de sudoku omzetten in een string
            return SudokuToString();
        }

        public void VindFout(Tuple<int> cell1, Tuple<int> cell2)
        {
            if (cell1 != null && cell2 != null)
            {
                //Update de fout !!MAAR HOE????!!
            }
            else
            {
                //Vind de fout van de hele sudoku !!ALLEEN DE EERSTE KEER!!
            }
        }

        public Tuple<int> Swap(MiniSudoku miniSudoku)
        {
            List<Tuple<int>> evalCell = new List<Tuple<int>>();

            //Doe alle swaps en voeg de fouten + swap toe aan evalCell


            //Daarna de kleinste fout vinden en die tuple sturen
            List<Tuple<int>> sortedList = evalCell.OrderBy(tuple => tuple.Item1).ToList();
            Tuple<int> smallestTuple = sortedList.First();

            return smallestTuple;
        }

        public string SudokuToString()
        {
            return "";
        }
    }
}