namespace SudokuKiller
{
    public class Swap
    {
        public int eval { get; set; }
        public Tuple<int, int> pos_1 { get; set; }
        public Tuple<int, int> pos_2 { get; set; }
        
        public Swap(int eval, Tuple<int, int> pos_1, Tuple<int, int> pos_2)
        {
            this.eval = eval;
            this.pos_1 = pos_1;
            this.pos_2 = pos_2;
        }
    }
}