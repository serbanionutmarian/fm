using UnityEngine;
using System;
using System.Collections.Generic;

namespace FMGUI
{
	public enum MenuPages
	{
		PAGE_LOGIN,
		PAGE_NEW_USER,
		PAGE_MAIN_MENU,
		PAGE_TEAMMANAGEMENT,
		PAGE_NUM,
	}

	public class MenuManager
	{
		static private readonly MenuManager mInstance = new MenuManager();

		// From ID to instance of menu page
		public static MenuPage[] mMenuPages = new MenuPage[(int)MenuPages.PAGE_NUM]
		{
			new MenuPage_Login(),
			new MenuPage_NewUser(),
			new MenuPage_MainMenu(),
			new MenuPage_TeamManagement()
		};

		// From ID to name of the scene. Each scene is a menu page
		static private string[] mSceneNames = new string[]
		{
			"page_login",
			"page_newuser",
			"page_mainmenu",
			"page_teammanagement2"
		};

		public string GetSceneNameForPage(MenuPages menuPage)
		{
			return mSceneNames [(int)menuPage];
		}
		
		public static MenuManager Instance
		{
			get
			{
				return mInstance;
			}
		}

		public void ClearAllPages()
		{
			while(mPages.Count > 0)
			{
				MenuPage removed = mPages.Pop ();
				removed.OnClosed();
			}
		}

		public void OpenNewPage(MenuPages menuId, bool closeAllOtherPages)
		{
			OpenNewPage (mMenuPages [(int)menuId], closeAllOtherPages);
		}

		private void OpenNewPage(MenuPage newPage, bool closeAllOtherPages)
		{
			if (closeAllOtherPages)
			{
				ClearAllPages();
			}

			MenuPage currentPage = mPages.Count > 0 ? mPages.Peek () : null;
			if (currentPage != null)
			{
				currentPage.OnHide ();
			}

			mPages.Push (newPage);
			newPage.OnRequested ();
		}

		public bool CanGoBack()
		{
			return mPages.Count > 1;
		}

		public void GoBack()
		{
			if (!CanGoBack())
			{
				return;
			}

			MenuPage currentPage = mPages.Pop();
			currentPage.OnClosed ();

			MenuPage prevPage = GetCurrentPage ();
			prevPage.OnRequested ();
		}

		public MenuPage GetCurrentPage() { return mPages.Count > 0 ? mPages.Peek () : null; }

		public void OnCurrentPageLoaded()
		{
			MenuPage currentPage = null;
			if (mPages.Count > 0)
			{
				currentPage = mPages.Peek();
			}

			currentPage.OnOpened ();
		}

		Stack<MenuPage> mPages = new Stack<MenuPage> ();			   
	}

	abstract public class MenuPage
	{
		abstract public void OnEnter ();	// Called when a page is focused
		abstract public void OnOpened();	// Called when a new page is opened for the first time, also calls OnEntered
		abstract public void OnHide();		// Called when a page is hidden. For example when oppening a new one 
		abstract public void OnClosed();	// Called when a page is closed. Internally should call OnHide
		abstract public void OnRequested();	// Called when a page is requested to open
	}
}

