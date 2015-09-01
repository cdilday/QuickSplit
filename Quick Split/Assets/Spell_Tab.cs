using UnityEngine;
using System.Collections;

public class Spell_Tab : MonoBehaviour {

	/* Spell tabs are activated when the spell is ready in the handler
	 * they pop up on the bottom, hovering over them(or holding onto them)
	 * will spawn the explination text in the center
	 * clicking (or tapping quickly, maybe pulling up) will activate the power
	 */

	public string spellColor;
	GameObject description;
	Vector3 textStartPos;

	SpellHandler spellHandler;

	bool isReady;

	void Start () {
		transform.position = new Vector3 (transform.position.x, transform.position.y - 3f, transform.position.z);
		GameObject shObject = transform.parent.gameObject;
		if (shObject != null) {
			spellHandler = shObject.GetComponent<SpellHandler>();
		}
		description = transform.GetChild(0).gameObject;
		textStartPos = description.transform.position;
		description.transform.position = new Vector3 (textStartPos.x + 20, textStartPos.y, textStartPos.z);
		isReady = false;
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
		description.transform.position = textStartPos;
	}
	
	void OnMouseExit()
	{
		description.transform.position = new Vector3 (textStartPos.x + 20, textStartPos.y, textStartPos.z);
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
		}
	}

}
