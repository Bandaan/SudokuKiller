using System.Diagnostics;

namespace SudokuKiller
{
    /// <summary>
    /// Represents a class that runs the hill-climbing/ random walk algorithm on the sudoku.
    /// </summary>
    public class Algorithm
    {
        // Declare variables
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
        public Algorithm(Sudoku sudoku, int randomwalkstart, int randomwalklength, string type, long maxtime, bool logsudoku)
        {
            this.sudoku = sudoku;
            randomWalkStart = randomwalkstart;
            randomWalkLength = randomwalklength;
            improvement = (type == "best") ? true : false;
            maxTime = maxtime;
            logSudoku = logsudoku;
        }

        /// <summary>
        /// Calculates the mistake of the entire sudoku, then runs the algorithm while the mistake of the sudoku is not 0 and makes final swaps suggested if they're
        /// smaller or equal to the current mistake in the sudoku. Also keeps track of platea's or local minima and decides to do a random walk.
        /// </summary>
        /// <returns>The time it took to solve the sudoku, the solved sudoku as a string and the settings used to solve the sudoku</returns>
        public async Task<Tuple<long, string, int, int, string>> RunAlgorithm()
        {
            // Prints start log to console and starts timer
            ConsoleHelper.BeginLog(randomWalkLength, randomWalkStart, improvement ? "best" : "first");
            stopwatch.Start();

            // Calculates the mistake in the entire sudoku
            evalSudoku = InstantiateEval();
            
            // Runs the algorithm while the mistake in the sudoku is not 0
            while (evalSudoku != 0)
            {
                // Stops the algorithm if it took longer than the maxTime
                if (stopwatch.ElapsedMilliseconds > maxTime)
                {
                    break;
                }
                
                // Picks a random MiniSudoku and finds the swap resulting in the smallest mistake in the sudoku
                MiniSudoku miniSudoku = sudoku.GetRandomMiniSudoku();
                Swap smallestSwap = SwapSuggest(miniSudoku);

                // Checks if the mistake from the smallest swap is less or more than the current mistake in the sudoku
                if (smallestSwap.eval <= evalSudoku)
                {
                    // If the mistake is equal to the current mistake in the sudoku we add to the counter
                    if (smallestSwap.eval == evalSudoku)
                    {
                        counter++;
                    }
                    // Otherwise we reset the counter
                    else
                    {
                        counter = 0;
                    }
                    
                    // We actually make the swap in the sudoku and update the sudoku's errors since this results in either a lower or equal mistake
                    miniSudoku.Swap(smallestSwap.pos1, smallestSwap.pos2);
                    
                    
                    SetErrors(evalColumns, evalRows, smallestSwap.pos1.error, smallestSwap.pos2.error);
                    evalSudoku = smallestSwap.eval;
                }
                // If the mistake is bigger than the current mistake in the sudoku we add to the counter
                else
                {
                    counter++;
                }

                // If the counter is equal to randomWalkStart we start a random walk and reset the timer
                if (counter >= randomWalkStart)
                {
                    counter = 0;
                    RandomWalk();
                }
            }
            
            // Stops the timer and returns all the results
            stopwatch.Stop();
            ConsoleHelper.EndLog(stopwatch.ElapsedMilliseconds > maxTime, stopwatch.ElapsedMilliseconds, logSudoku, sudoku);
            return await Task.FromResult(new Tuple<long, string, int, int, string>(stopwatch.ElapsedMilliseconds, ConsoleHelper.SudokuToString(sudoku), randomWalkLength, randomWalkStart, improvement ? "best" : "first"));
        }

        /// <summary>
        /// Sets the errors of specific columns and rows.
        /// </summary>
        /// <param name="columnError">Array containing the number of errors for each column.</param>
        /// <param name="rowError">Array containing the number of errors for each row.</param>
        /// <param name="point1">Error object containing the column, row and their respective updated errors of the first point.</param>
        /// <param name="punt2">Error object containing the column, row and their respective updated errors of the second point.</param>
        private void SetErrors(int[] columnError, int[] rowError, Error point1, Error point2)
        {
            // Sets the specific rows and columns to their specific updated errors
            columnError[point1.columIndex] = point1.columnError;
            columnError[point2.columIndex] = point2.columnError;

            rowError[point1.rowIndex] = point1.rowError;
            rowError[point2.rowIndex] = point2.rowError;
        }

