using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    public class SnakesAndLadders
    {
        private Configuration configuration;
        private List<int> minDiceRols;
        private int doneThreads;
        private int totalRolls;
        private int minRolls;
        private int maxRolls;
        private Dice dice;

        public EventHandler<WriteOnConsoleEventArgs> writeArgs;
        public ConsoleWriter writer;
        private GameCreator creator;
        private SALBoard Board { get; set; }
        private Dice SALDice { get; set; }

        private object locker;

        public SnakesAndLadders()
        {
            this.minDiceRols = new List<int>();
            this.maxRolls = 0;
            this.minRolls = int.MaxValue;
            this.totalRolls = 0;
            this.locker = new object();
            this.creator = new GameCreator();
            this.configuration = new Configuration();
            this.writer = new ConsoleWriter();
            this.writeArgs += this.writer.WriteOnConsole;
        }
        private void ConfigureSettings()
        {
            this.configuration.Configure();
        }


        private void PrintStats()
        {
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"BOARD: {this.configuration.BoardWidth} x {this.configuration.BoardHeight}\n", ConsoleOutputType.Message, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"DICE SIZE: {this.configuration.DiceSize}\n", ConsoleOutputType.Message, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"LADDER DISTRIBUTION: {this.configuration.LSDConfig.LadderDistribution}\n", ConsoleOutputType.Message, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"LADDER RATE: {this.configuration.LSDConfig.LadderRate}\n", ConsoleOutputType.Message, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"AMOUNT OF LADDERS: {this.configuration.LSDConfig.LaddersAmount}\n", ConsoleOutputType.Message, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"SNAKE DISTRIBUTION: {this.configuration.LSDConfig.SnakeDistribution}\n", ConsoleOutputType.Message, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"SNAKE RATE: {this.configuration.LSDConfig.SnakeRate}\n", ConsoleOutputType.Message, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"AMOUNT OF SNAKES: {this.configuration.LSDConfig.SnakesAmount}\n", ConsoleOutputType.Message, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"SIMULATIONS: {this.configuration.Simulations}\n", ConsoleOutputType.Message, this));


            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("\n--------------\nStatistics\n--------------\n", ConsoleOutputType.Message, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("(if avg. moves is close to 400 or/and highest moves is 400 it means some games needed more...\nthan 400 rolls to finish.Tipp: try setting your dice and board size better)\n", ConsoleOutputType.Other,ConsoleColor.Cyan, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"AVERAGE MOVES: {this.totalRolls / this.configuration.Simulations}\n--------------\n", ConsoleOutputType.Information, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"LOWEST MOVES: {this.minRolls}\n", ConsoleOutputType.Information, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"LOWEST MOVE DICE ROLLS: |", ConsoleOutputType.Information, this));
            foreach (var roll in this.minDiceRols)
            {
                this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($" {roll} |", ConsoleOutputType.Information, this));
            }
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"\n--------------\n", ConsoleOutputType.Information, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"HIGHEST MOVES: {this.maxRolls}\n", ConsoleOutputType.Information, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"--------------\n", ConsoleOutputType.Information, this));
        }
        public void Run()
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                this.ConfigureSettings();
                this.SALDice = new Dice(this.configuration.DiceSize);
                this.Board = this.creator.CreateBoard(this.configuration);

                Console.Clear();
                this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("Running simulations...\n", ConsoleOutputType.Information, this));
                var tasks = new Task[this.configuration.Simulations];
                stopWatch.Start();
                for (int i = 0; i < this.configuration.Simulations; i++)
                {
                    tasks[i] = (Task.Factory.StartNew(() => Play(i)));
                }

                Task.WaitAll(tasks);
                stopWatch.Stop();
                this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"OVERALL RUNNING TIME: {stopWatch.ElapsedMilliseconds}ms\n", ConsoleOutputType.Information, this));
                this.PrintStats();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs(ex.Message, ConsoleOutputType.Error, this));
                Console.ReadLine();
            }

            //await Task.Run(this.Simulate); 
        }
        public void Play(object threadInt)
        {
            if(!(threadInt is int))
            {
                throw new InvalidCastException("Play method receidev an invalid thread number");
            }
            List<int> rollsList = new List<int>();
            int threadNumber = (int)threadInt;
            int roll = 0;
            int rolls = 0;
            Player player = new Player(this.Board.SALSquares[this.Board.Height - 1, 0]);
            while (player.CurrentSquare.Number != this.configuration.BoardHeight * this.configuration.BoardWidth)
            {
                if (rolls >= 400)
                {
                    break;
                }
                roll = this.SALDice.Roll();
                rollsList.Add(roll);
                rolls++;
                Square s = this.Board.MovePlayer(player, roll);
                player.CurrentSquare = s;
            }
            lock (locker)
            {
                this.totalRolls = this.totalRolls + rolls;
                if (this.minRolls > rolls)
                {
                    this.minRolls = rolls;
                    this.minDiceRols = rollsList;
                }
                if (this.maxRolls < rolls)
                {
                    this.maxRolls = rolls;
                }
                this.doneThreads++;
                //    Console.Clear();
                //    this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"Simulations done: {this.configuration.Simulations - this.doneThreads}\n", ConsoleOutputType.Information, this));
            }
        }
    }
}
