using System.Reflection.Metadata.Ecma335;

namespace SudokuKiller
{
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