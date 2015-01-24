using UnityEngine;
using System.Collections;
using UnityEngine.UI;
	
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
				hackActivated = true;
				MenuManager.Instance.OpenNewPage(firstMenuPage, true);
			}
			else
			{
				if (hackActivated == false)
				{
					// Need this to load a team for user 
					//----
					// TODO: populate it from a file or something....
					CommonMessages.UserConnectedDataMsg userTeam = new CommonMessages.UserConnectedDataMsg ();
					Gameplay.GameDatabase.Instance.CreateNewOnlineGame (userTeam);
					//-----

					hackActivated = true;
					firstMenuPage = MenuPages.PAGE_MAIN_MENU;	// Uncomment this to set your homepage fast
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
		// MenuPage_TeamManagement
		//-------------------------------
		public void OnTeamManagementClick()
		{
			MenuPage_MainMenu.OnTeamManagementPressed ();
		}

		public void OnTeamInstructionsClick()
		{
			MenuManager.Instance.OpenNewPage(MenuPages.PAGE_TEAMINSTRUCTIONS, false);
		}
		//-------------------------------
		// Team instructions
		//-------------------------------
		static string[] mMentalityTexts 		= new string[]{"Ultra defensive", "Defensive", "Normal", "Attacking", "Ultra attacking"};
		static float[] mWeightsForMentality 	= new float[]{0.0f, 0.2f, 0.4f, 0.6f, 0.8f};
		public void OnTeamMentalityChanged(float newValue)
		{
			GameObject sliderMentGo    			= GameObject.Find ("Label_MentalityDesc");
			Text sliderMent 					= sliderMentGo.GetComponentInChildren<Text> ();			
			int index 							= Gameplay.Utils.GetIndexByWeight (newValue, mWeightsForMentality, mWeightsForMentality.Length);
			sliderMent.text 					= mMentalityTexts[index];
		}

		static string[] mTempoTexts	 			= new string[]{"Play around back", "Slow build-up", "Standard", "High-speed passing", "One touch-soccer"};
		static float[] mWeightsForTempo	 		= new float[]{0.0f, 0.2f, 0.4f, 0.6f, 0.8f};
		public void OnTeamTempoChanged(float newValue)
		{
			GameObject sliderTempoGo   			= GameObject.Find ("Label_TempoDesc");
			Text sliderTempo 					= sliderTempoGo.GetComponentInChildren<Text> ();			
			int index 							= Gameplay.Utils.GetIndexByWeight (newValue, mWeightsForTempo, mWeightsForTempo.Length);
			sliderTempo.text 					= mTempoTexts[index];
		}

		public void OnTeamPressureChanged(float newValue)
		{
			GameObject sliderPressureGo			= GameObject.Find ("Label_PressureDesc");
			Text sliderPressure 				= sliderPressureGo.GetComponentInChildren<Text> ();
			int presureInt 						= (int)(newValue*100);
			sliderPressure.text					= presureInt +"%";
		}
	}
}

