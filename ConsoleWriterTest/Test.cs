using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Monte_Carlo_Simulation.Code;

namespace ConsoleWriterTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestValidateMessage()
        {
            ConsoleWriter writer = new ConsoleWriter();

            PrivateObject obj = new PrivateObject(writer);
            Assert.IsFalse((bool)obj.Invoke("ValidateMessage", string.Empty));
            Assert.IsFalse((bool)obj.Invoke("ValidateMessage", " "));
            Assert.IsFalse((bool)obj.Invoke("ValidateMessage", new object[] { null }));
            Assert.IsTrue((bool)obj.Invoke("ValidateMessage", "Hi"));
        }
    }
}
