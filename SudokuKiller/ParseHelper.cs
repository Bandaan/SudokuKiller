namespace SudokuKiller;

/// <summary>
/// Represents a helper class that provides parse functions for the sudoku.
/// </summary>
public static class ParseHelper
{
    /// <summary>
    /// Adds two numbers and returns the result.
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The sum of the two numbers.</returns>
    public static Sudoku ParseSudoku(string[] input)
    {
        Sudoku sudoku = new Sudoku();
        
        for (int i = 0; i < 9; i++)
        {
            int[] miniSudoku = new int[9];
            for (int j = 0; j < 9; j++)
            {
                miniSudoku[j] = int.Parse(input[(i / 3 * 3 + j / 3) * 9 + i % 3 * 3 + j % 3]);
            }
            
            sudoku.AddMiniSudoku(FillNumbers(miniSudoku));
        }
        return sudoku;
    }

    public static List<int> FindRemainingNumbers(int[] minisoduku)
    {
        List<int> numbers = new List<int>();
        for (int i = 1; i <= 9; i++)
        {
            if (!minisoduku.Contains(i))
            {
                numbers.Add(i);
            }
        }
        return numbers;
    }
    
    public static MiniSudoku FillNumbers(int[] miniSudoku)
    {
        MiniSudoku newMini = new MiniSudoku();
        List<int> remainingNumbers = FindRemainingNumbers(miniSudoku);

        int index = -1;
        foreach (var number in miniSudoku)
        {
            newMini.AddGetal((number != 0)
                ? new Getal(number, true)
                : new Getal(remainingNumbers[++index], false));
        }

        return newMini;
    }

}