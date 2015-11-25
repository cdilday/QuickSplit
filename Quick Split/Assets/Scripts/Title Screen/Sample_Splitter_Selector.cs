using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sample_Splitter_Selector : MonoBehaviour {
	//recoloring the other splitter on the how to play screen
	public Image htpSplitter;
	
	public int index = 0;
	public Text headerText;

	Achievement_Script achievementHandler;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Image> ().sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter ();
		htpSplitter.sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter ();

		achievementHandler = GameObject.FindGameObjectWithTag ("Achievement Handler").GetComponent<Achievement_Script> ();

		headerText.text = PlayerPrefs.GetString("Splitter Type", "Default");
		index = achievementHandler.Splitter_Lookup_Index_by_Name (headerText.text);
	}

	public void Left_Button()
	{
		do {
			if (index <= 0)
				index = achievementHandler.Splitters.Length - 1;
			else
				index--;
		} while (!achievementHandler.is_Splitter_Unlocked(achievementHandler.Splitters[index]));

		PlayerPrefs.SetString ("Splitter Type", achievementHandler.Splitters[index]);
		headerText.text = achievementHandler.Splitters [index];
		//TODO: Too many look ups, make a global variable and load it once rather than every time
		gameObject.GetComponent<Image> ().sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter (index);
		htpSplitter.sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter (index);
	}

	public void Right_Button()
	{
		do{
			if (index >= achievementHandler.Splitters.Length - 1)
				index = 0;
			else
				index++;
		} while (!achievementHandler.is_Splitter_Unlocked(achievementHandler.Splitters[index]));
		
		PlayerPrefs.SetString ("Splitter Type", achievementHandler.Splitters[index]);
		headerText.text = achievementHandler.Splitters [index];
		gameObject.GetComponent<Image> ().sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter (index);
		htpSplitter.sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter (index);
	}

}
