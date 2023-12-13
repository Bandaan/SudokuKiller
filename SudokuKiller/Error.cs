namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that defines the errors in a certain column and row.
    /// </summary>
    public class Error
    {
        // Declare variables
        public int columIndex { get; }
        public int columnError { get; }
        public int rowIndex { get; }
        public int rowError { get; }

        /// <summary>
        /// Creates constructor.
        /// </summary>
        /// <param name="columnindex">Index of changed number in column error array.</param>
        /// /// <param name="columnerror">Error number in column error array.</param>
        /// /// <param name="rowindex">Index of changed number in row error array.</param>
        /// /// /// <param name="rowerror">Error number in row error array.</param>
        public Error(int columnindex, int columnerror, int rowindex, int rowerror)  
        {
            columIndex = columnindex;
            columnError = columnerror;
            rowIndex = rowindex;
            rowError = rowerror;
        }
    }
}