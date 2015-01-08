using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Generic.Dictonary;
using System;

namespace Gameplay
{
	public enum CompetitionType
	{
		COMP_TYPE_NONE,
		COMP_TYPE_KNOCKOUT,
		COMP_TYPE_LEAGUE,
	}


	public abstract class Competition
	{
		protected CompetitionType  mCompType;	// Type
		public 	String 			   mCompName;	// Competition name

		public abstract bool IsCompetitionEnded (int clubId);
		public abstract DateTime GetDateOfNextRound (int clubId);

		public Competition()
		{
			mCompType = CompetitionType.COMP_TYPE_NONE;
			mCompName = "Not set";
		}
	}

	public class Cup: Competition
	{
		// TODO !
		public override bool IsCompetitionEnded (int clubId) { return true; }
		public override DateTime GetDateOfNextRound (int clubId) { return DateTime.Now; }
	}

	public class LeagueRoundMatch
	{
		public int mTeamIdA;
		public int mTeamIdB;		// team ids

		public int mResultA;		// Num goals scoared per each team
		public int mResultB;			

		public bool mPlayed;		// True if match was played

		public int matchIdx;				// For fast identification in the match table

		public LeagueRoundMatch()
		{
			mTeamIdA = mTeamIdB = 0;
			mResultA = mResultB = -1;
			mPlayed = false;
			matchIdx = -1;
		}
	}

	public class LeagueRound
	{
		public DateTime	mDate;				// Date when this round will be played
		public LeagueRoundMatch[] mMatches;	//Teams and results for all matches in this round

		LeagueRound()
		{
			mMatches = null;
			mDate = DateTime.Today;
		}
	}

	public class League: Competition
	{
		
		static public DateTime[][] mIntervalsNotPlaying_Online = null;	// There are no pause in an online game
		static public DateTime[][] mIntervalsNotPlaying_Offline = new DateTime[2][] { 	new DateTime[] { new DateTime(0, 12, 10), new DateTime(0, 2, 10) },
																						new DateTime[] { new DateTime(0, 6, 20), new DateTime(0, 8, 20)  },																																									
																					};

		static public int mOptimalDaysBetweenMatches_Online = 1;	// 1 day between matches in an online game
		static public int mOptimalDaysBetweenMatches_Offline = 7;


		// In this version, I consider that rounds will be played in order with no delays or swaps
		public LeagueRound[] mRounds;
		public Dictionary<int, int> mNextRoundIdForClubId;
		public int mDivisionId;
		public int mLeagueId;	// Inside divisionId

		public int[] mClubsId;

		public League() 
		{
			mCompType = CompetitionType.COMP_TYPE_LEAGUE;
			mNextRoundIdForClubId.Clear ();

			mDivisionId = -1;
			mLeagueId = -1;
		}

		public override bool IsCompetitionEnded (int clubId)
		{
			if (!mNextRoundIdForClubId.ContainsKey(clubId))
			{
				return true;
			}

			int nextRoundId = mNextRoundIdForClubId [clubId];
			return nextRoundId >= mRounds.Length;
		}

		public override DateTime GetDateOfNextRound (int clubId)
		{
			if (IsCompetitionEnded(clubId))
			{
				return DateTime.MinValue;
			}

			int nextRoundId = mNextRoundIdForClubId [clubId];
			return mRounds[nextRoundId].mDate;
		}

