namespace SudokuKiller
{
    public class Swap
    {
        public int eval { get; set; }
        public Error error { get; set; }
        public Coordinaat pos1 { get; set; }
        public Coordinaat pos2 { get; set; }
        
        public Swap(int fout, Error mistake, Coordinaat pos_1, Coordinaat pos_2)
        {
            eval = fout;
            error = mistake;
            pos1 = pos_1;
            pos2 = pos_2;
        }
    }
}