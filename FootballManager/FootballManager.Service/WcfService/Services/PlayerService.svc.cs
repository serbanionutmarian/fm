using DataModel;
using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfService.Interfaces;

namespace WcfService.Services
{
    public class PlayerService : WcfService.Interfaces.IPlayerService
    {
        private DataService.Interfaces.IPlayerService _playerService;
        public PlayerService(DataService.Interfaces.IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public List<Classification> GetAllPlayerAttributes()
        {
            return _playerService.GetPlayerAttributes();
        }
    }
}
