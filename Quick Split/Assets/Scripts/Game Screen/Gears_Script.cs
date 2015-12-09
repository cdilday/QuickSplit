using UnityEngine;
using System.Collections;

public class Gears_Script : MonoBehaviour {

	bool isRight;
	Animator animator;

	public Sprite[] gears;

	SpriteRenderer spriteRenderer;

	Splitter_script splitter;

	// Use this for initialization
	void Start () {
		splitter = transform.parent.GetComponent<Splitter_script> ();
		if(transform.position.x < 0)
			isRight = false;
		else
			isRight = true;
	}

	void FixedUpdate()
	{
		if (splitter.getState("isMoving")) {
			//it's moving, find direction
			if(splitter.moveDirection == 1)
			{
				//it's going up
				if(isRight){
					transform.Rotate(new Vector3(0,0,-20f));
				}
				else {
					transform.Rotate(new Vector3(0,0,-20f));
				}
			}
			else{
				//it's going down
				if(isRight){
					transform.Rotate(new Vector3(0,0,20f));
				}
				else {
					transform.Rotate(new Vector3(0,0,-20f));
				}
			}
		}

	}
	
}
