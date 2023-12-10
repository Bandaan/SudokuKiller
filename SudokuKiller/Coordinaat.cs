namespace SudokuKiller
{
    public class Coordinaat
    {
        public int column;
        public int row;
        
        public Error error;

        public Coordinaat(int x, int y)
        {
            column = x;
            row = y;
        }
    }
    
}