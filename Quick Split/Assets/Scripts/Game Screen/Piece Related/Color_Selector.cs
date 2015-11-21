using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Color_Selector : MonoBehaviour {

	public Text selectionText;
	Splitter_script splitter;
	SpellHandler spellHandler;

	// Use this for initialization
	void Awake () {
		selectionText = GameObject.Find ("Color Selector Text").GetComponent<Text> ();
		//selectionText.pixelOffset = new Vector2 (Screen.width / 2f, Screen.height / 2f);
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
		selectionText.text = "";
		Destroy (gameObject);
	}

	void onDestroy()
	{
		selectionText.text = "";
	}
}
