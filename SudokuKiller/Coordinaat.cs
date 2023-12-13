namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that stores coordinations for positions .
    /// </summary>
    public class Coordinaat
    {
        // Declare variables
        public int column { get; set; }
        public int row { get; set; }
        public Error error { get; set; }

        /// <summary>
        /// Creates constructor.
        /// </summary>
        /// <param name="x">Position of x index.</param>
        /// /// <param name="y">Position of y index.</param>
        public Coordinaat(int x, int y)
        {
            column = x;
            row = y;
        }
    }
    
}