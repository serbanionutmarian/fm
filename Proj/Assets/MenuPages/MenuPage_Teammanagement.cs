using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FMGUI
{
	public class MenuPage_TeamManagement : MenuPage
	{
		int mFormationIdWaiting = -1;
		bool mIsPageReady 		= false;

		public override void OnOpened ()
		{
			OnEnter ();
		}
		
		public override void OnClosed()
		{
			OnHide ();
		}
		
		public override void OnRequested()
		{
			mIsPageReady = false;
			Application.LoadLevel (MenuManager.Instance.GetSceneNameForPage(MenuPages.PAGE_TEAMMANAGEMENT));
		}
		
		public override void OnEnter()
		{
			mIsPageReady = true;

			if (mFormationIdWaiting != -1)
			{
				OnFormationChanged(mFormationIdWaiting);
				mFormationIdWaiting = -1;
			}
		}
		
		public override void OnHide()
		{
			
		}
				
		public override void OnBack()
		{
			// Save the tactics, instructions and selection changes !
			CommonMessages.ToServerMessages.SendTacticsUpdateMsg ();
		}

		public void OnFormationChangedMsg(int formationId)
		{
			if (mIsPageReady)
			{
				OnFormationChanged(formationId);
			}
			else
			{
				mFormationIdWaiting = formationId;
			}
		}

		public void OnFormationChanged(int formationId)
		{
			// TODO
			Debug.Log ("OnFormationChanged " + formationId);
			Gameplay.Club userClub 					= Gameplay.GameDatabase.Instance.GetLocalUserClub();
			userClub.mTactics.mCurrentTacticIndex 	= formationId;
		}
	}
}
