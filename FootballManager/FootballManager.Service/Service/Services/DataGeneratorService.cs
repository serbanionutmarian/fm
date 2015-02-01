using DataModel;
using DataModel.Player;
using DataModel.Tables;
using DataService.Interfaces;
using Repository;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataService.Services
{
    public class DataGeneratorService : IDataGeneratorService
    {
        #region ctor and props

        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly ILeagesConfigurationRepository _leagesConfigurationRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ISeriesRepository _seriesRepository;

        private Random _random;
        private int _maxAttrValueAtGeneration = 20;
        class AttributeRange
        {
            public PlayerAttribute PlayerAttribute { get; private set; }
            public int Min { get; private set; }
            public int Max { get; private set; }

            public AttributeRange(PlayerAttribute playerAttribute, int min, int max)
            {
                PlayerAttribute = playerAttribute;
                Min = min;
                Max = max;
            }
        }
        enum PlayerPosition
        {
            FIELD_POS_GK = 0,
            FIELD_POS_DEF = 1,
            FIELD_POS_MID = 2,
            FIELD_POS_ATT = 3
        }

        static class Constants
        {
            public static Dictionary<PlayerPosition, int> NumberOfPlayersPerPositions;

            public static Dictionary<PlayerPosition, List<AttributeRange>> AttributesRankePerFieldPostions { get; set; }
            static Constants()
            {
                NumberOfPlayersPerPositions = new Dictionary<PlayerPosition, int>();
                NumberOfPlayersPerPositions.Add(PlayerPosition.FIELD_POS_GK, 3);
                NumberOfPlayersPerPositions.Add(PlayerPosition.FIELD_POS_DEF, 8);
                NumberOfPlayersPerPositions.Add(PlayerPosition.FIELD_POS_MID, 10);
                NumberOfPlayersPerPositions.Add(PlayerPosition.FIELD_POS_ATT, 4);

                AttributesRankePerFieldPostions = new Dictionary<PlayerPosition, List<AttributeRange>>();
                AttributesRankePerFieldPostions.Add(PlayerPosition.FIELD_POS_GK, new List<AttributeRange>() { 
                    new AttributeRange(PlayerAttribute.ATTR_HANDLING,8,13),
                    new AttributeRange(PlayerAttribute.ATTR_REFLEXES,8,13)
                });
                AttributesRankePerFieldPostions.Add(PlayerPosition.FIELD_POS_DEF, new List<AttributeRange>() { 
                      new AttributeRange(PlayerAttribute.ATTR_TACKLING,8,12),
                      new AttributeRange(PlayerAttribute.ATTR_MARKING,5,10),
                      new AttributeRange(PlayerAttribute.ATTR_HEADING,5,12),
                      new AttributeRange(PlayerAttribute.ATTR_SPEED,4,8),
                      new AttributeRange(PlayerAttribute.ATTR_POSITIONING,5,12)
                });
                AttributesRankePerFieldPostions.Add(PlayerPosition.FIELD_POS_MID, new List<AttributeRange>() { 
                      new AttributeRange(PlayerAttribute.ATTR_PASSING,8,12),
                      new AttributeRange(PlayerAttribute.ATTR_TEAMWORK,8,12),
                      new AttributeRange(PlayerAttribute.ATTR_CROSSING,5,12),
                      new AttributeRange(PlayerAttribute.ATTR_CREATIVITY,5,12),
                      new AttributeRange(PlayerAttribute.ATTR_SPEED,8,12),
                      new AttributeRange(PlayerAttribute.ATTR_ACCELERATION,8,12)
                });
                AttributesRankePerFieldPostions.Add(PlayerPosition.FIELD_POS_ATT, new List<AttributeRange>() { 
                      new AttributeRange(PlayerAttribute.ATTR_FINISHING,8,12),
                      new AttributeRange(PlayerAttribute.ATTR_LONG_SHOTS,4,7),
                      new AttributeRange(PlayerAttribute.ATTR_DRIBBLING,6,9),
                      new AttributeRange(PlayerAttribute.ATTR_TECHNIQUE,5,9),
                      new AttributeRange(PlayerAttribute.ATTR_OFF_THE_BALL,3,8),
                      new AttributeRange(PlayerAttribute.ATTR_SPEED,5,10),
                      new AttributeRange(PlayerAttribute.ATTR_ACCELERATION,5,10),
                      new AttributeRange(PlayerAttribute.ATTR_STRENGTH,5,10),
                      new AttributeRange(PlayerAttribute.ATTR_HEADING,5,10),
                      new AttributeRange(PlayerAttribute.ATTR_POSITIONING,5,10)
                });
            }
        }

        public DataGeneratorService(
            IUnitOfWork unitOfWork,
            ITeamRepository teamRepository,
            ILeagesConfigurationRepository leagesConfigurationRepository,
            ICountryRepository countryRepository,
            ISeriesRepository seriesRepository)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _leagesConfigurationRepository = leagesConfigurationRepository;
            _countryRepository = countryRepository;
            _seriesRepository = seriesRepository;
        }

        #endregion

        /// <summary>
        /// iterate throw all countries and add it required new leages
        /// </summary>
        public void AddLeagesToAllCountries()
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(2, 0, 0)))
            {
                var leagesConfigurations = _leagesConfigurationRepository.GetAll();
                var countries = _countryRepository.GetAll();
                foreach (var country in countries)
                {
                    AddLeagesToCountry(leagesConfigurations, country);
                }
                transaction.Complete();
            }
        }
        //test bulk inset
        //private static void PerformBulkCopy()
        //{
        //    string connectionString = @"Data Source=VM-NET2013\SQLEXPRESS;Initial Catalog=FootballManager;Integrated Security=True";
        //    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionString))
        //    {
        //        bulkCopy.BatchSize = 500;
        //        bulkCopy.DestinationTableName = "PlayersAttributesValues";
        //        var data = new List<DataRow>();
        //        DataTable table = new DataTable();
        //        table.Columns.Add("PlayerId");
        //        table.Columns.Add("AttributeId");
        //        table.Columns.Add("Value");
        //        for (int i = 0; i < 1000 * 1000; i++)
        //        {
        //            var row = table.NewRow();
        //            row["PlayerId"] = 30150;
        //            row["AttributeId"] = i + 1000 * 1000;
        //            row["Value"] = i;
        //            table.Rows.Add(row);
        //        }
        //        bulkCopy.WriteToServer(table);
        //    }
        //}
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
            var nrOfTeamsInSeries = country.CurrentNrOfLeages == 0 ?
                1 :
                GetInitialNumberOfTeamsInSeries(leagesConfigurations, country.CurrentNrOfLeages);

            for (int i = 0; i < country.NrOfLeagesToAdd; i++)
            {
                currentNrOfLeages++;
                var configuration = leagesConfigurations.SingleOrDefault(leageConfiguration => leageConfiguration.Id == currentNrOfLeages);
                nrOfTeamsInSeries *= configuration.NrOfBranchSeries;
                if (configuration == null)
                {
                    throw new NotSupportedException("Number of leages you want to add is less than maxim number of configured leages!!");
                }
                if (parentSeries != null)
                {
                    foreach (var series in parentSeries)
                    {
                        parentSeries = AddSeriesToCountry(configuration, series, country.Id, nrOfTeamsInSeries);
                    }
                }
                else
                {
                    parentSeries = AddSeriesToCountry(configuration, null, country.Id, nrOfTeamsInSeries);
                }
            }
            country.CurrentNrOfLeages += country.NrOfLeagesToAdd;
            country.NrOfLeagesToAdd = 0;
            _unitOfWork.SaveChanges();
        }

        private int GetInitialNumberOfTeamsInSeries(IEnumerable<LeagesConfiguration> leagesConfigurations, int currentNrOfLeages)
        {
            var result = 1;
            foreach (var leagesConfiguration in leagesConfigurations)
            {
                result *= leagesConfiguration.NrOfBranchSeries;
                if (leagesConfiguration.Id == currentNrOfLeages)
                {
                    return result;
                }
            }
            return result;
        }

        private List<DataModel.Tables.Series> AddSeriesToCountry(DataModel.Tables.LeagesConfiguration configuration, DataModel.Tables.Series parentSeries, int countryId, int nrOfTeamsInSeries)
        {
            var result = new List<DataModel.Tables.Series>();
            for (int i = 0; i < nrOfTeamsInSeries; i++)
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
                    AddTeamBoot(series, configuration);
                }
            }
            return result;
        }

        private void AddTeamBoot(DataModel.Tables.Series series, DataModel.Tables.LeagesConfiguration configuration)
        {
            // TO DO!! (generate team name)
            var team = new DataModel.Tables.Team()
            {
                Name = "team " + Guid.NewGuid(),
                IsBoot = true
            };

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

                    int attributesSum = _random.Next(configuration.MinOverallSkills, configuration.MaxOverallSkills);
                    GenerateAttributesForPlayer(player, playerPositionCount.Key, attributesSum);
                    team.Players.Add(player);
                }
            }
            series.Teams.Add(team);
        }

        private void GenerateAttributesForPlayer(Player player, PlayerPosition fieldPos, int attributesSum)
        {
            player.Name = "player_" + Guid.NewGuid();
            // Max morale

            player.ATTR_MORALE = _maxAttrValueAtGeneration;

            // Avg form
            player.ATTR_FORM = _maxAttrValueAtGeneration;

            // Leadership, Tenacity - 1/8 to be a good leader
            int leaderProb = _random.Next(1, 8);
            if (leaderProb == 1)
            {
                player.ATTR_LEADERSHIP = _random.Next(10, 20);
                player.ATTR_TENACITY = _random.Next(10, 20);
            }
            //----

            // Set pieces
            int penTakerProb = 5;		// Out of 20
            int freekickTakerProb = 3;
            int cornerTakerProb = 4;
            int throwerTakerProb = 2;

            player.ATTR_PENALTY = _random.Next(1, 20) <= penTakerProb ? _random.Next(14, 20) : _random.Next(3, 14);
            player.ATTR_FREE_KICKS = _random.Next(1, 20) <= freekickTakerProb ? _random.Next(14, 20) : _random.Next(3, 14);
            player.ATTR_CORNERS = _random.Next(1, 20) <= cornerTakerProb ? _random.Next(14, 20) : _random.Next(3, 14);
            player.ATTR_THROWINS = _random.Next(1, 20) <= throwerTakerProb ? _random.Next(14, 20) : _random.Next(3, 14);
            //----

            // Generate the minimum attributes
            List<AttributeRange> minAttrForFieldPos = Constants.AttributesRankePerFieldPostions[fieldPos];
            foreach (var attrFieldPos in minAttrForFieldPos)
            {
                player.SetAttribute(attrFieldPos.PlayerAttribute, _random.Next(attrFieldPos.Min, attrFieldPos.Max));
            }
            //---------

            // Randomly distribute the rest of attributes
            //ATTR_NUM_ATTRIBUTES??
            foreach (var playerAttribute in Enum.GetValues(typeof(PlayerAttribute)))
            {
                attributesSum -= player.GetAttribute((PlayerAttribute)playerAttribute);
            }
            //     System.Diagnostics.Debug.Assert(attributesSum > 20);	// We expect to remain with something after setting the min attributes..

            var numberOfAttributes = Enum.GetValues(typeof(PlayerAttribute)).Length;
            for (int i = 0; i < attributesSum; i++)
            {
                PlayerAttribute playerAttr = (PlayerAttribute)_random.Next(0, numberOfAttributes - 1);	// TODO: Should we have different probabilities for attributes ??
                player.SetAttribute(playerAttr, player.GetAttribute(playerAttr) + 1);
            }
            //---------

            //// Make all attributes between [0,1]
            //for (int i = 0; i < ATTR_NUM_ATTRIBUTES; i++)
            //{
            //    PlayerAttribute playerAttr = (PlayerAttribute)i;
            //    player.SetAttribute(playerAttr, player.GetAttribute(playerAttr) / _maxAttrValueAtGeneration);
            //    if (player.GetAttribute(playerAttr) > 1)
            //    {
            //        player.SetAttribute(playerAttr, 1); // TODO: need to keep an eye on this...
            //    }
            //}
            ////---------
        }
    }
}
