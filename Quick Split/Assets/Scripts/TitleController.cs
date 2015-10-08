using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	public GameObject gameModeLayer;
	public GameObject[] howToPlayLayers;
	public GameObject creditsLayer;
	public Shutter_Handler shutter;
	// Use this for initialization
	void Start () {
		Goto_Game_Mode_Layer ();
		shutter.Begin_Horizontal_Open ();
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
	}

	//loads the How to Play Layer and unloads the other layers
	public void Goto_How_To_Play_Layer()
	{
		howToPlayLayers[0].SetActive (true);
		howToPlayLayers[1].SetActive (false);
		gameModeLayer.SetActive (false);
		creditsLayer.SetActive (false);
	}

	public void Goto_Controls_Layer()
	{
		howToPlayLayers[0].SetActive (false);
		howToPlayLayers[1].SetActive (true);
		gameModeLayer.SetActive (false);
		creditsLayer.SetActive (false);
	}

	//loads the Credits layer and unloads the other layers
	public void Goto_Credits_Layer()
	{
		creditsLayer.SetActive (true);
		gameModeLayer.SetActive (false);
		howToPlayLayers[0].SetActive (false);
		howToPlayLayers[1].SetActive (false);
	}

	public IEnumerator GameTransition()
	{
		shutter.Begin_Vertical_Close ();
		yield return new WaitForSeconds(2f);
		Application.LoadLevel("Game Scene");
	}
}
