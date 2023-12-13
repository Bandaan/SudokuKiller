namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that defines a number in a cell of the sudoku.
    /// </summary>
    public class Number
    {
        // Declare variables
        public int number { get; set; }
        public bool stuck { get; set; }
        
        /// <summary>
        /// Creates constructor.
        /// </summary>
        /// <param name="integer">Number from position.</param>
        /// /// <param name="solid">Number fixed.</param>
        public Number(int integer, bool solid)
        {
            number = integer;
            stuck = solid;
        }
    }
}