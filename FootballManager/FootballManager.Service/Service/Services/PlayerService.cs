using DataModel;
using DataModel.Player;
using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Services
{
    public class PlayerService : IPlayerService
    {
        private IClassificationService _classificationService;
        public PlayerService(IClassificationService classificationService)
        {
            _classificationService = classificationService;
        }

        public List<Classification> GetPlayerAttributes()
        {
            return _classificationService.GetAll<PlayerAttribute>();
        }
    }
}
