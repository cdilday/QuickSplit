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
			if(gameObject.GetComponent<SpriteRenderer>())
				gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
			else
				gameObject.GetComponent<Image>().sprite = sprites[0];
			break;
		case "Orange":
			if(gameObject.GetComponent<SpriteRenderer>())
				gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
			else
				gameObject.GetComponent<Image>().sprite = sprites[1];
			break;
		case "Yellow":
			if(gameObject.GetComponent<SpriteRenderer>())
				gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
			else
				gameObject.GetComponent<Image>().sprite = sprites[2];
			break;
		case "Green":
			if(gameObject.GetComponent<SpriteRenderer>())
				gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
			else
				gameObject.GetComponent<Image>().sprite = sprites[3];
			break;
		case "Blue":
			if(gameObject.GetComponent<SpriteRenderer>())
				gameObject.GetComponent<SpriteRenderer>().sprite = sprites[4];
			else
				gameObject.GetComponent<Image>().sprite = sprites[4];
			break;
		case "Purple":
			if(gameObject.GetComponent<SpriteRenderer>())
				gameObject.GetComponent<SpriteRenderer>().sprite = sprites[5];
			else
				gameObject.GetComponent<Image>().sprite = sprites[5];
			break;
		case "Cyan":
			if(gameObject.GetComponent<SpriteRenderer>())
				gameObject.GetComponent<SpriteRenderer>().sprite = sprites[6];
			else
				gameObject.GetComponent<Image>().sprite = sprites[6];
			break;
		case "White":
			if(gameObject.GetComponent<SpriteRenderer>())
				gameObject.GetComponent<SpriteRenderer>().sprite = sprites[7];
			else
				gameObject.GetComponent<Image>().sprite = sprites[7];
			break;
		}

	}
}
