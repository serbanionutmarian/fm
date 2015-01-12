using DataModel.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("Players")]
    public class Player : Entity<int>
    {
        public Player()
        {
            PlayersAttributesValues = new List<PlayerAttributeValue>();
        }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public int TeamId { get; set; }

        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; }

        public virtual ICollection<PlayerAttributeValue> PlayersAttributesValues { get; set; }

        public void SetAttribute(PlayerAttribute playerAttribute, decimal value) { 
        }
    }
}
