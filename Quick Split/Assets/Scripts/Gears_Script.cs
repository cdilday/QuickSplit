using UnityEngine;
using System.Collections;

public class Gears_Script : MonoBehaviour {

	bool isRight;
	Animator animator;

	public Sprite[] gears;

	SpriteRenderer spriteRenderer;

	int index;

	bool movingUp;

	// Use this for initialization
	void Start () {
		if(transform.position.x < 0)
			isRight = false;
		else
			isRight = true;
		if( GetComponent<Animator>())
		{
			animator = GetComponent<Animator> ();
			animator.SetBool ("isMoving", false);
			animator.SetBool ("Clockwise", false);
		}
		if (GetComponent<SpriteRenderer> ()) {
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		index = 0;
	}

	void FixedUpdate()
	{
		/*int index = ((int)GetComponentInParent<Transform> ().position.y) % gears.Length ;
		spriteRenderer.sprite = gears [index];*/
	}

	public void Going_Up()
	{
		if (isRight) {
			spriteRenderer.sprite = next_sprite ();
		}
		else
		{
			spriteRenderer.sprite = prev_sprite ();
		}
		movingUp = true;
	}

	public void Going_Down()
	{
		if (isRight) {
			spriteRenderer.sprite = prev_sprite ();
		}
		else
		{
			spriteRenderer.sprite = next_sprite ();
		}
		movingUp = true;
	}

	public void Stopping()
	{
		if(movingUp){
			if (isRight) {
				spriteRenderer.sprite = next_sprite ();
			}
			else
			{
				spriteRenderer.sprite = prev_sprite ();
			}
		}
		else
		{
			if (isRight) {
				spriteRenderer.sprite = prev_sprite ();
			}
			else
			{
				spriteRenderer.sprite = next_sprite ();
			}
		}
		/*animator.SetBool ("isMoving", false);*/
	}

	Sprite next_sprite()
	{
		if (index < gears.Length - 1) {
			index++;
		} 
		else{
			index = 0;
		}
		return gears[index];
	}

	Sprite prev_sprite()
	{
		if (index == 0) {
			index = gears.Length -1;
		} 
		else{
			index--;
		}
		return gears[index];
	}
}
