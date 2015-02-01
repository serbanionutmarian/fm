using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("Teams")]
    public class Team : Entity<int>
    {
        public Team()
        {
            Players = new HashSet<Player>();
        }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public int SeriesId { get; set; }

        [ForeignKey("SeriesId")]
        public virtual Series Series { get; set; }

        [Required]
        public bool IsBoot { get; set; }

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

        public virtual ICollection<Player> Players { get; set; }
    }
}
