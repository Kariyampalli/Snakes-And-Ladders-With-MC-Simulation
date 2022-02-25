using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    public class Dice
    {
        private int sides;
        public int Sides
        {
            get
            {
                return this.sides;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException("Dice sides are too small!");
                }
                this.sides = value;
            }
        }
        private readonly Random random;
        public Dice(int s)
        {
            this.Sides = s;
            this.random = new Random();
        }
        public int Roll()
        {
            return this.random.Next(1, Sides + 1);
        }
    }
}
