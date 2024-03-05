using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public class Player
    {
        private ePlayerColor m_Color;
        private int m_GamesWon;
        public Player(ePlayerColor color, int gamesWon)
        {
            GamesWon = gamesWon;
            Color = color;
        }
        public ePlayerColor Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }
        public int GamesWon
        {
            get { return m_GamesWon; }
            set { m_GamesWon = value; }
        }
    }
}
