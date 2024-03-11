using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Project2
{
    public class Game
    {
        private Board m_Board;
        private Player m_Player1;
        private Player m_Player2;
        private Player m_currentPlayer;
        private bool m_GameMode;
        private bool m_IsGameOver;
        private bool m_IsForfeit; 

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
        public bool GamePcMode
        {
            get { return m_GameMode; }
            set { m_GameMode = value; }
        }
        public bool IsForfeit
        {
            get { return m_IsForfeit; }
            set { m_IsForfeit = value; }
        }
        public static bool IsValidInputSize(string i_Input)
        {
            int number;
            bool validInput = false;
            if (int.TryParse(i_Input, out number))
            {
                if (number <= 8 && number >= 4)
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
                if (currentPlayer.Color == m_Player1.Color)
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

            for (int i = 0; i < currentBoard.Cols; i++)
            {
                if (currentBoard[0, i] == ePlayerColor.Empty)
                {
                    boardIsFull = false;
                }
            }
            return boardIsFull;
        }
        public void InsertTokenToColumn(Player player, Board gameBoard, int i_Column)
        {
            //Console.WriteLine($"{i_Column},{gameBoard.Cols},{gameBoard.Rows}");
            int rowToInsert = gameBoard.EmptyCellsInCol[i_Column - 1];
            gameBoard[rowToInsert, i_Column - 1] = player.Color;
            gameBoard.EmptyCellsInCol[i_Column - 1]--;
        }
        public void ChangePlayerTurn()
        {
            currentPlayer = currentPlayer == player1 ? player2 : player1;
        }
        public bool CheckIfColumnIsValid(int i_Column, Board gameBoard)
        {
            bool columnIsValid = false;

            if (i_Column >= 1 && i_Column <= gameBoard.Cols)
            {
                if (gameBoard.EmptyCellsInCol[i_Column - 1] >= 0)
                {
                    columnIsValid = true;
                }
            }
            return columnIsValid;
        }
        private bool IsInBoardRange(int i_NextRow, int i_NextCol)
        {
            return ((i_NextRow >= 0) && (i_NextRow < BoardGame.Rows) && (i_NextCol >= 0) && (i_NextCol < BoardGame.Cols));
        }
        private bool thereIsKInLine(int i_Row, int i_Col, int i_Direction)
        {
            int[] directionOfRow = { 0, 1, 1, 1 };
            int[] diractionCol = { 1, 1, 0, -1 };
            bool v_FlagResult = true;

            for (int i = 1; i < 4; i++)
            {
                int nextR = i_Row + directionOfRow[i_Direction] * i;
                int nextC = i_Col + diractionCol[i_Direction] * i;
                if (!IsInBoardRange(nextR, nextC) || this.BoardGame[nextR, nextC] != this.BoardGame[i_Row, i_Col])
                {
                    v_FlagResult = false;
                    break;
                }
            }

            return v_FlagResult;
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

            if (BoardGame.EmptyCellsInCol.All(valueOfIndex => valueOfIndex == -1))
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

            for (int optionalColumn = 1; optionalColumn <= this.BoardGame.Cols; optionalColumn++)
            {
                if (CheckIfColumnIsValid(optionalColumn, this.BoardGame))
                {

                    InsertTokenToColumn(currentPlayer, this.BoardGame, optionalColumn);
                    this.BoardGame.EmptyCellsInCol[optionalColumn - 1]++;
                    if (CheckIfFutureMoveIsAWinningMove()==true)
                    {
                        this.BoardGame[BoardGame.EmptyCellsInCol[optionalColumn - 1], optionalColumn - 1] = ePlayerColor.Empty;
                        resultColumn = optionalColumn;
                        break;
                    }
                    this.BoardGame[BoardGame.EmptyCellsInCol[optionalColumn - 1], optionalColumn - 1] = ePlayerColor.Empty;
                }
            }

            return resultColumn;
        }
        private int findBestColumn()
        {
            int bestAvailableColumn;
            bestAvailableColumn = scanBoardToWinOrBlock();
            if (bestAvailableColumn != 0)
            {
                return bestAvailableColumn;
            }
            else
            {
                this.ChangePlayerTurn();
                bestAvailableColumn = scanBoardToWinOrBlock();
                if (bestAvailableColumn != 0)
                {
                    ChangePlayerTurn();
                    return bestAvailableColumn;
                }

                ChangePlayerTurn();
            }

            if (isBlockMoveForThreeInRow(out bestAvailableColumn))
            {
                return bestAvailableColumn + 1;
            }

            // If no winning or blocking move is found, choose a random available column
            List<int> availableColumns = Enumerable.Range(1, BoardGame.Cols).Where(col => CheckIfColumnIsValid(col, BoardGame)).ToList();
            Random rnd = new Random();

            return availableColumns[rnd.Next(availableColumns.Count)];
        }
        private bool isBlockMoveForThreeInRow(out int o_ExpectedColumnToBlock)
        {
            bool v_FlagResult = false;

            o_ExpectedColumnToBlock = -1;
            for (int i = 0; i < BoardGame.Rows; i++)
            {
                for (int j = 0; j < BoardGame.Cols; j++)
                {
                    if (this.BoardGame[i, j] != ePlayerColor.Empty)
                    {
                        for (int d = 0; d < 4; d++) // Check for a win in all directions
                        {
                            if (thereIs2InLineAndPotentialTofillMore(i, j, d, out o_ExpectedColumnToBlock))
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
        private bool thereIs2InLineAndPotentialTofillMore(int i_Row, int i_Col, int i_Direction, out int o_expcetedValCol)
        {
            int[] diractionRow = { 0, 1, 1, 1 };
            int[] diractionCol = { 1, 1, 0, -1 };
            int backRow = i_Row - diractionRow[i_Direction],
                  backColumn = i_Col - diractionCol[i_Direction],
                  nextRow = i_Row + diractionRow[i_Direction],
                  nextColumn = i_Col + diractionCol[i_Direction],
                  nextNextRow = nextRow + diractionRow[i_Direction],
                  nextNextColumn = nextColumn + diractionCol[i_Direction];
            bool backCell = IsInBoardRange(backRow, backColumn) && this.BoardGame[backRow, backColumn] == ePlayerColor.Empty;
            bool forwardCell = IsInBoardRange(nextRow, nextColumn) && this.BoardGame[nextRow, nextColumn] == this.BoardGame[i_Row, i_Col];
            bool positionThreeInRow = IsInBoardRange(nextNextRow, nextNextColumn) && this.BoardGame[nextNextRow, nextNextColumn] == ePlayerColor.Empty;
            bool v_FlagResult = true;

            if (!(backCell && forwardCell))
            {
                o_expcetedValCol = -1;
                return false;
            }

            if (positionThreeInRow && this.BoardGame.EmptyCellsInCol[nextNextColumn] == nextNextRow)
            {
                o_expcetedValCol = nextNextColumn;
                return v_FlagResult;
            }
            else if (backCell && this.BoardGame.EmptyCellsInCol[backColumn] == backRow)
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
        private bool CheckIfFutureMoveIsAWinningMove()
        {
            bool v_FlagResult = false;

            for (int i = 0; i < BoardGame.Rows; i++)
            {
                for (int j = 0; j < BoardGame.Cols; j++)
                {
                    if (this.BoardGame[i, j] != ePlayerColor.Empty)
                    {
                        for (int d = 0; d < 4; d++) 
                        {
                            if (thereIsKInLine(i, j, d))
                            {
                                v_FlagResult = !v_FlagResult;
                                break;
                            }
                        }
                        if(v_FlagResult == false)
                        {
                            break;
                        }
                    }
                }
                if (v_FlagResult == false)
                {
                    break;
                }
            }

            return v_FlagResult;
        }
        public Game(int i_Rows, int i_Cols, int i_PlayAgainst)
        {
            BoardGame = new Board(i_Rows, i_Cols);
            m_Player1 = new Player(ePlayerColor.Red, 0);
            m_Player2 = new Player(ePlayerColor.Green, 0);
            currentPlayer = player1;
            IsForfeit = false;
            if (i_PlayAgainst == 1)
            {
                GamePcMode = true;
            }//to do vs computer
            else
            {
                GamePcMode = false;
            }
        }

    }
}