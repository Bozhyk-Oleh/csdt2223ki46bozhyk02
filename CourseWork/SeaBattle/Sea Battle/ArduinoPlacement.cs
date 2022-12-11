using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sea_Battle
{
    public class ArduinoPlacement
    {
        private SerialPort ardSerialPort = new SerialPort("COM4");

        public void CheckInArduino(List<GridPosition> occupied,
                                         GridPosition start,
                                         GridPosition end,
                                         ShipType shipType)
        {

            var ar_data = new byte[17]; // 1 operation 1 ship type 13 map 2 positions
            ar_data[0] = 2;
            ar_data[1] = ((byte)shipType);
            int k = 0;
            int index = 1;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++, k++)
                {
                    if (k % 8 == 0) index++;
                    if (occupied.Any(x => x.X == j && x.Y == i && !x.IsMissed && !x.IsHit))
                    {

                        ar_data[index] |= (byte)(1 << (k % 8));
                    }
                }
            }
            ar_data[15] = (byte)((start.X << 4) | start.Y);
            ar_data[16] = (byte)((end.X << 4) | end.Y);

            ardSerialPort.Open();
            ardSerialPort.ReadTimeout = 5000;
            ardSerialPort.Write(ar_data, 0, 17);
            int result = ardSerialPort.ReadByte();
            ardSerialPort.Close();
        }
    }
}
