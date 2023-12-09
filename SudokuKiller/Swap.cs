namespace SudokuKiller
{
    public class Swap
    {
        public int eval { get; set; }
        public Coordinaat pos1 { get; set; }
        public Coordinaat pos2 { get; set; }
        
        public Swap(int fout, Coordinaat pos_1, Coordinaat pos_2)
        {
            eval = fout;
            pos1 = pos_1;
            pos2 = pos_2;
        }
    }
}