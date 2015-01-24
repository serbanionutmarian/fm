using UnityEngine;


namespace CommonMessages
{
	public class ToServerMessages
	{
		public static void SendTacticsUpdateMsg()
		{
			Gameplay.Club club = Gameplay.GameDatabase.Instance.GetLocalUserClub();

			// TODO: save these
			//club.mTactics.mCurrentTacticIndex
			//club.mTactics.mSelectedMatchPlayers
			//club.mTactics.mTeamInstructions

			Debug.Log ("Saved!");
		}
	}
}

