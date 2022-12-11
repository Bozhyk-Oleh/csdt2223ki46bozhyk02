using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sea_Battle
{
    public class Enums
    {
        //https://www.codingame.com/playgrounds/2487/c---how-to-display-friendly-names-for-enumerations
        public static string GetDescription(Enum enumName)
        {
            Type enumNameType = enumName.GetType();
            MemberInfo[] memberInfo = enumNameType.GetMember(enumName.ToString());
            if ((memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs.Count() > 0))
                {
                    return ((DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return enumName.ToString();
        }
    }

    public enum ShipType
    {

        None = 0,
        PatrolBoat = 1,
        Submarine = 2,
        Battleship = 3,
        AircraftCarrier = 4,

    }

    public enum TileType
    {
        Water,
        ShipCenterHorizontal,
        ShipCenterVertical,
        ShipEndLeft,
        ShipEndRight,
        ShipEndUp,
        ShipEndDown,
        ShipSolo
    }

    public enum PlacementType
    {
        Solo,
        Horizontal,
        Vertical,
        Invalid,
        Occupied,
        Connection_error,
        Bridge_connection_error
    }

    public enum UpdateType
    {
        PlayerGrid,
        EnemyGrid
    }

    public enum ResponseType
    {
        Accepted,
        Rejected
    }
}
