namespace SudokuKiller
{
    public class Swap
    {
        public int eval { get; set; }
        public int errorColumn1 { get; set; }
        public int errorColumn2 { get; set; }
        public int errorRow1 { get; set; }
        public int errorRow2 { get; set; }
        public Tuple<int, int> pos_1 { get; set; }
        public Tuple<int, int> pos_2 { get; set; }
        
        public Swap(int eval, int errorColumn1, int errorColumn2, int errorRow1, int errorRow2, Tuple<int, int> pos_1, Tuple<int, int> pos_2)
        {
            this.eval = eval;
            this.errorColumn1 = errorColumn1;
            this.errorColumn2 = errorColumn2;
            this.errorRow1 = errorRow1;
            this.errorRow2 = errorRow2;
            this.pos_1 = pos_1;
            this.pos_2 = pos_2;
        }
    }
}