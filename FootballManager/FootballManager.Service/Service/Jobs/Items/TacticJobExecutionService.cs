using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Jobs.Items
{
    public class TacticJobExecutionService : JobExecutionBase
    {
        private readonly ITeamRepository _teamRepostory;
        public TacticJobExecutionService(ITeamRepository teamRepository)
        {
            _teamRepostory = teamRepository;
        }

        public override void Execute()
        {

        }
    }
}
