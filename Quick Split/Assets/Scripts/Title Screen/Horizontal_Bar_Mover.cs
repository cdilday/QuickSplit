using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Horizontal_Bar_Mover : MonoBehaviour {

	//this script moves the Bars on the title screen properly

	public bool isHorizontal;
	public float upLimit;
	public float leftLimit;
	RectTransform rectTransform;

	public float speed;
	public Vector2 ResetPosition;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//if this bar is moving right to left
		if (isHorizontal) {
			//if it's hit the boundary;
			if(rectTransform.localPosition.x < leftLimit){
				rectTransform.localPosition = ResetPosition;
			}
			else{
				//move it left slightly
				rectTransform.Translate(speed, 0, 0);
			}
		}
		// else this bar is moving up and down
		else {
			//if it's hit the boundary;
			if(rectTransform.localPosition.y >= upLimit){
				rectTransform.localPosition = ResetPosition;
			}
			else{
				//move it left slightly
				rectTransform.Translate(0, speed, 0);
			}
		}
	}
}
