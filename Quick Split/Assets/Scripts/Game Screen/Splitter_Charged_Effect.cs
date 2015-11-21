using UnityEngine;
using System.Collections;

public class Splitter_Charged_Effect : MonoBehaviour {

	Splitter_script splitter;
	Animator animator;
	SpriteRenderer spriteRenderer;

	bool prevCharged;

	GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		if (gameController.gameType != "Wiz" && gameController.gameType != "Holy")
			Destroy (gameObject);
		splitter = gameObject.GetComponentInParent<Splitter_script> ();

		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite = null;
		prevCharged = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (splitter.getState ("yellowReady")) {
			if(!prevCharged)
			{
				animator.SetBool("inActive", false);
				prevCharged = true;
			}
		}
		else if(prevCharged)
		{
			animator.SetBool("inActive", true);
			spriteRenderer.sprite = null;
			prevCharged = false;
		}
	}
}
