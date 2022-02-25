using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    public class WriteOnConsoleEventArgs
    {
        public string Message { get; set; }
        public ConsoleOutputType OutputType { get; set; }
        public ConsoleColor Color { get; set; }
        public WriteOnConsoleEventArgs(string output, ConsoleOutputType type, object sender)
        {   
            if(type == ConsoleOutputType.Other)
            {
                throw new InvalidOperationException($"ConsoleOutputType Other given to the wrong WriteOnConsoleEventArgs constructor in {sender.ToString()} class");
            }
            this.Message = output;
            this.OutputType = type;
        }

        public WriteOnConsoleEventArgs(string output, ConsoleOutputType type, ConsoleColor color, object sender)
        {
            if (type != ConsoleOutputType.Other)
            {
                throw new InvalidOperationException($"ConsoleOutputType {type.ToString()} given to the wrong WriteOnConsoleEventArgs constructor in {sender.ToString()} class");
            }
            this.Message = output;
            this.OutputType = type;
            this.Color = color;
        }
    }
}
