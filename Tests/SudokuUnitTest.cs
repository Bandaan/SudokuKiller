namespace SudokuKiller.Tests
{
    [TestFixture]
    public class SudokuUnitTest
    {
        [Test]
        public void TestFunc_FindRow()
        {

            string[] ParseToString(int[] input)
            {
                string[] inputString = new string[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    inputString[i] = input[i].ToString();
                }

                return inputString;
            }

            void Check(int[] actual, int[] expected)
            {
                for (int i = 0; i < expected.Length; i++)
                {
                    Assert.AreEqual(actual[i], expected[i]);
                }
            }
            
            int[] actual1 = new int[]
            {
                1, 1, 1, 1, 1, 1, 1, 1, 1,
                2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 2, 3, 4, 5, 6, 7, 8, 9,
                3, 3, 3, 3, 3, 3, 3, 3, 3,
                4, 4, 4, 4, 4, 4, 4, 4, 4,
                5, 5, 5, 5, 5, 5, 5, 5, 5,
                6, 6, 6, 6, 6, 6, 6, 6, 6,
                7, 7, 7, 7, 7, 7, 7, 7, 7,
                8, 8, 8, 8, 8, 8, 8, 8, 8
            };

            Sudoku sudoku = ParseHelper.ParseSudoku(ParseToString(actual1));

            int[] expected1 = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int[] expected2 = new int[] { 8, 8, 8, 8, 8, 8, 8, 8, 8 };
            
            int[] expected3 = new int[] { 1, 2, 4, 3, 4, 5, 6, 7, 8 };
            int[] expected4 = new int[] { 1, 2, 6, 3, 4, 5, 6, 7, 8 };
            
            Check(sudoku.GetRow(0), expected1);
            Check(sudoku.GetRow(8), expected2);
            
            Check(sudoku.GetColumn(3), expected3);
            Check(sudoku.GetColumn(5), expected4);
        }
    
    }
    
}