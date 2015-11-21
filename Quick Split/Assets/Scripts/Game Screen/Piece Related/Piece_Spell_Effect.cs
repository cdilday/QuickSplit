using UnityEngine;
using System.Collections;

//this script is used for spell effects that overlay the pieces

public class Piece_Spell_Effect : MonoBehaviour {

	SpriteRenderer spriteRenderer;
	Animator animator;

	bool whiteActive;
	bool greyActive;
	bool purpleActive;
	bool greenBlueActive;
	bool purpleEnd;
	int greyStage = 0;

	bool check = false;
	public bool lastPiece = false;

	string spellColor;

	Vector2 gridPos;

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
		greyActive = false;
		purpleActive = false;
		purpleEnd = false;

		spriteRenderer.sprite = null;
		greenBlueActive = false;
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
			else if(greyStage == 3)
			{
				//this takes care of the case when the bomb is in the process of exploding but the piece moves because of the sidebars
				if(piece != null){
					transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, transform.position.z);
					gridPos = piece.gridPos;
				}

				if ((animator.GetCurrentAnimatorStateInfo(0).length / 1.75f) + startTime < Time.time){
					for(int r = 0; r < 3; r++)
					{
						for (int c = 0; c < 3; c++)
						{
							//check to make sure it's a valid move
							if((int)(gridPos.x-1+r) >= 0 && (int)(gridPos.x-1+r) <= 7 && (int)gridPos.y-1+c >= 0 && (int)gridPos.y-1+c <= 15 &&
							   gameController.grid[(int)piece.gridPos.x - 1 + r, (int) piece.gridPos.y - 1 + c] != null)
							{
								Destroy(gameController.grid[(int)gridPos.x - 1 + r, (int) gridPos.y - 1 + c]);
								gameController.grid[(int)gridPos.x - 1 + r, (int) gridPos.y - 1 + c] = null;
								gameController.colorGrid[(int)gridPos.x - 1 + r, (int) gridPos.y - 1 + c] = null;
							}
						}
					}
					if(piece != null)
						Destroy (piece.gameObject);
					gameController.collapse ();
					greyStage = 4;
				}
			}
			else if (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time)
			{
				Destroy(gameObject);
			}
		}
		else if(purpleActive)
		{

			if (!check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time))
			{
				check = true;
				startTime = Time.time;
				if(piece.pieceColor == spellColor)
				{
					transform.SetParent(null);
					Destroy (piece.gameObject);
					animator.SetBool ("inActive", false);
					animator.SetBool ("Purple Fade", true);
					purpleEnd = true;
				}
				else{
					animator.SetBool ("Purple Active", false);
				}
			}
			else if (check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time))
			{
				check = false;
				purpleActive = false;
				animator.SetBool ("inActive", true);
				animator.SetBool ("Purple Fade", false);
				animator.SetBool ("Purple Active", false);
				spellColor = null;
				if(lastPiece){
					gameController.collapse ();
					StartCoroutine (gameController.boardWaiter ());
					gameController.splitter.setState ("isActive", true);
				}
				if(purpleEnd)
				{
					Destroy(gameObject);
				}
			}
		}
		else if (greenBlueActive)
		{
			if(!check && ((animator.GetCurrentAnimatorStateInfo(0).length/2) + startTime < Time.time))
			{
				check = true;
				piece.ConvertColor(spellColor);
				//change color here
			}
			else if( check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time))
			{
				greenBlueActive = false;
				animator.SetBool("Green Active", false);
				animator.SetBool("Blue Active", false);
				animator.SetBool("inActive", true);
				check = false;
				spriteRenderer.sprite = null;
				if(lastPiece){
					gameController.checkBoard();
					gameController.splitter.setState ("isActive", true);
					lastPiece = false;
				}
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
			gridPos = piece.gridPos;
		}
	}

	public void Activate_Purple(string color)
	{
		purpleActive = true;
		spellColor = color;
		startTime = Time.time;
		animator.SetBool ("inActive", false);
		animator.SetBool ("Purple Active", true);
	}

	public void Activate_Green(string color)
	{
		greenBlueActive = true;
		spellColor = color;
		startTime = Time.time;
		animator.SetBool ("inActive", false);
		animator.SetBool ("Green Active", true);
	}

	public void Activate_Blue(string color)
	{
		greenBlueActive = true;
		spellColor = color;
		startTime = Time.time;
		animator.SetBool ("inActive", false);
		animator.SetBool ("Blue Active", true);
	}
}
