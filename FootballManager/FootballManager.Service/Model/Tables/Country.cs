using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    [Table("Countries")]
    public class Country : Entity<int>
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
