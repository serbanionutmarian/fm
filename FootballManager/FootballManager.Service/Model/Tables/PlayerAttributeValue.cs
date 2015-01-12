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
        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public virtual Player Player { get; set; }
        
        [Required]
        //to do!! urgent!!
        public int AttributeId { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}
