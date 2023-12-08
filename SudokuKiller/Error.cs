namespace SudokuKiller
{
    public class Error
    {
        public Coordinaat punt1 { get; set; }
        public Coordinaat punt2 { get; set; }
        public Coordinaat error1 { get; set; }
        public Coordinaat error2 { get; set; }
        
        public Error(int column1, int column2, int row1, int row2, int errorColumn1, int errorColumn2, int errorRow1, int errorRow2)
        {
            this.column_1 = column_1;
            this.column_2 = column_2;
            this.row_1 = row_1;
            this.row_2 = row_2;
        }
    }
}