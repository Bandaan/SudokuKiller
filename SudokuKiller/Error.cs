namespace SudokuKiller
{
    public class Error
    {
        public int columIndex;
        public int columnError;
        public int rowIndex;
        public int rowError;

        public Error(int columindex, int columerror, int rowindex, int rowerror)  
        {
            columIndex = columindex;
            columnError = columerror;
            rowIndex = rowindex;
            rowError = rowerror;
        }
    }
}