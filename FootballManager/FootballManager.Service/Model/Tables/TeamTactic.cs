using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("TeamTactics")]
    public class TeamTactic : BaseEntity
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        public string SelectedPlayers { get; set; }

        [Required]
        public int ShotsType { get; set; }

        [Required]
        public int MarkingType { get; set; }

        [Required]
        public int AggressivityType { get; set; }

        [Required]
        public int OffsideTrapType { get; set; }

        [Required]
        public decimal Mentality { get; set; }

        [Required]
        public decimal Pressure { get; set; }

        [Required]
        public decimal Tempo { get; set; }

        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; }
    }
}
