using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class High_Score_Calculator : MonoBehaviour
{

    //This script handles the High score list on the high score menu in the Main Menu
    //High scores are stored using the lookup "<Gamemode> score <#>" where <Gamemode> is the short name for the mode and <#> is the score's rank

    public Text[] ScoreList;
    public VerticalScrollSnap Scopes;
    public HorizontalScrollSnap Modes;
    private int currScope;
    private int prevScope;
    private int currMode;
    private int prevMode;

    // Use this for initialization
    private void Start()
    {
        currScope = 0;
        prevScope = -1;
        currMode = 0;
        prevMode = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //check to make sure the scores for the proper scope and mode are on display, and if they aren't, change them
        if (Scopes.gameObject.activeInHierarchy)
        {
            currScope = Scopes.CurrentPage;
            currMode = Modes.CurrentPage;
            if (currScope != prevScope || currMode != prevMode)
            {
                Populate_Scores_List();
            }
            prevScope = currScope;
            prevMode = currMode;
        }
    }

    //this actually fills the list in
    public void Populate_Scores_List()
    {
        if (Lookup_Scope(Scopes.CurrentPage) == "Local")
        {
            GameMode gameMode = (GameMode)Modes.CurrentPage;
            for (int i = 0; i < 15; i++)
            {
                int currScore = PlayerPrefs.GetInt(gameMode + " score " + i, 0);
                if (currScore == 0)
                {
                    ScoreList[i].text = (i + 1) + ". -------";
                }
                else
                {
                    ScoreList[i].text = (i + 1) + ". " + currScore;
                }
            }
        }
        else
        {
            //TODO: Implement global and friends lists for real
            for (int i = 0; i < 14; i++)
            {
                ScoreList[i].text = (i + 1) + " -------";
            }
        }
    }



    //this resets all scores locally
    public void Reset_All_Scores()
    {
        for (int g = 0; g < 4; g++)
        {
            for (int i = 0; i < 15; i++)
            {
                PlayerPrefs.SetInt((GameMode)g + " score " + i, 0);
            }
        }

        // reset custom top scores. Stores in strings which need to be looked up
        for (int i = 0; i < Achievement_Script.NumberOfCustomScores; i++)
        {
            string rulesetStr = PlayerPrefs.GetString(Constants.CustomHighScoreRulesetLookup + i.ToString());
            PlayerPrefs.DeleteKey(rulesetStr + Constants.TopScorePredicate);
            PlayerPrefs.DeleteKey(Constants.CustomHighScoreRulesetLookup + i.ToString());
        }

        PlayerPrefs.SetInt(Constants.CustomHighScoreCountNumLookup, 0);
        Achievement_Script.NumberOfCustomScores = 0;
        //This will not affect server scores (if they existed)
    }


    //This reutrns the name of the scope at the given index
    private string Lookup_Scope(int index)
    {
        switch (index)
        {
            case 0:
                return "Local";
            case 1:
                return "Friends";
            case 2:
                return "Global";
            default:
                return "Local";
        }
    }

    public void LoadUsersandScores(ILeaderboard lb)
    {
        ScoreList[0].text = "successfully called load";
        if (lb == null)
        {
            ScoreList[1].text = "but failed to get HS";
        }
        else
        {
            ScoreList[1].text = lb.localUserScore.ToString();
        }
    }

}