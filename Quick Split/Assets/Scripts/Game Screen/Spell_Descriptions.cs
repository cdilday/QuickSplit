using UnityEngine;
using UnityEngine.UI;

public class Spell_Descriptions : MonoBehaviour
{

    //This script handles the spell desciptions that pop up during the game when the player begins to select a spell

    private GameObject descObject;
    private Text description;

    // Use this for initialization
    private void Start()
    {
        /*popupObject = transform.GetChild (0).gameObject;
		if (popupObject != null) {
			popup = popupObject.GetComponent<Image>();
		}*/
        descObject = transform.GetChild(1).gameObject;
        if (descObject != null)
        {
            description = descObject.GetComponent<Text>();
        }
        hide();
    }

    //this will put the descriptions out of sight, called when the user stops touching a tab
    public void hide()
    {
        gameObject.GetComponent<Canvas>().planeDistance = 25;
        description.text = "";
    }

    //this displays the desc on screen
    public void display(PieceColor color)
    {
        gameObject.GetComponent<Canvas>().planeDistance = 7;
        switch (color)
        {
            case PieceColor.Red:
                description.text = "The Red spell remove the pieces in each row closes to the middle";
                break;
            case PieceColor.Orange:
                description.text = "The Orange spell switches all the pieces of a chosen color on the left with all the pieces of a different chosen color on the right";
                break;
            case PieceColor.Yellow:
                description.text = "The Yellow spell destroys all the pieces in the row the splitter is in";
                break;
            case PieceColor.Green:
                description.text = "The Green spell recolors any 3 pieces in the holder/splitter to the selected colors";
                break;
            case PieceColor.Blue:
                description.text = "The Blue spell recolors a selected piece to a selected color 3 seperate times";
                break;
            case PieceColor.Purple:
                description.text = "The Purple spell deletes all pieces of a selected color on the board";
                break;
            case PieceColor.Cyan:
                description.text = "The Cyan spell turns the splitter's pieces into bombs that will blow up on impact";
                break;
            case PieceColor.White:
                description.text = "The White spell sorts the entire board in color order";
                break;
            default:
                break;
        }
    }

}