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
            Console.WriteLine(Gameboard.CheckIfColumnIsValid(1, Gameboard.BoardGame));
            Player player1 = new Player(ePlayerColor.Green, 1);
            Gameboard.InsertTokenToColumn(player1, Gameboard.BoardGame, 1);
            UI.PrintBoard(Gameboard.BoardGame);
            Console.ReadLine();
        }
    }
}
