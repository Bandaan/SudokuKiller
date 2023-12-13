using NUnit.Framework;

namespace SudokuKiller.Tests
{
    [TestFixture]
    public class ParseHelperUnitTests
    {
        [Test]
        public void TestFunc_ParseSudokuRemainingNumbers()
        {   
            int[] actual1 = { 0, 2, 3, 5, 8, 0, 0, 0, 1 };
            List<int> expected1 = new List<int>() { 4, 6, 7, 9};

            Assert.AreEqual(ParseHelper.FindRemainingNumbers(actual1), expected1);
            
            int[] actual2 = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<int> expected2 = new List<int>() { 1, 2, 3, 4, 5 , 6, 7, 8, 9};

            Assert.AreEqual(ParseHelper.FindRemainingNumbers(actual2), expected2);
            
            int[] actual3 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<int> expected3 = new List<int>() {};


            int[] actual4 = { 0, 0, 3, 0, 2, 0, 6, 0, 0 };
            List<int> expected4 = new List<int>() {1, 4, 5, 7, 8, 9 };
            
            Assert.AreEqual(ParseHelper.FindRemainingNumbers(actual3), expected3);
        }
        
        [Test]
        public void TestFunc_ParseSudokuFillNumbers()
        {

            void Check(Number[,] actual, Number[,] expected)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Assert.AreEqual(actual[i, j].stuck, expected[i, j].stuck);
                        Assert.AreEqual(actual[i, j].number, expected[i, j].number);
                    }
                }
                
            }
            int[] actual1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Number[,] expected1 = {
                { new Number(1, true) , new Number(2, true), new Number(3, true)},
                { new Number(4, true) , new Number(5, true), new Number(6, true)},
                { new Number(7, true) , new Number(8, true), new Number(9, true)}
            };
            
            Check(ParseHelper.FillNumbers(actual1).MiniSudokuList, expected1);
            
            int[] actual2 = { 0, 2, 3, 5, 8, 0, 0, 0, 1 };

            Number[,] expected2 = {
                { new Number(4, false) , new Number(2, true), new Number(3, true)},
                { new Number(5, true) , new Number(8, true), new Number(6, false)},
                { new Number(7, false) , new Number(9, false), new Number(1, true)}
            };
            
            Check(ParseHelper.FillNumbers(actual2).MiniSudokuList, expected2);
            
            
            int[] actual3 = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            Number[,] expected3 = {
                { new Number(1, false) , new Number(2, false), new Number(3, false)},
                { new Number(4, false) , new Number(5, false), new Number(6, false)},
                { new Number(7, false) , new Number(8, false), new Number(9, false)}
            };

        }
    }
}