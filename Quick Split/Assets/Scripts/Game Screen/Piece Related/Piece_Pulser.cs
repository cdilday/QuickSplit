using UnityEngine;
using System.Collections;

public class Piece_Pulser : MonoBehaviour {

	//this script handles the pulsing effect that happens when pieces are selectable for spells or when they're in danger

	public SpriteRenderer spriteRenderer;
	Piece parentPiece;

	Color activeColor;
	Color inActiveColor;
	Color dangerColor;

	bool isPulsing;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		parentPiece = transform.GetComponentInParent<Piece> ();
		spriteRenderer.sprite = parentPiece.GetComponent<SpriteRenderer> ().sprite;
		dangerColor = new Color (0, 0, 0, 0.25f);
		activeColor = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
		inActiveColor = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
		spriteRenderer.color = inActiveColor;
		isPulsing = false;
		//move slightly in front of the so as to make it obvious when it's active
		transform.Translate (new Vector3 (0, 0, -0.1f));
		//gameController = parentPiece.gameController;
	}

	void FixedUpdate () {
		//check if this pulser should be active
		if (!parentPiece.selectable && parentPiece.gridPos.y != 6 && parentPiece.gridPos.y != 9) {
			transform.localScale = Vector3.one;
			//set to inactivecolor only on the frame after it was active
			if(isPulsing){
				isPulsing = false;
				spriteRenderer.color = inActiveColor;
			}
			return;
		}
		//set to Active color if it was previously inactive
		if (!isPulsing) {
			isPulsing = true;
			if(parentPiece.selectable)
				spriteRenderer.color = activeColor;
			else
				spriteRenderer.color = dangerColor;
		}

		Vector3 newScale = transform.localScale;
		if (parentPiece.selectable) {
			newScale = new Vector3 ((Mathf.Sin (Time.time * 4 ) *0.25f) + 1.25f, (Mathf.Sin (Time.time * 4 ) *0.25f) + 1.25f, newScale.z);
		}
		else {
			newScale = new Vector3 ((Mathf.Sin (Time.time * 8 ) *0.1f) + 1.1f, (Mathf.Sin (Time.time * 8 ) *0.1f) + 1.1f, newScale.z);
		}
		transform.localScale = newScale;
	}

}