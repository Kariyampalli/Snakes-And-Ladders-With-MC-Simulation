using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    public class Square
    {
        public int Number { get; private set; }
        private int x;
        private int y;
        public int X 
        {
            get
            {
                return this.x;
            }
            private set
            {
                if(value < 0)
                {
                    throw new InvalidOperationException("X-value cannot be less than 0!");
                }
                this.x = value;
            }
        }
        public int Y
        {
            get
            {
                return this.y;
            }
            private set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Y-value cannot be less than 0!");
                }
                this.y = value;
            }
        }
        public ISL ISnkLdr { get; set; }

        public Square(int number, int x, int y)
        {
            this.Number = number;
            this.X = x;
            this.Y = y;
        }
    }
}
