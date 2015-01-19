using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Gameplay
{
	public class DataGenerator
	{
		//---------------------------------------------------  LEAGUES GENERATION / UPDATE -------------------------------------------------------
		// Generate a league			( intervalsNotPlaying[i][0]-intervalsNotPlaying[i][1]  represents interval i when the league is suspended
		// give dates as day / month not years to be generic. years should always be 0 !
		static public League GenerateLeague(int divisionIdx, int leagueIdx, String name, Club[] clubs, DateTime startDate, DateTime[][] intervalsNotPlaying, int optimalDaysBetweenMatches)
		{
			int numTeams = clubs.Length;

			if (numTeams % 2 != 0)
			{
				System.Diagnostics.Debug.Assert(false);
				return null;
			}

			// Name
			League newLeague = new League ();
			newLeague.mCompName = name;
			newLeague.mDivisionId = divisionIdx;
			newLeague.mLeagueId = leagueIdx;

			// Allocate rounds, matches
			//------------
			int numRounds = (numTeams - 1) << 1;
			newLeague.mRounds = new LeagueRound[numRounds];
			int matchesPerRound = numTeams << 1;
			for (int i = 0; i < numRounds; i++) 
			{
				newLeague.mRounds [i].mMatches = new LeagueRoundMatch[matchesPerRound];
				for (int matchIdx = 0; matchIdx < matchesPerRound; matchIdx++)
				{
					newLeague.mRounds[i].mMatches[matchIdx].matchIdx = matchIdx;
				}
			}
			//------------

			// Establish maches per each round
			//--------------

			newLeague.mClubsId = new int[numTeams];
			for (int i = 0; i < numTeams; i++) 
			{
				newLeague.mClubsId[i] = clubs[i].mClubId;
				clubs[i].mDivisionId = divisionIdx;;
				clubs[i].mLeagueId = leagueIdx;
			}

			newLeague.ScheduleMatches(startDate, optimalDaysBetweenMatches, intervalsNotPlaying);
			return newLeague;
		}

		static public void UpdateLeague(League leagueToUpdate, int[] clubIdsToRemove, int[] clubIdsToAdd, DateTime startDate, int optimalDaysBetweenMatches, DateTime[][] intervalsNotPlaying)
		{
			if (clubIdsToAdd.Length != clubIdsToRemove.Length || clubIdsToAdd.Length < 1)
			{
				System.Diagnostics.Debug.Assert(false);
				return;
			}

			// We could sort both mClubIds and clubIdsToRemove to get O(n+m) but it doesn't worth because clubIdsToRemove has 2-4 components...
			int nextClubIdxToAdd = 0;
			for (int i = 0; i < leagueToUpdate.mClubsId.Length; i++)
			{
				bool shouldRemove = false;
				for (int j = 0; j < clubIdsToRemove.Length; j++)
				{
					if (leagueToUpdate.mClubsId[i] == clubIdsToAdd[j])
					{
						shouldRemove = true;
						break;
					}
				}

				if (shouldRemove)
				{
					leagueToUpdate.mClubsId[i] = clubIdsToAdd[nextClubIdxToAdd];
					nextClubIdxToAdd++;
					if (nextClubIdxToAdd == clubIdsToAdd.Length)
					{
						break;
					}
				}
			}

			leagueToUpdate.ScheduleMatches(startDate, optimalDaysBetweenMatches, intervalsNotPlaying);
		}

		static int[] mNumPlayersPerFieldPos = new int[(int)FieldPos.FIELD_POS_NUM] { 3, 8, 10, 4 };
		static int[][] mTotalAttributesOfAPlayer = new int[CompetitionEngine.mNumDivisions][]
		{  
			new int[2] { 300, 550 },
			new int[2] { 250, 370 },
			new int[2] { 170, 300 },
		};

		struct AttributeRange
		{
			public PlayerAttribute mAttrib;
			public int             mMin;
			public int             mMax;
			
			public AttributeRange (PlayerAttribute playerAttr, int min, int max)
			{
				mAttrib = playerAttr;
				mMin = min;
				mMax = max;
			}
		}

		static public int mMaxAttrValueAtGeneration = 20; // We use this to generate better smaller values but then we divide by this value and get values in [0,1]
		static AttributeRange[][] mAttributesRangePerFieldPos = new AttributeRange[(int)FieldPos.FIELD_POS_NUM][]
		{  
			// GK
			new AttributeRange[] 
			{
				new AttributeRange(PlayerAttribute.ATTR_HANDLING,			8,13),
				new AttributeRange(PlayerAttribute.ATTR_REFLEXES,			8,13),
			},
			
			// DEF
			new AttributeRange[] 
			{
				new AttributeRange(PlayerAttribute.ATTR_TACKLING,			8,12),
				new AttributeRange(PlayerAttribute.ATTR_MARKING, 			5,10),
				new AttributeRange(PlayerAttribute.ATTR_HEADING, 			5,12),
				new AttributeRange(PlayerAttribute.ATTR_SPEED,   			4,8),
				new AttributeRange(PlayerAttribute.ATTR_POSITIONING, 		5,12),
			},
			
			// MID
			new AttributeRange[] 
			{
				new AttributeRange(PlayerAttribute.ATTR_PASSING,			8,12),
				new AttributeRange(PlayerAttribute.ATTR_TEAMWORK,			8,12),
				new AttributeRange(PlayerAttribute.ATTR_CROSSING,			5,12),
				new AttributeRange(PlayerAttribute.ATTR_CREATIVITY,			5,12),
				new AttributeRange(PlayerAttribute.ATTR_SPEED,				8,12),
				new AttributeRange(PlayerAttribute.ATTR_ACCELERATION,		8,12),
			},
			
			// ATT
			new AttributeRange[] 
			{
				new AttributeRange(PlayerAttribute.ATTR_FINISHING,			8,12),
				new AttributeRange(PlayerAttribute.ATTR_LONG_SHOTS,			4,7),
				new AttributeRange(PlayerAttribute.ATTR_DRIBBLING,			6,9),
				new AttributeRange(PlayerAttribute.ATTR_TECHNIQUE,			5,9),
				new AttributeRange(PlayerAttribute.ATTR_OFF_THE_BALL,		3,8),
				new AttributeRange(PlayerAttribute.ATTR_SPEED,				5,10),
				new AttributeRange(PlayerAttribute.ATTR_ACCELERATION,		5,10),
				new AttributeRange(PlayerAttribute.ATTR_STRENGTH,			5,10),
				new AttributeRange(PlayerAttribute.ATTR_HEADING,			5,10),
				new AttributeRange(PlayerAttribute.ATTR_POSITIONING,		5,10),
			},
		};

		static public string GenerateRandomClubName() { return "TODO"; }
		static public string GenerateRandomManagerOrPlayerName() { return "TODO"; }

		static public void GenerateDataForClub(Club club, int division)
		{
			club.mDivisionId = division;
			club.mClubName = GenerateRandomClubName ();
			club.mManagerName = GenerateRandomManagerOrPlayerName ();

			// Generate all players for this team
			//----------------------------------
			club.mTeam.Reset ();

			int numTotalPlayers = 0;
			for (int i = 0; i < (int)FieldPos.FIELD_POS_NUM; i++)
			{
				numTotalPlayers += mNumPlayersPerFieldPos[i];
			}

			if (numTotalPlayers != CompetitionEngine.mAvgPlayersPerTeam)
			{
				System.Diagnostics.Debug.Assert(false); // Please solve this assert
			}

			for (int i = 0; i < numTotalPlayers; i++)
			{
				Player newPlayer = GameDatabase.Instance.mPlayersPool.AddPlayer();
				club.mTeam.mAllPlayers.Add(newPlayer.mPlayerId);				
			}

			int playerIdxToFill = 0;
			for (int fieldIter = 0; fieldIter < (int)FieldPos.FIELD_POS_NUM; fieldIter++)
			{
				for (int playerIter = 0; playerIter < mNumPlayersPerFieldPos[fieldIter]; playerIter++)
				{
					int playerId = club.mTeam.mAllPlayers[playerIdxToFill];
					Player player = GameDatabase.Instance.mPlayersPool.GetPlayer(playerId);
					DataGenerator.GenerateDataForPlayer(division, player, (FieldPos)fieldIter);
					player.mClubId = club.mClubId;
					playerIdxToFill++;
				}
			}

			club.mTeam.SelectBestTeam ();
			//-----------------------------------
		}

		static public void GenerateDataForPlayer(int division, Player player, FieldPos fieldPos)
		{
			// Name and birthdate
			//-------------------
			player.mName 			= GenerateRandomManagerOrPlayerName ();		
			DateTime currentDate 	= GameDatabase.Instance.mCurrentTime;

			int age 				= GameDatabase.Instance.mRandomGenerator.Next (18, 30);
			int month 				= GameDatabase.Instance.mRandomGenerator.Next (1, 12);
			int day  				= GameDatabase.Instance.mRandomGenerator.Next (1, 28);
			player.mBornDate 		= new DateTime (currentDate.Year, currentDate.Month, currentDate.Day);

			DateTime tempDate 		= new DateTime(age, month, day);
			player.mBornDate.Subtract (tempDate);
			//-------------------

			// Position, sides 
			player.mFieldPos 		= fieldPos;
			player.mFieldSide     	= (FieldSide)    GameDatabase.Instance.mRandomGenerator.Next (0, (int)FieldSide.FIELD_SIDE_NUM - 1);

			if (player.mFieldPos == FieldPos.FIELD_POS_MID)
			{
				player.mMidfielderPos = (MidfielderPos)GameDatabase.Instance.mRandomGenerator.Next (0, (int)MidfielderPos.MID_POS_NUM - 1);
			}
			//----------------

			// Generates attributes
			int attributesSum = GameDatabase.Instance.mRandomGenerator.Next (mTotalAttributesOfAPlayer[division][0], mTotalAttributesOfAPlayer[division][1]);
			GenerateAttributesForPlayer(player, fieldPos, attributesSum);
		}

		static void GenerateAttributesForPlayer(Player player, FieldPos fieldPos, int attributesSum)
		{
			System.Random rndGen = GameDatabase.Instance.mRandomGenerator;

			// Max morale
			player.mAttributes.SetAttr (PlayerAttribute.ATTR_MORALE, mMaxAttrValueAtGeneration);

			// Avg form
			player.mAttributes.SetAttr (PlayerAttribute.ATTR_FORM, mMaxAttrValueAtGeneration);

			// Leadership, Tenacity - 1/8 to be a good leader
			int leaderProb = rndGen.Next (1, 8);
			if (leaderProb == 1)
			{
				player.mAttributes.SetAttr (PlayerAttribute.ATTR_LEADERSHIP, rndGen.Next(10, 20));
				player.mAttributes.SetAttr (PlayerAttribute.ATTR_TENACITY,   rndGen.Next(10, 20));
			}
			//----

			// Set pieces
			int penTakerProb = 5;		// Out of 20
			int freekickTakerProb = 3;
			int cornerTakerProb = 4;
			int throwerTakerProb = 2;

			player.mAttributes.SetAttr(PlayerAttribute.ATTR_PENALTY, (rndGen.Next(1, 20) <= penTakerProb ?  		rndGen.Next(14, 20) : rndGen.Next(3, 14)));
			player.mAttributes.SetAttr(PlayerAttribute.ATTR_FREE_KICKS, (rndGen.Next(1, 20) <= freekickTakerProb ?  rndGen.Next(14, 20) : rndGen.Next(3, 14)));
			player.mAttributes.SetAttr(PlayerAttribute.ATTR_CORNERS, (rndGen.Next(1, 20) <= cornerTakerProb ?  		rndGen.Next(14, 20) : rndGen.Next(3, 14)));
			player.mAttributes.SetAttr(PlayerAttribute.ATTR_THROWINS, (rndGen.Next(1, 20) <= throwerTakerProb ?  	rndGen.Next(14, 20) : rndGen.Next(3, 14)));
			//----

			// Generate the minimum attributes
			AttributeRange[] minAttrForFieldPos = mAttributesRangePerFieldPos [(int)fieldPos];
			for (int i = 0; i < minAttrForFieldPos.Length; i++)
			{
				AttributeRange range = minAttrForFieldPos[i];
				player.mAttributes.SetAttr(range.mAttrib, rndGen.Next(range.mMin, range.mMax));
			}
			//---------

			// Randomly distribute the rest of attributes
			for (int i = 0; i < (int)PlayerAttribute.ATTR_NUM_ATTRIBUTES; i++)
			{
				PlayerAttribute playerAttr = (PlayerAttribute)i;
				attributesSum -= (int)player.mAttributes.GetAttr(playerAttr);
			}
			System.Diagnostics.Debug.Assert (attributesSum > 20);	// We expect to remain with something after setting the min attributes..

			for (int i = 0; i < attributesSum; i++)
			{
				PlayerAttribute playerAttr = (PlayerAttribute)rndGen.Next(0, (int)PlayerAttribute.ATTR_NUM_ATTRIBUTES - 1);	// TODO: Should we have different probabilities for attributes ??
				player.mAttributes.SetAttr(playerAttr, player.mAttributes.GetAttr(playerAttr) + 1);
			}
			//---------

			// Make all attributes between [0,1]
			for (int i = 0; i < (int)PlayerAttribute.ATTR_NUM_ATTRIBUTES; i++)
			{
				PlayerAttribute playerAttr = (PlayerAttribute)i;
				player.mAttributes.SetAttr(playerAttr, player.mAttributes.GetAttr(playerAttr) / mMaxAttrValueAtGeneration);
				if (player.mAttributes.GetAttr(playerAttr) > 1.0f)
				{
					player.mAttributes.SetAttr(playerAttr, 1.0f); // TODO: need to keep an eye on this...
				}
			}
			//---------
		}
		//------------------------------------------------------------------------------------------------------------
	}
}


