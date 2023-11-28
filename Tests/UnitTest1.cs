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
            List<int> expected1 = new List<int>() { 4, 6, 7, 9, 0 };

            Assert.AreEqual(ParseHelper.FindRemainingNumbers(actual1), expected1);
            
            int[] actual2 = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<int> expected2 = new List<int>() { 1, 2, 3, 4, 5 , 6, 7, 8, 9, 0 };

            Assert.AreEqual(ParseHelper.FindRemainingNumbers(actual2), expected2);
            
            int[] actual3 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<int> expected3 = new List<int>() {0};


            int[] actual4 = { 0, 0, 3, 0, 2, 0, 6, 0, 0 };
            List<int> expected4 = new List<int>() {1, 4, 5, 7, 8, 9, 0 };
            
            Assert.AreEqual(ParseHelper.FindRemainingNumbers(actual3), expected3);
        }
        
        [Test]
        public void TestFunc_ParseSudokuFillNumbers()
        {   
            int[] actual1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Getal[,] actualMiniSudokuList = ParseHelper.FillNumbers(actual1).MiniSudokuList;
            
            Getal[,] expected1 = {
                { new Getal(1, true) , new Getal(2, true), new Getal(3, true)},
                { new Getal(4, true) , new Getal(5, true), new Getal(6, true)},
                { new Getal(7, true) , new Getal(8, true), new Getal(9, true)}
            };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(actualMiniSudokuList[i, j].Fixed, expected1[i, j].Fixed);
                    Assert.AreEqual(actualMiniSudokuList[i, j].Number, expected1[i, j].Number);
                }
                
            }

            //CollectionAssert.AreEqual(expected1, actualMiniSudokuList);
            

            //CollectionAssert.AreEqual(ParseHelper.FillNumbers(actual1).MiniSudokuList, expected1, new Getal);
            
        }
    }
}