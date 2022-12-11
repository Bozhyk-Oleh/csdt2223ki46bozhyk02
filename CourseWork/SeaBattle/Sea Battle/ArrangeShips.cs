using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO.Ports;

namespace Sea_Battle
{
    public partial class ArrangeShips : Form
    {

        private ShipType _selectedShipType;
        private int _isPatrolBoatSet = 0;
        private int _isSubmarineSet = 0;
        private int _isBattleshipSet = 0;
        private int _isAircraftCarrierSet = 0;

        public List<GridTile> _BattleMap = new List<GridTile>();
        public List<GridTile> _pl1BattleMap = new List<GridTile>();
        private List<GridPosition> _occupiedPositions = new List<GridPosition>();
        private List<GridPosition> _pl1occupiedPositions = new List<GridPosition>();
        private SerialPort _myserialPort;



        bool pl2arrangeTurn = false;

        public ArrangeShips(SerialPort myserialPort)
        {
            InitializeComponent();
            _myserialPort = myserialPort;
            MinimumSize = MaximumSize = Size;

            _selectedShipType = ShipType.None;


            InitGrid(tlpPlayerGrid);

            pl2arrangeTurn = true;
            btnSubmit.Enabled = false;
            rtbInfo.Text = "Click on (+) to select a ship.";
        }

        public ArrangeShips(List<GridTile> pl1BattleMap, List<GridPosition> pl1occupiedPositions, SerialPort myserialPort)
        {
            _myserialPort = myserialPort;
            _pl1BattleMap = pl1BattleMap;
            _pl1occupiedPositions = pl1occupiedPositions;

            InitializeComponent();
            MinimumSize = MaximumSize = Size;

            _selectedShipType = ShipType.None;

            InitGrid(tlpPlayerGrid);
            btnSubmit.Enabled = false;
            rtbInfo.Text = "Click on (+) to select a ship.";
        }
        private void InitGrid(TableLayoutPanel grid)
        {
            for (int column = 0; column < grid.ColumnCount; column++)
            {
                for (int row = 0; row < grid.RowCount; row++)
                {
                    var gridTile = new GridTile
                    {
                        Dock = DockStyle.Fill,
                        BackgroundImageLayout = ImageLayout.Center,
                        Margin = new Padding(0),

                        ShipType = ShipType.None,
                        GridPosition = new GridPosition(column, row)
                    };

                    gridTile.SetTile(TileType.Water);

                    grid.Controls.Add(gridTile, column, row);

                    gridTile.MouseDown += PlayerGrid_Click;
                }
            }
        }
        private void SelectShip(ShipType type)
        {
            _selectedShipType = type;
        }

        private void btnPatrolSelect_Click(object sender, EventArgs e) {
            SelectShip(ShipType.PatrolBoat);
            rtbInfo.Text = "Click on first position";
            _firstClick = null;
            _secondClick = null;

        }

        private void btnSubmarineSelect_Click(object sender, EventArgs e)
        {
            SelectShip(ShipType.Submarine);
            rtbInfo.Text = "Click on first position";
            _firstClick = null;
            _secondClick = null;
        }

        private void btnBattleshipSelect_Click(object sender, EventArgs e)
        {
            SelectShip(ShipType.Battleship);
            rtbInfo.Text = "Click on first position";
            _firstClick = null;
            _secondClick = null;
        }

        private void btnAricraftSelect_Click(object sender, EventArgs e)
        {
            SelectShip(ShipType.AircraftCarrier);
            rtbInfo.Text = "Click on first position";
            _firstClick = null;
            _secondClick = null;
        }

