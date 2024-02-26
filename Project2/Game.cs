using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public class Game
    {
        private Board m_Board;
        private Player m_Player1;
        private Player m_Player2;
        private bool m_IsGameOver;

        public Board BoardGame
        {
            get { return m_Board; }
            set { m_Board = value; }
        }
        public static bool IsValidInputSize(string i_Input)
        {
            int number;
            bool validInput = false;
            if(int.TryParse(i_Input,out number))
            {
                if(number <= 8 && number >= 4)
                {
                    validInput = !validInput;
                }
            }
            return validInput;
        }
        public static bool IsValidInputForPlayAgainst(string i_Input)
        {
            int number;
            bool validInput = false;
            if (int.TryParse(i_Input, out number))
            {
                if (number == 2 || number == 1)
                {
                    validInput = !validInput;
                }
            }
            return validInput;
        }
        public bool CheckIfColumnIsValid(int i_Column, Board gameBoard)
        {
            bool columnIsValid = false;

            if(i_Column - 1 <= gameBoard.Cols || i_Column - 1 >= 0)
            {
                for (int i = 0; i < gameBoard.Rows; i++)
                {
                    if (gameBoard[i, i_Column - 1] == ePlayerColor.Empty)
                    {
                        columnIsValid = true;
                    }
                }
            }
            return columnIsValid;
        } 
        public void InsertTokenToColumn(Player player, Board gameBoard, int i_Column) 
        {
            for(int i = gameBoard.Rows - 1; i >= 0 ; i--)
            {
                if (gameBoard[i, i_Column-1] == ePlayerColor.Empty)
                {
                    gameBoard[i, i_Column-1] = player.Color;
                    break;
                }
            }
        }

        public Game(int i_Rows,int i_Cols,int i_PlayAgainst)
        {
            BoardGame = new Board(i_Rows, i_Cols);
            if(i_PlayAgainst == 1)
            {
                m_Player1 = new Player(ePlayerColor.Red, 0);
                m_Player2 = new Player(ePlayerColor.Green, 0);
            }//to do vs computer
        }
       
    }
}
