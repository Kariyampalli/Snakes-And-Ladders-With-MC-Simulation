using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Monte_Carlo_Simulation.Code;

namespace GameCreatorTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestCreateBoard()
        {
            GameCreator creator = new GameCreator();
            try
            {
                creator.CreateBoard(null);
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Configuration can't be null!");
            }
        }
        [TestMethod]
        public void TestPrintAvailableSquares()
        {
            GameCreator creator = new GameCreator();
            PrivateObject obj = new PrivateObject(creator);
            try
            {
                obj.Invoke("PrintAvailableSquares", new object[] { null });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Configuration can't be null!");
            }
        }
        [TestMethod]
        public void TestPlaceSLManually()
        {
            GameCreator creator = new GameCreator();
            PrivateObject obj = new PrivateObject(creator);
            Square[,] squares = (Square[,])obj.Invoke("CreateSquares", new object[] { 10, 10 });
           
            try
            {
                obj.Invoke("PlaceSLManually", new object[] { null, squares, "s","f", "l", true });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "PlaceSLManually method received invalid parameter values!");
            }

            try
            {
                obj.Invoke("PlaceSLManually", new object[] { new Configuration(), null, "s", "f", "l", true });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "PlaceSLManually method received invalid parameter values!");
            }

            try
            {
                obj.Invoke("PlaceSLManually", new object[] { new Configuration(), squares, string.Empty, "f", "l", true });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "PlaceSLManually method received invalid parameter values!");
            }

            try
            {
                obj.Invoke("PlaceSLManually", new object[] { new Configuration(), squares, null, "f", "l", true });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "PlaceSLManually method received invalid parameter values!");
            }

            try
            {
                obj.Invoke("PlaceSLManually", new object[] { new Configuration(), squares, "s", string.Empty, "l", true });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "PlaceSLManually method received invalid parameter values!");
            }

            try
            {
                obj.Invoke("PlaceSLManually", new object[] { new Configuration(), squares, "s", null, "l", true });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "PlaceSLManually method received invalid parameter values!");
            }

            try
            {
                obj.Invoke("PlaceSLManually", new object[] { new Configuration(), squares, "s", "a", string.Empty, true });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "PlaceSLManually method received invalid parameter values!");
            }

            try
            {
                obj.Invoke("PlaceSLManually", new object[] { new Configuration(), squares, "s", "a", null, true });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "PlaceSLManually method received invalid parameter values!");
            }
        }
        [TestMethod]
        public void TestTempCreateSnakes()
        {
            GameCreator creator = new GameCreator();
            PrivateObject obj = new PrivateObject(creator);
            Square[,] squares = (Square[,])obj.Invoke("CreateSquares", new object[] { 10, 10 });

            try
            {
                obj.Invoke("TempCreateSnakes", new object[] { null, squares });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "TempCreateSnakes method received invalid parameter values!");
            }

            try
            {
                obj.Invoke("TempCreateSnakes", new object[] { new Configuration(), null });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "TempCreateSnakes method received invalid parameter values!");
            }
        }

        [TestMethod]
        public void TestTempCreateLadders()
        {
            GameCreator creator = new GameCreator();
            PrivateObject obj = new PrivateObject(creator);
            Square[,] squares = (Square[,])obj.Invoke("CreateSquares", new object[] { 10, 10 });

            try
            {
                obj.Invoke("TempCreateLadders", new object[] { null, squares });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "TempCreateLadders method received invalid parameter values!");
            }

            try
            {
                obj.Invoke("TempCreateLadders", new object[] { new Configuration(), null });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "TempCreateLadders method received invalid parameter values!");
            }
        }

        [TestMethod]
        public void TestCreateSquares()
        {
            GameCreator creator = new GameCreator();
            PrivateObject obj = new PrivateObject(creator);

            try
            {
                obj.Invoke("CreateSquares", new object[] { 0, 8 });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "CreateSquares method received invalid board sizes!");
            }

            try
            {
                obj.Invoke("CreateSquares", new object[] { 8, 0 });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "CreateSquares method received invalid board sizes!");
            }

            try
            {
                obj.Invoke("CreateSquares", new object[] { 9, 10 });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "CreateSquares method received invalid board sizes!");
            }

           Square[,]squares = (Square[,])obj.Invoke("CreateSquares", new object[] { 10, 10 });
            Assert.IsTrue(squares.Length == 100);
        }

        [TestMethod]
        public void TestCreateSnakesRandomly()
        {
            GameCreator creator = new GameCreator();
            PrivateObject obj = new PrivateObject(creator);

            try
            {
                obj.Invoke("CreateSnakesRandomly", new object[] { null });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "CreateSnakesRandomly method received invalid configuration!");
            }
        }

        [TestMethod]
        public void TestCreateLaddersRandomly()
        {
            GameCreator creator = new GameCreator();
            PrivateObject obj = new PrivateObject(creator);

            try
            {
                obj.Invoke("CreateLaddersRandomly", new object[] { null });
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "CreateLaddersRandomly method received invalid configuration!");
            }
        }
    }
}
