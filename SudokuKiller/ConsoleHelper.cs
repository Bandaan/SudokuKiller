namespace SudokuKiller
{
    public static class ConsoleHelper
    {
        public static void BeginLog(int randomWalkLength, int randomWalkStart, string improvement)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{DateTime.Now.ToString("h:mm:ss tt").Split(" ")[0]}]-[Task-{Task.CurrentId}] walkLength-{randomWalkLength} :: walkStart-{randomWalkStart} :: {improvement}-improvement");
        }
        
        public static void EndLog(bool executed, long time, bool log, Sudoku sudoku)
        {
            if (executed)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now.ToString("h:mm:ss tt").Split(" ")[0]}]-[Task-{Task.CurrentId}] Did not execute in time");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"[{DateTime.Now.ToString("h:mm:ss tt").Split(" ")[0]}]-[Task-{Task.CurrentId}] Executed in {time} Milliseconds ");

                if (log)
                {
                    LogSudoku(sudoku);
                }
            }
        }
        
        static void LogSudoku(Sudoku sudoku)
        {
            Console.ForegroundColor = ConsoleColor.White;
            string sudokuString = "";

            Console.WriteLine("\n");
            for (int i = 0; i < 9; i++)
            {
                int[] row = sudoku.GetRow(i);
                for (int j = 0; j < row.Length; j++)
                {
                    if (j % 3 == 0)
                    {
                        // Add | before every block of 3 columns
                        sudokuString += "| ";
                    }

                    sudokuString += $"{row[j]} ";

                    if (j == 8)
                    {
                        // Add | after the last column in the row
                        sudokuString += "|";
                    }
                }

                sudokuString += "\n";

                if (i % 3 == 2 && i < 8)
                {
                    // Add horizontal line between every block of 3 rows
                    sudokuString += "+-----------------------+\n";
                }
            }
            Console.WriteLine(sudokuString);
        }

        public static string SudokuToString(Sudoku sudoku)
        {
            string sudokuString = "";

            for (int i = 0; i < 9; i++)
            {
                int[] row = sudoku.GetRow(i);

                for (int j = 0; j < row.Length; j++)
                {
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

            return sudokuString;
        }

    
    }
}
