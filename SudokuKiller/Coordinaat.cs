namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that stores coordinations for positions .
    /// </summary>
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