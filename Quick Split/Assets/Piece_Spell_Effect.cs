using UnityEngine;
using System.Collections;

//this script is used for spell effects that overlay the pieces

public class Piece_Spell_Effect : MonoBehaviour {

	SpriteRenderer spriteRenderer;
	Animator animator;
	
	bool whiteActive;

	float startTime;

	GameController gameController;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		if (gameController.gameType == "Wit" || gameController.gameType == "Quick")
			Destroy (gameObject);

		whiteActive = false;

		spriteRenderer.sprite = null;
	}

	void FixedUpdate()
	{
		if (whiteActive) {
			if(animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time)
			{
				whiteActive = false;
				animator.SetBool ("inActive", true);
				animator.SetBool ("White Active", false);
				spriteRenderer.sprite = null;
			}
		}
 	}
	
	public void Activate_White()
	{
		whiteActive = true;
		animator.SetBool ("inActive", false);
		animator.SetBool ("White Active", true);
		startTime = Time.time;
	}
}
