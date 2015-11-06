using UnityEngine;
using System.Collections;

public class Piece_Pulser : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	piece_script parentPiece;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		parentPiece = transform.GetComponentInParent<piece_script> ();
		spriteRenderer.sprite = parentPiece.GetComponent<SpriteRenderer> ().sprite;
		spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
		//move slightly behind the piece so as not to block it
		transform.Translate(new Vector3(0,0,0.1f));
	}

	void FixedUpdate () {
		//check if this pulser should be active
		if (!parentPiece.selectable && parentPiece.gridPos.y != 6 && parentPiece.gridPos.y != 9) {
			transform.localScale = Vector3.one;
			return;
		}

		Vector3 newScale = transform.localScale;
		newScale = new Vector3 ((Mathf.Sin (Time.time * 4 ) *0.25f) + 1.25f, (Mathf.Sin (Time.time * 4 ) *0.25f) + 1.25f, newScale.z);
		transform.localScale = newScale;

	}
}
