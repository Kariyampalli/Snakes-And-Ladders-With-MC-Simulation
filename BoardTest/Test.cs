using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Monte_Carlo_Simulation.Code;
namespace BoardTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestProperties()
        {
            GameCreator creator = new GameCreator();
            SALBoard board;
            PrivateObject obj = new PrivateObject(creator);
            Square[,] squares = (Square[,])obj.Invoke("CreateSquares", new object[] { 10, 10 }); 

            try
            {
                board = new SALBoard(squares,10,7);
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Board height too small!");
            }
            try
            {
                board = new SALBoard(squares, 10, -1);
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Board height too small!");
            }

            try
            {
                board = new SALBoard(squares, 7, 10);
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Board width too small!");
            }

            try
            {
                board = new SALBoard(squares, -1, 10);
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Board width too small!");
            }

            try
            {
                board = new SALBoard(null, 10, 7);
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Squares was null");
            }
        }

        [TestMethod]
        public void TestMovePlayer()
        {           
            GameCreator creator = new GameCreator();
            PrivateObject obj = new PrivateObject(creator);
            Square[,] squares = (Square[,])obj.Invoke("CreateSquares", new object[] { 10, 10 });
            SALBoard board = new SALBoard(squares, 10, 10);
            Player player = new Player(board.SALSquares[9,0]);

            try
            {
                board.MovePlayer(null, 10);
            }
            catch(InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Player was null or steps had the value 0");
            }

            try
            {
                board.MovePlayer(null, 10);
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message == "Player was null or steps had the value 0");
            }

            Assert.IsTrue(board.MovePlayer(player,13).Number == 14);
            player = new Player(board.SALSquares[0, 1]);
            Assert.IsTrue(board.MovePlayer(player, 3).Number == 99);
            Assert.IsTrue(board.MovePlayer(player, 1).Number == 100);

            Snake snake = new Snake();
            snake.From = board.SALSquares[9, 1];
            snake.To = board.SALSquares[0, 0];
            board.SALSquares[9, 1].ISnkLdr = snake;
            player = new Player(board.SALSquares[9, 0]); //Sqaure 1, jumps on to two and slides on its snake up to 100
            Assert.IsTrue(board.MovePlayer(player, 1).Number == 100);
        }
    }
}
