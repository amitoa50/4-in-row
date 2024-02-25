using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class Program
    {
        public static void Main()
        {
            Game Gameboard = UI.buildGame();
            UI.PrintBoard(Gameboard.BoardGame);
            Console.ReadLine();
        }
    }
}