		// Called after a match was played
		public void OnRoundMatchPlayed(LeagueRoundMatch matchPlayedInfo)	
		{
			if (IsCompetitionEnded(matchPlayedInfo.mTeamIdA) || IsCompetitionEnded(matchPlayedInfo.mTeamIdB))
			{
				System.Diagnostics.Debug.Assert(false);
				return;
			}
	
			int roundTeamA = mNextRoundIdForClubId [matchPlayedInfo.mTeamIdA];
			int roundTeamB = mNextRoundIdForClubId [matchPlayedInfo.mTeamIdB];
			if (roundTeamA != roundTeamB)
			{
				System.Diagnostics.Debug.Assert(false);
				return;
			}

			// Find the matchIdx
			LeagueRoundMatch leagueRoundMatch = mRounds [roundTeamA].mMatches [matchPlayedInfo.matchIdx];
			leagueRoundMatch.mResultA = matchPlayedInfo.mResultA;
			leagueRoundMatch.mResultB = matchPlayedInfo.mResultB;
			leagueRoundMatch.mPlayed = true;

			System.Diagnostics.Debug.Assert(leagueRoundMatch.mResultA >= 0 && leagueRoundMatch.mResultB >= 0);

			// Advance rounds for both teams
			mNextRoundIdForClubId [matchPlayedInfo.mTeamIdA]++;
			mNextRoundIdForClubId [matchPlayedInfo.mTeamIdB]++;
		}

		public void ScheduleMatches(DateTime startDate, int optimalDaysBetweenMatches, DateTime[][] intervalsNotPlaying)
		{
			int numTeams = mClubsId.Length;

			int numRounds = (numTeams - 1) << 1;
			int matchesPerRound = numTeams << 1;

			int[] clubsIds = new int[numTeams];	/// Using a linked list would have same complexity but less op 
			for (int i = 0; i < numTeams; i++) 
			{
				clubsIds[i]= mClubsId[i];				
			}
			
			for (int i = numTeams/2; i < numTeams/2 + numTeams/4; i++)	// Reverse the second half of the array
			{
				int temp = clubsIds[i];
				int swapIdx = (numTeams - 1) - i + numTeams/2;
				clubsIds[i] = clubsIds[swapIdx];
				clubsIds[swapIdx] = temp;
			}
			
			for (int roundIdx = 0; roundIdx < numRounds/2; roundIdx++)
			{
				// Split in two and get rounds
				for (int matchId = 0; matchId < matchesPerRound; matchId++)
				{
					int teamA = clubsIds[matchId];
					int teamB = clubsIds[matchId + matchesPerRound];
					
					if (roundIdx % 2 == 0)	// One match home, the other one away
					{
						mRounds[roundIdx].mMatches[matchId].mTeamIdA = teamA;
						mRounds[roundIdx].mMatches[matchId].mTeamIdB = teamB;
					}
					else
					{
						mRounds[roundIdx].mMatches[matchId].mTeamIdA = teamB;
						mRounds[roundIdx].mMatches[matchId].mTeamIdB = teamA;
					}
				}
				
				// rotate other than pivot (first component)
				int temp = clubsIds[numTeams/2];
				for (int i = numTeams/2; i < numTeams - 1; i++)
				{
					clubsIds[i] = clubsIds[i+1];
				}
				clubsIds[numTeams - 1] = clubsIds[numTeams/2 - 1];
				for (int i = numTeams/2 - 1; i > 1; i--)
				{
					clubsIds[i] = clubsIds[i-1];
				}
				clubsIds[1] = temp;
			}
			
			// Double-leg
			for (int roundIdx = numRounds/2; roundIdx < numRounds; roundIdx++)
			{
				LeagueRound second = mRounds[roundIdx];
				LeagueRound first = mRounds[roundIdx - numRounds/2];
				
				for (int matchId = 0; matchId < matchesPerRound; matchId++)
				{
					second.mMatches[matchId].mTeamIdA = first.mMatches[matchId].mTeamIdB;
					second.mMatches[matchId].mTeamIdB = first.mMatches[matchId].mTeamIdA;
				}
			}		
			//--------------
			
			// Set date for each round
			//--------------
			DateTime currentDate = startDate;
			int nextIntervalNotPlaying = 0;
			// Find first valid non-playing interval
			if (intervalsNotPlaying != null)
			{
				while (nextIntervalNotPlaying < intervalsNotPlaying.Length && 
				        (currentDate.Month < intervalsNotPlaying[nextIntervalNotPlaying][0].Month || 
				         (currentDate.Month == intervalsNotPlaying[nextIntervalNotPlaying][0].Month && currentDate.Day < intervalsNotPlaying[nextIntervalNotPlaying][0].Day)
				        )
				      ) 
				{
					nextIntervalNotPlaying++;
				}
			}			
			
			for (int i = 0; i < numRounds; i++) 
			{
				if (intervalsNotPlaying != null)
				{
					while(nextIntervalNotPlaying < intervalsNotPlaying.Length && 
					      Utils.IsDateBetweenDayMonth(currentDate, intervalsNotPlaying[nextIntervalNotPlaying][0], intervalsNotPlaying[nextIntervalNotPlaying][1]))
						
					{
						nextIntervalNotPlaying++;
						
						int monthsToAdd = intervalsNotPlaying[nextIntervalNotPlaying][1].Month - currentDate.Month;
						int daysToAdd = intervalsNotPlaying[nextIntervalNotPlaying][1].Day - currentDate.Day;
						
						System.Diagnostics.Debug.Assert(monthsToAdd > 0 || (monthsToAdd == 0 && daysToAdd >= 0));
						daysToAdd++;	// to go over restricted interval
						
						currentDate.AddMonths(monthsToAdd);
						currentDate.AddDays(daysToAdd);
					}
				}

				mRounds[i].mDate = currentDate;
				currentDate.AddDays(optimalDaysBetweenMatches);
			}
		}
	}		       

