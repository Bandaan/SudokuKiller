namespace SudokuKiller
{
    public class Sudoku
    {
        private MiniSudoku[,] SudokuList = new MiniSudoku[3, 3];
        private int x, y;

        public Sudoku()
        {
            x = 0;
            y = 0;
        }

        public void AddMiniSudoku(MiniSudoku miniSudoku)
        {
            SudokuList[y, x] = miniSudoku;
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
    }
    
}