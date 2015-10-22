using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Piece_Colorer : MonoBehaviour {
	public string pieceColor;

	Sprite[] sprites = new Sprite[8];

	public Piece_Sprite_Holder spriteHolder;
	// Use this for initialization
	void Start () {
		spriteHolder = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ();
		update_color ();
	}

	public void update_color()
	{
		sprites = spriteHolder.Get_Sprites ();
		switch (pieceColor)
		{
		case "Red":
			gameObject.GetComponent<Image>().sprite = sprites[0];
			break;
		case "Orange":
			gameObject.GetComponent<Image>().sprite = sprites[1];
			break;
		case "Yellow":
			gameObject.GetComponent<Image>().sprite = sprites[2];
			break;
		case "Green":
			gameObject.GetComponent<Image>().sprite = sprites[3];
			break;
		case "Blue":
			gameObject.GetComponent<Image>().sprite = sprites[4];
			break;
		case "Purple":
			gameObject.GetComponent<Image>().sprite = sprites[5];
			break;
		case "Grey":
			gameObject.GetComponent<Image>().sprite = sprites[6];
			break;
		case "White":
			gameObject.GetComponent<Image>().sprite = sprites[7];
			break;
		}

	}
}
