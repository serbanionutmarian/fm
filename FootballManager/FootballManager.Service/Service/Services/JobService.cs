using DataModel;
using DataModel.Tables;
using DataService.Interfaces;
using DataService.Jobs;
using LightInject;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Services
{
    public class JobService : IJobService
    {
        private IJobRepository _jobRepository;
        private IServiceContainer _serviceContainer;

        private DateTime _now;
        public JobService(IJobRepository jobRepository, IServiceContainer serviceContainer)
        {
            _jobRepository = jobRepository;
            _serviceContainer = serviceContainer;
            _now = DateTime.Now;
        }

        public void Do()
        {
            var jobs = _jobRepository.GetAll().ToList();
            foreach (var job in jobs)
            {
                if (MustRun(job))
                {
                    Run(job);
                    job.UpdatedAt = _now;
                    _jobRepository.Save();
                }
            }
        }

        private void Run(Job job)
        {
            var implementationType = Type.GetType(job.ImplementationPath);
            var executionBase = (JobExecutionBase)_serviceContainer.GetInstance(implementationType);
            executionBase.Execute();
        }

        private bool MustRun(Job job)
        {
            if (!job.UpdatedAt.HasValue)
            {
                return true;
            }
            var currentDate = _now.Date;
            if (job.JobExecutionType == JobExecutionType.Daily && job.UpdatedAt.Value.Date < currentDate)
            {
                return true;
            }
            if (job.JobExecutionType == JobExecutionType.LastDayOfMonth && job.UpdatedAt.Value.Date < currentDate && currentDate.Day == DateTime.DaysInMonth(currentDate.Year, currentDate.Day))
            {
                return true;
            }
            return false;
        }
    }
}
