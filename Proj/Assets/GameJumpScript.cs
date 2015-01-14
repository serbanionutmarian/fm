using UnityEngine;
using System.Collections;

namespace GUI
{

public class GameJumpScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
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
		MenuManager.Instance.GoBack ();
	}
}




} // namespace GUI