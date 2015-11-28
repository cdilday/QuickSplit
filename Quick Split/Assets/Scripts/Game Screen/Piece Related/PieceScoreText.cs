using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PieceScoreText : MonoBehaviour {

	public string pieceColor;
	public Color textColor;
	public int scoreValue = 1;
	Text text;

	int liveCount = 180;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		switch (pieceColor) {
		case "Red":
			textColor = Color.red;
			break;
		case "Orange":
			textColor = new Color(1f, 0.5f, 0f);
			break;
		case "Yellow":
			textColor = Color.yellow;
			break;
		case "Green":
			textColor = Color.green;
			break;
		case "Blue":
			textColor = Color.blue;
			break;
		case "Purple":
			textColor = new Color(0.6f, 0, 0.6f);
			break;
		case "Cyan":
			textColor = Color.cyan;
			break;
		case "White":
			textColor = Color.white;
			break;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		text.text = "" + scoreValue;
		text.color = textColor;
		liveCount--;
		transform.position = new Vector2 (transform.position.x, transform.position.y + 0.001f);
		if (liveCount % 20 == 0 && liveCount >= 60) {
			if(textColor.a == 1f)
			{
				textColor = new Color(textColor.r, textColor.g, textColor.b, 0.5f);
			}
			else
				textColor = new Color(textColor.r, textColor.g, textColor.b, 1f);
		}

		if (liveCount < 60) {
			textColor = new Color(textColor.r, textColor.g, textColor.b, 1f * (liveCount / 60f));
		}

		if (liveCount <= 0)
			Destroy (transform.gameObject);
	}
}
