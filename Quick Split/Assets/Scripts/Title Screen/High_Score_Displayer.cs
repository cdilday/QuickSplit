using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class High_Score_Displayer : MonoBehaviour {

	//this script is attached to the green high score previews on the game mode select screen and handles those updates

	public string gameType;
	int score;

	public void update_scores(){
		score = PlayerPrefs.GetInt (gameType + " score 0", 0);
		gameObject.GetComponent<Text> ().text = gameType + " High Score: " + score;
	}

}