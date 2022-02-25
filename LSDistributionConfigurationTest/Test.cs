using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Monte_Carlo_Simulation.Code;

namespace LSDistributionConfigurationTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestProperties()
        {
            LSDistributionConfiguration config = new LSDistributionConfiguration();
            try
            {
                config.LaddersAmount = -1;
            }
            catch(InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Amount of ladders is too small!");
            }

            try
            {
                config.SnakesAmount = -1;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Amount of snakes is too small!");
            }

            config.SnakesAmount = 10;
            config.LaddersAmount = 10;
        }
    }
}
