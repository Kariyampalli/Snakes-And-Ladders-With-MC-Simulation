using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Monte_Carlo_Simulation.Code;
namespace DiceTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestProperties()
        {
            Dice dice;
            try
            {
                dice = new Dice(0);
            }
            catch(InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Dice sides are too small!");
            }
            dice = new Dice(10);
        }

        [TestMethod]
        public void TestRoll()
        {
            Dice dice = new Dice(10);
            int roll = dice.Roll();
            Assert.IsTrue(roll >= 1 && roll <= 10);
        }
    }
}
