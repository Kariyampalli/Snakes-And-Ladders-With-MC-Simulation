using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    public class GameCreator
    {
        private ArrayList squaresList;
        private Random random;
        public EventHandler<WriteOnConsoleEventArgs> writeArgs;
        public ConsoleWriter writer;
        public GameCreator()
        {
            this.squaresList = new ArrayList();
            this.random = new Random();
            this.writer = new ConsoleWriter();
            this.writeArgs += this.writer.WriteOnConsole;
        }
        public SALBoard CreateBoard(Configuration configuration)
        {
            if (configuration == null)
            {
                throw new InvalidOperationException("Configuration can't be null!");
            }
            Square[,] squares = this.CreateSquares(configuration.BoardWidth, configuration.BoardHeight);
            TempCreateSnakes(configuration, squares);
            TempCreateLadders(configuration, squares);
            return new SALBoard(squares, configuration.BoardWidth, configuration.BoardHeight);
        }
        private void PrintAvailableSquares(Configuration config)
        {
            if (config == null)
            {
                throw new InvalidOperationException("Configuration can't be null!");
            }
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("\n--------------\nFree Sqaures\n--------------\n", ConsoleOutputType.Message, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("(Yellow --> Can't be the beginning of a snake or ladder)\n", ConsoleOutputType.Other, ConsoleColor.Yellow, this));
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("| ", ConsoleOutputType.Message, this));
            for (int i = 0; i < this.squaresList.Count; i++)
            {
                Square square = (Square)this.squaresList[i];
                if (square.ISnkLdr != null)
                {
                    throw new Exception("Square allowed to be chosen already has a snake or ladder placed on him");
                }
                else
                {
                    if (square.Number == 1 || square.Number == config.BoardHeight * config.BoardWidth)
                    {
                        this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"{square.Number}", ConsoleOutputType.Other, ConsoleColor.Yellow, this));
                    }
                    else
                    {
                        this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"{square.Number}", ConsoleOutputType.Message, this));
                    }
                    this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs(" | ", ConsoleOutputType.Message, this));
                }
            }
            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("\n----------------------------------\n)", ConsoleOutputType.Message, this));
        }

        private void PlaceSLManually(Configuration config, Square[,] squares, string sl, string from, string to, bool isSanke)
        {
            if (config == null || squares == null || string.IsNullOrEmpty(sl) || string.IsNullOrWhiteSpace(sl) || string.IsNullOrEmpty(from) || string.IsNullOrWhiteSpace(from) || string.IsNullOrEmpty(to) || string.IsNullOrWhiteSpace(to))
            {
                throw new InvalidOperationException("PlaceSLManually method received invalid parameter values!");
            }
            bool askAgain1 = true;
            bool askAgain2;
            string ft = from;
            ISL isl = new Snake();
            while (askAgain1)
            {
                this.PrintAvailableSquares(config);
                if (ft == from)
                {
                    if (isSanke)
                    {
                        isl = new Snake();
                    }
                    else
                    {
                        isl = new Ladder();
                    }
                }
                askAgain2 = true;
                while (askAgain2)
                {
                    if(isSanke)
                    {
                        if(config.LSDConfig.SnakesAmount == (int)Math.Floor(((double)config.BoardHeight * (double)config.BoardWidth - 2) / 2) - 1)
                        {
                            askAgain1 = false;
                            break;
                        }
                    }
                    else
                    {
                        if (config.LSDConfig.LaddersAmount == (int)Math.Floor(((double)config.BoardHeight * (double)config.BoardWidth - 2) / 2) - 1)
                        {
                            askAgain1 = false;
                            break;
                        }
                    }
                    if (config.SLMaxAmount == 0)
                    {
                        if(config.LSDConfig.LaddersAmount <=0 || config.LSDConfig.SnakesAmount <=0)
                        {
                            throw new InvalidOperationException("Amount of snakes or ladders are 0 and the max amount for both are 0 as well!");
                        }
                        askAgain1 = false;
                        break;
                    }
                    this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"Please put in the square(1-{squares.Length}) of the {ft} for the {sl} to be placed!\n", ConsoleOutputType.Message, this));
                    if (ft == from)
                    {
                        this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("|type f if finished|\n", ConsoleOutputType.Other, ConsoleColor.Cyan, this));
                    }
                    try
                    {
                        this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs(">> ", ConsoleOutputType.Message, this));
                        string input = Console.ReadLine();
                        input = input.Trim();
                        this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("\n", ConsoleOutputType.Message, this));
                        if (input == "f" && ft == from)
                        {
                            askAgain1 = false;
                            askAgain2 = false;
                            break;
                        }
                        int val = Convert.ToInt32(input);

                        if ((ft == from && (val == config.BoardHeight * config.BoardWidth || val == 1)) || val < 1 || val > config.BoardHeight * config.BoardWidth)
                        {
                            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("Value is invalid, taken or couldn't be found or was beggining/end of board!\n", ConsoleOutputType.Error, this));
                        }
                        for (int i = 0; i < this.squaresList.Count; i++)
                        {
                            Square square = (Square)this.squaresList[i];
                            if (square.Number == val)
                            {
                                if (ft == from)
                                {
                                    isl.From = square;
                                    square.ISnkLdr = isl;
                                    ft = to;
                                }
                                else if (ft == to)
                                {
                                    if (isSanke)
                                    {
                                        config.LSDConfig.SnakesAmount++;
                                    }
                                    else
                                    {
                                        config.LSDConfig.LaddersAmount++;
                                    }
                                    config.SLMaxAmount--;
                                    isl.To = square;
                                    square.ISnkLdr = isl;
                                    ft = from;
                                }                          
                                this.squaresList.RemoveAt(i);
                                askAgain2 = false;
                                break;
                            }
                        }
                        if(askAgain2)
                        {
                            this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("Value is invalid, taken or couldn't be found or was beggining/end of board!\n", ConsoleOutputType.Error, this));
                        }
                        
                    }
                    catch (Exception)
                    {
                        this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs("Value is invalid, taken or couldn't be found or was beggining/end of board!\n", ConsoleOutputType.Error, this));
                    }
                }
            }
        }
        private void TempCreateSnakes(Configuration configuration, Square[,] squares)
        {
            if (configuration == null || squares == null)
            {
                throw new InvalidOperationException("TempCreateSnakes method received invalid parameter values!");
            }
            switch (configuration.LSDConfig.SnakeDistribution)
            {
                case Distribution.Manually:
                    this.PlaceSLManually(configuration, squares, "snake", "HEAD", "TAIL", true);
                    break;
                case Distribution.Randomly:
                    this.CreateSnakesRandomly(configuration);
                    break;
                default:
                    break;
            }
        }

        private void TempCreateLadders(Configuration configuration, Square[,] squares)
        {
            if (configuration == null || squares == null)
            {
                throw new InvalidOperationException("TempCreateLadders method received invalid parameter values!");
            }
            switch (configuration.LSDConfig.LadderDistribution)
            {
                case Distribution.Manually:
                    this.PlaceSLManually(configuration, squares, "ladder", "FOOT", "TOP", false);
                    break;
                case Distribution.Randomly:
                    this.CreateLaddersRandomly(configuration);
                    break;
                default:
                    break;
            }
        }
        private Square[,] CreateSquares(int width, int height)
        {
            if (width != height || width < 8 || height < 8)
            {
                throw new InvalidOperationException("CreateSquares method received invalid board sizes!");
            }
            //Console.Clear();
            Square[,] squares = new Square[width, height];

            int anz = width * height;
            int currentVal = 1;
            int val = 1;
            int x = 0;
            int y = height - 1;
            do
            {
                //////this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"square placed: {currentVal} in x:{x} y:{y}\n", ConsoleOutputType.Other, ConsoleColor.Magenta, this));
                squares[y, x] = new Square(currentVal, x, y);
                this.squaresList.Add(squares[y, x]);
                x++;
                if (x == width)
                {
                    x = 0;
                    currentVal = currentVal + width;
                    y--;
                    val = val * -1;
                }
                else
                {
                    currentVal = currentVal + val;
                }
            } while (currentVal != anz + 1);

            return squares;
        }

        private void CreateSnakesRandomly(Configuration config)
        {
            if (config == null)
            {
                throw new InvalidOperationException("CreateSnakesRandomly method received invalid configuration!");
            }
            for (int i = 0; i < config.LSDConfig.SnakesAmount; i++)
            {
                ISL snake = new Snake();

                int randomPosInSquareList = this.random.Next(0, this.squaresList.Count);
                Square chosenFromSquare = (Square)this.squaresList[randomPosInSquareList];

                if (chosenFromSquare.Number != config.BoardWidth * config.BoardHeight && chosenFromSquare.Number != 1)
                {
                    snake = new Snake();
                    snake.From = chosenFromSquare;
                    chosenFromSquare.ISnkLdr = snake;

                    this.squaresList.Remove(chosenFromSquare);

                    randomPosInSquareList = this.random.Next(0, this.squaresList.Count);
                    Square chosenEndSquare = (Square)this.squaresList[randomPosInSquareList];
                    if (chosenEndSquare.ISnkLdr == null)
                    {
                        chosenEndSquare.ISnkLdr = snake;
                    }
                    else
                    {
                        throw new Exception();
                    }
                    snake.To = chosenEndSquare;
                    //this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"Snake --> from: y: {chosenFromSquare.Y}, x:{chosenFromSquare.X}  | to: {chosenEndSquare.Y}, x:{chosenEndSquare.X}\n", ConsoleOutputType.Other, ConsoleColor.Magenta, this));
                    this.squaresList.Remove(chosenEndSquare);
                }
            }
            //this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"{config.LSDConfig.SnakesAmount} snakes have been placed!\n", ConsoleOutputType.Other, ConsoleColor.Magenta, this));
        }
        private void CreateLaddersRandomly(Configuration config)
        {
            if (config == null)
            {
                throw new InvalidOperationException("CreateLaddersRandomly method received invalid configuration!");
            }
            for (int i = 0; i < config.LSDConfig.LaddersAmount; i++)
            {
                ISL ladder = new Ladder();

                int randomPosInSquareList = this.random.Next(0, this.squaresList.Count);
                Square chosenFromSquare = (Square)this.squaresList[randomPosInSquareList];

                if (chosenFromSquare.Number != config.BoardWidth * config.BoardHeight && chosenFromSquare.Number != 1)
                {
                    ladder = new Ladder();
                    ladder.From = chosenFromSquare;
                    chosenFromSquare.ISnkLdr = ladder;


                    this.squaresList.Remove(chosenFromSquare);
                    randomPosInSquareList = this.random.Next(0, this.squaresList.Count);
                    Square chosenEndSquare = (Square)this.squaresList[randomPosInSquareList];
                    if (chosenEndSquare.ISnkLdr == null)
                    {
                        chosenEndSquare.ISnkLdr = ladder;
                    }

                    ladder.To = chosenEndSquare;
                    //this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"Ladder --> from: y: {chosenFromSquare.Y}, x:{chosenFromSquare.X}  | to: {chosenEndSquare.Y}, x:{chosenEndSquare.X}\n", ConsoleOutputType.Other, ConsoleColor.Magenta, this));
                    this.squaresList.Remove(chosenEndSquare);
                }
            }
            //this.writeArgs?.Invoke(this, new WriteOnConsoleEventArgs($"{config.LSDConfig.LaddersAmount} ladders have been placed!\n", ConsoleOutputType.Other, ConsoleColor.Magenta, this));
        }
    }
}

