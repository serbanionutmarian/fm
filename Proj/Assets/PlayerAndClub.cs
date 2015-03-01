using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Gameplay
{
	public enum PlayerAttribute
	{
		// ATTACKING
		ATTR_FINISHING,
		ATTR_LONG_SHOTS,
		ATTR_OFF_THE_BALL,
		ATTR_CREATIVITY,
		ATTR_TECHNIQUE,
		ATTR_DRIBBLING,
		ATTR_HEADING,
		
		// Defending
		//ATTR_HEADING
		ATTR_TACKLING,
		ATTR_MARKING,
		ATTR_AGGRESIVITY,
		ATTR_POSITIONING,
		
		// Set pieces
		ATTR_FREE_KICKS,
		ATTR_PENALTY,
		ATTR_CORNERS,
		ATTR_THROWINS,
		
		// Physics
		ATTR_AGILITY,
		ATTR_ACCELERATION,
		ATTR_SPEED,
		ATTR_STAMINA,
		ATTR_STRENGTH,
		ATTR_INJURY_PRONENESS,
		ATTR_BALANCE,
		
		// Construction
		ATTR_PASSING,
		ATTR_TEAMWORK,
		ATTR_CROSSING,
		//ATTR_OFF_THE_BALL
		
		// Mentality
		ATTR_LEADERSHIP,
		//ATTR_AGGRESIVITY
		//ATTR_TEAMWORK,
		ATTR_TENACITY,
		//ATTR_VISION
		//ATTR_CREATIVITY
		
		
		// GK
		ATTR_HANDLING,
		ATTR_REFLEXES,
		//ATTR_HEADING,
		//ATTR_PASSING,
		
		ATTR_CONDITION,	// 100% very good to play in a match, 50% not fit/too tired for a match
		ATTR_MORALE,	// 100% very good 

		// Hidden attributes
		ATTR_FORM,
		
		ATTR_NUM_ATTRIBUTES
	}

	public enum PlayerAttributesAverages
	{
		ATTR_AVG_GK,
		ATTR_AVG_DEF,
		ATTR_AVG_ATT,
		ATTR_AVG_MID,
		ATTR_AVG_PHYS,
		
		ATTR_AVG_NUM,
	};

	public enum StadiumType
	{
		STAD_TYPE_5000,
		STAD_TYPE_10000,
		STAD_TYPE_30000,
		STAD_TYPE_50000,
		STAD_TYPE_70000
	}

	public class PlayerAttributes
	{
		float[]  									mAttributes;		// all in [0-1]
		float[]										mCachedAvg;
		float 										mCachedPhys;
		bool										mCachedAvgDirty;
		
		public PlayerAttributes()
		{
			mCachedAvgDirty 		= true;
			mCachedPhys 			= 0.0f;
			
			mAttributes 			= new float[(int)PlayerAttribute.ATTR_NUM_ATTRIBUTES];
			mCachedAvg 				= new float[(int)PlayerAttributesAverages.ATTR_AVG_NUM];
			mAttributes.Initialize ();
		}
		
		public void UpdateAvg()
		{
			mCachedAvg[(int)PlayerAttributesAverages.ATTR_AVG_PHYS]=  (mAttributes [(int)PlayerAttribute.ATTR_AGILITY]    	* 0.10f + 
			                                                           mAttributes [(int)PlayerAttribute.ATTR_ACCELERATION] 	* 0.20f + 
			                                                           mAttributes [(int)PlayerAttribute.ATTR_SPEED]   		* 0.25f + 
			                                                           mAttributes [(int)PlayerAttribute.ATTR_STAMINA]      	* 0.10f +
			                                                           mAttributes [(int)PlayerAttribute.ATTR_STRENGTH]   	* 0.25f +
			                                                           mAttributes [(int)PlayerAttribute.ATTR_BALANCE]   	* 0.10f		                   
			                                                           );
			
			mCachedAvg[(int)PlayerAttributesAverages.ATTR_AVG_GK] =    (mAttributes [(int)PlayerAttribute.ATTR_HANDLING] 	* 0.60f + 
			                                                            mAttributes [(int)PlayerAttribute.ATTR_REFLEXES] 	* 0.30f + 
			                                                            mAttributes [(int)PlayerAttribute.ATTR_HEADING] 	* 0.05f + 
			                                                            mAttributes [(int)PlayerAttribute.ATTR_PASSING] 	* 0.05f
			                                                            );
			
			mCachedAvg[(int)PlayerAttributesAverages.ATTR_AVG_DEF]= 	(mAttributes [(int)PlayerAttribute.ATTR_TACKLING]     * 0.25f + 
			                                                          mAttributes [(int)PlayerAttribute.ATTR_MARKING]      * 0.25f + 
			                                                          mAttributes [(int)PlayerAttribute.ATTR_AGGRESIVITY]  * 0.10f + 
			                                                          mAttributes [(int)PlayerAttribute.ATTR_HEADING]      * 0.20f +
			                                                          mAttributes [(int)PlayerAttribute.ATTR_POSITIONING]  * 0.20f +
			                                                          mAttributes [(int)PlayerAttribute.ATTR_STRENGTH]	  * 0.20f
			                                                          );
			
			mCachedAvg[(int)PlayerAttributesAverages.ATTR_AVG_ATT]= 	(mAttributes [(int)PlayerAttribute.ATTR_FINISHING]    * 0.15f + 
			                                                          mAttributes [(int)PlayerAttribute.ATTR_LONG_SHOTS]   * 0.10f + 
			                                                          mAttributes [(int)PlayerAttribute.ATTR_OFF_THE_BALL] * 0.10f + 
			                                                          mAttributes [(int)PlayerAttribute.ATTR_CREATIVITY]   * 0.10f +
			                                                          mAttributes [(int)PlayerAttribute.ATTR_TECHNIQUE]    * 0.15f +
			                                                          mAttributes [(int)PlayerAttribute.ATTR_DRIBBLING]    * 0.15f +
			                                                          mAttributes [(int)PlayerAttribute.ATTR_HEADING]   	  * 0.10f +
			                                                          mCachedPhys										  * 0.15f
			                                                          );
			
			mCachedAvg[(int)PlayerAttributesAverages.ATTR_AVG_MID]= (  mAttributes [(int)PlayerAttribute.ATTR_PASSING]   	  * 0.30f + 
			                                                         mAttributes [(int)PlayerAttribute.ATTR_TEAMWORK]     * 0.10f + 
			                                                         mAttributes [(int)PlayerAttribute.ATTR_CROSSING]  	  * 0.5f + 
			                                                         mAttributes [(int)PlayerAttribute.ATTR_CREATIVITY]   * 0.15f +
			                                                         mAttributes [(int)PlayerAttribute.ATTR_TECHNIQUE]    * 0.5f +
			                                                         mAttributes [(int)PlayerAttribute.ATTR_DRIBBLING]    * 0.10f +
			                                                         mAttributes [(int)PlayerAttribute.ATTR_MARKING]   	  * 0.10f +
			                                                         mCachedPhys										  * 0.15f
			                                                         );
			
			mCachedAvgDirty    = false;
		}
		
		public void SetAttr(PlayerAttribute attr, float newValue)
		{
			mAttributes [(int)attr] = newValue;
			mCachedAvgDirty    = true;
		}
		
		public float GetAttr(PlayerAttribute attr)
		{
			return mAttributes [(int)attr];
		}
		
		public float GetAvg(PlayerAttributesAverages attrAvg) 
		{
			if (mCachedAvgDirty)
			{
				UpdateAvg();
			}
			
			return mCachedAvg[(int)attrAvg];
		}
	}

	public enum FieldPos
	{
		// DO NOT MODIFY THIS ORDER !! These are expected to be in this order at least in the attributes' ranges specification
		FIELD_POS_GK,
		FIELD_POS_DEF,
		FIELD_POS_MID,
		FIELD_POS_ATT,
		FIELD_POS_NUM,
	}

	public enum MidfielderPos
	{
		MID_POS_M,	// Midfielder line
		MID_POS_D,  // Defensive
		MID_POS_MD, // Both mid and def
		MID_POS_A,	// Attacking midfielder
		MID_POS_AM,	// Attacking and mid line
		MID_POS_NUM,
	}

	public enum FieldSide
	{
		FIELD_SIDE_L,		// Left
		FIELD_SIDE_R,		// Right
		FIELD_SIDE_C,		// Central
		FIELD_SIDE_LC,		// All others are combinations
		FIELD_SIDE_RC,
		FIELD_SIDE_RL,
		FIELD_SIDE_LRC,
		FIELD_SIDE_NUM,
	}

    enum GeneralFieldSide
    {
        GENERAL_FIELDSIDE_LEFT,
        GENERAL_FIELDSIDE_RIGHT,
        GENERAL_FIELDSIDE_CENTER,
        GENERAL_FIELDSIDE_NUM,
    }

	public class Player //: ISerializable
	{
		public PlayerAttributes mAttributes;		
		public string 			mName;
		public DateTime 		mBornDate;
		public FieldPos 		mFieldPos;
		public MidfielderPos    mMidfielderPos;	// This will be valid only if the mFieldPos = FIELD_POS_MID
		public FieldSide		mFieldSide;
		
		public int 				mPlayerId;	// Don't modify this. This will be modified on creation of a new player
		public int 				mClubId;
		public bool 			mIsValid;	// Because we have pre-allocated chuncks of players, we need a way to see if a player is active or not.


		public int GetAge()
		{
			return (int)((DateTime.Now - mBornDate).TotalDays/365);
		}
		
		public Player()
		{
			mName = "notset";

			mBornDate = DateTime.Now;
			mPlayerId = -1;
			mIsValid = false;

			mMidfielderPos = MidfielderPos.MID_POS_M;
			mFieldPos = FieldPos.FIELD_POS_MID;
			mFieldSide = FieldSide.FIELD_SIDE_C;
		}
	}

	public class TeamController
	{
		public List<int> mAllPlayers = new List<int>();	// All the players in this team

		public TeamController()
		{
			mAllPlayers.Capacity = 30;
		}

		public void Reset()
		{
			mAllPlayers.Clear ();
		}

		// TODO: ADD staff
		// ADD tactics
	}

	public class FinancesContoller
	{
		// TODO
	}

	public class StadiumController
	{
		public StadiumType mStadiumType;
		
		public StadiumController()
		{
			mStadiumType = StadiumType.STAD_TYPE_5000;
		}
	}

	public class Club
	{
		public string 				mClubName;
		public string 				mManagerName;
		
		// THese two will be filled when generating/updating the league where this club belong
		public int 					mDivisionId;	
		public int 					mLeagueId;		
		//-----
		
		// This will be set when creating a club. Don't modify this !!
		public int 					mClubId;
		//-----
		
		public TeamController 		mTeam;
		public TeamTactics			mTactics;
		public StadiumController	mStadium;
		public FinancesContoller 	mFinances;
		
		public Club()
		{
			mClubName = "notset";
			mManagerName = "notset";
			mClubId = -1;
			
			mDivisionId = -1;
			mLeagueId = -1;
			
			mTeam = new TeamController ();
			mStadium = new StadiumController ();
			mFinances = new FinancesContoller ();
			mTactics = new TeamTactics ();
		}
	}



	public class PlayersPool	// TODO Online : we need a table with all players. Then, we need to copy all players in this structure at connection (or at least the ones from the users's league)
	{
		// This contains an array with all players. All other instances contains only indices that refer to this array
		// The array must be greater than the number of players needed to have some buffer for new players...
		// When a plager retires, we make the component 
		public Player[] mPlayersArr;
		public int mNextPlayerIndex;
		public Stack<int> mRemovedIndices;	// I'm using a stack for a better cache memory usage
		
		public void AllocatePlayersArr(int numMaxPlayers)
		{
			mPlayersArr = new Player[numMaxPlayers];
			for (int i = 0; i < numMaxPlayers; i++)
			{
				mPlayersArr[i].mIsValid = false;
				mPlayersArr[i].mPlayerId = i;
			}
		}
		
		public Player AddPlayer()	// Use this when you want to create a new player
		{
			// First, try to reuse one of the removed components
			int indexToUse = -1;
			if (mRemovedIndices.Count > 0)
			{
				indexToUse = mRemovedIndices.Pop();
			}
			else if (mPlayersArr.Length <= mNextPlayerIndex)
			{
				indexToUse = mNextPlayerIndex;
				mNextPlayerIndex++;
				System.Diagnostics.Debug.Assert(false);	// No more indices, allocate a bigger chunck or reuse properly the deleted components...
			}
			
			if (indexToUse < 0 || indexToUse >= mPlayersArr.Length)
			{
				System.Diagnostics.Debug.Assert(false);	// No more indices, allocate a bigger chunck or reuse properly the deleted components...
				return null;
			}
			
			mPlayersArr [indexToUse].mIsValid = true;
			return mPlayersArr [indexToUse];
		}
		
		public bool RemovePlayer(int playerId)
		{
			if (playerId < 0 || playerId >= mPlayersArr.Length)
			{
				System.Diagnostics.Debug.Assert(false);
				return false;
			}
			
			mPlayersArr [playerId].mIsValid = false;
			mRemovedIndices.Push (playerId);
			return true;
		}
		
		public Player GetPlayer(int playerId)
		{
			if (playerId < 0 || playerId >= mPlayersArr.Length)
			{
				System.Diagnostics.Debug.Assert(false);
				return null;
			}
			
			return mPlayersArr[playerId];
		}
	}
}


