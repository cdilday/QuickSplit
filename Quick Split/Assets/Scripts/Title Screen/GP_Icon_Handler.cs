using UnityEngine;
using System.Collections;

public class GP_Icon_Handler : MonoBehaviour {

	// This script will change the color of the google Play Button depending one whether or not the player is connected
	// if it isn't mobile, then it deletes the button

	public Sprite coloredIcon;
	public Sprite blackenedIcon;

	// Use this for initialization
	void Start () {
		GameObject GPGHObject = GameObject.FindGameObjectWithTag ("Google Play");
		if (GPGHObject == null) {
			Destroy (gameObject);
			return;
		}
	}
}
