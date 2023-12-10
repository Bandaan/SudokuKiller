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
                MiniSudoku miniSudoku = sudoku.GetRandomMiniSudoku();
                Swap smallestSwap = SwapSuggest(miniSudoku);
                
                Console.WriteLine($"Kleiner swap ={smallestSwap.eval} evalsudoku = {evalSudoku}");
                
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
            
            punt1.error = new Error(row1, FindError(sudoku.GetColumn(row1)), column1, FindError(sudoku.GetRow(column1)));
            
            int column2 = miniSudoku.column * 3 + punt2.column;
            int row2 = miniSudoku.row * 3 + punt2.row;
            
            punt2.error = new Error(row2, FindError(sudoku.GetColumn(row2)), column2, FindError(sudoku.GetRow(column2)));

            int[] tempColumn = (int[])evalColumns.Clone();
            int[] tempRow = (int[])evalRows.Clone();
            
            
            SetErrors(tempColumn, tempRow, punt1.error, punt2.error);
            
            // Console.WriteLine("voor alles");
            // Console.WriteLine("column1 " + string.Join(", ", FindError(sudoku.GetColumn(column1))));
            // Console.WriteLine("column2 " + string.Join(", ", FindError(sudoku.GetColumn(column2))));
            // Console.WriteLine("row1 " + string.Join(", ", FindError(sudoku.GetColumn(row1))));
            // Console.WriteLine("row2 " + string.Join(", ", FindError(sudoku.GetColumn(row2))));
            //
            // Console.WriteLine("van de functie zelf");
            //
            // Console.WriteLine("echte error");
            // Console.WriteLine("column " + string.Join(", ", evalColumns));
            // Console.WriteLine("row " + string.Join(", ", evalRows));
            
            //InstantiateEval(column1, column2, row1, row2);
            
            
            // Console.WriteLine("temp columns");
            // Console.WriteLine($"indexen aangepast {column1} , {column2}");
            // Console.WriteLine("column " + string.Join(", ", tempColumn));
            //
            // Console.WriteLine($"indexen aangepast {row1} , {row2}");
            // Console.WriteLine("row " + string.Join(", ", tempRow));
            //
            // Console.WriteLine("echte error");
            // Console.WriteLine("column " + string.Join(", ", evalColumns));
            // Console.WriteLine("row " + string.Join(", ", evalRows));
            
            //Thread.Sleep(2000000);
            //SetErrors(evalColumns, evalRows, punt1.error, punt2.error);
            
            // Console.WriteLine(CombineError(evalColumns, evalRows));
            // Console.WriteLine(CombineError(tempColumn, tempRow));
            
            //Thread.Sleep(3000);
            int result = CombineError(evalColumns, evalRows);
            
            //evalColumns = (int[])tempColumn.Clone();
            //evalRows = (int[])tempRow.Clone();
            
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