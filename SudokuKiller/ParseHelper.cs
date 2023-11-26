namespace SudokuKiller;

public static class ParseHelper
{
    public static Sudoku ParseSudoku(string[] input)
    {
        Sudoku sudoku = new Sudoku();
        for (int i = 0; i < 9; i++)
        {
            Getal[] miniSudoku = new Getal[9];
            for (int j = 0; j < 9; j++)
            {
                if (int.Parse(input[i * 9 + j]) == 0)
                {
                    miniSudoku[j] = new Getal(0, false);
                }
                else
                {
                    miniSudoku[j] = new Getal(int.Parse(input[i * 9 + j]), true);
                }
            }
            sudoku.AddMiniSudoku(FillNumbers(miniSudoku));
        }
        return sudoku;
    }

    static IEnumerable<int> FindRemainingNumbers(Getal[] minisoduku)
    {
        for (int i = 1; i <= 9; i++)
        {
            if (!minisoduku.Any(getal => getal.Number == i))
            {
                yield return i;
            }
        }
    }
    
    static MiniSudoku FillNumbers(Getal[] miniSudoku)
    {
        // DEZE FUNCTIE MOET NOG WORDEN AFGEMAAKT
        // ALLE NUMMERS MET 0 OMWISSELN MET NUMMER UIT "remainingnumbers"
        
        int index = -1;
        var remainingNumbers = FindRemainingNumbers(miniSudoku);
        MiniSudoku newMini = new MiniSudoku();
    
        for (int j = 0; j < miniSudoku.Length; j++)
        {
            if (miniSudoku[j].Number.Equals(0))
            {
                index++;
                
                if (index < remainingNumbers.Count())
                {
                    miniSudoku[j].Number = remainingNumbers.ElementAt(index);
                    newMini.AddGetal(miniSudoku[j]);
                }
            }
        }
    
        return newMini;
    }

}