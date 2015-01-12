using DataModel;
using DataModel.Player;
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
    public class DataGeneratorService : IDataGeneratorService
    {
        #region ctor and props

        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly ILeagesConfigurationRepository _leagesConfigurationRepository;
        private readonly ICountryService _countryService;
        private readonly ISeriesRepository _seriesRepository;
        private Random _random;

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

            _random = new Random();
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
            // TO DO!! (generate team name)
            _teamRepository.Add(new DataModel.Tables.Team()
            {
                Name = "team " + Guid.NewGuid(),
                IsBoot = true,
                Series = series
            });
            foreach (PlayerPosition playerPosition in Enum.GetValues(typeof(PlayerPosition)))
            {
                foreach (KeyValuePair<PlayerPosition, int> playerPositionCount in Constants.NumberOfPlayersPerPositions)
                {
                    Player player = new Player();

                    var currentDate = DateTime.Now;
                    int age = _random.Next(18, 30);
                    int month = _random.Next(1, 12);
                    int day = _random.Next(1, 28);
                    player.BirthDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
                    DateTime tempDate = new DateTime(age, month, day);
                    player.BirthDate.Subtract(tempDate);

                    // to do!! (how to implement this)
                    int attributesSum = _random.Next(0, 100);
                    //GenerateAttributesForPlayer(player, playerPositionCount.Key, attributesSum);
                }
            }
        }

        //static void GenerateAttributesForPlayer(Player player, PlayerPosition fieldPos, int attributesSum)
        //{
        //    // Max morale
        //    player.mAttributes.SetAttr(PlayerAttribute.ATTR_MORALE, mMaxAttrValueAtGeneration);

        //    // Avg form
        //    player.mAttributes.SetAttr(PlayerAttribute.ATTR_FORM, mMaxAttrValueAtGeneration);

        //    // Leadership, Tenacity - 1/8 to be a good leader
        //    int leaderProb = _random.Next(1, 8);
        //    if (leaderProb == 1)
        //    {
        //        player.mAttributes.SetAttr(PlayerAttribute.ATTR_LEADERSHIP, _random.Next(10, 20));
        //        player.mAttributes.SetAttr(PlayerAttribute.ATTR_TENACITY, _random.Next(10, 20));
        //    }
        //    //----

        //    // Set pieces
        //    int penTakerProb = 5;		// Out of 20
        //    int freekickTakerProb = 3;
        //    int cornerTakerProb = 4;
        //    int throwerTakerProb = 2;

        //    player.mAttributes.SetAttr(PlayerAttribute.ATTR_PENALTY, (_random.Next(1, 20) <= penTakerProb ? _random.Next(14, 20) : _random.Next(3, 14)));
        //    player.mAttributes.SetAttr(PlayerAttribute.ATTR_FREE_KICKS, (_random.Next(1, 20) <= freekickTakerProb ? _random.Next(14, 20) : _random.Next(3, 14)));
        //    player.mAttributes.SetAttr(PlayerAttribute.ATTR_CORNERS, (_random.Next(1, 20) <= cornerTakerProb ? _random.Next(14, 20) : _random.Next(3, 14)));
        //    player.mAttributes.SetAttr(PlayerAttribute.ATTR_THROWINS, (_random.Next(1, 20) <= throwerTakerProb ? _random.Next(14, 20) : _random.Next(3, 14)));
        //    //----

        //    // Generate the minimum attributes
        //    AttributeRange[] minAttrForFieldPos = mAttributesRangePerFieldPos[(int)fieldPos];
        //    for (int i = 0; i < minAttrForFieldPos.Length; i++)
        //    {
        //        AttributeRange range = minAttrForFieldPos[i];
        //        player.mAttributes.SetAttr(range.mAttrib, _random.Next(range.mMin, range.mMax));
        //    }
        //    //---------

        //    // Randomly distribute the rest of attributes
        //    for (int i = 0; i < (int)PlayerAttribute.ATTR_NUM_ATTRIBUTES; i++)
        //    {
        //        PlayerAttribute playerAttr = (PlayerAttribute)i;
        //        attributesSum -= (int)player.mAttributes.GetAttr(playerAttr);
        //    }
        //    System.Diagnostics.Debug.Assert(attributesSum > 20);	// We expect to remain with something after setting the min attributes..

        //    for (int i = 0; i < attributesSum; i++)
        //    {
        //        PlayerAttribute playerAttr = (PlayerAttribute)_random.Next(0, (int)PlayerAttribute.ATTR_NUM_ATTRIBUTES - 1);	// TODO: Should we have different probabilities for attributes ??
        //        player.mAttributes.SetAttr(playerAttr, player.mAttributes.GetAttr(playerAttr) + 1);
        //    }
        //    //---------

        //    // Make all attributes between [0,1]
        //    for (int i = 0; i < (int)PlayerAttribute.ATTR_NUM_ATTRIBUTES; i++)
        //    {
        //        PlayerAttribute playerAttr = (PlayerAttribute)i;
        //        player.mAttributes.SetAttr(playerAttr, player.mAttributes.GetAttr(playerAttr) / mMaxAttrValueAtGeneration);
        //        if (player.mAttributes.GetAttr(playerAttr) > 1.0f)
        //        {
        //            player.mAttributes.SetAttr(playerAttr, 1.0f); // TODO: need to keep an eye on this...
        //        }
        //    }
        //    //---------
        //}
    }
}
