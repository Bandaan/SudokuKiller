using System.Diagnostics;

namespace SudokuKiller
{
    /// <summary>
    /// Represents a class that runs the hill-climbing/ random walk algorithm on the sudoku.
    /// </summary>
    public class Algoritme
    {
        Sudoku sudoku;
        int evalSudoku = -1;
        int[] evalColumns = new int[9];
        int[] evalRows = new int[9];
        int randomWalkLength;
        int counter;
        int randomWalkStart;
        bool improvement;
        long maxTime;
        bool logSudoku;
        Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Calculates all the blocks and parses to sudoku.
        /// </summary>
        /// <param name="sudoku">Sudoku object.</param>
        /// <param name="randomwalkstart">Number of repetitions of the same evalSudoku before entering a random walk.</param>
        /// <param name="randomwalklength">Number of repititions a random swap is executed in a random walk.</param>
        /// <param name="type">Either best (best improvement search) or first (first improvement search) deciding which algorithm should be used.</param>
        /// <param name="maxtime">The maximum amount of time the algorithm is allowed to run for before aborting.</param>
        /// <param name="logsudoku">Boolean that represents if the solved sudoku should be printed to the console or not.</param>
        public Algoritme(Sudoku sudoku, int randomwalkstart, int randomwalklength, string type, long maxtime, bool logsudoku)
        {
            this.sudoku = sudoku;
            randomWalkStart = randomwalkstart;
            randomWalkLength = randomwalklength;
            improvement = (type == "best") ? true : false;
            maxTime = maxtime;
            logSudoku = logsudoku;
        }

        public async Task<Tuple<long, string, int, int, string>> RunAlgoritme()
        {
            ConsoleHelper.BeginLog(randomWalkLength, randomWalkStart, improvement ? "best" : "first");
            stopwatch.Start();
            evalSudoku = InstantiateEval();
            
            while (evalSudoku != 0)
            {
                if (stopwatch.ElapsedMilliseconds > maxTime)
                {
                    break;
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
            ConsoleHelper.EndLog(stopwatch.ElapsedMilliseconds > maxTime, stopwatch.ElapsedMilliseconds, logSudoku, sudoku);
            return await Task.FromResult(new Tuple<long, string, int, int, string>(stopwatch.ElapsedMilliseconds, ConsoleHelper.SudokuToString(sudoku), randomWalkLength, randomWalkStart, improvement ? "best" : "first"));
        }

        /// <summary>
        /// Sets the errors of specific columns and rows.
        /// </summary>
        /// <param name="columnError">Array containing the number of errors for each column.</param>
        /// <param name="rowError">Array containing the number of errors for each row.</param>
        /// <param name="punt1">Error object containing the column, row and their respective updated errors of the first point.</param>
        /// <param name="punt2">Error object containing the column, row and their respective updated errors of the second point.</param>
        private void SetErrors(int[] columnError, int[] rowError, Error punt1, Error punt2)
        {
            columnError[punt1.columIndex] = punt1.columnError;
            columnError[punt2.columIndex] = punt2.columnError;

            rowError[punt1.rowIndex] = punt1.rowError;
            rowError[punt2.rowIndex] = punt2.rowError;
        }

        /// <summary>
        /// Performs a number (stored in randomWalkLength) of random swaps in random mini sudoku's and updates the errors and evaluation of the sudoku.
        /// </summary>
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

        /// <summary>
        /// Calculates the mistakes in each column and row of the sudoku.
        /// </summary>
        /// <returns>The combined number of mistakes in the sudoku</returns>
        private int InstantiateEval()
        {
            for (int i = 0; i < 9; i++)
            {
                evalColumns[i] = FindError(sudoku.GetColumn(i));
                evalRows[i] = FindError(sudoku.GetRow(i));
            }
            return CombineError(evalColumns, evalRows);
        }
        
        /// <summary>
        /// Represents a helper method to combine the errors stored in 2 arrays.
        /// </summary>
        /// <returns>The combined number of mistakes in the sudoku</returns>
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

        /// <summary>
        /// Temporarily updates the amount of mistakes in the columns and rows of point 1 and 2 to calculate the evaluation of the sudoku this swap gives us
        /// </summary>
        /// <param name="punt1">Coordinaat object which represents the first swapped cell's location and error.</param>
        /// <param name="punt2">Coordinaat object which represents the second swapped cell's location and error.</param>
        /// <param name="miniSudoku">MiniSudoku object to help with finding the location of point 1 and 2 in the sudoku</param>
        /// <returns>The combined number of mistakes in this temporary configuration of the sudoku</returns>
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

        /// <summary>
        /// Calculates the amount of unique numbers there are in an array to find how many are missing
        /// </summary>
        /// <param name="array">An array which is either a column or a row from the sudoku</param>
        /// <returns>The number of missing numbers from 1-9</returns>
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
        
        /// <summary>
        /// Gives depending on the improvement bool either the swap that gives the lowest evalSudoku or the swap that gives the first improvement on evalSudoku
        /// </summary>
        /// <param name="miniSudoku">MiniSudoku object to swap numbers in</param>
        /// <returns>The number of missing numbers from 1-9</returns>
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

        /// <summary>
        /// Performs a random swap in the given MiniSudoku and updates the new evaluation and column and row errors
        /// </summary>
        /// <param name="miniSudoku">MiniSudoku object to randomly swap numbers in</param>
        /// <returns>A swap object containing the random swap and it's error</returns>
        private Swap RandomSwap(MiniSudoku miniSudoku)
        {
            Tuple<Coordinaat, Coordinaat> swap = miniSudoku.GetRandomSwap();
            
            int eval = FindEval(swap.Item1, swap.Item2, miniSudoku);

            return new Swap(eval, swap.Item1, swap.Item2);

        }
        
    }
}