	public class CompetitionEngine		// TODO Online: this should be stored on the server and each client should receive at connection only the info needed in mLeagues/mCups
	{									// Users can take a team while a league/cup is in play
		// TODO Online and Offline: need to ensure that no matches are scheduled in the same day. Need a way to reschedule these

		public void AddLeague(League league, int divisionId, int leagueId)
		{
			mLeagues[divisionId][leagueId] = league;
		}

		public void AddCup(Cup cup)
		{
			mCups.Add (cup);
		}

		// Transfer window static functionality 
		//-----------------------------------------------------------------------------
		// 20 dec - 10 feb  |   15 ian - 1 sept
		public DateTime winterTransferWindow_Start = new DateTime(0, 12, 20);
		public DateTime winterTransferWindowEnd    = new DateTime(0, 2, 10);
		public DateTime fallTransferWindow_Start = new DateTime(0, 1, 15);
		public DateTime fallTransferWindowEnd    = new DateTime(0, 9, 1);
		
		public bool IsTransferWindowOpen(DateTime date)
		{
			if (GameDatabase.Instance.mIsOnlineMatch) 
			{
				return true;	// 
			}
			else
			{
				bool isInWinterWindow =  Utils.IsDateBetweenDayMonth(date, winterTransferWindow_Start, winterTransferWindowEnd);
				bool isInFallWindow   =  Utils.IsDateBetweenDayMonth(date, fallTransferWindow_Start, fallTransferWindowEnd);
				
				return (isInFallWindow || isInWinterWindow);
			}				
		}
		//-----------------------------------------------------------------------------

		public DateTime GetNextMatchDateForClub(Club club)
		{
			DateTime nextMatch = DateTime.MaxValue;

			DateTime matchDate = mLeagues[club.mDivisionId][club.mLeagueId].GetDateOfNextRound(club.mClubId);
			if (nextMatch > matchDate)
			{
				nextMatch = matchDate;
			}

			foreach (Cup cup in mCups)
			{
				DateTime matchDateCup = cup.GetDateOfNextRound(club.mClubId);
				if (nextMatch > matchDateCup)
				{
					nextMatch = matchDateCup;
				}
			}

			return nextMatch;
		}

		public CompetitionEngine()
		{
			mLeagues = null;
			mCups.Clear ();
		}

		// Data:
		public const  int mNumDivisions = 3;
		public static int mNumBranchingFactor = 2;	// Each league in an upper division will generate mNumBranchingFactor leagues in the below level division
		public static int mNumTeamsPerLeague = 12;
		public static int mAvgPlayersPerTeam = 25;	// This is the initial number of players that each team has

		public League[][] mLeagues;
		public List<Cup>  mCups;
	}
}		                    



