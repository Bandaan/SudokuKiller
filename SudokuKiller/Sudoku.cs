namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that stores the entire sudoku.
    /// </summary>
    public class Sudoku
    {
        // Declare variables.
        public MiniSudoku[,] SudokuList = new MiniSudoku[3, 3];
        private int x, y;
        private Random rnd;
        
        
        /// <summary>
        /// Creates constructor.
        /// </summary>
        public Sudoku()
        {
            x = 0;
            y = 0;
            rnd = new Random();
        }
        /// <summary>
        /// Adds block to sudoku 
        /// </summary>
        /// <param name="miniSudoku">Block (miniSudoku)</param>
        public void AddMiniSudoku(MiniSudoku miniSudoku)
        {
            // Place block in sudoku array
            SudokuList[y, x] = miniSudoku;
            
            // Set the index of the block
            miniSudoku.column = y;
            miniSudoku.row = x;
            
            // Increment indexes for next block
            IncrementIndices();
        }
        
        /// <summary>
        /// Increment indexes to the block are well placed in array
        /// </summary>
        private void IncrementIndices()
        {
            // Increase x by one
            x++;
            if (x > 2)
            {
                // x bigger than 2 increase y with one and set x to one
                x = 0;
                y++;
                if (y > 2)
                {
                    // y bigger than 2 set y to zero
                    y = 0;
                }
            }
        }
        
        /// <summary>
        /// Get the row from the sudoku.
        /// </summary>
        /// <param name="y">y index of 9 x 9 sudoku array</param>
        /// <returns>Array with row numbers</returns>/// 
        public int[] GetRow(int y)
        {
            // Declare variables
            int[] rowArray = new int[9];
            int index = -1;
            int columns = SudokuList.GetLength(1);
            
            // Loop over all the columns
            for (int col = 0; col < columns; col++)
            {
                // Loop over all the blocks
                // Get all the numbers from the block that are in that row
                foreach (var number in SudokuList[y / 3, col].GetRow(y % 3))
                {
                    // Place number in array and increase index by one
                    index++;
                    rowArray[index] = number;
                }
            }
            // Return array with all the row numbers
            return rowArray;
        }
        
        /// <summary>
        /// Get the column from the sudoku.
        /// </summary>
        /// <param name="x">x index of 9 x 9 sudoku array</param>
        /// <returns>Array with column numbers</returns>/// 
        public int[] GetColumn(int x)
        {
            // Declare variables
            int[] columnArray = new int[9];
            int index = -1;
            int rows = SudokuList.GetLength(0);
            
            // Loop over all the rows
            for (int row = 0; row < rows; row++)
            {
                // Loop over all the blocks
                // Get all the numbers from the block that are in that column
                foreach (var number in SudokuList[row, x / 3].GetColumn(x % 3))
                {
                    // Place number in array and increase index by one
                    index++;
                    columnArray[index] = number;
                }
            }
            // Return array with all the column numbers
            return columnArray;

        }
        
        /// <summary>
        /// Get random block from sudoku array
        /// </summary>
        /// <returns>Random block (miniSudoku)</returns>/// 
        public MiniSudoku GetRandomMiniSudoku()
        {
            // Return block by random number between 0 and 2 for x and y
            return SudokuList[rnd.Next(3), rnd.Next(3)];
        }
    }
}