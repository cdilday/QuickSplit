using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Circle_Rotater : MonoBehaviour {

	//this Script rotates the circles on the title scene

	RectTransform rectTransform;

	public float speed;
	public bool randomize;

	public bool Clockwise;

	void Start()
	{
		rectTransform = GetComponent<RectTransform> ();
		if (randomize) {
			speed = Random.Range(-10f, 10f);
		}
		if (speed == 0)
			speed = 1f;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(Clockwise){
			rectTransform.Rotate(new Vector3(0,0,speed));
		}
		else {
			rectTransform.Rotate(new Vector3(0,0,speed));
		}
	}
}
