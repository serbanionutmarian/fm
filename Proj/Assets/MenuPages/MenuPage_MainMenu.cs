using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FMGUI
{
	public class MenuPage_MainMenu : MenuPage
	{
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
			Application.LoadLevel (MenuManager.Instance.GetSceneNameForPage(MenuPages.PAGE_MAIN_MENU));
		}
		
		public override void OnEnter()
		{

		}
		
		public override void OnHide()
		{
			
		}

		public static void OnTeamManagementPressed()
		{
			MenuManager.Instance.OpenNewPage (MenuPages.PAGE_TEAMMANAGEMENT, false);
		}
	}
}
