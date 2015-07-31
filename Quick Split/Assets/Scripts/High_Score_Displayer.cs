using UnityEngine;
using System.Collections;

public class High_Score_Displayer : MonoBehaviour {
	public string gameType;
	int score;

	// Use this for initialization
	void Start () {
		score = PlayerPrefs.GetInt (gameType, 0);
		gameObject.GetComponent<GUIText> ().text = gameType + " High Score: " + score;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
