using NUnit.Framework;

namespace SudokuKiller.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void TestFunc_ShouldReturnTrue()
        {
            // Arrange
            SudokuKiller.Program program = new SudokuKiller.Program();

            // Act
            bool result = program.testFunc();

            // Assert
            Assert.IsTrue(result);
        }
    }
}