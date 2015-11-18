﻿using UnityEngine;
using System.Collections;

public class Red_Spell_Script : MonoBehaviour {

	Animator animator;
	Animator creationAnimator;

	SpriteRenderer spriteRenderer;
	SpriteRenderer creationSpriteRenderer;

	int row;
	int col;
	bool isRight;

	bool activated;
	bool isMoving;
	bool hasHit;
	bool hasDestroyed;

	Vector3 startPosition;
	
	float creationPlayTime;
	float creationStartTime;
	float speed = 0.2f;

	SpellHandler spellHandler;
	GameController gameController;
	// Use this for initialization
	void Awake () {
		spellHandler = GameObject.Find ("Spell Handler").GetComponent<SpellHandler> ();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		startPosition = transform.position;

		animator = GetComponent<Animator> ();
		creationAnimator = transform.GetChild (0).GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		creationSpriteRenderer = transform.GetChild (0).GetComponent<SpriteRenderer> ();

		spriteRenderer.sprite = null;
		creationSpriteRenderer.sprite = null;

		row = (int)transform.position.y;
		if (!( transform.localPosition.x < 0)){
			isRight = true;
			name = ("Red Spell Effect Row " + row + " Right");
		}
		else{
			isRight = false;
			name = ("Red Spell Effect Row " + row + " Left");
		}
		activated = false;
		isMoving = false;
		hasHit = false;
	}

	void FixedUpdate(){
		if (activated) {
			//End the creation animation, begin moving the gameobject
			if(!isMoving && !hasHit && animator.GetBool("inActive") && (creationAnimator.GetCurrentAnimatorStateInfo(0).length/ 2f) + creationStartTime < Time.time)
			{
				animator.SetBool("inActive", false);

			}
			else if(!isMoving && !hasHit && creationAnimator.GetCurrentAnimatorStateInfo(0).length + creationStartTime < Time.time)
			{
				float animPlayLength = creationAnimator.GetCurrentAnimatorStateInfo(0).length * (1/creationAnimator.speed);
				if(animPlayLength + creationStartTime < Time.time)
				{
					creationAnimator.SetBool ("isCreated", false);
					creationSpriteRenderer.sprite = null;
					isMoving = true;
				}
			}
			else if (isMoving && !hasHit)
			{
				if(isRight)
				{
					transform.Translate(Vector3.right * speed);
				}
				else{
					transform.Translate(Vector3.left * speed);
				}
			}
			else if(hasHit)
			{
				if(!hasDestroyed && (animator.GetCurrentAnimatorStateInfo(0).length / 2f) + creationStartTime < Time.time)
				{
					Destroy (gameController.grid[row,col]);
					gameController.colorGrid[row,col] = null;
					gameController.grid[row,col] = null;
					hasDestroyed = true;
				}
				if(animator.GetCurrentAnimatorStateInfo(0).length + creationStartTime < Time.time)
				{
					float animPlayLength = animator.GetCurrentAnimatorStateInfo(0).length * (1/animator.speed);
					if(animPlayLength + creationStartTime < Time.time)
					{
						reset ();
						spellHandler.Red_Spell_Helper();
					}
				}

			}
		}
	}

	public void Activate()
	{
		activated = true;
		creationAnimator.SetBool ("isCreated", true);
		creationStartTime = Time.time;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Piece" && isMoving) {
			if(other.GetComponent<piece_script>().inSplitter || other.GetComponent<piece_script>().inSideHolder)
			{
				spellHandler.Red_Spell_Helper();
				reset ();
				return;
			}
			isMoving = false;
			hasHit = true;
			transform.position = other.transform.position;
			creationStartTime = Time.time;
			col = (int) other.GetComponent<piece_script>().gridPos.y;
			if(isRight)
			{
				animator.SetBool("LeftCollided", true);
			}
			else
			{
				animator.SetBool ("RightCollided", true);
			}
		}
		else if (other.name == "Grid Boundaries")
		{
			spellHandler.Red_Spell_Helper();
			reset ();
		}

	}

	void reset()
	{
		animator.SetBool ("LeftCollided", false);
		animator.SetBool ("RightCollided", false);
		animator.SetBool ("inActive", true);
		spriteRenderer.sprite = null;
		transform.position = startPosition;
		isMoving = false;
		hasHit = false;
		activated = false;
		hasDestroyed = false;
		gameObject.SetActive (false);
	}
}