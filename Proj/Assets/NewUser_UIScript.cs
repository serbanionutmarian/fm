using UnityEngine;
using System.Collections;

public class NewUser_UIScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void OnGUI()
	{
		FMGUI.MenuPage_NewUser thisMenuPage = (FMGUI.MenuPage_NewUser) FMGUI.MenuManager.mMenuPages[(int)FMGUI.MenuPages.PAGE_NEW_USER];
		if (thisMenuPage != null)
		{
			thisMenuPage.mCountriesCombo.Show();
		}
	}
}
