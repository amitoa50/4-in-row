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
        private Player m_currentPlayer;
        private bool m_IsGameOver;
        public bool IsGameOver
        {
            get { return m_IsGameOver; }
            set { m_IsGameOver = value; }
        }
        public Player player1
        {
            get { return m_Player1; }
        }
        public Player currentPlayer
        {
            get { return m_currentPlayer; }
            set
            {
                m_currentPlayer = value;
            }
        }
        public Player player2
        {
            get { return m_Player2; }
        }
        

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
        public void ChangePlayerScore(Player currentPlayer, Board currentBoard)
        {
            bool boardIsFull = CheckIfBoardIsFull(currentBoard);
            if (boardIsFull != true)
            {
                if(currentPlayer.Color == m_Player1.Color)
                {
                    m_Player1.GamesWon++;
                }
                else
                {
                    m_Player2.GamesWon++; 
                }
            }
        }
        public bool CheckIfBoardIsFull(Board currentBoard)
        {
            bool boardIsFull = true;

            for (int i =0;i < currentBoard.Cols; i++)
            {
                if (currentBoard[0,i]==ePlayerColor.Empty)
                {
                    boardIsFull = false;
                }
            }
            return boardIsFull;
        }
        public void InsertTokenToColumn(Player player, Board gameBoard, int i_Column)
        {
            Console.WriteLine($"{i_Column},{gameBoard.Cols},{gameBoard.Rows}");
            int rowToInsert = gameBoard.EmptyCellsInCol[i_Column-1];
            gameBoard[rowToInsert, i_Column-1] = player.Color;
            gameBoard.EmptyCellsInCol[i_Column-1]--;
        }
        public void ChangePlayerTurn(Player i_CurrentPlayer)
        {
            i_CurrentPlayer = i_CurrentPlayer == player1 ? player2: player1;
        }
        public bool CheckIfColumnIsValid(int i_Column, Board gameBoard)
        {
            bool columnIsValid = false;

            if (i_Column - 1 <= gameBoard.Cols || i_Column - 1 >= 0)
            {
                if (gameBoard.EmptyCellsInCol[i_Column-1] >= 0)
                {
                    columnIsValid = true;
                }
            }
            return columnIsValid;
        }
        private bool isInBoardLimits(int i_NextRow, int i_NextCol)
        {
            return i_NextRow >= 0 && i_NextRow < BoardGame.Rows && i_NextCol >= 0 && i_NextCol < BoardGame.Cols;
        }
        private bool thereIsKInLine(int i_Row, int i_Col, int I_Diraction)
        {
            int[] diractionRow = { 0, 1, 1, 1 };
            int[] diractionCol = { 1, 1, 0, -1 };

            for (int i = 1; i < 4; i++)
            {
                int nextR = i_Row + diractionRow[I_Diraction] * i;
                int nextC = i_Col + diractionCol[I_Diraction] * i;
                if (!isInBoardLimits(nextR, nextC) || this.BoardGame[nextR, nextC] != this.BoardGame[i_Row, i_Col])
                {
                    return false;
                }
            }

            return true;
        }
        
        public void checkWinCondition(Player i_CurrentPlayer)
        {
            for (int i = 0; i < this.BoardGame.Rows; i++)
            {
                for (int j = 0; j < this.BoardGame.Cols; j++)
                {
                    if (this.BoardGame[i, j] != ePlayerColor.Empty)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            if (thereIsKInLine(i, j, d))
                            {
                                IsGameOver = true;
                                break;
                            }
                        }
                    }
                }
            }
            
             if (BoardGame.EmptyCellsInCol.All(c => c == -1))
             {
                 if (!m_IsGameOver)
                 {
                     m_IsGameOver = true;
                     i_CurrentPlayer.Color = ePlayerColor.Empty;
                 }
             }
        }

        public void PlaySmartPC()
        {
            int bestColumn = findBestColumn();

            InsertTokenToColumn(player2, BoardGame, bestColumn);
        }
        private int scanBoardToWinOrBlock()
        {
            int resultColumn = 0;

            for (int col = 0; col < BoardGame.Cols; col++)
            {
                if (CheckIfColumnIsValid(col, BoardGame))
                {
                    
                    InsertTokenToColumn(player2, BoardGame, col);
                    checkWinCondition(player2);
                    if (IsGameOver == true) 
                    {
                        this.BoardGame[BoardGame.EmptyCellsInCol[col], col] = ePlayerColor.Empty; 
                        resultColumn = col + 1; 
                        break;
                    }

                    BoardGame[BoardGame.EmptyCellsInCol[col], col] = ePlayerColor.Empty; // Undo the simulated move
                }
            }

            return resultColumn;
        }

        private int findBestColumn()
        {
            int bestAvailableColumn;

            // Iterate through each column and find the first one that allows a winning move
            bestAvailableColumn = scanBoardToWinOrBlock();
            if (bestAvailableColumn != 0)
            {
                return bestAvailableColumn;
            }
            else
            {
                this.ChangePlayerTurn(currentPlayer);
                bestAvailableColumn = scanBoardToWinOrBlock();
                if (bestAvailableColumn != 0)
                {
                    ChangePlayerTurn(currentPlayer);

                    return bestAvailableColumn;
                }

                ChangePlayerTurn(currentPlayer);
            }

            if (isBlockMoveForThreeInRow(out bestAvailableColumn))
            {
                return bestAvailableColumn + 1;
            }

            // If no winning or blocking move is found, choose a random available column
            List<int> availableColumns = Enumerable.Range(1, Cols).Where(col => IsValidPlay(col - 1)).ToList();
            Random rnd = new Random();

            return availableColumns[rnd.Next(availableColumns.Count)];
        }
        private bool isBlockMoveForThreeInRow(out int o_expcetedValCol)
        {
            bool v_FlagResult = false;

            o_expcetedValCol = -1;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (this[i, j] != eCells.Empty)
                    {
                        for (int d = 0; d < 4; d++) // Check for a win in all directions
                        {
                            if (thereIs2InLineAndPotentialTofillMore(i, j, d, out o_expcetedValCol))
                            {
                                v_FlagResult = !v_FlagResult;
                                break;
                            }
                        }

                        if (v_FlagResult)
                        {
                            break;
                        }
                    }
                }

                if (v_FlagResult)
                {
                    break;
                }
            }

            return v_FlagResult;
        }
        private bool thereIs2InLineAndPotentialTofillMore(int i_Row, int i_Col, int i_Diraction, out int o_expcetedValCol)
        {
            int[] diractionRow = { 0, 1, 1, 1 };
            int[] diractionCol = { 1, 1, 0, -1 };
            int backRow = i_Row - diractionRow[i_Diraction],
                  backColumn = i_Col - diractionCol[i_Diraction],
                  nextRow = i_Row + diractionRow[i_Diraction],
                  nextColumn = i_Col + diractionCol[i_Diraction],
                  nextNextRow = nextRow + diractionRow[i_Diraction],
                  nextNextColumn = nextColumn + diractionCol[i_Diraction];
            bool backCell = isInBoardLimits(backRow, backColumn) && this[backRow, backColumn] == eCells.Empty;
            bool forwardCell = isInBoardLimits(nextRow, nextColumn) && this[nextRow, nextColumn] == this[i_Row, i_Col];
            bool positionThreeInRow = isInBoardLimits(nextNextRow, nextNextColumn) && this[nextNextRow, nextNextColumn] == eCells.Empty;
            bool v_FlagResult = true;

            if (!(backCell && forwardCell))
            {
                o_expcetedValCol = -1;
                return false;
            }

            if (positionThreeInRow && m_ColumnFullnessCounterArray[nextNextColumn] == nextNextRow)
            {
                o_expcetedValCol = nextNextColumn;
                return v_FlagResult;
            }
            else if (backCell && m_ColumnFullnessCounterArray[backColumn] == backRow)
            {
                o_expcetedValCol = backColumn;
                return v_FlagResult;
            }

            o_expcetedValCol = -1;
            return !v_FlagResult;
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
        public Game(int i_Rows,int i_Cols,int i_PlayAgainst)
        {
            BoardGame = new Board(i_Rows, i_Cols);
            if (i_PlayAgainst == 1)
            {
                m_Player1 = new Player(ePlayerColor.Red, 0);
                m_Player2 = new Player(ePlayerColor.Green, 0);
                currentPlayer = player1;
            }//to do vs computer
        }

    }
}
