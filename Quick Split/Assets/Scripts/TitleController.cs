using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

	public GameObject gameModeLayer;
	public GameObject[] howToPlayLayers;
	public GameObject creditsLayer;
	public GameObject optionsLayer;
	public Shutter_Handler shutter;
	int resetPresses = 0;

	// Use this for initialization
	void Start () {
		Goto_Game_Mode_Layer ();
		shutter.Begin_Vertical_Open ();
	}

	//loads the proper game mode into the playerprefs for the game scene to read, then loads the game scene
	//This is where the UI for the transition would go
	public void Load_Game(int gameMode)
	{
		PlayerPrefs.SetInt("Mode", gameMode);
		StartCoroutine ("GameTransition");
	}

	//loads the game mode layer and unloads the other layers
	public void Goto_Game_Mode_Layer()
	{
		gameModeLayer.SetActive (true);
		howToPlayLayers[0].SetActive (false);
		howToPlayLayers[1].SetActive (false);
		creditsLayer.SetActive (false);
		optionsLayer.SetActive (false);
	}

	//loads the How to Play Layer and unloads the other layers
	public void Goto_How_To_Play_Layer()
	{
		howToPlayLayers[0].SetActive (true);
		howToPlayLayers[1].SetActive (false);
		gameModeLayer.SetActive (false);
		creditsLayer.SetActive (false);
		optionsLayer.SetActive (false);
	}

	public void Goto_Controls_Layer()
	{
		howToPlayLayers[0].SetActive (false);
		howToPlayLayers[1].SetActive (true);
		gameModeLayer.SetActive (false);
		creditsLayer.SetActive (false);
		optionsLayer.SetActive (false);
	}

	//loads the Credits layer and unloads the other layers
	public void Goto_Credits_Layer()
	{
		creditsLayer.SetActive (true);
		gameModeLayer.SetActive (false);
		howToPlayLayers[0].SetActive (false);
		howToPlayLayers[1].SetActive (false);
		optionsLayer.SetActive (false);
	}

	public void Goto_Options_Layer()
	{
		optionsLayer.SetActive (true);
		howToPlayLayers[0].SetActive (false);
		howToPlayLayers[1].SetActive (false);
		gameModeLayer.SetActive (false);
		creditsLayer.SetActive (false);
	}

	public void Reset_High_Scores()
	{
		if (resetPresses == 0)
		{
			Text rhst = GameObject.Find ("Reset High Scores Text").GetComponent<Text>();
			resetPresses++;
			rhst.text = "Are you sure?";
		}
		else
		{
			PlayerPrefs.SetInt ("Wit", 0);
			PlayerPrefs.SetInt ("Quick", 0);
			PlayerPrefs.SetInt ("Wiz", 0);
			PlayerPrefs.SetInt ("Holy", 0);
			Text rhst = GameObject.Find ("Reset High Scores Text").GetComponent<Text>();
			rhst.text = "High Scores Reset!";
		}
	}

	public IEnumerator GameTransition()
	{
		shutter.Begin_Vertical_Close ();
		yield return new WaitForSeconds(2f);
		Application.LoadLevel("Game Scene");
	}
}
