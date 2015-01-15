using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GUI
{
	public class MenuPage_TeamManagement : MenuPage
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
			Application.LoadLevel (MenuManager.Instance.GetSceneNameForPage(MenuPages.PAGE_TEAMMANAGEMENT));
		}
		
		public override void OnEnter()
		{
			
		}
		
		public override void OnHide()
		{
			
		}
	}
}
