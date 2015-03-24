using UnityEngine;
using System.Collections;

public class Color_Selector : MonoBehaviour {

	public GUIText selectionText;
	Splitter_script splitter;
	PowerHandler powerHandler;

	// Use this for initialization
	void Start () {
		selectionText.pixelOffset = new Vector2 (Screen.width / 2f, Screen.height / 2f);
		splitter = GameObject.FindGameObjectWithTag ("Splitter").GetComponent<Splitter_script> ();
		splitter.setState ("isActive", false);
		powerHandler = GameObject.Find ("Power Handler").GetComponent<PowerHandler> ();
	}

	public void givePurpose(string purpose)
	{
		selectionText.text = purpose;
	}

	public void colorSelected(string color)
	{
		powerHandler.colorSelected (color);
		Destroy (gameObject);
	}

	void onDestroy()
	{
		//broadcast to power handler the color selected
		// make the splitter active again
	}
}
