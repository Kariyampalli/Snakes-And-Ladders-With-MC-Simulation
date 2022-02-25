using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    public class Configuration
    {
        private LSDistributionConfiguration lsdConfig;
        private int slMaxAmount;
        private int simulations;
        private int boardHeight;
        private int boardWidth;
        private int diceSize;
        public EventHandler<WriteOnConsoleEventArgs> writeArgs;
        public ConsoleWriter writer;
        Random random;

        public LSDistributionConfiguration LSDConfig
        {
            get
            {
                return this.lsdConfig;
            }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Snakes and Ladders configuration was null!");
                }
                this.lsdConfig = value;
            }
        }

        public int SLMaxAmount
        {
            get
            {
                return this.slMaxAmount;
            }
            set
            {
                if (value > (int)Math.Floor(((double)this.boardHeight * (double)this.BoardWidth - 2) / 2))
                {
                    throw new InvalidOperationException("sl max amount is too large");
                }
                this.slMaxAmount = value;
            }
        }
        public int Simulations
        {
            get
            {
                return this.simulations;
            }
            set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException("Simulation value is too small or too large!");
                }
                this.simulations = value;
            }
        }
        public int BoardHeight
        {
            get
            {
                return this.boardHeight;
            }
            set
            {
                if (value < 8)
                {
                    throw new InvalidOperationException("Board height too small!");
                }
                this.boardHeight = value;
            }
        }
        public int BoardWidth
        {
            get
            {
                return this.boardWidth;
            }
            set
            {
                if (value < 8)
                {
                    throw new InvalidOperationException("Board width too small!");
                }
                this.boardWidth = value;
            }
        }

        public int DiceSize
        {
            get
            {
                return this.diceSize;
            }
            set
            {
                if (value < 1 || value > this.BoardHeight * this.BoardWidth)
                {
                    throw new InvalidOperationException("dice size too small or too large!");
                }
                this.diceSize = value;
            }
        }
        public Configuration()
        {
            this.LSDConfig = new LSDistributionConfiguration();
            this.writer = new ConsoleWriter();
            this.writeArgs += this.writer.WriteOnConsole;
            this.random = new Random();
        }
        public void Configure()
        {
            this.SetBoardSize();

            Console.Clear();
            this.SetDiceSize();

            Console.Clear();
            this.SetSnakes();

            Console.Clear();
            this.SetLadders();

            Console.Clear();
            this.SetSimulationAmount();
        }

        private void SetBoardSize()
        {
            
            int val = this.GetIntegerVal("Please put in a board dimensions (e.g. 8 = 8x8)!",8, 100);
            this.BoardHeight = val;
            this.BoardWidth = val;
            //this.boardWidth = this.GetIntegerVal("Please put in a board width!", 8, int.MaxValue);
            this.SLMaxAmount = (int)Math.Floor(((double)this.boardHeight * (double)this.BoardWidth - 2)/2);
        }
        private Distribution GetDistribution(string txt)
        {
            if(string.IsNullOrEmpty(txt) || string.IsNullOrWhiteSpace(txt))
            {
                throw new InvalidOperationException("Distribution text was invalid!");
            }
            Distribution distribution = Distribution.Randomly;
            bool askAgain = true;
            string input = "";
            while (askAgain)
            {
                this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"{txt}\n", ConsoleOutputType.Message, this));
                this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($">> ", ConsoleOutputType.Message, this));
                input = Console.ReadLine();
           
                switch (input.Trim())
                {
                    case "m":
                        askAgain = false;
                        distribution = Distribution.Manually;
                        break;
                    case "r":
                        askAgain = false;
                        distribution = Distribution.Randomly;
                        break;
                    default:
                        this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("Input is invalid!\n", ConsoleOutputType.Error, this));
                        break;
                }
            }
            return distribution;
        }

        private Tuple<Rate,int> GetRateAndAmount(Distribution distribution, string sl)
        {
            string input = "";
            Rate rate = Rate.Fixed;
            int amount = 0;

            if (distribution == Distribution.Randomly)
            {
                bool askAgain = true;
                while (askAgain)
                {
                    this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("Fixed rate or Max rate (f/m)\n", ConsoleOutputType.Message, this));
                    this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($">> ", ConsoleOutputType.Message, this));
                    input = Console.ReadLine();

                    int slMax = 0;
                    if(this.SLMaxAmount == (int)Math.Floor(((double)this.BoardHeight * (double)this.BoardWidth - 2) / 2))
                    {
                        slMax = this.SLMaxAmount - 1;
                    }
                    else
                    {
                        slMax = this.SLMaxAmount;
                    }

                    switch (input.Trim())
                    {
                        case "f":
                            rate = Rate.Fixed;
                            amount = this.GetIntegerVal($"Put in the amount of {sl} you want to be set on the board (max: {slMax})", 1, slMax);
                            this.SLMaxAmount = this.SLMaxAmount - amount;
                            askAgain = false;
                            break;
                        case "m":
                            rate = Rate.Max;
                            amount = this.GetIntegerVal($"Put in the max amount of {sl} you want to be set on the board (max: {slMax})", 1, slMax);
                            amount = this.random.Next(1, amount + 1);
                            this.SLMaxAmount = this.SLMaxAmount - amount;
                            askAgain = false;
                            break;
                        default:
                            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("Invalid input!\n", ConsoleOutputType.Error, this));
                            break;
                    }
                }
            }
            return Tuple.Create(rate,amount);
        }
        private void SetSnakes()
        {          
            this.LSDConfig.SnakeDistribution = this.GetDistribution("Set snakes manually or randomly (m/r)");
            Tuple<Rate,int> t = this.GetRateAndAmount(this.LSDConfig.SnakeDistribution, "snakes");
            this.LSDConfig.SnakeRate = t.Item1;
            this.LSDConfig.SnakesAmount = t.Item2;
        }

        private void SetLadders()
        {
            this.LSDConfig.LadderDistribution = this.GetDistribution("Set Ladders manually or randomly (m/r)");
            Tuple<Rate, int> t = this.GetRateAndAmount(this.LSDConfig.LadderDistribution, "ladders");
            this.LSDConfig.LadderRate = t.Item1;
            this.LSDConfig.LaddersAmount = t.Item2;
        }
        private void SetDiceSize()
        {
            bool askAgain = true;
            int val = 0;
            while (askAgain)
            {
                val = this.GetIntegerVal("Please put in a dice size!");
                if (val < 1 || val > this.BoardWidth * this.BoardHeight)
                {
                    this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"Value can't be less than 1 and bigger than the game board ({this.BoardWidth * this.BoardHeight})\n", ConsoleOutputType.Error, this));
                    continue;
                }
                askAgain = false;
            }
            this.diceSize = val;
        }

        private void SetSimulationAmount()
        {
            this.simulations = this.GetIntegerVal("How many simulations do you want to run?",1,int.MaxValue);
        }
        private int GetIntegerVal(string txt, int min, int max)
        {
            bool askAgain = true;
            int val = 0;
            while(askAgain)
            {
                val = this.GetIntegerVal(txt);
                if(val < min || val > max)
                {
                    this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"Value has to be between {min} and {max}\n", ConsoleOutputType.Error, this));
                    continue;
                }
                askAgain = false;
            }
            return val;
        }
        private int GetIntegerVal(string txt)
        {
            bool askAgain = true;
            int input = 0;
            while (askAgain)
            {
                try
                {
                    this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"{txt}\n", ConsoleOutputType.Message, this));
                    this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($">> ", ConsoleOutputType.Message, this));
                    input = Convert.ToInt32(Console.ReadLine());
                    askAgain = false;
                }
                catch (Exception)
                {
                    this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"Input is an invalid integer!\n", ConsoleOutputType.Error, this));
                }
            }
            return input;
        }
    }
}
