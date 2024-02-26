﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public static class UI
    {
        public static Game buildGame()
        {
            Console.WriteLine("what is the board size you want?");
            Console.WriteLine("the minimum size is 4X4 and the maximum is 8X8");
            Console.WriteLine("enter number of rows");
            string i_Rows = Console.ReadLine();
            while (!Game.IsValidInputSize(i_Rows))
            {
                Console.WriteLine("the minimum size is 4X4 and the maximum is 8X8");
                Console.WriteLine("enter number of rows");
                i_Rows = Console.ReadLine();
            }
            Console.WriteLine("enter number of columns");
            string i_Cols = Console.ReadLine();
            while (!Game.IsValidInputSize(i_Cols))
            {
                Console.WriteLine("the minimum size is 4X4 and the maximum is 8X8");
                Console.WriteLine("enter number of columns");
                i_Cols = Console.ReadLine();
            }
            Console.WriteLine("if you want to play vs computer press 1 vs player press 2");
            string i_PlayAgainst = Console.ReadLine();
            while (!Game.IsValidInputForPlayAgainst(i_PlayAgainst))
            {
                Console.WriteLine("if you want to play vs computer press 1 vs player press 2");
                i_PlayAgainst = Console.ReadLine();
            }
            Game currentGame = new Game(int.Parse(i_Rows), int.Parse(i_Cols), int.Parse(i_PlayAgainst));
            return currentGame;
        }
        public static void PrintBoard(Board board)
        {
            StringBuilder gameBoard = new StringBuilder();
            gameBoard.Append(' ',2);
            for(int i = 1; i < board.Cols; i++)
            {
                gameBoard.Append(i).Append(' ', 3); ;
            }
            gameBoard.Append(board.Cols).Append(' ', 2).AppendLine();
            for (int i = 0; i < board.Rows; i++)
            {
                for (int j = 0; j < board.Cols; j++)
                {
                    gameBoard.Append("| ").Append((char)board[i,j]).Append(' ');
                }
                gameBoard.Append('|').AppendLine();
                gameBoard.Append('=').Append('=', 4 * (board.Cols)).AppendLine();
            }
            Console.Write(gameBoard);
        }
        public bool Turn(Player player, Board gameBoard)
        {
            int convertedInputToNumber;
            bool isTurnOver = false;
            bool isColumnValid = false;
            string i_Column;

            while (isTurnOver)
            {
                Console.WriteLine("Which column would you like to insert token to?");
                i_Column = Console.ReadLine();
                if (i_Column.ToUpper() == "Q")
                {
                    break;
                }
                else if (int.TryParse(i_Column, out convertedInputToNumber))
                {
                    isColumnValid = CheckIfColumnIsValid(convertedInputToNumber, gameBoard);
                    if (isColumnValid == true)
                    {
                        InsertTokenToColumn(player, gameBoard, convertedInputToNumber);
                        isTurnOver = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input - Not in range!");
                        Console.WriteLine($"the range is 1 to {gameBoard.Cols}");
                        continue;
                    }
                }
            }
            return isTurnOver;

        }
        public bool PlayGamePlayerVsPlayer(Player player1, Player player2, Board gameBoard)
        {
            bool playersWantAnotherGame = false;
            bool gameIsOver = false;
            Player currentPlayer = player1;

            while (gameIsOver)
            {

            }
        }
    }
}
