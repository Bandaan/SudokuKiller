namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that stores positions and evaluation number of a possible swap.
    /// </summary>
    public class Swap
    {
        // Declare variables
        public int eval { get;}
        public Coordinaat pos1 { get;}
        public Coordinaat pos2 { get;}
        
        /// <summary>
        /// Creates constructor.
        /// </summary>
        /// <param name="fout">Evaluate mistake.</param>
        /// /// <param name="pos_1">First coordinate of swap.</param>
        /// /// <param name="pos_2">Second coordinate of swap.</param>
        public Swap(int fout, Coordinaat pos_1, Coordinaat pos_2)
        {
            eval = fout;
            pos1 = pos_1;
            pos2 = pos_2;
        }
    }
}