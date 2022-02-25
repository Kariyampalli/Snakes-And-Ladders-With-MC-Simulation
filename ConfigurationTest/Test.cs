using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Monte_Carlo_Simulation.Code;
namespace ConfigurationTest
{
    [TestClass]
    public class Test
    {

        [TestMethod]
        public void TestProperties()
        {
            Configuration configuration = new Configuration();
            try
            {
                configuration.BoardHeight = 7;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Board height too small!");
            }

            try
            {
                configuration.BoardHeight = -1;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Board height too small!");
            }

            try
            {
                configuration.BoardWidth = 7;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Board width too small!");
            }

            try
            {
                configuration.BoardWidth = -1;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Board width too small!");
            }

            try
            {
                configuration.DiceSize = 0;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "dice size too small or too large!");
            }

            try
            {
                configuration.BoardWidth = 10;
                configuration.BoardHeight = 10;
                configuration.DiceSize = 101;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "dice size too small or too large!");
            }

            try
            {
                configuration.Simulations = 0;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Simulation value is too small or too large!");
            }

            try
            {
                configuration.Simulations = -1;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Simulation value is too small or too large!");
            }

            try
            {
                configuration.SLMaxAmount = 101;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "sl max amount is too large");
            }

            try
            {
                configuration.LSDConfig = null;
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Snakes and Ladders configuration was null!");
            }
            configuration.DiceSize = 80;
            configuration.SLMaxAmount = 49;
        }

        [TestMethod]
        public void TestGetDistribution()
        {
            Configuration config = new Configuration();
            PrivateObject obj = new PrivateObject(config);
            try
            {
                obj.Invoke("GetDistribution", new object[] { null });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Distribution text was invalid!");
            }

            try
            {
                obj.Invoke("GetDistribution", string.Empty);
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Distribution text was invalid!");
            }

            //Cant test everymethod because they await input per Console.Readline()
        }
    }
}
