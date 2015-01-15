using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Gameplay
{
	public class GameDatabase 			// TODO Online: in an online game this should be stored totally on server. A part of this will be also stored on client
	{
		public DateTime mCurrentTime;	// TODO Online: In an online game this should be server time
		public bool mIsOnlineMatch;		// TODO Online: true
				
		public Club[] mClubs;			// This contains info about the clubs in the user's league.
										// TODO Online: how do we keep this updated ? Think that clubs can exchange players between them, or make a bigger stadium.
										// Also, do we really need all info from all other clubs or just a part ? in this case we can create another class like PartialClubInfo

		public PlayersPool mPlayersPool;	

		public CompetitionEngine	mCompetitionEngine;			// TODO Online: check class def
		public LocalUserInfo		mLocalUserINfo;				// TODO Online: this must be taken from server not from a local file in an online game

		public System.Random	mRandomGenerator = new System.Random();

		GameDatabase() 
		{
			mCurrentTime = DateTime.Today;
			mIsOnlineMatch = false;

			mClubs = null;
			mPlayersPool = new PlayersPool ();
			mLocalUserINfo = new LocalUserInfo ();
			//mCompetitionEngine = new CompetitionEngine ();
		}

		private static readonly GameDatabase instance = new GameDatabase();
		public static GameDatabase Instance
		{
			get 
			{
				return instance;
			}
		}

		// Utils for transfer, time advancing, general gameplay stuff
		//-------------------------------------------------------------
		public bool IsTransferWindowOpen()
		{
			return mCompetitionEngine.IsTransferWindowOpen (GameDatabase.Instance.mCurrentTime);
		}

		public void AdvanceTime(Club club)	// For now, the parameter should be the user's team Id
		{
			DateTime nextMatchDate = mCompetitionEngine.GetNextMatchDateForClub (club);

			int daysToNextMatch = (nextMatchDate - mCurrentTime).Days;
			System.Diagnostics.Debug.Assert(daysToNextMatch >= 0);

			// TODO: Offline - we need to stop advancing time when an event occurs: injury, contract, transfer etc
			int daysToAdd = Math.Min (daysToNextMatch, 5);
			mCurrentTime.AddDays (daysToAdd);
		}
				
		// Called after a match to move to the next day
		public void AdvanceTimeAfterMatch()
		{
			mCurrentTime.AddDays (1);
		}

		// Check if there is any match today for a club
		public bool IsMatchTodayForTeam(Club club)
		{
			DateTime nextMatchDate = mCompetitionEngine.GetNextMatchDateForClub (club);
			int daysToNextMatch = (nextMatchDate - mCurrentTime).Days;
			System.Diagnostics.Debug.Assert(daysToNextMatch >= 0);

			return (nextMatchDate == mCurrentTime);
		}
		//-------------------------------------------------------------


		// Utils for creating/loading/saving game
		//-------------------------------------------------------------
		public void GenerateNewDatabase()	// Both for Offline and Online
		{
			int numTotalClubs = 0;
			for (int i = 0; i < CompetitionEngine.mNumDivisions; i++)
			{
				numTotalClubs += (int)Math.Pow(CompetitionEngine.mNumBranchingFactor, i);
			}
			numTotalClubs *= CompetitionEngine.mNumTeamsPerLeague;

			Club[] clubsInNewGeneratedLeague = new Club[CompetitionEngine.mNumTeamsPerLeague];	// temp buffer
			mClubs = new Club[numTotalClubs];
			int clubGlobalIndex = 0;

			mPlayersPool.AllocatePlayersArr(numTotalClubs * CompetitionEngine.mAvgPlayersPerTeam);

			for (int divId = 0; divId < CompetitionEngine.mNumDivisions; divId++)
			{
				int numLeaguesInThisDivision = (int)Math.Pow(CompetitionEngine.mNumBranchingFactor, divId);
				for (int leagueId = 0; leagueId < numLeaguesInThisDivision; leagueId++)
				{
					// Generate the clubs for this league
					for (int clubIdInLeague = 0; clubIdInLeague < CompetitionEngine.mNumTeamsPerLeague; clubIdInLeague++)
					{
						DataGenerator.GenerateDataForClub(mClubs[clubGlobalIndex], divId);
						mClubs[clubGlobalIndex].mClubId = clubGlobalIndex;
						clubsInNewGeneratedLeague[clubIdInLeague] = mClubs[clubGlobalIndex];
						clubGlobalIndex++;
					}

					DataGenerator.GenerateLeague(divId, leagueId, "League", clubsInNewGeneratedLeague, GameDatabase.Instance.mCurrentTime, 
					                             GameDatabase.Instance.mIsOnlineMatch ? League.mIntervalsNotPlaying_Online : League.mIntervalsNotPlaying_Offline,
					                             GameDatabase.Instance.mIsOnlineMatch ? League.mOptimalDaysBetweenMatches_Online : League.mOptimalDaysBetweenMatches_Offline);
				}
			}
		}

		public int GetRandomClubIdInDivision(int division)
		{
			if (division >= CompetitionEngine.mNumDivisions)
			{
				System.Diagnostics.Debug.Assert(false);
				division = CompetitionEngine.mNumDivisions - 1;
			}

			int indexStart = 0;
			int indexEnd = 0;
			for (int i = 0; i < division - 1; i++)
			{
				int numLeaguesInThisDivision = (int)Math.Pow(CompetitionEngine.mNumBranchingFactor, i);
				indexStart += (numLeaguesInThisDivision * CompetitionEngine.mNumTeamsPerLeague);
			}

			int numLeaguesInThisDiv = (int)Math.Pow(CompetitionEngine.mNumBranchingFactor, division);
			indexEnd = indexStart + (numLeaguesInThisDiv * CompetitionEngine.mNumTeamsPerLeague) - 1;

			int clubIdRand = mRandomGenerator.Next(indexStart, indexEnd);
			return clubIdRand;
		}
					                
		public void SelectClubForLocalUser(string userName, DateTime birthDate, int clubId)	// Both for Online and Offline
		{
			mLocalUserINfo = new LocalUserInfo ();

			// TODO Online/Offline: let user send his own start player here
			mLocalUserINfo.mName = userName;
			mLocalUserINfo.mClubId = clubId;
			mLocalUserINfo.mBirthDate = birthDate;

			// TODO Online: add these info on server...
		}

		public void CreateNewOfflineGame(string userName, string clubName, DateTime birthDate)
		{
			GameDatabase.Instance.mIsOnlineMatch = false;

			GenerateNewDatabase ();
			int clubId = GetRandomClubIdInDivision (CompetitionEngine.mNumDivisions - 1);	// Get a club in the last division
			mClubs [clubId].mClubName = clubName;
			mClubs [clubId].mManagerName = userName;
			SelectClubForLocalUser (userName, birthDate, clubId);
		}

		public void CreateNewOnlineGame(string userName, DateTime birthDate)
		{
			// TODO: the difference from the above is that we only need to call SelectClubForLocalUser 		
			// Also we need to handle clubName somehow!!! - it will be weird to modify the names of the clubs while in a competition...Maybe we'll apply the requested name at the begging of a new championship.

			GameDatabase.Instance.mIsOnlineMatch = true;
			int clubId = GetRandomClubIdInDivision (CompetitionEngine.mNumDivisions - 1);	// Get a club in the last division
			SelectClubForLocalUser (userName, birthDate, clubId);
		}
		//-------------------------------------------------------------
	}
}



