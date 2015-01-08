using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GUI
{
	public class MenuPage_Login : MenuPage
	{
		public override void OnOpened ()
		{
			OnEnter ();
		}
		
		public override void OnClosed()
		{
			OnHide ();
		}
		
		public override void OnEnter()
		{
			SetLoginFailedTextState (false);
		}

		public override void OnRequested()
		{
			Application.LoadLevel (MenuManager.Instance.GetSceneNameForPage(MenuPages.PAGE_LOGIN));
		}
		
		public override void OnHide()
		{
			
		}

		private static void SetLoginFailedTextState(bool visible)
		{
			GameObject loginFailedGO = GameObject.Find ("Label_LoginFailed");//("");

			Text loginFailedLabel		= loginFailedGO.GetComponent<Text> ();
			loginFailedLabel.enabled 	= visible;
		}

		public static void OnLoginButtonPressed()
		{
			GameObject userNameGO = GameObject.Find ("Input_UserName");
			InputField userNameInput 	= userNameGO.GetComponentInChildren<InputField> ();

			GameObject passwordGO    	= GameObject.Find ("Input_Password");
			InputField passwordInput 	= passwordGO.GetComponentInChildren<InputField> ();

			// TODO: Call here login
			Debug.Log ("Login: " + "user: " + userNameInput.text + " pass: " + passwordInput.text);
			OnLoginFailed ();
		}

		public static void OnNewUserButtonPressed()
		{
			MenuManager.Instance.OpenNewPage (MenuPages.PAGE_NEW_USER, false);
		}

		public static void OnLoginFailed()
		{
			SetLoginFailedTextState (true);
		}
	}
}

