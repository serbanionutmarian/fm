using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FMGUI
{

public class GameJumpScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		// Populate username, team name and set correctly the visibility of button 'Next'
		GameObject userNameGO 		= GameObject.Find ("Label_Username");
		Text userNameInput 			= userNameGO.GetComponentInChildren<Text> ();

		
		userNameInput.text 			= Gameplay.GameDatabase.Instance.mLocalUserInfo.mName + " " + 
									  Gameplay.GameDatabase.Instance.mLocalUserInfo.mClubName;
		
		GameObject buttonNextGO		= GameObject.Find ("Button_Next");
		Button  buttonNext			= buttonNextGO.GetComponentInChildren<Button> ();
		buttonNext.enabled 			= Gameplay.GameDatabase.Instance.mIsOnlineMatch == false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnJumpTomainMenu()
	{
		MenuManager.Instance.OpenNewPage (MenuPages.PAGE_MAIN_MENU, true);
		Debug.Log ("jumped");
	}

	public void OnLogout()
	{
		MenuManager.Instance.OpenNewPage(MenuPages.PAGE_LOGIN, true);
	}

	public void OnBack()
	{
		MenuManager.Instance.GetCurrentPage().OnBack();

		MenuManager.Instance.GoBack ();
	}
}




} // namespace FMGUI