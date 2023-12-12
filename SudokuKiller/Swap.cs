namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that stores positions and evaluation number of a possible swap.
    /// </summary>
    public class Swap
    {
        public int eval { get;}
        public Coordinaat pos1 { get;}
        public Coordinaat pos2 { get;}
        
        public Swap(int fout, Coordinaat pos_1, Coordinaat pos_2)
        {
            eval = fout;
            pos1 = pos_1;
            pos2 = pos_2;
        }
    }
}