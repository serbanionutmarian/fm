using DataModel.Schedule;
using DataService.Interfaces;
using Repository;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataService.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SchedulerService(IUnitOfWork unitOfWork,
            ISeriesRepository seriesRepository,
            ITeamRepository teamRepository,
            IMatchRepository matchRepository)
        {
            _unitOfWork = unitOfWork;
            _seriesRepository = seriesRepository;
            _teamRepository = teamRepository;
            _matchRepository = matchRepository;
        }

        public void AllTableMatches()
        {
            using (var transaction = new TransactionScope())
            {
                foreach (var series in _seriesRepository.GetAll())
                {
                    AllTableSeries(series.Id);
                }
                transaction.Complete();
            }
        }

        private void AllTableSeries(int seriesId)
        {
            var teams = _teamRepository.GetAllBySeriesId(seriesId).ToArray();
            var rounds = ScheduleTableAlgorithm(teams);
            // _matchRepository.DeleteAll();
            foreach (var round in rounds)
            {
                foreach (var match in round.Matches)
                {
                    _matchRepository.Add(new DataModel.Tables.Match()
                    {
                        HomeTeamId = match.Home.Id,
                        AwayTeamId = match.Away.Id,
                        MatchType = MatchType.League
                    });
                }
                _unitOfWork.SaveChanges();
            }
        }
   

        private Round<T>[] ScheduleTableAlgorithm<T>(T[] teams)
        {
            int nrOfTeams = teams.Length;

            //random teams
            var random = new Random();
            for (int i = 0; i < nrOfTeams; i++)
            {
                var temp = teams[i];
                var randomNr = random.Next(nrOfTeams);
                teams[i] = teams[randomNr];
                teams[randomNr] = temp;
            }

            int nrOfRounds = (nrOfTeams - 1) << 1;
            int nrOfMatchesPerRound = nrOfTeams >> 1;
            var rounds = new Round<T>[nrOfRounds];

            var nextMonth = DateTime.Now.AddDays(2);
            var useDays = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month) - 1;
            for (int roundNr = 0; roundNr < nrOfRounds / 2; roundNr++)
            {
                // Split in two and get rounds
                rounds[roundNr] = new Round<T>();
                var currentRound = rounds[roundNr];
                currentRound.Matches = new Round<T>.Match<T>[nrOfMatchesPerRound];
                for (int matchNr = 0; matchNr < nrOfMatchesPerRound; matchNr++)
                {
                    T home = teams[matchNr];
                    T away = teams[matchNr + nrOfMatchesPerRound];
                    rounds[roundNr].Matches[matchNr] = new Round<T>.Match<T>();
                    if (roundNr % 2 == 0)	// One match home, the other one away
                    {

                        currentRound.Matches[matchNr].Home = home;
                        currentRound.Matches[matchNr].Away = away;
                    }
                    else
                    {
                        currentRound.Matches[matchNr].Home = away;
                        currentRound.Matches[matchNr].Away = home;
                    }
                }

                // rotate objects than pivot (first component)
                T temp = teams[nrOfTeams / 2];
                for (int i = nrOfTeams / 2; i < nrOfTeams - 1; i++)
                {
                    teams[i] = teams[i + 1];
                }
                teams[nrOfTeams - 1] = teams[nrOfTeams / 2 - 1];
                for (int i = nrOfTeams / 2 - 1; i > 1; i--)
                {
                    teams[i] = teams[i - 1];
                }
                teams[1] = temp;
            }
            // Double-leg
            for (int roundNr = nrOfRounds / 2; roundNr < nrOfRounds; roundNr++)
            {
                var first = rounds[roundNr - nrOfRounds / 2];

                rounds[roundNr] = new Round<T>();
                var currentRound = rounds[roundNr];
                currentRound.Matches = new Round<T>.Match<T>[nrOfMatchesPerRound];

                for (int matchNr = 0; matchNr < nrOfMatchesPerRound; matchNr++)
                {
                    currentRound.Matches[matchNr] = new Round<T>.Match<T>();
                    currentRound.Matches[matchNr].Home = first.Matches[matchNr].Away;
                    currentRound.Matches[matchNr].Away = first.Matches[matchNr].Home;
                }
            }
            return rounds;
        }
    }
}
