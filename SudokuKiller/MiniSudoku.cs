namespace SudokuKiller
{
    public class MiniSudoku
    {
        public Getal[,] MiniSudokuList = new Getal[3, 3];
        private int x, y;

        public MiniSudoku()
        {
            x = 0;
            y = 0;
        }

        public void AddGetal(Getal getal)
        {
            MiniSudokuList[x, y] = getal;
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
