using UnityEngine;
using System.Collections;

public class Splitter_Animator_Script : MonoBehaviour {

	//This script applies the proper animations to the splitters and activates them on start

	SpriteRenderer spriteRenderer;
	Animator animator;
	Piece_Sprite_Holder pieceSpriteHolder;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator>();
		pieceSpriteHolder = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ();
		animator.runtimeAnimatorController = pieceSpriteHolder.Get_Splitter_Animation ();
		if (animator.runtimeAnimatorController == null) {
			spriteRenderer.sprite = null;
		}
	}
}
