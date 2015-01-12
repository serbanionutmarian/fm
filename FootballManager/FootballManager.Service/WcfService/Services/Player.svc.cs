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
    public class Player : IPlayer
    {
        private IPlayerService _playerService;
        public Player(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public List<Classification> GetAllPlayerAttributes()
        {
            return _playerService.GetPlayerAttributes();
        }
    }
}
