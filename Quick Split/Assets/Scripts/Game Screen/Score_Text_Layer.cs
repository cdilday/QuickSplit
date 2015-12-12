using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score_Text_Layer : MonoBehaviour {

	//This script handles spawning the score text that pops up when a piece is destroyed

	public GameObject ScoreTextPrefab;
	RectTransform rectTransform;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
	}

	//this spawns the score text with the given properties on the score text layer
	public void Spawn_Score_Text(Vector2 location, string color, int value)
	{
		GameObject scoreText = Instantiate (ScoreTextPrefab) as GameObject;
		scoreText.transform.SetParent(transform);
		scoreText.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
		PieceScoreText thisText = scoreText.GetComponent<PieceScoreText> ();
		thisText.pieceColor = color;
		location = new Vector2((location.x*rectTransform.sizeDelta.x) - (rectTransform.sizeDelta.x * 0.5f),
								(location.y*rectTransform.sizeDelta.y) - (rectTransform.sizeDelta.y* 0.5f));
		scoreText.GetComponent<RectTransform> ().localPosition = location;
		thisText.scoreValue = value;
	}

}