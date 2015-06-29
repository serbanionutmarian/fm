using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Gameplay
{
	public class GameDatabase 					// TODO Online: in an online game this should be stored totally on server. A part of this will be also stored on client
	{
		public DateTime mCurrentTime;			// TODO Online: In an online game this should be server time
		public bool mIsOnlineMatch = true;		// TODO Online: true
				

		// Clubs and players for Offline and online
		//##########################################################
		//--------- Offline case 
		public Club[] mClubs;			// This contains info about the clubs in the user's league.
										// TODO Online: how do we keep this updated ? Think that clubs can exchange players between them, or make a bigger stadium.
										// Also, do we really need all info from all other clubs or just a part ? in this case we can create another class like PartialClubInfo

		public PlayersPool mPlayersPool;	
		//--------- Online case
		// These will probably contain at most current user and the user which we are playing against
		public Hashtable mPlayersPool_Online = new Hashtable();
		public Hashtable mClubs_Online = new Hashtable();
		public Hashtable mUsers_Online = new Hashtable();
		//##########################################################

		public CompetitionEngine					mCompetitionEngine;			// TODO Online: check class def
		public CommonMessages.UserInfo				mLocalUserInfo;				// TODO Online: this must be taken from server not from a local file in an online game

		public System.Random	mRandomGenerator = new System.Random();

		GameDatabase() 
		{
			mCurrentTime = DateTime.Today;

			mClubs = null;
			mPlayersPool = new PlayersPool ();
			mLocalUserInfo = new CommonMessages.UserInfo ();
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
						DataGenerator.GenerateDataForClub(mClubs[clubGlobalIndex], divId, CountryId.CID_ROMANIA);
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
					                
		public void SelectClubForLocalUser(string userName, DateTime birthDate, int clubId, int userId)	// Both for Online and Offline
		{
			mLocalUserInfo = new CommonMessages.UserInfo ();

			// TODO Online/Offline: let user send his own start player here
			mLocalUserInfo.mName = userName;
			mLocalUserInfo.mClubId = clubId;
			mLocalUserInfo.mBirthDate = birthDate;
			mLocalUserInfo.mUserId = userId;

			// TODO Online: add these info on server...
		}

		public void CreateNewOfflineGame(string userName, string clubName, DateTime birthDate)
		{
			GameDatabase.Instance.mIsOnlineMatch = false;

			GenerateNewDatabase ();
			int clubId = GetRandomClubIdInDivision (CompetitionEngine.mNumDivisions - 1);	// Get a club in the last division
			mClubs [clubId].mClubName = clubName;
			mClubs [clubId].mManagerName = userName;
			SelectClubForLocalUser (userName, birthDate, clubId, 0);
		}

		public CommonMessages.UserInfo GetLocalUserInfo()
		{
			return mLocalUserInfo;
		}

		public Club GetLocalUserClub()
		{
			return GetClub (mLocalUserInfo.mUserId);
		}

		public Club GetClub(int clubId)
		{
			if (mIsOnlineMatch)
			{
				if (!mClubs_Online.ContainsKey(clubId))
				{
					// Should assert
					return null;
				}

				return (Club)mClubs_Online[clubId];
			}
			else
			{
				// TODO
				return mClubs[clubId];
			}
		}

		public Player GetPlayer(int playerId)
		{
			if (mIsOnlineMatch)
			{
				if (!mPlayersPool_Online.ContainsKey(playerId))
				{
					// Should assert
					return null;
				}
				
				return (Player)mPlayersPool_Online[playerId];
			}
			else
			{
				// TODO
				return mPlayersPool.GetPlayer(playerId);
			}
		}

		/// <summary>
		/// Online functionality
		/// </summary>
		/// <param name="userTeam">User team.</param>
		public void CreateNewOnlineGame(CommonMessages.UserConnectedDataMsg userTeam)
		{
			GameDatabase.Instance.mIsOnlineMatch = true;
			CommonMessages.UserInfo userInfo = userTeam.mUserInfo;
			SelectClubForLocalUser (userInfo.mName, userInfo.mBirthDate, userInfo.mClubId, userInfo.mUserId);

			LoadUser (userTeam);
		}

		public CommonMessages.UserInfo GetUserInfo(int userId)
		{
			if (mIsOnlineMatch)
			{
				if (!mUsers_Online.ContainsKey(userId))
				{
					// Should assert
					return null;
				}
				
				return (CommonMessages.UserInfo)mUsers_Online[userId];
			}
			else
			{
				// Should assert - this function is valid only for client
				return null;
			}
		}

		public void LoadUser(CommonMessages.UserConnectedDataMsg userTeam)
		{
			if (userTeam == null || userTeam.mUserInfo == null || userTeam.mClubInfo == null)
			{
				// Must assert when system will be ready
				return;
			}

			if (mUsers_Online.ContainsKey(userTeam.mUserInfo.mUserId))
			{
				UnloadUser(userTeam.mUserInfo.mUserId);
			}

			// Add to  local cache
			//##################################################################
			// User info
			mUsers_Online.Add (userTeam.mUserInfo.mUserId, userTeam.mUserInfo);

			// Create a Club with the info received
			Club userClub = new Club ();
			userClub.mClubId = userTeam.mUserInfo.mClubId;
			userClub.mClubName = userTeam.mUserInfo.mClubName;
			userClub.mDivisionId = userTeam.mUserInfo.mDivisionId;
			userClub.mLeagueId = userTeam.mUserInfo.mLeagueId;
			userClub.mManagerName = userTeam.mUserInfo.mName;
			userClub.mTactics = userTeam.mClubInfo.mTactics;
			userClub.mFinances = null;
			userClub.mStadium = null;

			userClub.mTeam = new TeamController ();
			foreach (Player pl in userTeam.mClubInfo.mPlayersList)
			{
				userClub.mTeam.mAllPlayers.Add(pl.mPlayerId);
			}
			mClubs_Online.Add (userTeam.mUserInfo.mClubId, userClub);
			//----

			// Add all players here
			foreach (Player pl in userTeam.mClubInfo.mPlayersList)
			{
				mPlayersPool_Online.Add(pl.mPlayerId, pl);
			}
		}

		void UnloadUser(int userId)
		{
			if (mUsers_Online.ContainsKey(userId) == false)
			{
				// Should assert after system will be ready
				return;
			}

			CommonMessages.UserInfo userInfo = (CommonMessages.UserInfo) mUsers_Online [userId];
			int clubId = userInfo.mClubId;

			if (!mClubs_Online.ContainsKey(clubId))
			{
				// Should assert after system will be ready
				return ;
			}

			CommonMessages.ClubInfo clubInfo = (CommonMessages.ClubInfo)mClubs_Online [clubId];
			foreach (Player pl in clubInfo.mPlayersList)
			{
				mPlayersPool_Online.Remove(pl.mPlayerId);
			}

			mClubs_Online.Remove (clubId);
			mUsers_Online.Remove (userId);
		}

		// Before playing against another user team we need to get his info and load this and team locally
		// This data should be unloaded after the match is played
		public void OnBeforePlayOnlineGameAgainst(CommonMessages.UserConnectedDataMsg userTeam)
		{
			LoadUser (userTeam);
		}
		//-------------------------------------------------------------
	}
}



