using UnityEngine;
using System.Collections;

public class Color_Selector : MonoBehaviour {

	public GUIText selectionText;
	Splitter_script splitter;
	SpellHandler spellHandler;

	// Use this for initialization
	void Start () {
		selectionText.pixelOffset = new Vector2 (Screen.width / 2f, Screen.height / 2f);
		splitter = GameObject.FindGameObjectWithTag ("Splitter").GetComponent<Splitter_script> ();
		splitter.setState ("isActive", false);
		spellHandler = GameObject.Find ("Spell Handler").GetComponent<SpellHandler> ();
	}

	public void givePurpose(string purpose)
	{
		selectionText.text = purpose;
	}

	public void colorSelected(string color)
	{
		spellHandler.colorSelected (color);
		splitter.setState ("inTransition", true);
		Destroy (gameObject);
	}

	void onDestroy()
	{
		//broadcast to spell handler the color selected
		// make the splitter active again
	}
}
