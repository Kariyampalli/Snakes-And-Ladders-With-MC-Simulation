using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Monte_Carlo_Simulation.Code;

namespace PlayerTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestProperties()
        {
            try
            {
                Player player = new Player(null);
            }
            catch(InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Player can't be placed on a non-existing square!");
            }
        }
    }
}
