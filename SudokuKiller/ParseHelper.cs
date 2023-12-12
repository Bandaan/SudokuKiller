namespace SudokuKiller;

/// <summary>
/// Represents a helper class that provides parse functions for the sudoku.
/// </summary>
public static class ParseHelper
{
    /// <summary>
    /// Calculates all the blocks and parses to sudoku.
    /// </summary>
    /// <param name="input">Array of 81 numbers.</param>
    /// <returns>Full sudoku</returns>
    public static Sudoku ParseSudoku(string[] input)
    {
        // Create new sudoku object
        Sudoku sudoku = new Sudoku();
        
        for (int i = 0; i < 9; i++)
        {
            // Create array for block of sudoku
            int[] miniSudoku = new int[9];
            for (int j = 0; j < 9; j++)
            {
                // Find the elements by index of current block
                miniSudoku[j] = int.Parse(input[(i / 3 * 3 + j / 3) * 9 + i % 3 * 3 + j % 3]);
            }
            
            // Add filled in block to sudoku
            sudoku.AddMiniSudoku(FillNumbers(miniSudoku));
        }
        
        // Return sudoku
        return sudoku;
    }
    
    /// <summary>
    /// Finds the remaining numbers of a block.
    /// </summary>
    /// <param name="miniSudoku">Minisudoku (block) from sudoku.</param>
    /// <returns>Remaining numbers between 1 till 9.</returns>
    /// 
    public static List<int> FindRemainingNumbers(int[] miniSoduku)
    {
        // Create list for remaining numbers
        List<int> numbers = new List<int>();
        for (int i = 1; i <= 9; i++)
        {
            if (!miniSoduku.Contains(i))
            {
                // If number not in block than add to list
                numbers.Add(i);
            }
        }
        // Return list of remaining numbers
        return numbers;
    }
    
    /// <summary>
    /// Fills in the remaining numbers for block.
    /// </summary>
    /// <param name="miniSudoku">Minisudoku (block) from sudoku.</param>
    /// <returns>Complete block from sudoku</returns>
    
    public static MiniSudoku FillNumbers(int[] miniSudoku)
    {
        // Create new block object
        MiniSudoku newMini = new MiniSudoku();
        // Create list for remaining numbers and call method.
        List<int> remainingNumbers = FindRemainingNumbers(miniSudoku);
        
        // Declare index -1
        int index = -1;
        foreach (var number in miniSudoku)
        {
            // If number not an zero than add to block with fixed to true
            // Else pick number from remaining numbers and add that to block with fixed false
            newMini.AddGetal((number != 0)
                ? new Getal(number, true)
                : new Getal(remainingNumbers[++index], false));
            // index + 1 to don't pick same number
        }
        
        // Return block
        return newMini;
    }

}