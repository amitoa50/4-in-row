using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex02.ConsoleUtils;
using System.Threading.Tasks;
using System.Threading;

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
        public static bool Turn(Player player, Game currentGame)
        {
            int convertedInputToNumber;
            bool isTurnOver = false;
            bool isColumnValid = false;
            string i_Column;

            while (isTurnOver==false)
            {
                Console.WriteLine("Which column would you like to insert token to?");
                i_Column = Console.ReadLine();
                if (i_Column.ToUpper() == "Q")
                {
                    break;
                }
                else if (int.TryParse(i_Column, out convertedInputToNumber))
                {
                    isColumnValid = currentGame.CheckIfColumnIsValid(convertedInputToNumber, currentGame.BoardGame);
                    if (isColumnValid == true)
                    {
                        currentGame.InsertTokenToColumn(player, currentGame.BoardGame, convertedInputToNumber);
                        isTurnOver = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input - Not in range!");
                        Console.WriteLine($"the range is 1 to {currentGame.BoardGame.Cols}");
                        continue;
                    }
                }
            }
            return isTurnOver;

        }
        public static bool CheckIfPlayerWantsAnotherGame(string i_Choice)
        {
            bool playersChoice = false;

            while (i_Choice.ToUpper() != "Y" && i_Choice.ToUpper() != "N")
            {
                Console.WriteLine("Do you want another game? (Y/N)");
                i_Choice = Console.ReadLine();
            }

            playersChoice = i_Choice.ToUpper() == "Y";
            return playersChoice;
        }

        
        public static void PlayGamePlayerVsPlayer(Game gameBoard)
        {
            bool playersWantAnotherGame = true;  
            while (playersWantAnotherGame)
            {
                
                gameBoard.IsGameOver = false;  

                while (!gameBoard.IsGameOver)
                {
                    gameBoard.IsGameOver = !Turn(gameBoard.currentPlayer, gameBoard);
                    gameBoard.ChangePlayerTurn(gameBoard.currentPlayer);
                    gameBoard.checkWinCondition(gameBoard.currentPlayer);
                    Screen.Clear();
                    if (gameBoard.IsGameOver)
                    {
                        Console.WriteLine($"Well done!! {gameBoard.currentPlayer.Color} has won!");
                    }
                    PrintBoard(gameBoard.BoardGame);
                }

                gameBoard.ChangePlayerScore(gameBoard.currentPlayer, gameBoard.BoardGame);
                playersWantAnotherGame = Replay(ref gameBoard);
            }
        }
        public static void PlayGamePlayerVsPc(Game gameBoard)
        {
            bool playersWantAnotherGame = true;

            int token;
            bool validInput;
            string userInput;

             while (playersWantAnotherGame)
            {
                PrintBoard(gameBoard.BoardGame);
                validInput = false;
                while (!validInput)
                {
                    Console.Write("Enter your column to put token: ");
                    userInput = Console.ReadLine();
                    if (userInput.ToUpper() == "Q")
                    {
                        gameBoard.IsGameOver = true;
                        gameBoard.CurrentPlayer = gameBoard.CurrentPlayer == eCells.Red ? eCells.Yellow : eCells.Red;
                        break;
                    }

                    if (int.TryParse(userInput, out token))
                    {
                        validInput = gameBoard.TokenInsertion(token);
                        if (validInput == false)//check valid userInput
                        {
                            if (!gameBoard.ValidCol(token - 1))
                            {
                                Console.WriteLine($"Invalid Input range, please insert number between 1 to {gameBoard.Cols}");
                            }
                            else
                            {
                                Console.WriteLine($"Invalid Input column is full, please insert to a diffrent column");
                            }
                            Thread.Sleep(1500);
                        }
                        else if (gameBoard.IsGameOver)
                        {
                            PrintGameIsfinished(gameBoard);
                        }
                        else
                        {
                            Screen.Clear();
                            gameBoard.PlaySmartPC();
                            if (gameBoard.IsGameOver)
                            {
                                PrintGameIsfinished(gameBoard);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, please try again");
                    }
                }
            }

            IncreaseNumOfWins(gameBoard);
        }

        public static bool Replay(ref Game currentGame)
        {
            bool playersWantAnotherGame = true;
            Console.WriteLine("Do you want another game? (Y/N)");
            string i_Choice = Console.ReadLine();
            playersWantAnotherGame = CheckIfPlayerWantsAnotherGame(i_Choice);
            if (playersWantAnotherGame == true)
            {
                currentGame.BoardGame = new Board(currentGame.BoardGame.Rows, currentGame.BoardGame.Cols);
                Console.WriteLine("Starting a new game in 3 seconds...");
                Thread.Sleep(3000);
                Screen.Clear();
                PrintBoard(currentGame.BoardGame);
            }
            else
            {
                Screen.Clear();
                Console.WriteLine($"Score is {currentGame.player1.GamesWon} for " +
                    $"{currentGame.player1.Color}  and {currentGame.player2.GamesWon} for {currentGame.player2.Color} ");
            }
            return playersWantAnotherGame;
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
        public static void setupGame()
        {
            Game Gameboard = UI.buildGame();
            UI.PrintBoard(Gameboard.BoardGame);
            UI.PlayGamePlayerVsPlayer(Gameboard);
            Console.ReadLine();
        }
    }
}
