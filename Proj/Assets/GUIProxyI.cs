using UnityEngine;
using System.Collections;
	
namespace GUI
{
	public class GUIProxyI : MonoBehaviour 
	{
		// Use this for initialization
		void Start () 
		{
			// Terrible hack !!!
			if (Application.loadedLevel == 0)
			{
				MenuManager.Instance.OpenNewPage(MenuPages.PAGE_LOGIN, true);
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
	}
}

