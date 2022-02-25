using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    public class SALBoard
    {
        private Square[,] squares;
        private int width;
        private int height;
        public int Width
        {
            get
            {
                return this.width;
            }
            private set
            {
                if(value < 8)
                {
                    throw new InvalidOperationException("Board width too small!");
                }
                this.width = value;
            }
        }
        public int Height
        {
            get
            {
                return this.height;
            }
            private set
            {
                if (value < 8)
                {
                    throw new InvalidOperationException("Board height too small!");
                }
                this.height = value;
            }
        }
        public Square[,] SALSquares
        {
            get
            {
                return this.squares;
            }
            private set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Squares was null");
                }
                this.squares = value;
            }
        }

        public SALBoard(Square[,] squares, int width, int height)
        {
            this.SALSquares = squares;
            this.Width = width;
            this.Height = height;
        }

        public Square MovePlayer(Player player, int steps)
        {
            if(player == null || steps == 0)
            {
                throw new InvalidOperationException("Player was null or steps had the value 0");
            }
            //If the following steps would put you on or over the finihline --> return the last/finsihing square
            if (player.CurrentSquare.Number + steps == this.Height * this.Width)
            {
                return this.SALSquares[0, 0];
            }
            if (player.CurrentSquare.Number + steps >= this.Height * this.Width)
            {
                return player.CurrentSquare;
            }

            int expectedSquare = player.CurrentSquare.Number + steps;
            Square currentlyPointedSquare = player.CurrentSquare;
            int x = player.CurrentSquare.X;
            int y = player.CurrentSquare.Y;

            for (int i = 0; i < steps; i++)
            {
                if (x != 0 && this.SALSquares[currentlyPointedSquare.Y, currentlyPointedSquare.X - 1].Number > this.SALSquares[y, x].Number)
                {
                    x = x - 1;
                    currentlyPointedSquare = this.SALSquares[y, x];
                }
                else if (x != this.Width - 1 && this.SALSquares[currentlyPointedSquare.Y, currentlyPointedSquare.X + 1].Number > this.SALSquares[y, x].Number)
                {
                    x = x + 1;
                    currentlyPointedSquare = this.SALSquares[y, x];
                }
                else if (x == 0 && y != this.Height - 1)
                {
                    y--;
                    currentlyPointedSquare = this.SALSquares[y, x];
                }
                else if (x == this.Width - 1)
                {
                    y--;
                    currentlyPointedSquare = this.SALSquares[y, x];
                }
            }
            if (currentlyPointedSquare.Number != expectedSquare)
            {
                throw new Exception($"Player didn't land on the expected square/field!\nExpected{expectedSquare}\nLanded{currentlyPointedSquare.Number}");
            }
            if (currentlyPointedSquare.ISnkLdr != null && currentlyPointedSquare.ISnkLdr.From == currentlyPointedSquare)
            {
                currentlyPointedSquare = currentlyPointedSquare.ISnkLdr.To;
            }
            return currentlyPointedSquare;
        }
    }
}
