using NUnit.Framework;

namespace SudokuKiller.Tests
{
    [TestFixture]
    public class FindErrorUnitTest
    {
        [Test]
        public void TestFunc_ParseFindError()
        {   
            int[] actual1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int expected1 = 0;

            Assert.AreEqual(Algoritme.FindError(actual1), expected1);
            
            int[] actual2 = { 1, 2, 4, 4, 5, 6, 7, 8, 9 };
            int expected2 = 1;

            Assert.AreEqual(Algoritme.FindError(actual2), expected2);

            int[] actual3 = { 4, 2, 4, 4, 5, 6, 7, 4, 9 };
            int expected3 = 3;

            Assert.AreEqual(Algoritme.FindError(actual3), expected3);

            int[] actual4 = { 4, 2, 2, 4, 8, 6, 7, 8, 9 };
            int expected4 = 3;

            Assert.AreEqual(Algoritme.FindError(actual4), expected4);
        }
    }
}