using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public enum ePlayerColor
    {
        Empty = ' ',
        Red = 'X',
        Green = 'O'
    }
    public class Board
    {
        private ePlayerColor[,] m_GameBoard;
        public ePlayerColor this[int i_row, int i_col]
        {
            get { return m_GameBoard[i_row, i_col]; }
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
            for (int i = 0;i < i_Rows; i++)
            {
                for(int j = 0;j < i_Columns; j++)
                {
                    m_GameBoard[i, j] = ePlayerColor.Empty;
                }
            }
        }
    }
}
