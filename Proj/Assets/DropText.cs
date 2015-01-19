using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DropText : MonoBehaviour 
{
	GUIContent[] comboBoxList;
	private ComboBox comboBoxControl;// = new ComboBox();
	private GUIStyle listStyle = new GUIStyle();

	//-----------
	GameObject[] mFormPitchPos;		// Pool of objects

	// TODO: optimize memory allocations - create a Vector3 for each pos on each tactic by considering the table below

	// These represents the offsets of the lines and sides in the tactics image.
	// Based on these we'll reconstruct every tactic
	private static float[] mPosOffsets_Sides = new float[(int)Gameplay.FieldSideTactic.FIELD_SIDET_NUM]
	{
		84, 168, 223, 286, 369,
	};

	private static float[] mPosOffsets_Lines = new float[(int)Gameplay.FieldLineTactic.FIELD_LINE_NUM_WITHOUT_SUB]
	{
		65, 175, 274, 366, 474, 572,
	};

	// Center of the tactis image...width/2, height/2
	private static Vector3 mTacticsImageCenter = new Vector3(355, 227, 0);
	//-----------


	int mCurrentListSelectedIndex = -1;	
	private void Start()
	{
		CreateFormationsDropbox ();
		CreatePitchPos ();
	}

	private void CreatePitchPos()
	{
		Object posPrefab = Resources.Load("Button_Pos");
		Debug.Log ("Pref inst: " + posPrefab);

		GameObject canvasTactics = GameObject.Find ("Image_Tactics");

		mFormPitchPos = new GameObject[Gameplay.TeamTactics.mNumPlayersOnField];
		for (int i = 0; i < Gameplay.TeamTactics.mNumPlayersOnField; i++)
		{
			mFormPitchPos[i] = (GameObject)Instantiate (posPrefab);
			mFormPitchPos[i].SetActive (true);
			mFormPitchPos[i].transform.SetParent (canvasTactics.transform);
			mFormPitchPos[i].transform.position = new Vector3(0,0);
		}
	}

	private void CreateFormationsDropbox()
	{
		comboBoxList = new GUIContent[Gameplay.TeamTactics.mTacticName.Length];
		for (int i = 0; i < Gameplay.TeamTactics.mTacticName.Length; i++)
		{
			comboBoxList[i] = new GUIContent(Gameplay.TeamTactics.mTacticName[i]);
		}
		
		listStyle.normal.textColor = Color.white; 
		listStyle.onHover.background =
			listStyle.hover.background = new Texture2D(2, 2);
		listStyle.padding.left =
			listStyle.padding.right =
				listStyle.padding.top =
				listStyle.padding.bottom = 4;
		
		listStyle.fixedHeight = 30;
		
		comboBoxControl = new ComboBox(new Rect(120, 0, 100, 30), comboBoxList[0], comboBoxList, "button", "box", listStyle);
		//comboBoxControl.SelectedItemIndex = mCurrentListSelectedIndex;
	}

	void Update()
	{
		int currentSelectedIndex = comboBoxControl.SelectedItemIndex;
		if (currentSelectedIndex != mCurrentListSelectedIndex)
		{
			FormationChanged(currentSelectedIndex);
			mCurrentListSelectedIndex = currentSelectedIndex;
		}
	}

	void FormationChanged(int formationId)
	{
		FMGUI.MenuPage_TeamManagement teamManPage = (FMGUI.MenuPage_TeamManagement)FMGUI.MenuManager.mMenuPages[(int)FMGUI.MenuPages.PAGE_TEAMMANAGEMENT];
		teamManPage.OnFormationChangedMsg(formationId);

		// Set positions
		// TODO: optimize allocations - check the comment above
		//int numTactics = Gameplay.TeamTactics.mTacticName.Length;

		for (int i = 0; i < Gameplay.TeamTactics.mNumPlayersOnField; i++)
		{

			Gameplay.TacticPosDescription posDesc = Gameplay.TeamTactics.mTacticDesc[formationId][i];
			float X = DropText.mPosOffsets_Lines[(int)posDesc.mLine]; //- mTacticsImageCenter.x;
			float Y = DropText.mPosOffsets_Sides[(int)posDesc.mSide] + 115;

			mFormPitchPos[i].transform.position = new Vector3(X, Y, 0.0f);
			Text textPos = mFormPitchPos[i].GetComponentInChildren<Text> ();
			textPos.text = posDesc.mString;
		}
	}
	
	private void OnGUI () 
	{
		comboBoxControl.Show();
	}
}

public class ComboBox
{
	private static bool forceToUnShow = false; 
	private static int useControlID = -1;
	private bool isClickedComboButton = false;
	private int selectedItemIndex = 0;
	
	private Rect rect;
	private GUIContent buttonContent;
	private GUIContent[] listContent;
	private string buttonStyle;
	private string boxStyle;
	private GUIStyle listStyle;
	
	public ComboBox( Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle ){
		this.rect = rect;
		this.buttonContent = buttonContent;
		this.listContent = listContent;
		this.buttonStyle = "button";
		this.boxStyle = "box";
		this.listStyle = listStyle;
	}
	
	public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, string buttonStyle, string boxStyle, GUIStyle listStyle){
		this.rect = rect;
		this.buttonContent = buttonContent;
		this.listContent = listContent;
		this.buttonStyle = buttonStyle;
		this.boxStyle = boxStyle;
		this.listStyle = listStyle;
	}
	
	public int Show()
	{
		if( forceToUnShow )
		{
			forceToUnShow = false;
			isClickedComboButton = false;
		}
		
		bool done = false;
		int controlID = GUIUtility.GetControlID( FocusType.Passive );       
		
		switch( Event.current.GetTypeForControl(controlID) )
		{
		case EventType.mouseUp:
		{
			if( isClickedComboButton )
			{
				done = true;
			}
		}
			break;
		}       

		if( GUI.Button( rect, buttonContent, buttonStyle ) )
		{
			if( useControlID == -1 )
			{
				useControlID = controlID;
				isClickedComboButton = false;
			}
			
			if( useControlID != controlID )
			{
				forceToUnShow = true;
				useControlID = controlID;
			}
			isClickedComboButton = true;
		}
		
		if( isClickedComboButton )
		{
			Rect listRect = new Rect( rect.x, rect.y + listStyle.CalcHeight(listContent[0], 1.0f),
			                         rect.width, listStyle.CalcHeight(listContent[0], 1.0f) * listContent.Length );
			
			GUI.Box( listRect, "", boxStyle );
			int newSelectedItemIndex = GUI.SelectionGrid( listRect, selectedItemIndex, listContent, 1, listStyle );
			if( newSelectedItemIndex != selectedItemIndex )
			{
				selectedItemIndex = newSelectedItemIndex;
				buttonContent = listContent[selectedItemIndex];
			}
		}
		
		if( done )
			isClickedComboButton = false;
		
		return selectedItemIndex;
	}
	
	public int SelectedItemIndex{
		get{
			return selectedItemIndex;
		}
		set{
			selectedItemIndex = value;
		}
	}
}