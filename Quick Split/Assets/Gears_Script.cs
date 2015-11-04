using UnityEngine;
using System.Collections;

public class Gears_Script : MonoBehaviour {

	bool isRight;
	Animator animator;

	// Use this for initialization
	void Start () {
		if(transform.position.x < 0)
			isRight = false;
		else
			isRight = true;
		animator = GetComponent<Animator> ();
		animator.SetBool ("isMoving", false);
		animator.SetBool ("Clockwise", false);
	}

	public void Going_Up()
	{
		animator.SetBool ("isMoving", true);
		if (isRight) {
			animator.SetBool ("Clockwise", false);
		}
		else
		{
			animator.SetBool ("Clockwise", true);
		}
	}

	public void Going_Down()
	{
		animator.SetBool ("isMoving", true);
		if (isRight) {
			animator.SetBool ("Clockwise", true);
		}
		else
		{
			animator.SetBool ("Clockwise", false);
		}
	}

	public void Stopping()
	{
		animator.SetBool ("isMoving", false);
	}
}
