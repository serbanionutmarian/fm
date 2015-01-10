using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("LeagesConfigurations")]
    public class LeagesConfiguration : Entity<int>
    {
        public int? NrOfTeamsPromoted { get; set; }

        public int? NrOfTeamsReleagated { get; set; }

        [Required]
        public int CurrentNrOfTeams { get; set; }

        [Required]
        public int NrOfBranchSeries { get; set; }
    }
}
