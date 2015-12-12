using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Circle_Rotater : MonoBehaviour {

	//this Script rotates the circles on the title scene

	RectTransform rectTransform;

	public bool Clockwise;

	void Start()
	{
		rectTransform = GetComponent<RectTransform> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(Clockwise){
			rectTransform.Rotate(new Vector3(0,0,1f));
		}
		else {
			rectTransform.Rotate(new Vector3(0,0,-1f));
		}
	}
}
