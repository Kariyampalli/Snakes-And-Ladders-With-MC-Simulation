using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Monte_Carlo_Simulation.Code;

namespace SnakesAndLaddersTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestPlay()
        {
            SnakesAndLadders sal = new SnakesAndLadders();
            try
            {
                sal.Play("asd");
            }
            catch(InvalidCastException ex)
            {
                Assert.IsTrue(ex.Message == "Play method receidev an invalid thread number");
            }

            try
            {
                sal.Play(null);
            }
            catch (InvalidCastException ex)
            {
                Assert.IsTrue(ex.Message == "Play method receidev an invalid thread number");
            }

        }
    }
}
