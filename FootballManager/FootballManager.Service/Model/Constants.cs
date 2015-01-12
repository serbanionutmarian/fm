using DataModel.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public static class Constants
    {
        public static Dictionary<PlayerPosition, int> NumberOfPlayersPerPositions;

        static Constants()
        {
            NumberOfPlayersPerPositions = new Dictionary<PlayerPosition, int>();
            NumberOfPlayersPerPositions.Add(PlayerPosition.FIELD_POS_GK, 3);
            NumberOfPlayersPerPositions.Add(PlayerPosition.FIELD_POS_DEF, 8);
            NumberOfPlayersPerPositions.Add(PlayerPosition.FIELD_POS_MID, 10);
            NumberOfPlayersPerPositions.Add(PlayerPosition.FIELD_POS_ATT, 4);
        }
    }
}
