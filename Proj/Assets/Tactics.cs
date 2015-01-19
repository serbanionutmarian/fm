using UnityEngine;
using System.Collections;

namespace Gameplay
{

public enum FieldSideTactic
{
	FIELD_SIDET_L,		// Left
	FIELD_SIDET_R,		// Right
	FIELD_SIDET_C,		// Central
	FIELD_SIDET_CR, 	// Central-left
	FIELD_SIDET_CL,		// Central-right
	FIELD_SIDET_NUM,
}

public enum FieldLineTactic
{
	FIELD_LINE_GK,
	FIELD_LINE_D,
	FIELD_LINE_DM,
	FIELD_LINE_M,
	FIELD_LINE_AM,
	FIELD_LINE_F,

	FIELD_LINE_NUM_WITHOUT_SUB,

	// Subs
	FIELD_LINE_S1 = FIELD_LINE_NUM_WITHOUT_SUB,
	FIELD_LINE_S2,
	FIELD_LINE_S3,
	FIELD_LINE_S4,
	FIELD_LINE_S5,
	FIELD_LINE_S6,
	FIELD_LINE_S7,
	FIELD_LINE_NUM
}

public class TacticPosDescription
{
	public FieldSideTactic	mSide;
	public FieldLineTactic	mLine;
	public string 			mString;		// DMC for example

		public TacticPosDescription(FieldLineTactic line, FieldSideTactic side, string str)
	{
		mSide 	= side;
		mLine 	= line;
		mString = str;
	}
}

public enum TacticType
{
	TACTIC_442,
	TACTIC_4312,
	TACTIC_NUM,
}
	

public class TeamTactics
{

	public TeamTactics()
	{
		mSelectedMatchPlayers = new int[TeamTactics.mNumTotalPlayers];		// Check mPosDesc[TACTIC] to see the positions
		for (int i = 0; i < TeamTactics.mNumTotalPlayers; i++)
		{
			mSelectedMatchPlayers[i] = TeamTactics.NO_PLAYER_SELECTED;
		}
	}

	public const int mNumPlayersOnField 	= 11;
	public const int mNumPlayersOnBench 	= 7;
	public const int mNumTotalPlayers 		= TeamTactics.mNumPlayersOnField + TeamTactics.mNumPlayersOnBench;
	public const int NO_PLAYER_SELECTED 	= -1;

	public int[] mSelectedMatchPlayers; // Check mTacticDesc[TACTIC] to see the positions

	public static string[] mTacticName = new string[] { "4-4-2", "4-3-1-2" };
	public static TacticPosDescription[][] mTacticDesc = new TacticPosDescription[(int)TacticType.TACTIC_NUM][]
	{
		new TacticPosDescription[mNumTotalPlayers]
		{
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_GK, FieldSideTactic.FIELD_SIDET_C, 	"GK"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_D, 	FieldSideTactic.FIELD_SIDET_L, 	"DL"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_D, 	FieldSideTactic.FIELD_SIDET_CL, "DC"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_D, 	FieldSideTactic.FIELD_SIDET_CR, "DC"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_D, 	FieldSideTactic.FIELD_SIDET_R, 	"DR"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_M, 	FieldSideTactic.FIELD_SIDET_L, 	"ML"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_M, 	FieldSideTactic.FIELD_SIDET_CL, "MC"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_M, 	FieldSideTactic.FIELD_SIDET_CR, "MC"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_M, 	FieldSideTactic.FIELD_SIDET_R, 	"MR"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_F, 	FieldSideTactic.FIELD_SIDET_CL, "ST"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_F, 	FieldSideTactic.FIELD_SIDET_CR, "ST"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S1, FieldSideTactic.FIELD_SIDET_C, 	"S1"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S2, FieldSideTactic.FIELD_SIDET_C, 	"S2"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S3, FieldSideTactic.FIELD_SIDET_C, 	"S3"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S4, FieldSideTactic.FIELD_SIDET_C, 	"S4"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S5, FieldSideTactic.FIELD_SIDET_C, 	"S5"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S6, FieldSideTactic.FIELD_SIDET_C, 	"s6"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S7, FieldSideTactic.FIELD_SIDET_C, 	"S7"),
		},
		
		new TacticPosDescription[mNumTotalPlayers]
		{
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_GK, FieldSideTactic.FIELD_SIDET_C, 	"GK"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_D, 	FieldSideTactic.FIELD_SIDET_L, 	"DL"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_D, 	FieldSideTactic.FIELD_SIDET_CL, "DC"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_D, 	FieldSideTactic.FIELD_SIDET_CR, "DC"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_D, 	FieldSideTactic.FIELD_SIDET_R, 	"DR"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_M, 	FieldSideTactic.FIELD_SIDET_CL, "MC"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_M,	FieldSideTactic.FIELD_SIDET_C, 	"MC"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_M, 	FieldSideTactic.FIELD_SIDET_CR, "MC"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_AM,	FieldSideTactic.FIELD_SIDET_C,  "AM"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_F, 	FieldSideTactic.FIELD_SIDET_C, 	"ST"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_F, 	FieldSideTactic.FIELD_SIDET_C, 	"ST"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S1, FieldSideTactic.FIELD_SIDET_C, 	"S1"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S2, FieldSideTactic.FIELD_SIDET_C, 	"S2"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S3, FieldSideTactic.FIELD_SIDET_C, 	"S3"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S4, FieldSideTactic.FIELD_SIDET_C, 	"S4"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S5, FieldSideTactic.FIELD_SIDET_C, 	"S5"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S6, FieldSideTactic.FIELD_SIDET_C, 	"s6"),
			new TacticPosDescription(FieldLineTactic.FIELD_LINE_S7, FieldSideTactic.FIELD_SIDET_C, 	"S7"),
		},
	};
}



}// namespace FMGUI



