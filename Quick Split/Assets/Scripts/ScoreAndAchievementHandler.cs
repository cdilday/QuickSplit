using System.Collections;
using UnityEngine;

public class ScoreAndAchievementHandler : MonoBehaviour
{
    //this is for checking specifically for the cyan splitter unlock, as it requires timing
    private bool cyanCheck;
    private float startTime;

    public bool[] splittersUnlocked;
    public bool[] piecesetsUnlocked;

    public AchievementNotification notification;

    public static int NumberOfCustomScores;

    // Use this for initialization
    private void Awake()
    {
        //there should only ever be 1 of these
        DontDestroyOnLoad(transform.gameObject);
        //get rid of redundant Achievement Handlers
        GameObject[] mcs = GameObject.FindGameObjectsWithTag("Achievement Handler");
        if (mcs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        //make sure default sets are always unlocked at start of game to prevent crashes
        PlayerPrefs.SetInt(GameMode.Wiz + Constants.GameModeUnlockedPredicate, 1);
        PlayerPrefs.SetInt(GameMode.Custom + Constants.GameModeUnlockedPredicate, 1);
        PlayerPrefs.SetInt(SplitterType.Default + Constants.SplitterUnlockedPredicate, 1);
        PlayerPrefs.SetInt(PieceSets.Default +Constants.PieceSetUnlockedPredicate, 1);
        PlayerPrefs.SetInt(PieceSets.Symbol + Constants.PieceSetUnlockedPredicate, 1);

        NumberOfCustomScores = PlayerPrefs.GetInt(Constants.CustomHighScoreCountNumLookup, 0);

        splittersUnlocked = new bool[Splitter_Helper.SplitterStrings.Length];
        piecesetsUnlocked = new bool[Piece_Set_Helper.PieceSetStrings.Length];

        //load the arrays for unlocks with the proper values already saved in prefs. This prevents longer lookups later
        for (int i = 0; i < splittersUnlocked.Length; i++)
        {
            if (PlayerPrefs.GetInt((SplitterType)i + Constants.SplitterUnlockedPredicate, 0) == 0)
            {
                splittersUnlocked[i] = false;
            }
            else
            {
                splittersUnlocked[i] = true;
            }
        }

        for (int i = 0; i < piecesetsUnlocked.Length; i++)
        {
            if (PlayerPrefs.GetInt((PieceSets)i + Constants.PieceSetUnlockedPredicate, 0) == 0)
            {
                piecesetsUnlocked[i] = false;
            }
            else
            {
                piecesetsUnlocked[i] = true;
            }
        }
        //the programmer unlocks require all other things to be unlocked, so do that check now
        if (PlayerPrefs.GetInt(SplitterType.Programmer + Constants.SplitterUnlockedPredicate, 0) == 0)
        {
            bool check = true;
            for (int i = 0; i < splittersUnlocked.Length; i++)
            {
                if (splittersUnlocked[i] == false && (SplitterType)i != SplitterType.Programmer)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                UnlockSplitter(SplitterType.Programmer);
            }
        }

        if (PlayerPrefs.GetInt(PieceSets.Programmer + Constants.PieceSetUnlockedPredicate, 0) == 0)
        {
            bool check = true;
            for (int i = 0; i < piecesetsUnlocked.Length; i++)
            {
                if (piecesetsUnlocked[i] == false && (PieceSets)i != PieceSets.Programmer)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                UnlockPieceset(PieceSets.Programmer);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //debug buttons
        // numpad 1 resets gamemode unlocks
        // numpad 5 resets all the unlocks
        // numpad 7 unlocks all the things
        // numpad 6 unlocks the pumpkin pieceset
        if (Debug.isDebugBuild)
        {
            if (Input.GetKey(KeyCode.Keypad5))
            {
                PlayerPrefs.SetInt(GameMode.Wiz + Constants.GameModeUnlockedPredicate, 1);
                PlayerPrefs.SetInt(GameMode.Quick + Constants.GameModeUnlockedPredicate, 0);
                PlayerPrefs.SetInt(GameMode.Wit + Constants.GameModeUnlockedPredicate, 0);
                PlayerPrefs.SetInt(GameMode.Holy + Constants.GameModeUnlockedPredicate, 0);
                PlayerPrefs.SetInt(GameMode.Custom + Constants.GameModeUnlockedPredicate, 1);
                Debug.Log("Unlocked GameModes Reset!");
                for (int i = 1; i < Splitter_Helper.SplitterStrings.Length; i++)
                {
                    splittersUnlocked[i] = false;
                    PlayerPrefs.SetInt((SplitterType)i + Constants.SplitterUnlockedPredicate, 0);
                }
                PlayerPrefs.SetInt(SplitterType.Default + Constants.SplitterUnlockedPredicate, 1);
                Debug.Log("Unlocked Splitters Reset!");
                for (int i = 1; i < Piece_Set_Helper.PieceSetStrings.Length; i++)
                {
                    piecesetsUnlocked[i] = false;
                    PlayerPrefs.SetInt((PieceSets)i + Constants.PieceSetUnlockedPredicate, 0);
                }
                PlayerPrefs.SetInt(PieceSets.Default + Constants.PieceSetUnlockedPredicate, 1);
                UnlockPieceset(PieceSets.Symbol);
                Debug.Log("Unlocked Piecesets Reset!");

            }
            else if (Input.GetKey(KeyCode.Keypad7))
            {
                PlayerPrefs.SetInt(GameMode.Wiz + Constants.GameModeUnlockedPredicate, 1);
                PlayerPrefs.SetInt(GameMode.Quick + Constants.GameModeUnlockedPredicate, 1);
                PlayerPrefs.SetInt(GameMode.Wit + Constants.GameModeUnlockedPredicate, 1);
                PlayerPrefs.SetInt(GameMode.Holy + Constants.GameModeUnlockedPredicate, 1);
                PlayerPrefs.SetInt(GameMode.Custom + Constants.GameModeUnlockedPredicate, 1);
                Debug.Log("All GameModes Unlocked!");
                for (int i = 0; i < Splitter_Helper.SplitterStrings.Length; i++)
                {
                    splittersUnlocked[i] = true;
                    PlayerPrefs.SetInt((SplitterType)i + Constants.SplitterUnlockedPredicate, 1);
                }
                Debug.Log("All Splitters Unlocked!");
                for (int i = 0; i < Piece_Set_Helper.PieceSetStrings.Length; i++)
                {
                    piecesetsUnlocked[i] = true;
                    PlayerPrefs.SetInt((PieceSets)i + Constants.PieceSetUnlockedPredicate, 1);
                }
                Debug.Log("All Piecesets Unlocked!");
            }
            else if (Input.GetKey(KeyCode.Keypad6))
            {
                UnlockPieceset(PieceSets.Pumpkin);
            }
            else if (Input.GetKey(KeyCode.Keypad1))
            {
                Debug.Log("Wiz " + PlayerPrefs.GetInt(GameMode.Wiz + Constants.GameModeUnlockedPredicate, 1));
                Debug.Log("Quick " + PlayerPrefs.GetInt(GameMode.Quick + Constants.GameModeUnlockedPredicate, 0));
                Debug.Log("Wit " + PlayerPrefs.GetInt(GameMode.Wit + Constants.GameModeUnlockedPredicate, 0));
                Debug.Log("Holy " + PlayerPrefs.GetInt(GameMode.Holy + Constants.GameModeUnlockedPredicate, 0));
                Debug.Log("Custom " + PlayerPrefs.GetInt(GameMode.Custom + Constants.GameModeUnlockedPredicate, 1));
            }
        }
    }

    /// <summary>
    /// Takes in a high score and adds it apropriately to the corresponding list in score order. if it's lower than the top 15 for that mode, it's discarded
    /// Custom scores only store the top score for the ruleset
    /// </summary>
    /// <param name="ruleSet"> RuleSet - the ruleset the player scored in</param>
    /// <param name="score"> int - the score</param>
    public void AddScore(RuleSet ruleSet, int score)
    {
        //you need to actually score to save a high score
        if (score == 0)
        {
            return;
        }

        int tempScore = score;

        if (ruleSet.Mode != GameMode.Custom)
        {
            for (int i = 0; i < 15; i++)
            {
                int currScore = PlayerPrefs.GetInt(ruleSet.ToString() + Constants.ScoreLookup + i, 0);
                if (currScore == 0)
                {
                    //We've hit the end of the list. Place the score here and exit
                    PlayerPrefs.SetInt(ruleSet.ToString() + Constants.ScoreLookup + i, tempScore);
                    break;
                }
                else if (currScore <= tempScore)
                {
                    //continue looking through the list
                    PlayerPrefs.SetInt(ruleSet.ToString() + Constants.ScoreLookup + i, tempScore);
                    tempScore = currScore;
                }
            }
        }
        else
        {
            // Custom game modes are unique per ruleset, we only keep the highest score here
            int topScore = PlayerPrefs.GetInt(ruleSet.ToString() + Constants.TopScorePredicate, 0);

            if (score > topScore)
            {
                if (topScore == 0)
                {
                    PlayerPrefs.SetString(Constants.CustomHighScoreRulesetLookup + NumberOfCustomScores, ruleSet.ToString());
                    NumberOfCustomScores++;
                    PlayerPrefs.SetInt(Constants.CustomHighScoreCountNumLookup, NumberOfCustomScores);
                }

                PlayerPrefs.SetInt(ruleSet.ToString() + Constants.TopScorePredicate, score);
            }
        }
    }

    private void FixedUpdate()
    {
        //More cyan unlock stuff. This resets it if the time has passed
        if (cyanCheck && startTime + 3f < Time.time)
        {
            cyanCheck = false;
        }
    }

    /// <summary>
    /// Unlocks the given splitter and shows the achievement notification
    /// </summary>
    /// <param name="splitterType"> SplitterType - the splitter unlocked</param>
    public void UnlockSplitter(SplitterType splitterType)
    {
        splittersUnlocked[(int)splitterType] = true;
        PlayerPrefs.SetInt(name + Constants.SplitterUnlockedPredicate, 1);
        if (notification != null)
        {
            notification.AchievementUnlocked(splitterType);
        }
    }

    /// <summary>
    /// Checks if the given the splitter is unlocked
    /// </summary>
    public bool isSplitterUnlocked(SplitterType splitter)
    {
        return splittersUnlocked[(int)splitter];
    }

    /// <summary>
    /// Unlocks the given pieceset and displays the notification for it
    /// </summary>
    /// <param name="pieceSet"> PieceSets - the piece set unlock</param>
    public void UnlockPieceset(PieceSets pieceSet)
    {
        piecesetsUnlocked[(int)pieceSet] = true;
        PlayerPrefs.SetInt(pieceSet + Constants.PieceSetUnlockedPredicate, 1);

        if (notification != null)
        {
            notification.AchievementUnlocked(pieceSet);
        }
    }

    /// <summary>
    /// Returns true or false depending if the given pieceset is unlocked
    /// </summary>
    public bool isPiecesetUnlocked(PieceSets pieceSet)
    {
        return piecesetsUnlocked[(int)pieceSet];
    }

    /// <summary>
    /// Checks which gamemodes should be unlocks and unlocks them if needed
    /// </summary>
    public void UpdateUnlockedGameModes()
    {
        //the unlock order goes from Wiz -> Quick -> Wit -> Holy
        if (PlayerPrefs.GetInt(Game_Mode_Helper.AllRuleSets[(int)GameMode.Wiz].ToString() + Constants.TopScorePredicate, 0) > 0)
        {
            PlayerPrefs.SetInt(GameMode.Quick + Constants.GameModeUnlockedPredicate, 1);
        }

        if (PlayerPrefs.GetInt(Game_Mode_Helper.AllRuleSets[(int)GameMode.Quick].ToString() + Constants.TopScorePredicate, 0) > 0)
        {
            PlayerPrefs.SetInt(GameMode.Wit + Constants.GameModeUnlockedPredicate, 1);
        }

        if (PlayerPrefs.GetInt(Game_Mode_Helper.AllRuleSets[(int)GameMode.Wit].ToString() + Constants.TopScorePredicate, 0) > 0)
        {
            PlayerPrefs.SetInt(GameMode.Holy + Constants.GameModeUnlockedPredicate, 1);
        }

        if (PlayerPrefs.GetInt(Game_Mode_Helper.AllRuleSets[(int)GameMode.Holy].ToString() + Constants.TopScorePredicate, 0) > 0)
        {
            if (isPiecesetUnlocked(PieceSets.Present) == false)
            {
                UnlockPieceset(PieceSets.Present);
            }
            PlayerPrefs.SetInt(GameMode.Custom + Constants.GameModeUnlockedPredicate, 1);
        }
    }

    /// <summary>
    /// This is for the conditions for unlocking the cyan splitter. It relies on timing being different for bombs, and receiving 
    /// that 18 pieces were deleted by bombs. The first time this is called it sets the start time, the second it checks if the
    /// conditions were met (determined in FixedUpdate)
    /// </summary>
    public void CyanSplitterChecker()
    {
        if (splittersUnlocked[(int)SplitterType.Cyan])
        {
            return;
        }
        else if (cyanCheck)
        {
            UnlockSplitter(SplitterType.Cyan);
        }
        else
        {
            startTime = Time.time;
            cyanCheck = true;
        }
    }

    /// <summary>
    /// Checks the conditions for unlocking the Purple splitter, which requires that 3 pieces in danger of ending the game were destroyed
    /// from the use of the purple spell
    /// </summary>
    /// <param name="oldDangerPieces"> int - number of pieces that are in danger of causing a game over</param>
    public IEnumerator PurpleSplitterChecker(int oldDangerPieces)
    {
        if (!isSplitterUnlocked(SplitterType.Purple) && oldDangerPieces < 3)
        {
            yield return new WaitForSeconds(3);
            if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Get_Danger_Pieces() == 0)
            {
                UnlockSplitter(SplitterType.Purple);
            }
        }
    }

    /// <summary>
    /// Handles the conditions fr unlocking the Blue Splitter, which gets unlocked when use the blue spell causes the destruction of
    /// at least 16 pieces
    /// </summary>
    public IEnumerator BlueSplitterChecker()
    {
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        int oldCount = 0;
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 16; c++)
            {
                if (gameController.grid[r, c] != null)
                {
                    oldCount++;
                }
            }
        }
        yield return new WaitForSeconds(2f);
        int newCount = 0;
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 16; c++)
            {
                if (gameController.grid[r, c] != null)
                {
                    newCount++;
                }
            }
        }

        if (oldCount - newCount >= 16)
        {
            UnlockSplitter(SplitterType.Blue);
        }
    }

}