        private GridTile _firstClick;
        private GridTile _secondClick;
        private void PlayerGrid_Click(object sender, MouseEventArgs e)
        {
            GridTile s = sender as GridTile;
            if (_selectedShipType != ShipType.None)
            {
                if (_firstClick == null)
                {
                    Debug.WriteLine("1 click");
                    rtbInfo.Text = "Click on second position";
                    _firstClick = s;
                }
                else
                {
                    Debug.WriteLine("2 click");
                    _secondClick = s;

                    GridPosition start = _firstClick.GridPosition;
                    GridPosition end = _secondClick.GridPosition;


                    if (start.X > end.X || start.Y > end.Y)
                    {
                        var swap = start;
                        start = end;
                        end = swap;
                    }

                    PlacementType placement = CanBePlaced(_occupiedPositions, start, end, _selectedShipType);
                    if (placement == PlacementType.Solo)
                    {
                        GridTile gridTile = (GridTile)tlpPlayerGrid.GetControlFromPosition(start.X, start.Y);

                        gridTile.SetTile(TileType.ShipSolo);
                        _occupiedPositions.Add(gridTile.GridPosition);
                        gridTile.ShipType = _selectedShipType;
                        _BattleMap.Add(gridTile);
                        SetShip(_selectedShipType);
                    }
                    else if (placement == PlacementType.Horizontal)
                    {
                        for (int i = start.X; i <= end.X; i++)
                        {
                            GridTile gridTile = (GridTile)tlpPlayerGrid.GetControlFromPosition(i, start.Y);

                            if (i == start.X) gridTile.SetTile(TileType.ShipEndLeft);
                            else if (i == end.X) gridTile.SetTile(TileType.ShipEndRight);
                            else gridTile.SetTile(TileType.ShipCenterHorizontal);

                            _occupiedPositions.Add(gridTile.GridPosition);
                            gridTile.ShipType = _selectedShipType;
                            _BattleMap.Add(gridTile);
                        }

                        SetShip(_selectedShipType);
                    }
                    else if (placement == PlacementType.Vertical)
                    {
                        for (int i = start.Y; i <= end.Y; i++)
                        {
                            GridTile gridTile = (GridTile)tlpPlayerGrid.GetControlFromPosition(start.X, i);

                            if (i == start.Y) gridTile.SetTile(TileType.ShipEndUp);
                            else if (i == end.Y) gridTile.SetTile(TileType.ShipEndDown);
                            else gridTile.SetTile(TileType.ShipCenterVertical);

                            _occupiedPositions.Add(gridTile.GridPosition);
                            gridTile.ShipType = _selectedShipType;
                            _BattleMap.Add(gridTile);
                        }
                        SetShip(_selectedShipType);
                    }
                    else if (placement == PlacementType.Invalid)
                    {
                        MessageBox.Show("Unable to insert the ship!");
                        rtbInfo.Text = "Click on first position";
                    }
                    else if (placement == PlacementType.Occupied)
                    {
                        MessageBox.Show("This position is already taken");
                        rtbInfo.Text = "Click on first position";
                    }
                    else if (placement == PlacementType.Connection_error)
                    {
                        MessageBox.Show("Lost connection with server");
                        rtbInfo.Text = "Click on first position";
                    }
                    _firstClick = null;
                    _secondClick = null;
                }
            }
            else {
                MessageBox.Show("You need to select a ship first");
                rtbInfo.Text = "Click on (+) to select a ship.";
                _firstClick = null;
            }
        }
        public PlacementType CanBePlaced(List<GridPosition> occupied,
                                         GridPosition start,
                                         GridPosition end,
                                         ShipType shipType)
        {
            List<GridPosition> newGridPositions = new List<GridPosition>();
            PlacementType response;
            if (!CheckCOMport()){
                return PlacementType.Bridge_connection_error;
            }
            //
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

            _myserialPort.Open();
            _myserialPort.ReadTimeout = 5000;
            _myserialPort.Write(ar_data, 0, 17);
            int result = _myserialPort.ReadByte();
            _myserialPort.Close();
            if (result == 101)
            {
                response = PlacementType.Connection_error;
            }else
                response = (PlacementType)result - 49;
            //Solo

            if(response == PlacementType.Solo)
            {
                newGridPositions.Add(new GridPosition(start.X, start.Y));
            }
            //Horizontal

            else if(response == PlacementType.Horizontal)
            {
                for (int x = start.X; x <= end.X; x++)
                {
                    newGridPositions.Add(new GridPosition(x, start.Y));
                }
            }
            //Vertical

            else if(response == PlacementType.Vertical)
            {
                for (int y = start.Y; y <= end.Y; y++)
                {
                    newGridPositions.Add(new GridPosition(start.X, y));
                }
            }

            return response;
        }
        private void SetShip(ShipType shipType)
        {
            if (shipType == ShipType.PatrolBoat)
            {
                _isPatrolBoatSet ++;
                if(_isPatrolBoatSet == 4)
                    btnPatrolSelect.Enabled = false;
            }

            if (shipType == ShipType.Submarine)
            {
                _isSubmarineSet++;
                if (_isSubmarineSet == 3)
                    btnSubmarineSelect.Enabled = false;

            }

            if (shipType == ShipType.Battleship)
            {
                _isBattleshipSet++;
                if (_isBattleshipSet == 2)
                    btnBattleshipSelect.Enabled = false;
            }

            if (shipType == ShipType.AircraftCarrier)
            {
                _isAircraftCarrierSet++;
                btnAricraftSelect.Enabled = false;
            }


            _selectedShipType = ShipType.None;
            rtbInfo.Text = "Click on (+) to select a ship.";


            if ((_isPatrolBoatSet == 4) && (_isSubmarineSet == 3) && (_isBattleshipSet == 2) && (_isAircraftCarrierSet==1))
            {
                AcceptButton = btnSubmit;
                btnSubmit.Enabled = true;
                rtbInfo.Text = "All ships are arranged";
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (pl2arrangeTurn)
            {

                ArrangeShips pl2arrangeships = new ArrangeShips(_BattleMap, _occupiedPositions, _myserialPort);
                pl2arrangeships.Show();

            }
            else
            {
                Sea_Battle battleform = new Sea_Battle(_pl1BattleMap, _BattleMap, _pl1occupiedPositions, _occupiedPositions, _myserialPort);
                battleform.Show();
            }

        }

        private bool CheckCOMport()
        {
            try
            {
                _myserialPort.Open();
                _myserialPort.Close();
                return true;
            }
            catch
            {
                MessageBox.Show("Can't open COM port. " +
                    "Access is denied to the port. " +
                    "-or- " +
                    "The current process, or another process on the system," +
                    " already has the specified COM port open");
                return false;
            }
        }

    }
}