using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sample_Piece_Selector : MonoBehaviour {

	//TODO: Find all instances of the Arcane pieceset named King and rename them for consistancy

	public string[] tileSets;
	public Piece_Colorer[] samplePieces;
	public int index = 0;
	public Text headerText;

	Achievement_Script achievementHandler;
	
	
	// Use this for initialization
	void Start () {
		achievementHandler = GameObject.FindGameObjectWithTag ("Achievement Handler").GetComponent<Achievement_Script> ();
		headerText.text = PlayerPrefs.GetString("Piece Set", "Default");
		switch (headerText.text) {
		case "Default":
			index = 0;
			break;
		case "King":
			index = 1;
			break;
		case "Retro":
			index = 2;
			break;
		case "Programmer":
			index = 3;
			break;
		case "Blob":
			index = 4;
			break;
		case "Domino":
			index = 5;
			break;
		case "Present":
			index = 6;
			break;
		case "Pumpkin":
			index = 7;
			break;
		case "Symbol":
			index = 8;
			break;
		case "Techno":
			index = 9;
			break;
		default:
			index = 0;
			break;
		}
	}

	public void Left_Button()
	{
		do{
			if (index == 0)
				index = tileSets.Length- 1;
			else
				index--;
		} while(!achievementHandler.is_Pieceset_Unlocked(tileSets[index]));
		
		PlayerPrefs.SetString ("Piece Set", tileSets[index]);
		headerText.text = tileSets [index];
		foreach (Piece_Colorer pc in samplePieces) {
			pc.update_color();
		}
	}
	
	public void Right_Button()
	{
		do{
			if (index == tileSets.Length - 1)
				index = 0;
			else
				index++;
		} while(!achievementHandler.is_Pieceset_Unlocked(tileSets[index]));
		
		PlayerPrefs.SetString ("Piece Set", tileSets[index]);
		headerText.text = tileSets [index];
		foreach (Piece_Colorer pc in samplePieces) {
			pc.update_color();
		}

	}
}
