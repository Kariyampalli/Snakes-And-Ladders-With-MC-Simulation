using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    public class ConsoleWriter
    {
        public ConsoleWriter()
        {
        }

        //Selects which way to write on the console
        public void WriteOnConsole(object sender, WriteOnConsoleEventArgs args) //Method called up by an event and choses writting method
        {
            if (args == null)
            {
                throw new InvalidOperationException("ConsoleWriter received an argument that was null!");
            }
            switch (args.OutputType)
            {

                case ConsoleOutputType.Error:
                    this.WriteError(args.Message);
                    break;
                case ConsoleOutputType.Information:
                    this.WriteInfo(args.Message);
                    break;
                case ConsoleOutputType.Message:
                    this.WriteMessage(args.Message);
                    break;
                case ConsoleOutputType.Other:
                    this.WriteOther(args.Message, args.Color);
                    break;
                default:
                    Console.WriteLine("Writing Type doesn't exist!");
                    break;
            }
        }

        private void WriteError(string message)
        {
            if (this.ValidateMessage(message))
            {
                this.ValidateMessage(message);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(message);
            }
        }

        private void WriteInfo(string message)
        {
            if (this.ValidateMessage(message))
            {
                this.ValidateMessage(message);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(message);
            }
        }

        private void WriteMessage(string message)
        {
            if (this.ValidateMessage(message))
            {
                this.ValidateMessage(message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(message);
            }
        }

        private void WriteOther(string message, ConsoleColor color)
        {
            if (this.ValidateMessage(message))
            {
                this.ValidateMessage(message);
                Console.ForegroundColor = color;
                Console.Write(message);
            }
        }
        private bool ValidateMessage(string message)
        {
            if (message != "\n" && (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)))
            {
                this.WriteError("Message tried to be sent is invalid");
                Thread.Sleep(4000);
                return false;
            }
            return true;
        }
    }
}
