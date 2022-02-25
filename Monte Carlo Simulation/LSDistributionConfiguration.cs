using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    public enum Rate
    {
        Fixed,
        Max
    }
    public enum Distribution
    {
        Manually,
        Randomly
    }
    public class LSDistributionConfiguration
    {
        private int snakesAmount;
        private int laddersAmount;
        public Distribution SnakeDistribution { get; set; }
        public Rate SnakeRate { get; set; }
        public int SnakesAmount
        {
            get
            {
                return this.snakesAmount;
            }
            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Amount of snakes is too small!");
                }
                this.snakesAmount = value;
            }
        }
        public Distribution LadderDistribution { get; set; }
        public Rate LadderRate { get; set; }
        public int LaddersAmount
        {
            get
            {
                return this.laddersAmount;
            }
            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Amount of ladders is too small!");
                }
                this.laddersAmount = value;
            }
        }
    }
}
