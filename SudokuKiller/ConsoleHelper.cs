namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that provides parse functions for the sudoku.
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Prints starts log to console
        /// </summary>
        /// <param name="randomWalkLength">Length of the random walk.</param>
        /// /// <param name="randomWalkStart">Length of random walk start.</param>
        /// /// <param name="improvement">Which improvement type</param>
        public static void BeginLog(int randomWalkLength, int randomWalkStart, string improvement)
        {
            // Log task initialisation in green color.
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{DateTime.Now.ToString("h:mm:ss tt").Split(" ")[0]}]-[Task-{Task.CurrentId}] walkLength-{randomWalkLength} :: walkStart-{randomWalkStart} :: {improvement}-improvement");
        }
        
        /// <summary>
        /// Prints end log to console
        /// </summary>
        /// <param name="executed">Executing of task.</param>
        /// /// <param name="time">Time of the task.</param>
        /// /// <param name="log">If a sudoku needs to be logged to console</param>
        /// /// /// <param name="sudoku">Solved sudoku</param>
        public static void EndLog(bool executed, long time, bool log, Sudoku sudoku)
        {
            if (executed)
            {
                // Task not executed in given time log task not executed in red.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now.ToString("h:mm:ss tt").Split(" ")[0]}]-[Task-{Task.CurrentId}] Did not execute in time");
                // Set color back to white
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                // Task is executed in given time log task executed in green.
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"[{DateTime.Now.ToString("h:mm:ss tt").Split(" ")[0]}]-[Task-{Task.CurrentId}] Executed in {time} Milliseconds ");
                // Set color back to white
                Console.ForegroundColor = ConsoleColor.White;
                if (log)
                {
                    // Sudoku logs out to console.
                    LogSudoku(sudoku);
                }
            }
        }
        
        /// <summary>
        /// Prints sudoku in good format to console.
        /// </summary>
        /// /// /// <param name="sudoku">Solved sudoku</param>
        static void LogSudoku(Sudoku sudoku)
        {
            // Declare sudoku string
            string sudokuString = "";

            Console.WriteLine("\n");
            for (int i = 0; i < 9; i++)
            {
                // Get all the rows of sudoku
                int[] row = sudoku.GetRow(i);
                for (int j = 0; j < row.Length; j++)
                {
                    if (j % 3 == 0)
                    {
                        // Add | before every block of 3 columns
                        sudokuString += "| ";
                    }
                    
                    // Add number to sudoku string
                    sudokuString += $"{row[j]} ";
                    if (j == 8)
                    {
                        // Add | after the last column in the row
                        sudokuString += "|";
                    }
                }
                
                // Add new line to sudoku string
                sudokuString += "\n";

                if (i % 3 == 2 && i < 8)
                {
                    // Add horizontal line between every block of 3 rows
                    sudokuString += "+-----------------------+\n";
                }
            }
            Console.WriteLine(sudokuString);
        }
        
        /// <summary>
        /// Formats solved sudoku to string.
        /// </summary>
        /// /// /// <param name="sudoku">Solved sudoku</param>
        /// /// <returns>Sudoku formatted in string</returns>
        public static string SudokuToString(Sudoku sudoku)
        {
            // Declare sudoku string
            string sudokuString = "";

            for (int i = 0; i < 9; i++)
            {
                // Get all the rows of sudoku
                int[] row = sudoku.GetRow(i);

                for (int j = 0; j < row.Length; j++)
                {
                    // Add number to sudoku string
                    sudokuString += row[j];

                    // Add a space or another separator between numbers in the same row
                    if (j < row.Length - 1)
                    {
                        sudokuString += " ";
                    }
                }

                // Add a line break between rows
                sudokuString += " ";
            }
            
            // Return sudoku string
            return sudokuString;
        }

    
    }
}
