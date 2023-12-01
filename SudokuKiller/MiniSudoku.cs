namespace SudokuKiller
{
    public class MiniSudoku
    {
        public Getal[,] MiniSudokuList = new Getal[3, 3];
        private int x, y;
        public int x_pos, y_pos;

        public MiniSudoku()
        {
            x = 0;
            y = 0;
        }

        // nog een change

        public void AddGetal(Getal getal)
        {
            MiniSudokuList[y, x] = getal;
            IncrementIndices();
        }
        
        private void IncrementIndices()
        {
            x++;
            if (x > 2)
            {
                x = 0;
                y++;
                if (y > 2)
                {
                    y = 0;
                }
            }
        }
        
        public IEnumerable<int> GetRow(int y)
        {
            int columns = MiniSudokuList.GetLength(1);
            for (int col = 0; col < columns; col++)
            {
                yield return MiniSudokuList[y, col].number;
            }
            
        }
        
        public IEnumerable<int> GetColumn(int x)
        {
            int rows = MiniSudokuList.GetLength(0);
            for (int row = 0; row < rows; row++)
            {
                yield return MiniSudokuList[row, x].number;
            }
            
        }
        
    }
}
