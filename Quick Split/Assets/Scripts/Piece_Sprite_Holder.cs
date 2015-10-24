using UnityEngine;
using System.Collections;

public class Piece_Sprite_Holder : MonoBehaviour {

	public Sprite[] RetroSprites = new Sprite[8];
	public Sprite[] ProgrammerSprites = new Sprite[8];
	public Sprite[] KingSprites = new Sprite[8];
	public Sprite[] DefaultSprites = new Sprite[8];
	//don't ever use these this is a joke
	public Sprite[] FaceSprites = new Sprite[8];

	public Sprite DefaultSplitter;
	public Sprite GreenSplitter;
	public Sprite ProgrammerSplitter;

	public string PieceSet;
	public string SplitterType;

	public RuntimeAnimatorController[] DefaultAnimations = new RuntimeAnimatorController[8];
	public RuntimeAnimatorController[] KingAnimations = new RuntimeAnimatorController[8];
	public RuntimeAnimatorController[] RetroAnimations = new RuntimeAnimatorController[8];

	// Use this for initialization
	void Start () {
		if (PieceSet == "")
			PieceSet = PlayerPrefs.GetString ("Piece Set", "Default");
		if (SplitterType == "")
			SplitterType = PlayerPrefs.GetString ("Splitter Type", "Default");
	}

	public Sprite[] Get_Sprites()
	{
		PieceSet = PlayerPrefs.GetString ("Piece Set", "Default");
		switch (PieceSet) {
		case "King":
			return KingSprites;
		case "Retro":
			return RetroSprites;
		case "Programmer":
			return ProgrammerSprites;
		case "Default":
			return DefaultSprites;
		case "Face":
			//this should never happen
			return FaceSprites;
		default:
			return DefaultSprites;
		}
	}

	public Sprite Get_Splitter()
	{
		SplitterType = PlayerPrefs.GetString ("Splitter Type", "Default");
		switch (SplitterType) {
		case "Default":
			return DefaultSplitter;
		case "Green":
			return GreenSplitter;
		case "Programmer":
			return ProgrammerSplitter;
		default:
			return DefaultSplitter;
		}
	}

	public Sprite Get_Splitter(string newSplitter)
	{
		SplitterType = newSplitter;
		switch (SplitterType) {
		case "Default":
			return DefaultSplitter;
		case "Green":
			return GreenSplitter;
		case "Programmer":
			return ProgrammerSplitter;
		default:
			return DefaultSplitter;
		}
	}

	public RuntimeAnimatorController[] Get_Animations()
	{
		switch (PieceSet) {
		case "Default":
			return DefaultAnimations;
		case "King":
			return KingAnimations;
		case "Retro":
			return RetroAnimations;
		case "Programmer":
			return null;
		case "Face":
		//this should never happen
			return null;
		default:
			return DefaultAnimations;
		}
	}
}
