using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonMessages
{


public class ClubInfo
{
	// Players and tactics of the user
	public List<Gameplay.Player> mPlayersList;
	public Gameplay.TeamTactics  mTactics; 

	public ClubInfo()
	{
		// TODO: these are only temporary to not crash when accessing data...should be deleted
		mPlayersList = new List<Gameplay.Player>();		
		mTactics = new Gameplay.TeamTactics();
	}
}

public class UserInfo
{
	public String 	mName;
	public DateTime mBirthDate;
	public int    	mClubId;
	public String 	mClubName;
	public int 		mUserId;
	public int 		mDivisionId;
	public int 		mLeagueId;
	// TODO: add other info: contract, rating etc
		
	public UserInfo()
	{
		mName = "Name not set";
		mClubName = "club not set";
		mBirthDate = DateTime.MinValue;
		mClubId = -1;
		mUserId = -1;
		mDivisionId = -1;
		mLeagueId = -1;
	}
}

public class UserConnectedDataMsg		// <<<<<---------------
{
	public UserInfo mUserInfo;
	public ClubInfo mClubInfo;

	public UserConnectedDataMsg()
	{
		// TODO: these are only temporary to not crash when accessing data...should be deleted
		mUserInfo = new UserInfo();			
		mClubInfo = new ClubInfo();
	}
}
	
} // namespace CommonMessages




