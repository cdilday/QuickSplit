using UnityEngine;
using System.Collections;

public class Piece_Sprite_Holder : MonoBehaviour {

	public Sprite[] RetroSprites = new Sprite[8];
	public Sprite[] ProgrammerSprites = new Sprite[8];
	public Sprite[] KingSprites = new Sprite[8];

	public string PieceSet;

	// Use this for initialization
	void Start () {
		if (PieceSet == "")
			PieceSet = PlayerPrefs.GetString ("Piece Set", "King");
	}

	public Sprite[] Get_Sprites()
	{
		switch (PieceSet) {
		case "King":
			return KingSprites;
		case "Retro":
			return RetroSprites;
		case "Programmer":
			return ProgrammerSprites;
		default:
			return KingSprites;
		}
	}
}
