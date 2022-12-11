using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sea_Battle
{
    public class GridPosition
    {
        //Souřadnice

        public int X { get; set; }

        public int Y { get; set; }

        //Vlastnosti

        public bool IsHit { get; set; }

        public bool IsMissed { get; set; }

        public bool IsSelected { get; set; }

        public GridPosition()
        {

        }

        public GridPosition(int x, int y)
        {
            IsHit = false;
            IsMissed = false;
            X = x;
            Y = y;
        }



        public override bool Equals(object other)
        {
            if (other is GridPosition)
            {
                if (((GridPosition)other).X == X && ((GridPosition)other).Y == Y) return true;
            }
            return false;
        }
    }
}
