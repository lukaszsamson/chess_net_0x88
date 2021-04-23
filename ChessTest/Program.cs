using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var t = new test();
            UCIInterface uci = new UCIInterface();
            while (!uci.Exit)
            {
                string line = Console.ReadLine();
                uci.ProcessCommand(line);
            }
        }
    }
}
