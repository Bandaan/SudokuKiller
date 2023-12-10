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
        bool improvement;

        public Algoritme(Sudoku sudoku, int randomwalkstart, int randomwalklength, string type)
        {
            this.sudoku = sudoku;
            randomWalkStart = randomwalkstart;
            randomWalkLength = randomwalklength;
            improvement = (type == "best") ? true : false;
        }
        public string RunAlgoritme()
        {
            evalSudoku = InstantiateEval();
            Console.WriteLine($"begin fout: {evalSudoku}");
            while (evalSudoku != 0)
            {
                if (counter == 1000)
                {
                    Console.WriteLine($"echte error {InstantiateEval()} :: onze error {evalSudoku}");
                    Thread.Sleep(10000);
                }
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
            
            return SudokuToString();
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
            Console.WriteLine("walk");
            for (int i = 0; i < randomWalkLength; i++)
            {
                Console.WriteLine("doet nu random swap");
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
            
            punt1.error = new Error(column1, FindError(sudoku.GetColumn(column1)), row1, FindError(sudoku.GetRow(row1)));
            
            int column2 = miniSudoku.column * 3 + punt2.column;
            int row2 = miniSudoku.row * 3 + punt2.row;
            
            punt2.error = new Error(column2, FindError(sudoku.GetColumn(column2)), row2, FindError(sudoku.GetRow(row2)));
            
            int[] tempColumn = (int[])evalColumns.Clone();
            int[] tempRow = (int[])evalRows.Clone();

            SetErrors(evalColumns, evalRows, punt1.error, punt2.error);
            
            // for (int i = 0; i < 9; i++)
            // {
            //     Console.WriteLine(tempColumn[i] + " " + tempRow[i]);
            // }
            // Console.WriteLine("====================");
            
            int result = CombineError(evalColumns, evalRows);
            
            evalColumns = (int[])tempColumn.Clone();
            evalRows = (int[])tempRow.Clone();
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
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            if (miniSudoku.MiniSudokuList[j, i].number != miniSudoku.MiniSudokuList[l, k].number && !miniSudoku.MiniSudokuList[l, k].vast && !miniSudoku.MiniSudokuList[j, i].vast)
                            {
                                Coordinaat getal1 = new Coordinaat(l, k);
                                Coordinaat getal2 = new Coordinaat(j, i);

                                miniSudoku.Swap(getal1, getal2);
                                int newEval = FindEval(getal1, getal2, miniSudoku);

                                // for (int m = 0; m < 9; m++)
                                // {
                                //     Console.WriteLine(evalColumns[m] + " " + evalRows[m]);
                                // }
                                //
                                // Console.WriteLine("====================");
                                //Thread.Sleep((3000));
                                
                                if (newEval < smallestElement.eval)
                                {
                                    smallestElement = new Swap(newEval, getal1, getal2);
                                }
                                miniSudoku.Swap(getal2, getal1);
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