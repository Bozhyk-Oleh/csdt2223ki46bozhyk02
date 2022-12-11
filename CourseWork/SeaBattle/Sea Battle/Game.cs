using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace Sea_Battle
{
    public class Game
    {
        private List<GridPosition> _pl1Position = new List<GridPosition>();
        private List<GridPosition> _pl2Position = new List<GridPosition>();
        private SerialPort _myserialPort;
        public Game(List<GridPosition> pl1Position, List<GridPosition> pl2Position, SerialPort myserialPort)
        {
                 
                _pl1Position = pl1Position;
                _pl2Position = pl2Position;
                _myserialPort = myserialPort;
        }
        public GridPosition FireOnPl1(GridPosition position)
        {
            int result = -1;
            var data = new byte[15]; // 1 operation  type 13 map 1 position
            data[0] = 1;
            int k = 0;
            int index = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++, k++)
                {
                    if (k % 8 == 0) index++;
                    if (_pl1Position.Any(x => x.X == j && x.Y == i && !x.IsMissed && !x.IsHit))
                    {

                        data[index] |= (byte)(1 << (k % 8));
                    }
                }
            }
            data[14] = (byte)((position.X << 4) | position.Y);
            _myserialPort.Open();
            _myserialPort.Write(data, 0, 15);
            while (result == -1)
            {
                result = _myserialPort.ReadByte();
            }
            _myserialPort.Close();
            if (result == 101)
            {
                GridPosition pos = new GridPosition(404, 404);
                return pos;
            }
            if (result == 49)
            {
                GridPosition pos = _pl1Position.Find(x => x.Equals(position));

                pos.IsHit = true;
                return pos;
            }
            else 
            {
                
                GridPosition pos = new GridPosition(position.X, position.Y);
                pos.IsMissed = true;
                return pos;
            }

        }
        public GridPosition FireOnPl2(GridPosition position)
        {
            int result = -1;
            var data = new byte[15]; // 1 operation 13 map 1 position
            int k = 0;
            int index = 0;
            data[0] = 1;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++, k++)
                {
                    if (k % 8 == 0) index++;
                    if (_pl2Position.Any(x => x.X == j && x.Y == i && !x.IsMissed && !x.IsHit))
                    {

                        data[index] |= (byte)(1 << (k % 8));
                    }
                }
            }

            data[14] = (byte)((position.X << 4) | position.Y);
            _myserialPort.Open();
            _myserialPort.Write(data, 0, 15);
            while (result == -1)
            {
                result = _myserialPort.ReadByte();
            }
            _myserialPort.Close();
            if (result == 101)
            {
                GridPosition pos = new GridPosition(404, 404);
                return pos;
            }
            if (result == 49)
            {
                GridPosition pos = _pl2Position.Find(x => x.Equals(position));

                pos.IsHit = true;
                return pos;
            }
            else
            {

                GridPosition pos = new GridPosition(position.X, position.Y);
                pos.IsMissed = true;
                return pos;
            }

        
          
        }
    }

}
