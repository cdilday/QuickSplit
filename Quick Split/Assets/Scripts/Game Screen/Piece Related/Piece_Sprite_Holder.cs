using UnityEngine;
using System.Collections;

public class Piece_Sprite_Holder : MonoBehaviour {

	public Sprite[] RetroSprites = new Sprite[8];
	public Sprite[] ProgrammerSprites = new Sprite[8];
	public Sprite[] ArcaneSprites = new Sprite[8];
	public Sprite[] DefaultSprites = new Sprite[8];
	public Sprite[] BlobSprites = new Sprite[8];
	public Sprite[] DominoSprites = new Sprite[8];
	public Sprite[] PresentSprites = new Sprite[8];
	public Sprite[] PumpkinSprites = new Sprite[8];
	public Sprite[] SymbolSprites = new Sprite[8];
	public Sprite[] TechnoSprites = new Sprite[8];
	//don't ever use these this is a joke
	public Sprite[] FaceSprites = new Sprite[8];

	public Sprite[] Splitters; 

	public string PieceSet;
	public string SplitterType;

	public RuntimeAnimatorController[] DefaultAnimations = new RuntimeAnimatorController[8];
	public RuntimeAnimatorController[] ArcaneAnimations = new RuntimeAnimatorController[8];
	public RuntimeAnimatorController[] RetroAnimations = new RuntimeAnimatorController[8];
	public RuntimeAnimatorController[] BlobAnimations = new RuntimeAnimatorController[8];
	public RuntimeAnimatorController[] DominoAnimations = new RuntimeAnimatorController[8];
	public RuntimeAnimatorController[] PresentAnimations = new RuntimeAnimatorController[8];
	public RuntimeAnimatorController[] PumpkinAnimations = new RuntimeAnimatorController[8];
	public RuntimeAnimatorController[] SymbolAnimations = new RuntimeAnimatorController[8];
	public RuntimeAnimatorController[] TechnoAnimations = new RuntimeAnimatorController[8];

	Achievement_Script achievementHandler;

	// Use this for initialization
	void Start () {
		achievementHandler = GameObject.FindGameObjectWithTag ("Achievement Handler").GetComponent<Achievement_Script> ();
		if (PieceSet == "")
			PieceSet = PlayerPrefs.GetString ("Piece Set", "Default");
		if (SplitterType == "")
			SplitterType = PlayerPrefs.GetString ("Splitter Type", "Default");
	}

	public Sprite[] Get_Sprites()
	{
		PieceSet = PlayerPrefs.GetString ("Piece Set", "Default");
		switch (PieceSet) {
		case "Arcane":
			return ArcaneSprites;
		case "Retro":
			return RetroSprites;
		case "Programmer":
			return ProgrammerSprites;
		case "Default":
			return DefaultSprites;
		case "Blob":
			return BlobSprites;
		case "Domino":
			return DominoSprites;
		case "Present":
			return PresentSprites;
		case "Pumpkin":
			return PumpkinSprites;
		case "Symbol":
			return SymbolSprites;
		case "Techno":
			return TechnoSprites;
		case "Face":
			//this should never happen
			return FaceSprites;
		default:
			return DefaultSprites;
		}
	}
	//returns sprite for currently active splitter
	public Sprite Get_Splitter()
	{
		int index = achievementHandler.Splitter_Lookup_Index_by_Name (PlayerPrefs.GetString ("Splitter Type", "Default"));
		return Splitters [index];
	}
	//returns sprite for the splitter with the given integer index
	public Sprite Get_Splitter(int newSplitterIndex)
	{
		return Splitters [newSplitterIndex];
	}
	//returns the array of animation controllers for the current pieceset
	public RuntimeAnimatorController[] Get_Animations()
	{
		switch (PieceSet) {
		case "Default":
			return DefaultAnimations;
		case "Arcane":
			return ArcaneAnimations;
		case "Retro":
			return RetroAnimations;
		case "Programmer":
			return null;
		case "Blob":
			return BlobAnimations;
		case "Domino":
			return DominoAnimations;
		case "Present":
			return PresentAnimations;
		case "Pumpkin":
			return PumpkinAnimations;
		case "Symbol":
			return SymbolAnimations;
		case "Techno":
			return TechnoAnimations;
		case "Face":
		//this should never happen
			return null;
		default:
			return DefaultAnimations;
		}
	}
}
