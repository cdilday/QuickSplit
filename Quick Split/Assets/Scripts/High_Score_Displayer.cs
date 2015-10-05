using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class High_Score_Displayer : MonoBehaviour {
	public string gameType;
	int score;

	// Use this for initialization
	void Start () {
		score = PlayerPrefs.GetInt (gameType, 0);
		gameObject.GetComponent<Text> ().text = gameType + " High Score: " + score;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
