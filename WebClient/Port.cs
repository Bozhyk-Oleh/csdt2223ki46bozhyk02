using Microsoft.EntityFrameworkCore;
using System.IO.Ports;
using WebClient.Db;

namespace WebClient
{
    public class Port
    {
        private readonly SerialPort _serialPort;
        private readonly Timer _timer;
        private readonly DataContext _db;
        private Model _model;
        private Terminal _terminal;


        private readonly object sync = new object();

        public Port(DataContext db, Terminal terminal)
        {
            _db = db;
            _terminal = terminal;

            _serialPort = new()
            {
                BaudRate = 9600,
                PortName = "empty"
            };
            _serialPort.DataReceived += SerialPort_DataReceived;

            _timer = new(CheckDb, null, 3000, 5000);
           
        }

        public string PortName
        {
            get => _serialPort.PortName;

            set
            {
                if (!_serialPort.IsOpen)
                    _serialPort.PortName = value;
            }
        }

        public object Sync => sync;

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string response = _serialPort.ReadLine();

            if (response.Contains("Wrong device connected") || response.Contains("No connection to server")
                || response.Contains("Lost connection to server"))
                return;

            _model.OutData = response;
            _model.OutDateTime = DateTime.UtcNow;
            _model.isSended = true;

            lock (sync)
            {
                _db.Models.Update(_model);
                _db.SaveChanges();
            }
            _terminal.log.Add(_model.OutDateTime +"< "+_model.OutData);
            if (_serialPort.IsOpen)
                _serialPort.Close();
        }

        private void CheckDb(object state)
        {
            lock (sync)
            {
                if (_serialPort.PortName != "empty" && _db.Models.AsNoTracking().Any(m => !m.isSended))
                {
                    if (!_serialPort.IsOpen)
                        _serialPort.Open();

                    _model = _db.Models.AsNoTracking().FirstOrDefault(m => !m.isSended);
                    _serialPort.Write(_model.InData);
                }
            }
        }
    }
}
