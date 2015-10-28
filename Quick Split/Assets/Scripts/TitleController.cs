using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

	public GameObject gameModeLayer;
	public GameObject[] howToPlayLayers;
	public GameObject creditsLayer;
	public GameObject optionsLayer;
	public Shutter_Handler shutter;

	public High_Score_Displayer[] hsds = new High_Score_Displayer[4]; 
	int resetPresses = 0;

	int activeMode;

	//gameobjects needed for transitions b/w game mode select and description scenes
	public GameObject[] GameButtons = new GameObject[4];

	public GameObject[] Descriptions = new GameObject[4];

	public GameObject[] Scores = new GameObject[4];

	public GameObject PlayButton;
	public GameObject BackButton;

	Vector2 Active_Button_Position = new Vector2(-220f, 10f);
	Vector2 Inactive_Button_Left_Position = new Vector2(-560f, 0f);
	Vector2 Inactive_Button_Right_Position = new Vector2(560f, 0f);

	Vector2[] Orig_Button_Positions = new Vector2[4];

	Vector2 Active_Score_Position = new Vector2(-220f, -90f);
	Vector2[] Orig_Score_Positions = new Vector2[4];

	Vector2 Active_Back_Position = new Vector2(275f, -150f);
	Vector2 Active_Play_Position = new Vector2(125f, -150f);

	Vector2 Inactive_Back_Position;
	Vector2 Inactive_Play_Position;

	bool isInPlayScreen;
	bool isTransitioning;

	float TransitionStartTime;
	public float TransitionLength;

	// Use this for initialization
	void Start () {
		for(int i= 0; i<4; i++){
			Orig_Button_Positions [i] = GameButtons[i].GetComponent<RectTransform> ().localPosition;
			Orig_Score_Positions [i] = Scores[i].GetComponent<RectTransform> ().localPosition;
		}

		Inactive_Back_Position = BackButton.GetComponent<RectTransform> ().localPosition;
		Inactive_Play_Position = PlayButton.GetComponent<RectTransform> ().localPosition;

		Goto_Game_Mode_Layer ();
		shutter.Begin_Vertical_Open ();
		isInPlayScreen = false;
		isTransitioning = false;
	}

	void FixedUpdate()
	{
		//transitioning handler
		if (isTransitioning) {
			//transitioning to play screen
			if(isInPlayScreen)
			{
				// if transition should have ended
				if(Time.time > TransitionStartTime + TransitionLength)
				{
					Color textColor;
					//all the game-specific UI elements
					for (int i = 0; i < 4; i++)
					{
						//check if it's the active button
						if( i == activeMode)
						{
							GameButtons[i].GetComponent<RectTransform> ().localPosition = Active_Button_Position;
							Scores[i].GetComponent<RectTransform> ().localPosition = Active_Score_Position;
							textColor = Descriptions[i].GetComponent<Text>().color;
							Descriptions[i].GetComponent<Text>().color = new Color(textColor.r, textColor.g, textColor.g, 1);
						}
						//even index means it goes left
						else if (i % 2 == 0)
						{
							GameButtons[i].GetComponent<RectTransform> ().localPosition = Inactive_Button_Left_Position;
							Scores[i].GetComponent<RectTransform> ().localPosition = Inactive_Button_Left_Position;
						}
						//odd index means it goes right
						else
						{
							GameButtons[i].GetComponent<RectTransform> ().localPosition = Inactive_Button_Right_Position;
							Scores[i].GetComponent<RectTransform> ().localPosition = Inactive_Button_Right_Position;
						}
					}
					//Play & Back buttons
					BackButton.GetComponent<RectTransform> ().localPosition = Active_Play_Position;
					PlayButton.GetComponent<RectTransform> ().localPosition = Active_Back_Position;
					// set button/score positions to their proper playscreen positions
					// makes sure descriptions are properly faded in
					isTransitioning = false;
				}
				// if it should still be transitioning
				else
				{
					// lerp positions for buttons and scores, checking the active score to make sure that's moving onto the play screen
					// fade in the right description
					// move in the play and back buttons
				}
			}
			// transitioning back to the game mode screen
			else
			{
				// if transition should have ended
				if(Time.time > TransitionStartTime + TransitionLength)
				{
					Color textColor = Descriptions[0].GetComponent<Text>().color;
					for (int i = 0; i < 4; i++)
					{
						//buttons
						GameButtons[i].GetComponent<RectTransform> ().localPosition = Orig_Button_Positions [i];
						//scores
						Scores[i].GetComponent<RectTransform> ().localPosition = Orig_Score_Positions [i];
						//descriptions
						Descriptions[i].GetComponent<Text>().color = new Color(textColor.r, textColor.g, textColor.g, 0);
					}

					//Play & Back buttons
					BackButton.GetComponent<RectTransform> ().localPosition = Inactive_Back_Position;
					PlayButton.GetComponent<RectTransform> ().localPosition = Inactive_Play_Position;;

					// makes sure descriptions are properly faded out
					isTransitioning = false;
				}
				else
				{
					// lerp positions for buttons and scores onto the screen
					// fade out the active mode description
					// move the play and back buttons offscreen
				}
			}

		}
	}

	//loads the proper game mode into the playerprefs for the game scene to read, then loads the game scene
	//This is where the UI for the transition would go
	public void Load_Game()
	{
		PlayerPrefs.SetInt("Mode", activeMode);
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
		if (isInPlayScreen || isTransitioning) {
			return;
		}
		isInPlayScreen = true;
		isTransitioning = true;
		activeMode = gameType;

		TransitionStartTime = Time.time;
	}

	public void Click_Back_Button(){
		if (!isInPlayScreen || isTransitioning) {
			return;
		}
		isInPlayScreen = false;
		isTransitioning = true;

		TransitionStartTime = Time.time;
	}
}
