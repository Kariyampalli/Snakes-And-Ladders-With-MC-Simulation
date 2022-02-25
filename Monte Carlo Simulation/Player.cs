using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    public class Player
    {
        private Square currentSquare;
        public Square CurrentSquare
        {
            get
            {
                return this.currentSquare;
            }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Player can't be placed on a non-existing square!");
                }
                this.currentSquare = value;
            }
        } //Current square player is on
        public Player(Square startingSqaure)
        {
            this.CurrentSquare = startingSqaure;
        }
    }
}
