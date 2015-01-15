using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("PlayersAttributesValues")]
    public class PlayerAttributeValue : BaseEntity
    {
        [Required]
        [Key, Column(Order = 0)]
        public int PlayerId { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        //to do!! must be changed to enum(PlayerAttrivbute)
        public int AttributeId { get; set; }

        [ForeignKey("PlayerId")]
        public virtual Player Player { get; set; }
        

        [Required]
        public int Value { get; set; }
    }
}
