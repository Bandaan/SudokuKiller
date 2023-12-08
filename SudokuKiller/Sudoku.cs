namespace SudokuKiller
{
    public class Sudoku
    {
        public MiniSudoku[,] SudokuList = new MiniSudoku[3, 3];
        private int x, y;
        Random rnd;

        public Sudoku()
        {
            x = 0;
            y = 0;
            rnd = new Random();
        }

        public void AddMiniSudoku(MiniSudoku miniSudoku)
        {
            SudokuList[y, x] = miniSudoku;
            miniSudoku.x_pos = x;
            miniSudoku.y_pos = y;
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

        public int[] GetRow(int y)
        {
            int[] rowArray = new int[9];
            int index = -1;
            int columns = SudokuList.GetLength(1);
            for (int col = 0; col < columns; col++)
            {
                foreach (var number in SudokuList[y / 3, col].GetRow(y % 3))
                {
                    index++;
                    rowArray[index] = number;
                }
            }

            return rowArray;
        }

        public int[] GetColumn(int x)
        {
            int[] columnArray = new int[9];
            int index = -1;
            int rows = SudokuList.GetLength(0);
            for (int row = 0; row < rows; row++)
            {
                foreach (var number in SudokuList[row, x / 3].GetColumn(x % 3))
                {
                    index++;
                    columnArray[index] = number;
                }
            }

            return columnArray;

        }

        public MiniSudoku GetRandomMiniSudoku()
        {
            return SudokuList[rnd.Next(3), rnd.Next(3)];
        }
    }
}