using System.Runtime.InteropServices;

namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that stores a 3x3 grid of cells from the sudoku.
    /// </summary>
    public class MiniSudoku
    {
        // Declare variables
        public Number[,] MiniSudokuList = new Number[3, 3];
        private int x, y;
        Random rnd;
        public int column, row;
        
        /// <summary>
        /// Creates constructor.
        /// </summary>
        public MiniSudoku()
        {
            x = 0;
            y = 0;
            rnd = new Random();
        }
        
        /// <summary>
        /// Adds number to block
        /// </summary>
        /// <param name="number">Number</param>
        public void AddGetal(Number number)
        {
            // Place number in block array
            MiniSudokuList[y, x] = number;
            // Increment indexes for next number
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
        /// <param name="y">y index of 3 x 3 block array</param>
        /// <returns>IEnumerable array with row numbers of block</returns>/// 
        public IEnumerable<int> GetRow(int y)
        {
            // Declare variables
            int columns = MiniSudokuList.GetLength(1);
            
            // Loop over all the columns
            for (int col = 0; col < columns; col++)
            {
                // Yield return row number
                yield return MiniSudokuList[y, col].number;
            }
            // Returns IEnumerable array with row numbers of block
            
        }
        
        /// <summary>
        /// Get the row column the sudoku.
        /// </summary>
        /// <param name="x">x index of 3 x 3 block array</param>
        /// <returns>IEnumerable array with column numbers of block</returns>/// 
        public IEnumerable<int> GetColumn(int x)
        {
            // Declare variables
            int rows = MiniSudokuList.GetLength(0);
            // Loop over all the rows
            for (int row = 0; row < rows; row++)
            {
                // Yield return column number
                yield return MiniSudokuList[row, x].number;
            }
            // Returns IEnumerable array with column numbers of block
        }
        
        /// <summary>
        /// Swap two elements in the block
        /// </summary>
        /// <param name="left">Left coordinate that's needs to be swapped</param>
        /// /// <param name="right">Right coordinate that's needs to be swapped</param>
        public void Swap(Coordinaat left, Coordinaat right)
        {
            // Set temp to left coordinate
            Number tempNumber = MiniSudokuList[left.column, left.row];
            
            // Swap left and right coordinate
            MiniSudokuList[left.column, left.row] = MiniSudokuList[right.column, right.row];
            MiniSudokuList[right.column, right.row] = tempNumber;
        }
        
        /// <summary>
        /// Get random swap
        /// </summary>
        /// <returns>Tuple with two coordinates block that can be swapped</returns>/// 
        public Tuple<Coordinaat, Coordinaat> GetRandomSwap()
        {
            // Declare variables
            Coordinaat pos1 = new Coordinaat(0, 0);
            Coordinaat pos2 = new Coordinaat(0, 0);
            
            // Coordinates the same calculate new random coordinates
            while (pos1.column == pos2.column && pos1.row == pos2.row || MiniSudokuList[pos1.column, pos1.row].stuck || MiniSudokuList[pos2.column, pos2.row].stuck)
            {
                // Get x, y by random between 0 and 2
                pos1.column = rnd.Next(3);
                pos1.row = rnd.Next(3);
                pos2.column = rnd.Next(3);
                pos2.row = rnd.Next(3);
            }
            
            // Return Tuple with coordinates of the random swap
            return new Tuple<Coordinaat, Coordinaat>(pos1, pos2);
        }
    }
}
