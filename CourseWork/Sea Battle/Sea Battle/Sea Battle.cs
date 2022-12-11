using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Sea_Battle
{
    public partial class Sea_Battle : Form
    {
        public List<GridTile> _BattleMap = new List<GridTile>();
        public List<GridTile> _EnemyBattleMap = new List<GridTile>();
        private List<GridPosition> _pl1Position = new List<GridPosition>();
        private List<GridPosition> _pl2Position = new List<GridPosition>();
        public SerialPort _myserialPort;

        public Game _game;

        int hit1count = 0;
        int hit2count = 0;
        bool shootturn = false; // false 1 pl, true 2 pl
        bool notShootAgain = true;

        private GridPosition _selectedPosition;

        public Sea_Battle(List<GridTile> BattleMap, List<GridTile> EnemyBattleMap,
            List<GridPosition> pl1Position, List<GridPosition> pl2Position, SerialPort myserialPort)
        {
            _pl1Position = pl1Position;
            _pl2Position = pl2Position;
            _EnemyBattleMap = EnemyBattleMap;
            _BattleMap = BattleMap;
            _myserialPort = myserialPort;
            _game = new Game(_pl1Position, _pl2Position, _myserialPort);
            InitializeComponent();
            MinimumSize = MaximumSize = Size;
            InitGrid(tlp1PlayerGrid, _BattleMap, false);
            InitGrid(tlp2PlayerGrid, _EnemyBattleMap, false);
            Player1label.ForeColor = Color.Green;
            Player2lanel.ForeColor = Color.Red;
            tlp1PlayerGrid.Enabled = false; 

        }
        private void InitGrid(TableLayoutPanel grid, List<GridTile> Map, bool show)
        {
            TileType temp;
            GridPosition temppos = new GridPosition();
            for (int column = 0; column < grid.ColumnCount; column++)
            {
                for (int row = 0; row < grid.RowCount; row++)
                {
                        temppos.X = column;
                        temppos.Y = row;
                        if (Map.Any(x => x.GridPosition.Equals(temppos)))
                        {
                            int i = Map.FindIndex(x => x.GridPosition.Equals(temppos));
                            if (show)
                            {
                                temp = Map[i].TileType;
                            }
                            else
                            {
                                temp = TileType.Water;
                            }
                            var gridTile = new GridTile
                            {
                                Dock = DockStyle.Fill,
                                BackgroundImageLayout = ImageLayout.Center,
                                Margin = new Padding(0),

                                ShipType = Map[i].ShipType,
                                GridPosition = new GridPosition(column, row)
                            };
                            gridTile.SetTile(temp);
                            grid.Controls.Add(gridTile, column, row);
                            gridTile.MouseDown += new MouseEventHandler(EnemyGrid_Click);
                        }
                        else
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
                            gridTile.MouseDown += new MouseEventHandler(EnemyGrid_Click);
                        }
                }
            }
        }
        private void NextTurn(bool isMyTurn)
        {
            _selectedPosition = null;
            if (isMyTurn)
            {
                tlp1PlayerGrid.Enabled = true;
                tlp2PlayerGrid.Enabled = false;
                Player1label.ForeColor = Color.Red;
                Player2lanel.ForeColor = Color.Green;


            }
            else
            {
                tlp2PlayerGrid.Enabled = true;
                tlp1PlayerGrid.Enabled = false;
                Player1label.ForeColor = Color.Green;
                Player2lanel.ForeColor = Color.Red;

            }
        }
        private void EnemyGrid_Click(object sender, MouseEventArgs e)
        {

            if (!shootturn)
            {

                if (_selectedPosition != null)
                {
                    GridTile oldTile = (GridTile)tlp2PlayerGrid.GetControlFromPosition(_selectedPosition.X, _selectedPosition.Y);
                    oldTile.GridPosition.IsSelected = false;
                    oldTile.Invalidate();
                }

                GridTile gridTile = (GridTile)sender;
               
                if (!gridTile.GridPosition.IsHit && !gridTile.GridPosition.IsMissed)
                {
                    _selectedPosition = gridTile.GridPosition;
                    gridTile.GridPosition.IsSelected = true;
                }
                else
                {
                    _selectedPosition = null;
                }
                gridTile.Invalidate();
            }
            else
            {

                if (_selectedPosition != null)
                {
                    GridTile oldTile = (GridTile)tlp1PlayerGrid.GetControlFromPosition(_selectedPosition.X, _selectedPosition.Y);
                    oldTile.GridPosition.IsSelected = false;
                    oldTile.Invalidate();
                }

                GridTile gridTile = (GridTile)sender;
                if (!gridTile.GridPosition.IsHit && !gridTile.GridPosition.IsMissed)
                {
                    _selectedPosition = gridTile.GridPosition;
                    gridTile.GridPosition.IsSelected = true;
                }
                else
                {
                    _selectedPosition = null;
                }

                gridTile.Invalidate();
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

        private void bttnFire_Click(object sender, EventArgs e)
        {
            if (_selectedPosition != null)
            {
                if (CheckCOMport())
                {
                    GridPosition targetposition;
                    if (!shootturn)
                    {
                        //1 pl shoot
                        targetposition = _game.FireOnPl2(_selectedPosition);
                        if (targetposition.X == 404 && targetposition.Y == 404)
                        {
                            notShootAgain = false;
                            MessageBox.Show("Lost connection with server");
                        }
                        else { 
                            GamePositionUpdateInfoDelegatePointer(targetposition, false);
                        }
                    }
                    else
                    {
                        //2 pl shoot
                        targetposition = _game.FireOnPl1(_selectedPosition);
                        if (targetposition.X == 404 && targetposition.Y == 404)
                        {
                            MessageBox.Show("Lost connection with server");
                            notShootAgain = false;
                        }
                        else
                        {
                            GamePositionUpdateInfoDelegatePointer(targetposition, true);
                        }
                    }
                    if (notShootAgain)
                    {
                        shootturn = !shootturn;
                        NextTurn(shootturn);
                        notShootAgain = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("First you have to choose a position!");
            }
        }
        private void GamePositionUpdateInfoDelegatePointer(GridPosition position, bool pl)
        {
            if(pl) 
            {
                GridTile tile = (GridTile)tlp1PlayerGrid.GetControlFromPosition(position.X, position.Y);
                int posIndex = _pl1Position.FindIndex(x => x.Equals(position));
                tile.GridPosition = position;
                tile.Invalidate();
                if (tile.GridPosition.IsHit) 
                { 
                    CheckIfDestroyed(tlp1PlayerGrid, position, true);
                    hit2count++;
                    checkIfWin(false);
                    notShootAgain = false;

                }
                else
                {
                    notShootAgain = true;
                }

            }
            else
            {
                GridTile tile = (GridTile)tlp2PlayerGrid.GetControlFromPosition(position.X, position.Y);
                int posIndex = _pl2Position.FindIndex(x => x.Equals(position));
                tile.GridPosition = position;
                tile.Invalidate();
                if (tile.GridPosition.IsHit) 
                { 
                    CheckIfDestroyed(tlp2PlayerGrid, position, false);
                    hit1count++;
                    checkIfWin(true);
                    notShootAgain = false;

                }
                else
                {
                    notShootAgain = true;
                }
            }
            
        }
        public void CheckIfDestroyed(TableLayoutPanel grid, GridPosition position, bool pl)
        {
            int i;
            GridTile tile;
            if (pl)//pl 1 grid
            {
              
                i = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y));
                switch (_BattleMap[i].ShipType)
                {
                    case ShipType.PatrolBoat:
                        tile = (GridTile)grid.GetControlFromPosition(_BattleMap[i].GridPosition.X, _BattleMap[i].GridPosition.Y);
                        tile.SetTile(_BattleMap[i].TileType);
                        tile.SetOnFire();
                        break;
                    case ShipType.Submarine:
                        int j = 0;

                        switch (_BattleMap[i].TileType)
                        {
                            case TileType.ShipEndLeft:
                                j = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 1) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndRight:
                                j = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndUp:
                                j = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 1));
                                break;
                            case TileType.ShipEndDown:
                                j = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                break;
                        }
                        if (_BattleMap[i].GridPosition.IsHit && _BattleMap[j].GridPosition.IsHit)
                        {
                            tile = (GridTile)grid.GetControlFromPosition(_BattleMap[i].GridPosition.X, _BattleMap[i].GridPosition.Y);
                            tile.SetTile(_BattleMap[i].TileType);
                            tile.SetOnFire();
                            tile = (GridTile)grid.GetControlFromPosition(_BattleMap[j].GridPosition.X, _BattleMap[j].GridPosition.Y);
                            tile.SetTile(_BattleMap[j].TileType);
                            tile.SetOnFire();
                        }
                        break;
                    case ShipType.Battleship:
                        int a = 0, b = 0;
                        switch (_BattleMap[i].TileType)
                        {
                            case TileType.ShipEndLeft:
                                a = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 1) && x.GridPosition.Y.Equals(position.Y));
                                b = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 2) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndRight:
                                a = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                b = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 2) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndUp:
                                a = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 1));
                                b = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 2));
                                break;
                            case TileType.ShipEndDown:
                                a = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                b = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 2));
                                break;
                            case TileType.ShipCenterHorizontal:
                                a = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                b = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 1) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipCenterVertical:
                                a = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                b = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 1));
                                break;
                        }
                        if (_BattleMap[i].GridPosition.IsHit && _BattleMap[a].GridPosition.IsHit && _BattleMap[b].GridPosition.IsHit)
                        {
                            tile = (GridTile)grid.GetControlFromPosition(_BattleMap[i].GridPosition.X, _BattleMap[i].GridPosition.Y);
                            tile.SetTile(_BattleMap[i].TileType);
                            tile.SetOnFire();
                            tile = (GridTile)grid.GetControlFromPosition(_BattleMap[a].GridPosition.X, _BattleMap[a].GridPosition.Y);
                            tile.SetTile(_BattleMap[a].TileType);
                            tile.SetOnFire();
                            tile = (GridTile)grid.GetControlFromPosition(_BattleMap[b].GridPosition.X, _BattleMap[b].GridPosition.Y);
                            tile.SetTile(_BattleMap[b].TileType);
                            tile.SetOnFire();
                        }
                        break;
                    case ShipType.AircraftCarrier:
                        int[] mxy = new int[3] { 0, 0, 0 };
                        switch (_BattleMap[i].TileType)
                        {
                            case TileType.ShipEndLeft:
                                mxy[0] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 1) && x.GridPosition.Y.Equals(position.Y));
                                mxy[1] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 2) && x.GridPosition.Y.Equals(position.Y));
                                mxy[2] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 3) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndRight:
                                mxy[0] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                mxy[1] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 2) && x.GridPosition.Y.Equals(position.Y));
                                mxy[2] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 3) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndUp:
                                mxy[0] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 1));
                                mxy[1] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 2));
                                mxy[2] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 3));
                                break;
                            case TileType.ShipEndDown:
                                mxy[0] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                mxy[1] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 2));
                                mxy[2] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 3));
                                break;
                            case TileType.ShipCenterVertical:
                                mxy[0] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 1));
                                if (_BattleMap[mxy[0]].TileType == TileType.ShipCenterVertical)
                                {
                                    mxy[1] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 2));
                                    mxy[2] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                }
                                else
                                {
                                    mxy[1] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                    mxy[2] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 2));
                                }

                                break;
                            case TileType.ShipCenterHorizontal:

                                mxy[0] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 1) && x.GridPosition.Y.Equals(position.Y));
                                if (_BattleMap[mxy[0]].TileType == TileType.ShipCenterHorizontal)
                                {
                                    mxy[1] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 2) && x.GridPosition.Y.Equals(position.Y));
                                    mxy[2] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                }
                                else
                                {
                                    mxy[1] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                    mxy[2] = _BattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 2) && x.GridPosition.Y.Equals(position.Y));
                                }
                                break;
                        }
                        if (_BattleMap[i].GridPosition.IsHit && _BattleMap[mxy[0]].GridPosition.IsHit && _BattleMap[mxy[1]].GridPosition.IsHit && _BattleMap[mxy[2]].GridPosition.IsHit)
                        {
                            tile = (GridTile)grid.GetControlFromPosition(_BattleMap[i].GridPosition.X, _BattleMap[i].GridPosition.Y);
                            tile.SetTile(_BattleMap[i].TileType);
                            tile.SetOnFire();
                            for (int k = 0; k < 3; ++k)
                            {
                                tile = (GridTile)grid.GetControlFromPosition(_BattleMap[mxy[k]].GridPosition.X, _BattleMap[mxy[k]].GridPosition.Y);
                                tile.SetTile(_BattleMap[mxy[k]].TileType);
                                tile.SetOnFire();
                            }
                        }
                        break;
                }
            }
            else
            {
                i = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y));
                switch (_EnemyBattleMap[i].ShipType)
                {
                    case ShipType.PatrolBoat:
                        tile = (GridTile)grid.GetControlFromPosition(_EnemyBattleMap[i].GridPosition.X, _EnemyBattleMap[i].GridPosition.Y);
                        tile.SetTile(_EnemyBattleMap[i].TileType);
                        tile.SetOnFire();
                        break;
                    case ShipType.Submarine:
                        int j = 0;
                        switch (_EnemyBattleMap[i].TileType)
                        {
                            case TileType.ShipEndLeft:
                                j = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 1) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndRight:
                                j = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndUp:
                                j = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 1));
                                break;
                            case TileType.ShipEndDown:
                                j = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                break;
                        }
                        if (_EnemyBattleMap[i].GridPosition.IsHit && _EnemyBattleMap[j].GridPosition.IsHit)
                        {
                            tile = (GridTile)grid.GetControlFromPosition(_EnemyBattleMap[i].GridPosition.X, _EnemyBattleMap[i].GridPosition.Y);
                            tile.SetTile(_EnemyBattleMap[i].TileType);
                            tile.SetOnFire();
                            tile = (GridTile)grid.GetControlFromPosition(_EnemyBattleMap[j].GridPosition.X, _EnemyBattleMap[j].GridPosition.Y);
                            tile.SetTile(_EnemyBattleMap[j].TileType);
                            tile.SetOnFire();
                        }
                        break;
                    case ShipType.Battleship:
                        int a = 0, b = 0;
                        switch (_EnemyBattleMap[i].TileType)
                        {
                            case TileType.ShipEndLeft:
                                a = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 1) && x.GridPosition.Y.Equals(position.Y));
                                b = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 2) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndRight:
                                a = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                b = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 2) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndUp:
                                a = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 1));
                                b = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 2));
                                break;
                            case TileType.ShipEndDown:
                                a = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                b = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 2));
                                break;
                            case TileType.ShipCenterHorizontal:
                                a = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                b = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 1) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipCenterVertical:
                                a = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                b = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 1));
                                break;
                        }
                        if (_EnemyBattleMap[i].GridPosition.IsHit && _EnemyBattleMap[a].GridPosition.IsHit && _EnemyBattleMap[b].GridPosition.IsHit)
                        {
                            tile = (GridTile)grid.GetControlFromPosition(_EnemyBattleMap[i].GridPosition.X, _EnemyBattleMap[i].GridPosition.Y);
                            tile.SetTile(_EnemyBattleMap[i].TileType);
                            tile.SetOnFire();
                            tile = (GridTile)grid.GetControlFromPosition(_EnemyBattleMap[a].GridPosition.X, _EnemyBattleMap[a].GridPosition.Y);
                            tile.SetTile(_EnemyBattleMap[a].TileType);
                            tile.SetOnFire();
                            tile = (GridTile)grid.GetControlFromPosition(_EnemyBattleMap[b].GridPosition.X, _EnemyBattleMap[b].GridPosition.Y);
                            tile.SetTile(_EnemyBattleMap[b].TileType);
                            tile.SetOnFire();
                        }

                        break;
                    case ShipType.AircraftCarrier:
                        int[] mxy = new int[3] { 0, 0, 0 };
                        switch (_EnemyBattleMap[i].TileType)
                        {
                            case TileType.ShipEndLeft:
                                mxy[0] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 1) && x.GridPosition.Y.Equals(position.Y));
                                mxy[1] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 2) && x.GridPosition.Y.Equals(position.Y));
                                mxy[2] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 3) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndRight:
                                mxy[0] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                mxy[1] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 2) && x.GridPosition.Y.Equals(position.Y));
                                mxy[2] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 3) && x.GridPosition.Y.Equals(position.Y));
                                break;
                            case TileType.ShipEndUp:
                                mxy[0] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 1));
                                mxy[1] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 2));
                                mxy[2] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 3));
                                break;
                            case TileType.ShipEndDown:
                                mxy[0] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                mxy[1] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 2));
                                mxy[2] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 3));
                                break;
                            case TileType.ShipCenterVertical:
                                mxy[0] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 1));
                                if (_EnemyBattleMap[mxy[0]].TileType == TileType.ShipCenterVertical)
                                {
                                    mxy[1] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y + 2));
                                    mxy[2] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                }
                                else
                                {
                                    mxy[1] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 1));
                                    mxy[2] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X) && x.GridPosition.Y.Equals(position.Y - 2));
                                }
                               
                                break;
                            case TileType.ShipCenterHorizontal:

                                mxy[0] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 1) && x.GridPosition.Y.Equals(position.Y));
                                if (_EnemyBattleMap[mxy[0]].TileType == TileType.ShipCenterHorizontal)
                                {
                                    mxy[1] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X + 2) && x.GridPosition.Y.Equals(position.Y));
                                    mxy[2] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                }
                                else
                                {
                                    mxy[1] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 1) && x.GridPosition.Y.Equals(position.Y));
                                    mxy[2] = _EnemyBattleMap.FindIndex(x => x.GridPosition.X.Equals(position.X - 2) && x.GridPosition.Y.Equals(position.Y));
                                }
                                break;
                        }
                        if (_EnemyBattleMap[i].GridPosition.IsHit && _EnemyBattleMap[mxy[0]].GridPosition.IsHit && _EnemyBattleMap[mxy[1]].GridPosition.IsHit && _EnemyBattleMap[mxy[2]].GridPosition.IsHit)
                        {
                            tile = (GridTile)grid.GetControlFromPosition(_EnemyBattleMap[i].GridPosition.X, _EnemyBattleMap[i].GridPosition.Y);
                            tile.SetTile(_EnemyBattleMap[i].TileType);
                            tile.SetOnFire();
                            for (int k = 0; k < 3; ++k)
                            {
                                tile = (GridTile)grid.GetControlFromPosition(_EnemyBattleMap[mxy[k]].GridPosition.X, _EnemyBattleMap[mxy[k]].GridPosition.Y);
                                tile.SetTile(_EnemyBattleMap[mxy[k]].TileType);
                                tile.SetOnFire();
                            }
                        }
                        break;
                }
            }
        }
        public void checkIfWin(bool pl)
        {
            if(pl){
                if (hit1count == 20)
                {
                    MessageBox.Show("Player1 won");
                    tlp1PlayerGrid.Enabled = false;
                    tlp2PlayerGrid.Enabled = false;
                    bttnFire.Enabled = false;
                }
            }
            else
            {
                if (hit2count == 20)
                {
                    MessageBox.Show("Player2 won");
                    tlp1PlayerGrid.Enabled = false;
                    tlp2PlayerGrid.Enabled = false;
                    bttnFire.Enabled = false;
                }
            }
        }

    }
}
