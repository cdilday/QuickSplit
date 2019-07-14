using UnityEngine;
using UnityEngine.UI;

public class SamplePieceSelector : MonoBehaviour
{

    //this script puts the correct sprite on the given pieces on the various creens showing pieces, and also handles option menu selection

    public PieceColorer[] samplePieces;
    public int index = 0;
    public Text headerText;
    private ScoreAndAchievementHandler achievementHandler;


    // Use this for initialization
    private void Start()
    {
        achievementHandler = GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<ScoreAndAchievementHandler>();
        PieceSets activePieceSet = (PieceSets) PlayerPrefs.GetInt(Constants.PieceSetOption, (int)PieceSets.Default);
        headerText.text = Piece_Set_Helper.Get_Pieceset_Name(activePieceSet);

        index = (int)activePieceSet;
    }

    public void LeftButton()
    {
        do
        {
            if (index == 0)
            {
                index = Piece_Set_Helper.PieceSetStrings.Length - 1;
            }
            else
            {
                index--;
            }
        } while (!achievementHandler.isPiecesetUnlocked((PieceSets)index));

        PlayerPrefs.SetInt(Constants.PieceSetOption, index);
        headerText.text = Piece_Set_Helper.PieceSetStrings[index];
        foreach (PieceColorer pc in samplePieces)
        {
            pc.updateColor();
        }
    }

    public void RightButton()
    {
        do
        {
            if (index == Piece_Set_Helper.PieceSetStrings.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        } while (!achievementHandler.isPiecesetUnlocked((PieceSets)index));

        PlayerPrefs.SetInt(Constants.PieceSetOption, index);
        headerText.text = Piece_Set_Helper.PieceSetStrings[index];
        foreach (PieceColorer pc in samplePieces)
        {
            pc.updateColor();
        }

    }
}
