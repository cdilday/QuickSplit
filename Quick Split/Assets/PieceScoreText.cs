using UnityEngine;
using System.Collections;

public class PieceScoreText : MonoBehaviour {
	
	public Color textColor;
	public int scoreValue = 1;
	GUIText text;

	int liveCount = 180;

	// Use this for initialization
	void Start () {
		text = guiText;
	}
	
	// Update is called once per frame
	void Update () {

		text.text = "" + scoreValue;
		//text.color = textColor;
		liveCount--;
		if (liveCount <= 0)
			Destroy (transform.gameObject);
	}
}
