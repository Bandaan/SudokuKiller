using System.Reflection.Metadata.Ecma335;

namespace SudokuKiller
{
    public class Getal
    {
        public int number { get; set; }
        public bool vast { get; set; }
        
        public Getal(int number, bool vast)
        {
            this.number = number;
            this.vast = vast;
        }
    }
}