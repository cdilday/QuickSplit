using UnityEngine;
using System.Collections;

public class Spell_Text : MonoBehaviour {

	//this script handles the spell progress text on the bottom of the game screen

	public string spellColor;
	GameObject description;
	Vector3 startPos;

	void Start()
	{
		description = transform.GetChild(0).gameObject;
		startPos = description.transform.position;
		description.transform.position = new Vector3 (startPos.x + 20, startPos.y, startPos.z);
	}

	void OnMouseOver()
	{
		description.transform.position = startPos;
	}

	void OnMouseExit()
	{
		description.transform.position = new Vector3 (startPos.x + 20, startPos.y, startPos.z);
	}

}