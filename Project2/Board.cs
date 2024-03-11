using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public class Board
    {
        private ePlayerColor[,] m_GameBoard;
        private int[] m_EmptyCellsInCol;
        public int[] EmptyCellsInCol
        {
            get { return m_EmptyCellsInCol; }
            set { m_EmptyCellsInCol = value; }
        }

        public ePlayerColor this[int i_row, int i_col]
        {
            get { return m_GameBoard[i_row, i_col]; }
            set { m_GameBoard[i_row, i_col] = value; }

        }
        public int Rows
        {
            get
            {
                return m_GameBoard.GetLength(0);
            }
        }
        public int Cols
        {
            get
            {
                return m_GameBoard.GetLength(1);
            }
        }
        public Board(int i_Rows, int i_Columns)
        {
            m_GameBoard = new ePlayerColor[i_Rows, i_Columns];
            m_EmptyCellsInCol = new int[Cols];

            setupEmptyBoard();
        }
        public void setupEmptyBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    m_GameBoard[i, j] = ePlayerColor.Empty;
                }
            }
            for (int i = 0; i < Cols; i++)
            {
                m_EmptyCellsInCol[i] = Rows - 1;
            }
        }
    }
}