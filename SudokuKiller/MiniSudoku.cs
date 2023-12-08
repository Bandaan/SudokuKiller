using System.Runtime.InteropServices;

namespace SudokuKiller
{
    public class MiniSudoku
    {
        public Getal[,] MiniSudokuList = new Getal[3, 3];
        private int x, y;
        Random rnd;
        
        public int x_pos, y_pos;

        public MiniSudoku()
        {
            x = 0;
            y = 0;
            rnd = new Random();
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

        public void Swap(Coordinaat left, Coordinaat right)
        {
            Getal tempGetal = MiniSudokuList[left.column, left.row];
            
            MiniSudokuList[left.column, left.row] = MiniSudokuList[right.column, right.row];
            MiniSudokuList[right.column, right.row] = tempGetal;
        }

        public Tuple<Coordinaat, Coordinaat> GetRandomSwap()
        {
            Coordinaat pos1 = new Coordinaat(0, 0);
            Coordinaat pos2 = new Coordinaat(0, 0);
            
            while (pos1.column == pos2.column && pos1.row == pos2.row || MiniSudokuList[pos1.column, pos1.row].vast || MiniSudokuList[pos2.column, pos2.row].vast)
            {
                pos1.column = rnd.Next(3);
                pos1.row = rnd.Next(3);
                pos2.column = rnd.Next(3);
                pos2.row = rnd.Next(3);
            }
            
            return new Tuple<Coordinaat, Coordinaat>(pos1, pos2);
        }

        public void Print()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(MiniSudokuList[j, i].number + " ");
                }
                Console.WriteLine();
            }
            
            Console.WriteLine("-----------------------");
        }
    }
}
