using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FMGUI
{
	public class MenuPage_NewUser : MenuPage
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
			Application.LoadLevel (MenuManager.Instance.GetSceneNameForPage(MenuPages.PAGE_NEW_USER));
		}
		
		public override void OnEnter()
		{
			SetCreateFailedTextState (false, "");
		}
		
		public override void OnHide()
		{
			
		}

		private static void SetCreateFailedTextState(bool visible, string reasonText)
		{
			GameObject createFailedGO = GameObject.Find ("Label_failcreate");						
			Text loginFailedLabel		= createFailedGO.GetComponent<Text> ();
			loginFailedLabel.enabled 	= visible;

			loginFailedLabel.text 		= "Can't create account. " + reasonText;
		}

		public static void OnNewUserButtonPressed()
		{
			GameObject userNameGO 		= GameObject.Find ("Input_UserName");
			InputField userNameInput 	= userNameGO.GetComponentInChildren<InputField> ();
			
			GameObject passwordGO    	= GameObject.Find ("Input_Password");
			InputField passwordInput 	= passwordGO.GetComponentInChildren<InputField> ();

			GameObject mailGO    		= GameObject.Find ("Input_Mail");
			InputField mailInput 		= mailGO.GetComponentInChildren<InputField> ();
			
			// TODO: Call here login
			Debug.Log ("New user: " + "user: " + userNameInput.text + " pass: " + passwordInput.text + " mail: " + mailInput.text);
			CommonMessages.UserConnectedDataMsg userTeam = new CommonMessages.UserConnectedDataMsg ();
			Gameplay.GameDatabase.Instance.CreateNewOnlineGame (userTeam);


			OnCreateUserSuccess ();
		}

		public static void OnCreateUserSuccess()
		{
			// TODO !!
			MenuManager.Instance.OpenNewPage (MenuPages.PAGE_MAIN_MENU, true);
		}

		public static void OnCreateUserFailed()
		{
			// TODO !!
			SetCreateFailedTextState (true, "Reason: esti un bou");
		}
	}
}

