using System.Reflection.Metadata.Ecma335;

namespace SudokuKiller
{
    public class Getal
    {
        public int Number { get; set; }
        public bool Fixed { get; set; }
        
        public Getal(int number, bool vast)
        {
            Number = number;
            Fixed = vast;
        }
    }
}