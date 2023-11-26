namespace SudokuKiller;

public static class ParseHelper
{
    public static Sudoku ParseSudoku(string[] input)
    {
        foreach (var VARIABLE in input)
        {
            
            
        }
        
    }

    public static IEnumerable<int> FindRemainingNumbers(int[] minisoduku)
    {
        for (int i = 1; i <= 9; i++)
        {
            if (!minisoduku.Contains(i))
            {
                yield return i;
            }
        }
    }
    
}