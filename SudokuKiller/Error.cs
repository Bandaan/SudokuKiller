namespace SudokuKiller
{
    public class Error
    {
        public int column1 { get; set; }
        public int column2 { get; set; }
        public int row1 { get; set; }
        public int row2 { get; set; }
        public int errorColumn1 { get; set; }
        public int errorColumn2 { get; set; }
        public int errorRow1 { get; set; }
        public int errorRow2 { get; set; }
        
        public int eval;
        
        public Error(int fout, int column_1, int column_2, int row_1, int row_2, int errorColumn_1, int errorColumn_2, int errorRow_1, int errorRow_2)
        {
            eval = fout;
            column1 = column_1;
            column2 = column_2;
            row1 = row_1;
            row2 = row_2;
            errorColumn1 = errorColumn_1;
            errorColumn2 = errorColumn_2;
            errorRow1 = errorRow_1;
            errorRow2 = errorRow_2;
        }
    }
}