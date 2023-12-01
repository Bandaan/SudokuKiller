namespace SudokuKiller
{
    public class Error
    {
        public int column_1 { get; set; }
        public int column_2 { get; set; }
        public int row_1 { get; set; }
        public int row_2 { get; set; }
        public int errorColumn_1 { get; set; }
        public int errorColumn_2 { get; set; }
        public int errorRow_1 { get; set; }
        public int errorRow_2 { get; set; }
        
        public Error(int column_1, int column_2, int row_1, int row_2, int errorColumn_1, int errorColumn_2, int errorRow_1, int errorRow_2)
        {
            this.column_1 = column_1;
            this.column_2 = column_2;
            this.row_1 = row_1;
            this.row_2 = row_2;
            this.errorColumn_1 = errorColumn_1;
            this.errorColumn_2 = errorColumn_2;
            this.errorRow_1 = errorRow_1;
            this.errorRow_2 = errorRow_2;
        }
    }
}