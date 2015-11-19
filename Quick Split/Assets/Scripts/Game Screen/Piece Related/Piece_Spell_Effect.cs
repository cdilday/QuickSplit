using UnityEngine;
using System.Collections;

//this script is used for spell effects that overlay the pieces

public class Piece_Spell_Effect : MonoBehaviour {

	SpriteRenderer spriteRenderer;
	Animator animator;

	bool whiteActive;
	bool greyActive;
	int greyStage = 0;

	float startTime;

	GameController gameController;
	piece_script piece;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		piece = gameObject.GetComponentInParent<piece_script> ();

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
		else if (greyActive && greyStage > 1)
		{
			if (greyStage == 2)
			{
				animator.SetBool ("Grey Active", false);
				greyStage = 3;
			}
			else if(greyStage == 3 && ((animator.GetCurrentAnimatorStateInfo(0).length / 1.75f) + startTime < Time.time))
			{
				for(int r = 0; r < 3; r++)
				{
					for (int c = 0; c < 3; c++)
					{
						//check to make sure it's a valid move
						if((int)(piece.gridPos.x-1+r) >= 0 && (int)(piece.gridPos.x-1+r) <= 7 && (int)piece.gridPos.y-1+c >= 0 && (int)piece.gridPos.y-1+c <= 15 &&
						   gameController.grid[(int)piece.gridPos.x - 1 + r, (int) piece.gridPos.y - 1 + c] != null)
						{
							Destroy(gameController.grid[(int)piece.gridPos.x - 1 + r, (int) piece.gridPos.y - 1 + c]);
							gameController.grid[(int)piece.gridPos.x - 1 + r, (int) piece.gridPos.y - 1 + c] = null;
							gameController.colorGrid[(int)piece.gridPos.x - 1 + r, (int) piece.gridPos.y - 1 + c] = null;
						}
					}
				}
				transform.SetParent(null);
				Destroy (piece.gameObject);
				gameController.collapse ();
				greyStage = 4;
			}
			else if (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time)
			{
				Destroy(gameObject);
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

	public void Activate_Grey()
	{
		if(greyStage == 0)
		{
			animator.SetBool ("inActive", false);
			animator.SetBool ("Grey Active", true);
			greyActive = true;
			greyStage = 1;
		}
		else
		{
			transform.SetParent(null);
			startTime = Time.time;
			greyStage = 2;
			animator.SetBool ("inActive", false);
			animator.SetBool ("Grey Active", true);
		}
	}
}
