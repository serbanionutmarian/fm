using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tables
{
    [Table("Jobs")]
    public class Job : Entity<int>
    {
        public string ImplementationPath { get; set; }

        [Column("ExecutionType")]
        public JobExecutionType JobExecutionType { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
