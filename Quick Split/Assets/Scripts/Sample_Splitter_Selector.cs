using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sample_Splitter_Selector : MonoBehaviour {
	//recoloring the other splitter on the how to play screen
	public Image htpSplitter;

	public string[] splitters;
	public int index = 0;
	public Text headerText;


	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Image> ().sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter ();
		htpSplitter.sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter ();

		headerText.text = PlayerPrefs.GetString("Splitter Type", "Default");
		switch (headerText.text) {
		case "Default":
			index = 0;
			break;
		case "King":
			index = 1;
			break;
		case "Programmer":
			index = 2;
			break;
		default:
			index = 0;
			break;
		}
	}

	public void Left_Button()
	{
		if (index <= 0)
			index = splitters.Length - 1;
		else
			index--;

		PlayerPrefs.SetString ("Splitter Type", splitters[index]);
		headerText.text = splitters [index];
		//TODO: Too many look ups, make a global variable and load it once rather than every time
		gameObject.GetComponent<Image> ().sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter (splitters[index]);
		htpSplitter.sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter (splitters[index]);
	}

	public void Right_Button()
	{
		if (index >= splitters.Length - 1)
			index = 0;
		else
			index++;
		
		PlayerPrefs.SetString ("Splitter Type", splitters[index]);
		headerText.text = splitters [index];
		gameObject.GetComponent<Image> ().sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter (splitters[index]);
		htpSplitter.sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter (splitters[index]);
	}

}
