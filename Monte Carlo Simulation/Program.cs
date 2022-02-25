using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monte_Carlo_Simulation.Code
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                SnakesAndLadders sal = new SnakesAndLadders();
                sal.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
