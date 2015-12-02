using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TitleController : MonoBehaviour {

	public GameObject gameModeLayer;
	public GameObject howToPlayLayer;
	public GameObject creditsLayer;
	public GameObject optionsLayer;
	public Shutter_Handler shutter;
	public VerticalScrollSnap gameModeScroller;

	public GameObject ScrollUp;
	public GameObject ScrollDown;

	public High_Score_Displayer[] hsds = new High_Score_Displayer[4]; 
	int resetPresses = 0;

	Achievement_Script achievementHandler;

	//gameobjects needed for transitions b/w game mode select and description scenes
	public GameObject[] GameButtons = new GameObject[4];
	string[] OrigButtonText = new string[4];
	public GameObject[] Descriptions = new GameObject[4];
	string[] OrigDescText = new string[4];
	public GameObject[] Scores = new GameObject[4];

	public GameObject PlayButton;
	public GameObject BackButton;

	bool isInPlayScreen;

	public int activeMode;
	int prevMode;

	// Use this for initialization
	void Start () {
		achievementHandler = GameObject.Find ("Achievement Handler").GetComponent<Achievement_Script> ();

		Goto_Game_Mode_Layer ();
		shutter.Begin_Vertical_Open ();

		//just in case this is the first time playing, set Wiz to be for sure unlocked
		PlayerPrefs.SetInt ("Wiz unlocked", 1);
		//tell achievmement handler to check gamemodes that are supposed to be active
		achievementHandler.Check_Gamemode_Unlocked ();

		activeMode = 0;
		prevMode = 0;
		ScrollDown.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
		for (int i = 0; i < 4; i++) {
			OrigButtonText[i] = GameButtons[i].GetComponentInChildren<Text>().text;
			OrigDescText[i] = Descriptions[i].GetComponent<Text>().text;
		}
		GameMode_Unlocker ();
	}

	void FixedUpdate()
	{
		if(gameModeLayer.activeSelf){
			activeMode = gameModeScroller.CurrentScreen ();
			if (activeMode != prevMode) {
				if(activeMode == 0){
					ScrollDown.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
				}
				else if(prevMode == 0){
					ScrollDown.BroadcastMessage ("FadeIn", null, SendMessageOptions.DontRequireReceiver);
				}
				else if(activeMode == 4){
					ScrollUp.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
				}
				else if(prevMode == 4){
					ScrollUp.BroadcastMessage ("FadeIn", null, SendMessageOptions.DontRequireReceiver);
				}
			}
			prevMode = activeMode;
		}
	}

	//loads the proper game mode into the playerprefs for the game scene to read, then loads the game scene
	//This is where the UI for the transition would go
	public void Load_Game()
	{
		activeMode = gameModeScroller.CurrentScreen ();
		if(activeMode == 4){
			Goto_Credits_Layer();
		} else if(gameNum_unlock_checker(activeMode)){
			PlayerPrefs.SetInt("Mode", activeMode);
			StartCoroutine ("GameTransition");
		}
	}

	//loads the game mode layer and unloads the other layers
	public void Goto_Game_Mode_Layer()
	{
		gameModeLayer.SetActive (true);
		howToPlayLayer.SetActive (false);
		creditsLayer.SetActive (false);
		optionsLayer.SetActive (false);
	}

	//loads the How to Play Layer and unloads the other layers
	public void Goto_How_To_Play_Layer()
	{
		howToPlayLayer.SetActive (true);
		gameModeLayer.SetActive (false);
		creditsLayer.SetActive (false);
		optionsLayer.SetActive (false);
	}

	//loads the Credits layer and unloads the other layers
	public void Goto_Credits_Layer()
	{
		creditsLayer.SetActive (true);
		gameModeLayer.SetActive (false);
		howToPlayLayer.SetActive (false);
		optionsLayer.SetActive (false);
	}

	public void Goto_Options_Layer()
	{
		optionsLayer.SetActive (true);
		howToPlayLayer.SetActive (false);
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
			PlayerPrefs.SetInt ("Wiz", 0);
			PlayerPrefs.SetInt ("Quick", 0);
			PlayerPrefs.SetInt ("Wit", 0);
			PlayerPrefs.SetInt ("Holy", 0);
			Text rhst = GameObject.Find ("Reset High Scores Text").GetComponent<Text>();
			foreach (High_Score_Displayer hsd in hsds)
			{
				hsd.update_scores();
			}
			rhst.text = "High Scores Reset!";
		}
	}

	public IEnumerator GameTransition()
	{
		shutter.Begin_Vertical_Close ();
		yield return new WaitForSeconds(2f);
		Application.LoadLevel("Game Scene");
	}

	public void Click_Game_Button(int gameType)
	{
		//this is a remnant of older UI's, may need to be replaced with a static image or given a better function
	}

	public void Click_Scores_Button(){
		//go to that page's high score page
	}

	bool gameNum_unlock_checker(int gameNum)
	{
		switch (gameNum) {
		case 0:
			return achievementHandler.is_Gamemode_Unlocked ("Wiz");
		case 1:
			return achievementHandler.is_Gamemode_Unlocked ("Quick");
		case 2:
			return achievementHandler.is_Gamemode_Unlocked ("Wit");
		case 3:
			return achievementHandler.is_Gamemode_Unlocked ("Holy");
		}
		return false;
	}

	void GameMode_Unlocker(){
		for(int i = 0; i < 4; i++){
			if(!gameNum_unlock_checker(i)){
				GameButtons[i].GetComponentInChildren<Text>().text = "LOCKED";
				Descriptions[i].GetComponent<Text>().text = "Score in the last Game Mode to unlock this one!";
				Scores[i].GetComponent<Text>().text = "";
			}
			else{
				GameButtons[i].GetComponentInChildren<Text>().text = OrigButtonText[i];
				Descriptions[i].GetComponent<Text>().text = OrigDescText[i];
				Scores[i].BroadcastMessage("update_scores", null, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
