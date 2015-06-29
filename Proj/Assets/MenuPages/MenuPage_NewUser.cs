using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FMGUI
{
	public class MenuPage_NewUser : MenuPage
	{
		public ComboBox mCountriesCombo;
		GUIContent[]     mComboItems;

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

			PopulateContriesDropbox ();
		}

		private void PopulateContriesDropbox()
		{
			mComboItems = new GUIContent[(int)Gameplay.CountryId.CID_NUM];
			for (int i = 0; i < mComboItems.Length; i++)
			{
				mComboItems[i] = new GUIContent(Gameplay.Utils.GetCountryNameById((Gameplay.CountryId)i));
			}
			
			GUIStyle listStyle = new GUIStyle();
			listStyle.normal.textColor = Color.white; 
			listStyle.onHover.background =
				listStyle.hover.background = new Texture2D(2, 2);
			listStyle.padding.left =
				listStyle.padding.right =
					listStyle.padding.top =
					listStyle.padding.bottom = 4;
			
			listStyle.fixedHeight = 30;
			mCountriesCombo = new ComboBox(new Rect(450, 430, 100, 30), mComboItems[0], mComboItems, "button", "box", listStyle);
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

			MenuPage_NewUser thisMenuPage = (MenuPage_NewUser) MenuManager.mMenuPages[(int)MenuPages.PAGE_NEW_USER];
			Gameplay.CountryId countryId = (Gameplay.CountryId)thisMenuPage.mCountriesCombo.SelectedItemIndex; 
			
			// TODO: Call here login
			Debug.Log ("New user: " + "user: " + userNameInput.text + " pass: " + passwordInput.text + " mail: " + mailInput.text + countryId);
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

