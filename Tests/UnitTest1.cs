using NUnit.Framework;

namespace SudokuKiller.Tests
{
    [TestFixture]
    public class ProgramTests
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

            void Check(Getal[,] actual, Getal[,] expected)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Assert.AreEqual(actual[i, j].Fixed, expected[i, j].Fixed);
                        Assert.AreEqual(actual[i, j].Number, expected[i, j].Number);
                    }
                }
                
            }
            int[] actual1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Getal[,] expected1 = {
                { new Getal(1, true) , new Getal(2, true), new Getal(3, true)},
                { new Getal(4, true) , new Getal(5, true), new Getal(6, true)},
                { new Getal(7, true) , new Getal(8, true), new Getal(9, true)}
            };
            
            Check(ParseHelper.FillNumbers(actual1).MiniSudokuList, expected1);
            
            int[] actual2 = { 0, 2, 3, 5, 8, 0, 0, 0, 1 };

            Getal[,] expected2 = {
                { new Getal(4, false) , new Getal(2, true), new Getal(3, true)},
                { new Getal(5, true) , new Getal(8, true), new Getal(6, false)},
                { new Getal(7, false) , new Getal(9, false), new Getal(1, true)}
            };
            
            Check(ParseHelper.FillNumbers(actual2).MiniSudokuList, expected2);
            
            
            int[] actual3 = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            Getal[,] expected3 = {
                { new Getal(1, false) , new Getal(2, false), new Getal(3, false)},
                { new Getal(4, false) , new Getal(5, false), new Getal(6, false)},
                { new Getal(7, false) , new Getal(8, false), new Getal(9, false)}
            };

        }
    }
}