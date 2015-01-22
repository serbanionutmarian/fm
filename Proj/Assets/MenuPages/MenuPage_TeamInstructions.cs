using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FMGUI
{
	public class MenuPage_TeamInstructions : MenuPage
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
			Application.LoadLevel (MenuManager.Instance.GetSceneNameForPage(MenuPages.PAGE_TEAMINSTRUCTIONS));
		}
		
		public override void OnEnter()
		{
			
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

		public override void OnBack ()
		{
			base.OnBack ();

			// Complete data
			SetInstructions ();
		}

		// TODO: move this to a general file because it can be used in more situations
		// Gets the index of the item selected
		public int GetIndexOfSelectedInToggleGroup(string[] toggleNames)
		{
			for (int i = 0; i < toggleNames.Length; i++)
			{
				GameObject gameObject 	= GameObject.Find (toggleNames[i]);
				Toggle toggle	 		= gameObject.GetComponentInChildren<Toggle> ();

				if (toggle.isOn)
				{
					return i;
				}
			}

			return -1;
		}
		
		public void SetInstructions()
		{
			Gameplay.Club userClub 		= Gameplay.GameDatabase.Instance.GetLocalUserClub ();
			Gameplay.TeamInstructions teaminstr = userClub.mTactics.mTeamInstructions;


			int aggrIndex 				= GetIndexOfSelectedInToggleGroup(new string[]{"Toggle_AggressivityNormal", "Toggle_AggressivityVery"});
			teaminstr.mAggresivityType 	= (Gameplay.AggressivityType) aggrIndex;

			int shotsIndex 				= GetIndexOfSelectedInToggleGroup(new string[]{"Toggle_Mixed", "Toggle_Short", "Toggle_Long"});
			teaminstr.mShotsType 		= (Gameplay.ShotsType) shotsIndex;

			int markingIndex 			= GetIndexOfSelectedInToggleGroup(new string[]{"Toggle_ManMarking", "Toggle_ZonalMarking"});
			teaminstr.mMarkingType 		= (Gameplay.MarkingType) markingIndex;

			int offIndex 				= GetIndexOfSelectedInToggleGroup(new string[]{"Toggle_OffsideYes", "Toggle_OffsideNo"});
			teaminstr.mOffsideTrapType 	= (Gameplay.OffsideTrapType) offIndex;

			GameObject sliderMentGo    	= GameObject.Find ("Slider_Mentality");
			Slider sliderMent 			= sliderMentGo.GetComponentInChildren<Slider> ();			
			GameObject sliderTempoGo   	= GameObject.Find ("Slider_Tempo");
			Slider sliderTemp			= sliderTempoGo.GetComponentInChildren<Slider> ();
			GameObject sliderPressGo   	= GameObject.Find ("Slider_Pressure");
			Slider sliderPress			= sliderPressGo.GetComponentInChildren<Slider> ();

			teaminstr.mMentality 		= sliderMent.value;
			teaminstr.mTempo			= sliderTemp.value;
			teaminstr.mPressure 		= sliderPress.value;
		}
	}
}

