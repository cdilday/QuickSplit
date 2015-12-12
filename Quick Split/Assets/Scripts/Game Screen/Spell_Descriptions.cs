using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spell_Descriptions : MonoBehaviour {

	//This script handles the spell desciptions that pop up during the game when the player begins to select a spell

	GameObject descObject;
	Text description;
	
	// Use this for initialization
	void Start () {
		/*popupObject = transform.GetChild (0).gameObject;
		if (popupObject != null) {
			popup = popupObject.GetComponent<Image>();
		}*/
		descObject = transform.GetChild (1).gameObject;
		if (descObject != null) {
			description = descObject.GetComponent<Text>();
		}
		hide ();
	}

	//this will put the descriptions out of sight, called when the user stops touching a tab
	public void hide(){
		gameObject.GetComponent<Canvas> ().planeDistance = 25;
		description.text = "";
	}

	//this displays the desc on screen
	public void display(string color){
		gameObject.GetComponent<Canvas> ().planeDistance = 7;
		switch (color) {
		case "Red":
			description.text = "The Red spell remove the pieces in each row closes to the middle";
			break;
		case "Orange":
			description.text = "The Orange spell switches all the pieces of a chosen color on the left with all the pieces of a different chosen color on the right";
			break;
		case "Yellow":
			description.text = "The Yellow spell destroys all the pieces in the row the splitter is in";
			break;
		case "Green":
			description.text = "The Green spell recolors any 3 pieces in the holder/splitter to the selected colors";
			break;
		case "Blue":
			description.text = "The Blue spell recolors a selected piece to a selected color 3 seperate times";
			break;
		case "Purple":
			description.text = "The Purple spell deletes all pieces of a selected color on the board";
			break;
		case "Cyan":
			description.text = "The Cyan spell turns the splitter's pieces into bombs that will blow up on impact";
			break;
		case "White":
			description.text = "The White spell sorts the entire board in color order";
			break;
		default:
			break;
		}
	}

}