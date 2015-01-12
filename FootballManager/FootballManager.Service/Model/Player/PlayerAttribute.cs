using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Player
{
    [DataContract]
    public enum PlayerAttribute
    {
        // ATTACKING
        [Description("Finishing")]
        ATTR_FINISHING = 0,
        [Description("Long Shoots")]
        ATTR_LONG_SHOTS = 1,
        ATTR_OFF_THE_BALL = 2,
        ATTR_CREATIVITY = 3,
        ATTR_TECHNIQUE = 4,
        ATTR_DRIBBLING = 5,
        ATTR_HEADING = 6,

        // Defending
        //ATTR_HEADING
        ATTR_TACKLING = 7,
        ATTR_MARKING = 8,
        ATTR_AGGRESIVITY = 9,
        ATTR_POSITIONING = 10,

        // Set pieces
        ATTR_FREE_KICKS = 11,
        ATTR_PENALTY = 12,
        ATTR_CORNERS = 13,
        ATTR_THROWINS = 14,

        // Physics
        ATTR_AGILITY = 15,
        ATTR_ACCELERATION = 16,
        ATTR_SPEED = 17,
        ATTR_STAMINA = 18,
        ATTR_STRENGTH = 19,
        ATTR_INJURY_PRONENESS = 20,
        ATTR_BALANCE = 21,

        // Construction
        ATTR_PASSING = 22,
        ATTR_TEAMWORK = 23,
        ATTR_CROSSING = 24,
        //ATTR_OFF_THE_BALL

        // Mentality
        ATTR_LEADERSHIP = 25,
        //ATTR_AGGRESIVITY
        //ATTR_TEAMWORK,
        ATTR_TENACITY = 26,
        //ATTR_VISION
        //ATTR_CREATIVITY


        // GK
        ATTR_HANDLING = 27,
        ATTR_REFLEXES = 28,
        //ATTR_HEADING,
        //ATTR_PASSING,

        ATTR_CONDITION = 29,	// 100% very good to play in a match, 50% not fit/too tired for a match
        ATTR_MORALE = 30,	// 100% very good 

        // Hidden attributes
        ATTR_FORM = 31
    }
}
