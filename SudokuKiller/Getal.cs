using System.Reflection.Metadata.Ecma335;

namespace SudokuKiller
{
    public class Getal
    {
        private int Number { get; }
        private bool Fixed { get; set; }
        
        public Getal(int number, bool vast)
        {
            Number = number;
            Fixed = vast;
        }
    }
}