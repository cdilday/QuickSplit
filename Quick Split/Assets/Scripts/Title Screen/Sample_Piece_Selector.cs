using UnityEngine;
using UnityEngine.UI;

public class Sample_Piece_Selector : MonoBehaviour
{

    //this script puts the correct sprite on the given pieces on the various creens showing pieces, and also handles option menu selection

    public Piece_Colorer[] samplePieces;
    public int index = 0;
    public Text headerText;
    private Achievement_Script achievementHandler;


    // Use this for initialization
    private void Start()
    {
        achievementHandler = GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<Achievement_Script>();
        headerText.text = PlayerPrefs.GetString("Piece Set", "Default");

        //TODO: Extract this out
        switch (headerText.text)
        {
            case "Default":
                index = 0;
                break;
            case "Arcane":
                index = 1;
                break;
            case "Retro":
                index = 2;
                break;
            case "Programmer":
                index = 3;
                break;
            case "Blob":
                index = 4;
                break;
            case "Domino":
                index = 5;
                break;
            case "Present":
                index = 6;
                break;
            case "Pumpkin":
                index = 7;
                break;
            case "Symbol":
                index = 8;
                break;
            case "Techno":
                index = 9;
                break;
            default:
                index = 0;
                break;
        }
    }

    public void Left_Button()
    {
        do
        {
            if (index == 0)
            {
                index = PieceSetHelper.PieceSetStrings.Length - 1;
            }
            else
            {
                index--;
            }
        } while (!achievementHandler.is_Pieceset_Unlocked((PieceSets)index));

        PlayerPrefs.SetString("Piece Set", ((PieceSets)index).ToString());
        headerText.text = PieceSetHelper.PieceSetStrings[index];
        foreach (Piece_Colorer pc in samplePieces)
        {
            pc.update_color();
        }
    }

    public void Right_Button()
    {
        do
        {
            if (index == PieceSetHelper.PieceSetStrings.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        } while (!achievementHandler.is_Pieceset_Unlocked((PieceSets)index));

        PlayerPrefs.SetString("Piece Set", PieceSetHelper.PieceSetStrings[index]);
        headerText.text = PieceSetHelper.PieceSetStrings[index];
        foreach (Piece_Colorer pc in samplePieces)
        {
            pc.update_color();
        }

    }
}
