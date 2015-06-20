using DataModel.Tables;
using DataService.Interfaces;
using Repository;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Services
{
    public class LogService : EntityService<Log>, ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(IUnitOfWork unitOfWork, ILogRepository logRepository)
            : base(unitOfWork, logRepository)
        {
            _logRepository = logRepository;
        }
    }
}
