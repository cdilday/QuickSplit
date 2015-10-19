﻿using UnityEngine;
using System.Collections;

public class Spell_Tab : MonoBehaviour {

	/* Spell tabs are activated when the spell is ready in the handler
	 * they pop up on the bottom, hovering over them(or holding onto them)
	 * will spawn the explination text in the center
	 * clicking (or tapping quickly, maybe pulling up) will activate the power
	 */

	public string spellColor;

	Splitter_script splitter;

	SpellHandler spellHandler;
	GameObject DescCanvas;

	bool isReady;

	void Start () {
		transform.position = new Vector3 (transform.position.x, transform.position.y - 3f, transform.position.z);
		GameObject shObject = transform.parent.gameObject;
		if (shObject != null) {
			spellHandler = shObject.GetComponent<SpellHandler>();
		}
		isReady = false;
		DescCanvas = GameObject.Find ("Description Canvas");
		splitter = GameObject.FindGameObjectWithTag ("Splitter").GetComponent<Splitter_script> ();
	}

	void FixedUpdate()
	{
		if(!isReady)
		{
			bool checkColor;
			switch (spellColor) {
			case "Red":
				checkColor = spellHandler.redReady;
				break;
			case "Orange":
				checkColor = spellHandler.orangeReady;
				break;
			case "Yellow":
				checkColor = spellHandler.yellowReady;
				break;
			case "Green":
				checkColor = spellHandler.greenReady;
				break;
			case "Blue":
				checkColor = spellHandler.blueReady;
				break;
			case "Purple":
				checkColor = spellHandler.purpleReady;
				break;
			case "Grey":
				checkColor = spellHandler.greyReady;
				break;
			case "White":
				checkColor = spellHandler.whiteReady;
				break;
			default:
				checkColor = false;
				break;
			}
			if (checkColor) {
				isReady = true;
				transform.position = new Vector3 (transform.position.x, transform.position.y + 3f, transform.position.z);
			}
		}

	}

	void OnMouseOver()
	{
		//prevent mouse controls from firing when you click on the tab
		splitter.setState ("canShoot", false);
		DescCanvas.GetComponent<Spell_Descriptions> ().display (spellColor);
	}
	
	void OnMouseExit()
	{
		splitter.setState ("canShoot", true);
		DescCanvas.GetComponent<Spell_Descriptions> ().hide ();
	}

	void OnMouseDown()
	{
		if(isReady)
		{
			switch (spellColor) {
			case "Red":
				spellHandler.Redspell();
				break;
			case "Orange":
				spellHandler.Orangespell();
				break;
			case "Yellow":
				spellHandler.Yellowspell();
				break;
			case "Green":
				spellHandler.Greenspell();
				break;
			case "Blue":
				spellHandler.Bluespell();
				break;
			case "Purple":
				spellHandler.Purplespell();
				break;
			case "Grey":
				spellHandler.Greyspell();
				break;
			case "White":
				spellHandler.Whitespell();
				break;
			}
			transform.position = new Vector3 (transform.position.x, transform.position.y - 3f, transform.position.z);
			isReady = false;
		}
	}

}