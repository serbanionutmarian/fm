using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("Logs")]
    public class Log : Entity<long>
    {
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string Message { get; set; }

        public string Source { get; set; }

        [Required]
        public string StackTrace { get; set; }

        [Required]
        public string Request { get; set; }

        public string AbsoluteUri { get; set; }
    }
}
