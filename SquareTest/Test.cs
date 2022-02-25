using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Monte_Carlo_Simulation.Code;

namespace SquareTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestPropertiess()
        {
            Square square;
            try
            {
               square = new Square(1, -1, 1);
            }
            catch(InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "X-value cannot be less than 0!");
            }

            try
            {
                square = new Square(1, 1, -1);
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Y-value cannot be less than 0!");
            }
            square = new Square(1, 1, 1);
        }
    }
}
