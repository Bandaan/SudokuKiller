
namespace SudokuKiller
{
    /// <summary>
    /// Represents a helper class that defines a number in a cell of the sudoku.
    /// </summary>
    public class Getal
    {
        public int number { get; set; }
        public bool vast { get; set; }

        public Getal(int nummer, bool solid)
        {
            number = nummer;
            vast = solid;
        }
    }
}