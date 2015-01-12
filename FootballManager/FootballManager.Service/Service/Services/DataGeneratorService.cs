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

        /// <summary>
        /// iterate throw all countries and add it required new leages
        /// </summary>
        public void AddLeagesToAllCountries()
        {
            var leagesConfigurations = _leagesConfigurationRepository.GetAll();
            var countries = _countryService.GetAll();
            foreach (var country in countries)
            {
                AddLeagesToCountry(leagesConfigurations, country);
            }
        }

        private void AddLeagesToCountry(IEnumerable<DataModel.Tables.LeagesConfiguration> leagesConfigurations, DataModel.Tables.Country country)
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
                        parentSeries = AddSeriesToCountry(configuration, series, country.Id);
                    }
                }
                else
                {
                    parentSeries = AddSeriesToCountry(configuration, null, country.Id);
                }
            }
            country.CurrentNrOfLeages += country.NrOfLeagesToAdd;
            country.NrOfLeagesToAdd = 0;
            _unitOfWork.SaveChanges();
        }

        private List<DataModel.Tables.Series> AddSeriesToCountry(DataModel.Tables.LeagesConfiguration configuration, DataModel.Tables.Series parentSeries, int countryId)
        {
            var result = new List<DataModel.Tables.Series>();
            for (int i = 0; i < configuration.NrOfBranchSeries; i++)
            {
                // add series
                var series = new DataModel.Tables.Series()
                {
                    CountryId = countryId,
                    LeagesConfigurationId = configuration.Id,
                    Parent = parentSeries
                };
                _seriesRepository.Add(series);
                result.Add(series);
                //add team boots
                for (int j = 0; j < configuration.CurrentNrOfTeams; j++)
                {
                    AddTeamBoot(series);
                }
            }
            return result;
        }

        private void AddTeamBoot(DataModel.Tables.Series series)
        {
            _teamRepository.Add(new DataModel.Tables.Team()
            {
                Name = "team " + Guid.NewGuid(),
                IsBoot = true,
                Series = series
            });
        }
    }
}
