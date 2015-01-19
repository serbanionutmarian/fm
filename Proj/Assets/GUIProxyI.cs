using UnityEngine;
using System.Collections;
	
namespace FMGUI
{
	public class GUIProxyI : MonoBehaviour 
	{
		static bool hackActivated = false;

		// Use this for initialization
		void Start () 
		{
			MenuPages firstMenuPage = MenuPages.PAGE_LOGIN;

			// Terrible hack !!!
			if (Application.loadedLevel == 0)
			{
				MenuManager.Instance.OpenNewPage(firstMenuPage, true);
			}
			else
			{
				if (hackActivated == false)
				{
					hackActivated = true;
					firstMenuPage = MenuPages.PAGE_TEAMMANAGEMENT;	// Uncomment this to set your homepage fast
					MenuManager.Instance.OpenNewPage(firstMenuPage, true);
				}
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}

		void OnLevelWasLoaded(int level) 
		{
			//Debug.Log ("level : " + level);
			MenuManager.Instance.GetCurrentPage ().OnOpened ();
		}

		//-------------------------------
		// MenuPage_Login 
		//-------------------------------
		public void OnLoginButtonClick()
		{
			MenuPage_Login.OnLoginButtonPressed ();
		}

		public void OnNewUserButtonClick()
		{
			MenuPage_Login.OnNewUserButtonPressed ();
		}
		//-------------------------------

		public void OnGenericBackButton()	// For all pages call this func
		{
			MenuManager.Instance.GoBack ();
		}

		//-------------------------------
		// MenuPage_NewUser 
		//-------------------------------
		public void OnNewUserCreateButtonClick()
		{
			MenuPage_NewUser.OnNewUserButtonPressed ();
		}
		//-------------------------------


		//-------------------------------
		// MenuPage_MainMenu
		//-------------------------------
		public void OnTeamManagementClick()
		{
			MenuPage_MainMenu.OnTeamManagementPressed ();
		}
	}
}

