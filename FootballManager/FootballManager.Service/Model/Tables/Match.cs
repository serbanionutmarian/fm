using DataModel.Schedule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("Matches")]
    public class Match : Entity<int>
    {
        [Required]
        public int HomeTeamId { get; set; }

        [Required]
        public int AwayTeamId { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public MatchType MatchType { get; set; }

        public int? HalfTimeScoreHome { get; set; }

        public int? HalfTimeScoreAway { get; set; }

        public int? FinalTimeScoreHome { get; set; }

        public int? FinalTimeScoreAway { get; set; }

        [ForeignKey("HomeTeamId")]
        public virtual Team HomeTeam { get; set; }


        [ForeignKey("AwayTeamId")]
        public virtual Team AwayTeam { get; set; }
    }
}
