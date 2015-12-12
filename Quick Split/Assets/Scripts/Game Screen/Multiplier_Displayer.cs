using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Multiplier_Displayer : MonoBehaviour {

	//This displays the current multiplier on the Game Scene

	GameController gameController;
	Text text;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "x" + gameController.multiplier;
	}

}