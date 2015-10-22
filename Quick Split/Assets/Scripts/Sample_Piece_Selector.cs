using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sample_Piece_Selector : MonoBehaviour {

	public string[] tileSets;
	public Piece_Colorer[] samplePieces;
	public int index = 0;
	public Text headerText;
	
	
	// Use this for initialization
	void Start () {
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
		default:
			index = 0;
			break;
		}
	}

	public void Left_Button()
	{
		if (index == 0)
			index = tileSets.Length- 1;
		else
			index--;
		
		PlayerPrefs.SetString ("Piece Set", tileSets[index]);
		headerText.text = tileSets [index];
		foreach (Piece_Colorer pc in samplePieces) {
			pc.update_color();
		}
	}
	
	public void Right_Button()
	{
		if (index == tileSets.Length - 1)
			index = 0;
		else
			index++;
		
		PlayerPrefs.SetString ("Piece Set", tileSets[index]);
		headerText.text = tileSets [index];
		foreach (Piece_Colorer pc in samplePieces) {
			pc.update_color();
		}

	}
}
