using UnityEngine;
using UnityEngine.UI;

public class High_Score_Displayer : MonoBehaviour
{

    //this script is attached to the green high score previews on the game mode select screen and handles those updates

    public GameMode gameType;
    private int score;

    public void update_scores()
    {
        score = PlayerPrefs.GetInt(Game_Mode_Helper.AllRuleSets[(int)gameType].ToString() + Constants.TopScorePredicate, 0);

        if (gameType != GameMode.Custom)
        {
            gameObject.GetComponent<Text>().text = gameType + " High Score: " + score;
        }
        else
        {
            gameObject.GetComponent<Text>().text = "Highest Score with these settings: \n" + score;
        }
    }

}