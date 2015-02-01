using DataModel.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("Players")]
    public class Player : Entity<int>
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public int TeamId { get; set; }

        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; }

        [Required]
        public int ATTR_FINISHING { get; set; }

        [Required]
        public int ATTR_LONG_SHOTS { get; set; }

        [Required]
        public int ATTR_OFF_THE_BALL { get; set; }

        [Required]
        public int ATTR_CREATIVITY { get; set; }

        [Required]
        public int ATTR_TECHNIQUE { get; set; }

        [Required]
        public int ATTR_DRIBBLING { get; set; }

        [Required]
        public int ATTR_HEADING { get; set; }

        [Required]
        public int ATTR_TACKLING { get; set; }

        [Required]
        public int ATTR_MARKING { get; set; }

        [Required]
        public int ATTR_AGGRESIVITY { get; set; }

        [Required]
        public int ATTR_POSITIONING { get; set; }

        [Required]
        public int ATTR_FREE_KICKS { get; set; }

        [Required]
        public int ATTR_PENALTY { get; set; }

        [Required]
        public int ATTR_CORNERS { get; set; }

        [Required]
        public int ATTR_THROWINS { get; set; }

        [Required]
        public int ATTR_AGILITY { get; set; }

        [Required]
        public int ATTR_ACCELERATION { get; set; }

        [Required]
        public int ATTR_SPEED { get; set; }

        [Required]
        public int ATTR_STAMINA { get; set; }

        [Required]
        public int ATTR_STRENGTH { get; set; }

        [Required]
        public int ATTR_INJURY_PRONENESS { get; set; }

        [Required]
        public int ATTR_BALANCE { get; set; }

        [Required]
        public int ATTR_PASSING { get; set; }

        [Required]
        public int ATTR_TEAMWORK { get; set; }

        [Required]
        public int ATTR_CROSSING { get; set; }

        [Required]
        public int ATTR_LEADERSHIP { get; set; }

        [Required]
        public int ATTR_TENACITY { get; set; }

        [Required]
        public int ATTR_HANDLING { get; set; }

        [Required]
        public int ATTR_REFLEXES { get; set; }

        [Required]
        public int ATTR_CONDITION { get; set; }

        [Required]
        public int ATTR_MORALE { get; set; }

        [Required]
        public int ATTR_FORM { get; set; }

        public void SetAttribute(PlayerAttribute playerAttribute, int value)
        {
            PropertyInfo propertyInfo = this.GetType().GetProperty(playerAttribute.ToString());
            propertyInfo.SetValue(this, Convert.ChangeType(value, propertyInfo.PropertyType), null);
        }

        public int GetAttribute(PlayerAttribute playerAttribute)
        {
            PropertyInfo propertyInfo = this.GetType().GetProperty(playerAttribute.ToString());
            return (int)propertyInfo.GetValue(this);
        }
    }
}
