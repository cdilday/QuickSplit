﻿using UnityEngine;
using System.Collections;

//this script is used for spell effects that overlay the pieces

public class Piece_Spell_Effect : MonoBehaviour {

	SpriteRenderer spriteRenderer;
	Animator animator;

	bool whiteActive;
	bool cyanActive;
	bool purpleActive;
	bool greenBlueActive;
	bool purpleEnd;
	int cyanStage = 0;

	bool orangeActive;
	int orangeStage = 0;
	public Sprite orangeMiddleSprite;

	bool check = false;
	public bool lastPiece = false;

	string spellColor;

	Vector2 gridPos;

	float startTime;

	GameController gameController;
	piece_script piece;
	SpellHandler spellHandler;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		piece = gameObject.GetComponentInParent<piece_script> ();
		//no need for these if there are no spells
		if (gameController.gameType == "Wit" || gameController.gameType == "Quick")
			Destroy (gameObject);
		else
			spellHandler = GameObject.Find ("Spell Handler").GetComponent<SpellHandler> ();

		whiteActive = false;
		cyanActive = false;
		purpleActive = false;
		purpleEnd = false;

		spriteRenderer.sprite = null;
		greenBlueActive = false;

		orangeActive = false;
	}

	void FixedUpdate()
	{
		//white spell
		if (whiteActive) {
			//this one just plays the animation, the spellhandler does the actual work.
			if(animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time){
				whiteActive = false;
				animator.SetBool ("inActive", true);
				animator.SetBool ("White Active", false);
				spriteRenderer.sprite = null;
			}
		}
		//cyan spell
		else if (cyanActive && cyanStage > 1){
			//the cyan spell requires stages due to its complex nature
			//stage 2 means that the piece has collided with the next piece
			if (cyanStage == 2){
				animator.SetBool ("Cyan Active", false);
				cyanStage = 3;
			}
			//stage 3 means the bomb is in the process of exploding and pieces need to be taken crae of
			else if(cyanStage == 3){
				//this takes care of the case when the bomb is in the process of exploding but the piece moves because of the sidebars
				if(piece != null){
					transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, transform.position.z);
					gridPos = piece.gridPos;
				}
				//the frame where the pieces are properly deleted behind the animation
				if ((animator.GetCurrentAnimatorStateInfo(0).length / 1.75f) + startTime < Time.time){
					//cyan splitter cheevo
					int numDeleted = 0;
					for(int r = 0; r < 3; r++){
						for (int c = 0; c < 3; c++){
							//check to make sure it's a valid move
							if((int)(gridPos.x-1+r) >= 0 && (int)(gridPos.x-1+r) <= 7 && (int)gridPos.y-1+c >= 0 && (int)gridPos.y-1+c <= 15 &&
							   gameController.grid[(int)piece.gridPos.x - 1 + r, (int) piece.gridPos.y - 1 + c] != null)
							{
								Destroy(gameController.grid[(int)gridPos.x - 1 + r, (int) gridPos.y - 1 + c]);
								gameController.grid[(int)gridPos.x - 1 + r, (int) gridPos.y - 1 + c] = null;
								gameController.colorGrid[(int)gridPos.x - 1 + r, (int) gridPos.y - 1 + c] = null;
								numDeleted++;
							}
						}
					}
					if(piece != null){
						numDeleted++;
						Destroy (piece.gameObject);
					}
					gameController.collapse ();
					cyanStage = 4;

					GameObject.FindGameObjectWithTag("Achievement Handler").BroadcastMessage("Cyan_Splitter_Checker", null, SendMessageOptions.DontRequireReceiver);
				}
			}
			//the end of the animation, the spell effect is at this point a standalone object that is useless
			else if (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time){
				spellHandler.spellWorking = false;
				Destroy(gameObject);
			}
		}
		//purple spell
		else if(purpleActive){
			//also relatively complicated, thought the traversal is handled in the spell handler
			//this checks when the first animation (scan) ends, and is when the animation changes
			if (!check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time)){
				check = true;
				startTime = Time.time;
				//delete and do the deletion animation
				if(piece.pieceColor == spellColor){
					transform.SetParent(null);
					Destroy (piece.gameObject);
					animator.SetBool ("inActive", false);
					animator.SetBool ("Purple Fade", true);
					purpleEnd = true;
				}
				else{
					//nothing happened, play the shrinking animation
					animator.SetBool ("Purple Active", false);
				}
			}
			//this simply plays out the animation chosen at the end of the last animation
			else if (check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time)){
				check = false;
				purpleActive = false;
				animator.SetBool ("inActive", true);
				animator.SetBool ("Purple Fade", false);
				animator.SetBool ("Purple Active", false);
				spellColor = null;
				//if it's the final piece, do the board checks
				if(lastPiece){
					StartCoroutine(GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<Achievement_Script>().Purple_Splitter_Checker(gameController.Get_Danger_Pieces()));
					gameController.collapse ();
					StartCoroutine (gameController.boardWaiter ());
					gameController.splitter.setState ("isActive", true);
					spellHandler.spellWorking = false;
				}
				//if the piece was deleted, this effect is standalone and therefore uselees
				if(purpleEnd){
					Destroy(gameObject);
				}
			}
		}
		// Green and Blue spells are similar, and can therefore be combined in most aspects
		else if (greenBlueActive){
			//this changes the color on the frame that the piece is behind the animation. check ensures it happens once
			if(!check && ((animator.GetCurrentAnimatorStateInfo(0).length/2) + startTime < Time.time)){
				check = true;
				piece.ConvertColor(spellColor);
			}
			//this handles the resetting the piece back to its original state after the animation is finished
			else if( check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time)){
				greenBlueActive = false;
				animator.SetBool("Green Active", false);
				animator.SetBool("Blue Active", false);
				animator.SetBool("inActive", true);
				check = false;
				spriteRenderer.sprite = null;
				//do the board checks if the piece was the final one. This allows for bigger combos using the immediate multiplier
				if(lastPiece){
					gameController.checkBoard();
					gameController.splitter.setState ("isActive", true);
					lastPiece = false;
				}
			}
		}
		//orange spell
		else if(orangeActive) {
			//these calculations need to happen the first frame after orange is activated
			if(orangeStage == 0){
				animator.Play("Orange Selected", -1, Time.time % animator.GetCurrentAnimatorStateInfo(0).length);
				orangeStage = 1;
				//left pieces need to be activated twice; once to trigger the first animation, and again to load with the right choice
				if(spellColor != "left"){
					//if the collor is selected skip to the next stage
					orangeStage = 2;
				}
			}
			//stage 2 marks the point where the animation should change 
			else if (orangeStage == 2) {
				if (spriteRenderer.sprite == orangeMiddleSprite) {
					orangeStage = 3;
					animator.SetBool("Orange Active", false);
					startTime = Time.time;
				}
			}
			//stage 3 plays out the remainder of the animation, changes color, then cleans up
			else if (orangeStage == 3)
			{
				//the color change
				if(!check && ((animator.GetCurrentAnimatorStateInfo(0).length/2) + startTime < Time.time)) {
					check = true;
					//delete pieces that were marked as uneven remainders
					if(spellColor== "dead"){
						Destroy(piece.gameObject);
					}
					else{
						piece.ConvertColor(spellColor);
					}
				}
				//the remainder of the animation
				else if (check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time)){
					orangeActive = false;
					orangeStage = 0;
					animator.SetBool ("inActive", true);
					spriteRenderer.sprite = null;
					check = false;
				}
			}
		}
 	}
	//called to start the White spell
	public void Activate_White()
	{
		whiteActive = true;
		animator.SetBool ("inActive", false);
		animator.SetBool ("White Active", true);
		startTime = Time.time;
	}
	//called to start the cyan/cyan spell
	public void Activate_Cyan()
	{
		//first stage is when the piece is stilll in splitter
		if(cyanStage == 0){
			animator.SetBool ("inActive", false);
			animator.SetBool ("Cyan Active", true);
			cyanActive = true;
			cyanStage = 1;
		}
		//second call tells it to explode
		else {
			transform.SetParent(null);
			startTime = Time.time;
			cyanStage = 2;
			animator.SetBool ("inActive", false);
			animator.SetBool ("Cyan Active", true);
			gridPos = piece.gridPos;
		}
	}
	//called to start the purple spell
	public void Activate_Purple(string color)
	{
		purpleActive = true;
		spellColor = color;
		startTime = Time.time;
		animator.SetBool ("inActive", false);
		animator.SetBool ("Purple Active", true);
	}
	//called to start the green spell
	public void Activate_Green(string color)
	{
		greenBlueActive = true;
		spellColor = color;
		startTime = Time.time;
		animator.SetBool ("inActive", false);
		animator.SetBool ("Green Active", true);
	}
	//called to start the blue spell
	public void Activate_Blue(string color)
	{
		greenBlueActive = true;
		spellColor = color;
		startTime = Time.time;
		animator.SetBool ("inActive", false);
		animator.SetBool ("Blue Active", true);
	}
	//called to start the orange spell
	public void Activate_Orange(string color)
	{
		orangeActive = true;
		animator.SetBool ("inActive", false);
		spellColor = color;
		//This requires multiple stages depending on whether these pieces are on the left or right side
		if(orangeStage == 0) {
			animator.SetBool ("Orange Active", true);
		}
		if(orangeStage == 1) {
			orangeStage = 2;
		}
	}
}