namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that defines a number in a cell of the sudoku.
    /// </summary>
    public class Getal
    {
        // Declare variables
        public int number { get; set; }
        public bool vast { get; set; }
        
        /// <summary>
        /// Creates constructor.
        /// </summary>
        /// <param name="nummer">Number from position.</param>
        /// /// <param name="solid">Number fixed.</param>
        public Getal(int nummer, bool solid)
        {
            number = nummer;
            vast = solid;
        }
    }
}