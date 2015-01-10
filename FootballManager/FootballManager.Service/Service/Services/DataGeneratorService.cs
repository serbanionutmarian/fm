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
    public class DataGeneratorService : IDataGeneratorService
    {
        #region ctor and props

        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly ILeagesConfigurationRepository _leagesConfigurationRepository;
        private readonly ICountryService _countryService;
        private readonly ISeriesRepository _seriesRepository;

        public DataGeneratorService(
            IUnitOfWork unitOfWork,
            ITeamRepository teamRepository,
            ILeagesConfigurationRepository leagesConfigurationRepository,
            ICountryService countryService,
            ISeriesRepository seriesRepository)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _leagesConfigurationRepository = leagesConfigurationRepository;
            _countryService = countryService;
            _seriesRepository = seriesRepository;
        }

        #endregion

        public void AddLeages()
        {
            var leagesConfigurations = _leagesConfigurationRepository.GetAll().ToList();
            var countries = _countryService.GetAll().ToList();
            foreach (var country in countries)
            {
                AddLeages(leagesConfigurations, country);
            }
        }

        private void AddLeages(IEnumerable<Model.Tables.LeagesConfiguration> leagesConfigurations, Model.Tables.Country country)
        {
            if (country.NrOfLeagesToAdd == 0)
            {
                return;
            }
            var currentNrOfLeages = country.CurrentNrOfLeages;
            var parentSeries = country.CurrentNrOfLeages == 0 ?
                   null :
                _seriesRepository.GetByLeageIdAnCountryid(currentNrOfLeages, country.Id);

            for (int i = 0; i < country.NrOfLeagesToAdd; i++)
            {

                currentNrOfLeages++;
                var configuration = leagesConfigurations.SingleOrDefault(leageConfiguration => leageConfiguration.Id == currentNrOfLeages);
                if (configuration == null)
                {
                    throw new NotSupportedException("Number of leages you want to add is less than maxim number of configured leages!!");
                }
                if (parentSeries != null)
                {
                    foreach (var series in parentSeries)
                    {
                        parentSeries = AddLeages(configuration, series, country.Id);
                    }
                }
                else
                {
                    parentSeries = AddLeages(configuration, null, country.Id);
                }
            }
            country.CurrentNrOfLeages += country.NrOfLeagesToAdd;
            country.NrOfLeagesToAdd = 0;
            _unitOfWork.Commit();
        }

        private List<Model.Tables.Series> AddLeages(Model.Tables.LeagesConfiguration configuration, Model.Tables.Series parentSeries, int countryId)
        {
            var result = new List<Model.Tables.Series>();
            for (int j = 0; j < configuration.NrOfBranchSeries; j++)
            {
                // add series
                var series = new Model.Tables.Series()
                {
                    CountryId = countryId,
                    LeagesConfigurationId = configuration.Id,
                    Parent = parentSeries
                };
                _seriesRepository.Add(series);
                result.Add(series);
                //add team boots
                for (int k = 0; k < configuration.CurrentNrOfTeams; k++)
                {
                    AddTeamBoot(series);
                }
            }
            return result;
        }

        private void AddTeamBoot(Model.Tables.Series parentSeries)
        {
            _teamRepository.Add(new Model.Tables.Team()
            {
                Name = "team " + Guid.NewGuid(),
                IsBoot = true,
                Series = parentSeries
            });
        }
    }
}