        /// <summary>
        /// Performs a number (stored in randomWalkLength) of random swaps in random mini sudoku's and updates the errors and evaluation of the sudoku.
        /// </summary>
        private void RandomWalk()
        {
            // Performs the random walk for randomWalkLength's amount of times
            for (int i = 0; i < randomWalkLength; i++)
            {
                // Picks a random MiniSudoku, makes a random swap and updates all the errors
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
            // Loops through all rows and columns
            for (int i = 0; i < 9; i++)
            {
                // Set the specific row and column's error to it's error
                evalColumns[i] = FindError(sudoku.GetColumn(i));
                evalRows[i] = FindError(sudoku.GetRow(i));
            }

            // returns the value of CombineError for these updates evalColumns and evalRows
            return CombineError(evalColumns, evalRows);
        }
        
        /// <summary>
        /// Represents a helper method to combine the errors stored in 2 arrays.
        /// </summary>
        /// <returns>The combined number of mistakes in the sudoku</returns>
        private int CombineError(int[] arrayColumns, int[] arrayRows)
        {
            // Adds up all the errors stored in evalColumns and evalRows
            int error = 0;
            for (int i = 0; i < arrayColumns.Length; i++)
            {
                error += arrayColumns[i];
                error += arrayRows[i];
            }

            // Returns this error as one int
            return error;
        }

        /// <summary>
        /// Temporarily updates the amount of mistakes in the columns and rows of point 1 and 2 to calculate the evaluation of the sudoku this swap gives us.
        /// </summary>
        /// <param name="point1">Coordinaat object which represents the first swapped cell's location and error.</param>
        /// <param name="point2">Coordinaat object which represents the second swapped cell's location and error.</param>
        /// <param name="miniSudoku">MiniSudoku object to help with finding the location of point 1 and 2 in the sudoku</param>
        /// <returns>The combined number of mistakes in this temporary configuration of the sudoku</returns>
        private int FindEval(Coordinaat point1, Coordinaat point2, MiniSudoku miniSudoku)
        {
            // Calculates the right index from it's local scope in the MiniSudoku to it's global scope in the entire sudoku
            int column1 = miniSudoku.column * 3 + point1.column;
            
            int row1 = miniSudoku.row * 3 + point1.row;
            
            // Sets point 1's error to a new error object and setting it using FindError, also storing the right index to easily update this later when making an actual swap
            // in the sudoku
            point1.error = new Error(row1, FindError(sudoku.GetColumn(row1)), column1, FindError(sudoku.GetRow(column1)));
            
            // The same but for point 2
            int column2 = miniSudoku.column * 3 + point2.column;
            int row2 = miniSudoku.row * 3 + point2.row;
            
            point2.error = new Error(row2, FindError(sudoku.GetColumn(row2)), column2, FindError(sudoku.GetRow(column2)));

            // Creates a copy of our current evalColumns and evalRows to temporarily update the mistake in certain columns and rows and easily revert changes
            int[] tempColumn = (int[])evalColumns.Clone();
            int[] tempRow = (int[])evalRows.Clone();
            
            // Updates the errors in these tempColumns and tempRows
            SetErrors(tempColumn, tempRow, point1.error, point2.error);
            
            // Also calculates the combined mistake in the sudoku by calling CombineError
            int result = CombineError(tempColumn, tempRow);
            
            // Returns this mistake as one int
            return result;

        }

        /// <summary>
        /// Calculates the amount of unique numbers there are in an array to find how many are missing.
        /// </summary>
        /// <param name="array">An array which is either a column or a row from the sudoku</param>
        /// <returns>The number of missing numbers from 1-9</returns>
        public static int FindError(int[] array)
        {
            // Creates an empty List to fill with unique numbers
            List<int> tempArray = new List<int>();

            // Loops through the entire array (either a column or a row)
            for (int i = 0; i < array.Length; i++)
            {
                // If the number is not yet in our list we add it to the list
                if (!tempArray.Contains(array[i]))
                {
                    tempArray.Add(array[i]);
                }
            }

            // We return 9 - the amount of unique elements in our list to get the amount of numbers missing from 1-9
            return 9-tempArray.Count;
        }
        
        /// <summary>
        /// Gives depending on the improvement bool either the swap that gives the lowest evalSudoku or the swap that gives the first improvement on evalSudoku.
        /// </summary>
        /// <param name="miniSudoku">MiniSudoku object to swap numbers in</param>
        /// <returns>The number of missing numbers from 1-9</returns>
        private Swap SwapSuggest(MiniSudoku miniSudoku)
        {
            // Creates a smallestElement to keep track of the swap holding the smallest mistake in the entire sudoku using int.MaxValue so all swaps will be smaller to begin with
            Swap smallestElement = new Swap(int.MaxValue, null, null);
            
            // Loops through all columns in the MiniSudoku for the first cell
            for (int i = 0; i < 3; i++)
            {
                // Loops through all rows in the MiniSudoku for the first cell
                for (int j = 0; j < 3; j++)
                {
                    // If the first cell in this MiniSudoku is fixed it skips this iteration
                    if (miniSudoku.MiniSudokuList[j, i].stuck)
                    {
                        continue;
                    }
                    // Loops through all the columns from the column of the first cell for the second cell as to not do all swaps double
                    for (int k = 0; k < 3; k++)
                    {
                        // Loops through all the rows from the row of the first cell for the second cell as to not do all swaps double
                        for (int l = 0; l < 3; l++)
                        {
                            // If the second cell is not fixed and both cells are not the same we execute the swap
                            if (miniSudoku.MiniSudokuList[j, i].number != miniSudoku.MiniSudokuList[l, k].number && !miniSudoku.MiniSudokuList[l, k].stuck)
                            {
                                // Creates coordinate objects for both cells
                                Coordinaat number1 = new Coordinaat(l, k);
                                Coordinaat number2 = new Coordinaat(j, i);

                                // Executes the swap, calculates the mistake in the sudoku and swaps back to not interfere with other swaps
                                miniSudoku.Swap(number1, number2);
                                int newEval = FindEval(number1, number2, miniSudoku);
                                miniSudoku.Swap(number2, number1);

                                // If first improvement has been selected in the algorithm we can return early if the mistake given by our new swap is lower
                                // than the current mistake in the entire sudoku
                                if (!improvement && newEval < evalSudoku)
                                {
                                    return new Swap(newEval, number1, number2);
                                }
                                
                                // If the mistake from the sudoku is lower than the one in smallestElement we change it to the current one and make a swap
                                // object out of it to store the details about this swap
                                if (newEval < smallestElement.eval)
                                {
                                    smallestElement = new Swap(newEval, number1, number2);
                                }
                            }
                        }
                    }
                }
            }

            //Returns the swap resulting in the smallest mistake in the sudoku
            return smallestElement;
        }

        /// <summary>
        /// Performs a random swap in the given MiniSudoku and updates the new evaluation and column and row errors.
        /// </summary>
        /// <param name="miniSudoku">MiniSudoku object to randomly swap numbers in</param>
        /// <returns>A swap object containing the random swap and it's error</returns>
        private Swap RandomSwap(MiniSudoku miniSudoku)
        {
            // Gets the coordinates of 2 cells to swap by calling GetRandomSwap from a MiniSudoku
            Tuple<Coordinaat, Coordinaat> swap = miniSudoku.GetRandomSwap();
            
            // Get's the new mistake in the entire sudoku by calling FindEval
            int eval = FindEval(swap.Item1, swap.Item2, miniSudoku);

            // Returns a new swap object which holds 2 swapped objects and the mistake of the entire sudoku
            return new Swap(eval, swap.Item1, swap.Item2);

        }
        
    }
}