using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("Series")]
    public class Series : Entity<int>
    {
        public Series()
        {
            Teams = new HashSet<Team>();
        }
        [Required]
        public int LeagesConfigurationId { get; set; }

        [ForeignKey("LeagesConfigurationId")]
        public virtual LeagesConfiguration LeagesConfiguration { get; set; }

        public int CountryId { get; set; }

        [Required]
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Series Parent { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
