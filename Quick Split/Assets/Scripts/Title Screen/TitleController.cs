using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TitleController : MonoBehaviour
{

    //This script handles most of the important UI and movements on the main menu, and acts on other objects in the menu as well

    public int versionNumber;

    public GameObject gameModeLayer;
    public GameObject howToPlayLayer;
    public GameObject creditsLayer;
    public GameObject optionsLayer;
    public GameObject highScoreLayer;
    public GameObject achievementLayer;
    public Shutter_Handler shutter;
    public VerticalScrollSnap gameModeScroller;

    public GameObject ScrollUp;
    public GameObject ScrollDown;

    public High_Score_Displayer[] hsds = new High_Score_Displayer[4];
    private int resetPresses = 0;
    private Achievement_Script achievementHandler;
    private Music_Controller mc;
    private High_Score_Calculator highScoreCalculator;

    //gameobjects needed for transitions b/w game mode select and description scenes
    public GameObject[] GameButtons = new GameObject[4];
    private Sprite[] OrigButtonSprite = new Sprite[4];
    public Sprite lockedSprite;
    public GameObject[] Descriptions = new GameObject[4];
    private string[] OrigDescText = new string[4];
    public GameObject[] Scores = new GameObject[4];

    public GameObject PlayButton;
    private Text playButtonText;
    public GameObject BackButton;
    private bool isInPlayScreen;

    public int activeMode;
    private int prevMode;

    //Specifically for the Pumpkin Achievement code
    private int codeStage = 0;

    public GameObject HTPEmphasizer;

    #region Custom mode controls

    public Toggle CustomSpellToggle;
    public Toggle CustomUnlockOverTimeToggle;
    public Text CustomSidesText;
    public Text CustomUnlockedColorCountText;
    public GameObject CustomTimedSidesOptions;
    public GameObject CustomSplitSidesOptions;
    public Text CustomSecondsSelector;
    private int secondsPerCrunch = 20;
    public Text CustomSplitsSelector;
    private int splitsPerCrunch = 16;
    public string[] SideOptions = { "None", "Timed", "Split-Based" };
    private int sideOptionIndex = 0;
    private int unlockedColorCount = 3;

    #endregion

    // Use this for initialization
    private void Start()
    {
        highScoreCalculator = GameObject.Find("High Score Calculator").GetComponent<High_Score_Calculator>();
        //first check if they're using the most recent version of the game
        if (PlayerPrefs.GetInt("Version", 0) != versionNumber)
        {
            highScoreCalculator.Reset_All_Scores();
            foreach (High_Score_Displayer hsd in hsds)
            {
                hsd.update_scores();
            }
            PlayerPrefs.SetInt("Version", versionNumber);
        }
        achievementHandler = GameObject.Find("Achievement Handler").GetComponent<Achievement_Script>();
        Goto_Game_Mode_Layer();
        shutter.Begin_Vertical_Open();

        GameObject MCobject = GameObject.FindGameObjectWithTag("Music Controller");
        mc = MCobject.GetComponent<Music_Controller>();

        playButtonText = PlayButton.GetComponentInChildren<Text>();

        mc.Play_Music("Menu");

        //just in case this is the first time playing, set Wiz to be for sure unlocked
        PlayerPrefs.SetInt("Wiz unlocked", 1);
        //tell achievmement handler to check gamemodes that are supposed to be active
        achievementHandler.Check_Gamemode_Unlocked();

        activeMode = gameModeScroller.ChildObjects.Length - 1;
        prevMode = activeMode;
        ScrollUp.BroadcastMessage("FadeOut", null, SendMessageOptions.DontRequireReceiver);
        for (int i = 0; i < 4; i++)
        {
            OrigButtonSprite[i] = GameButtons[i].GetComponent<Image>().sprite;
            OrigDescText[i] = Descriptions[i].GetComponent<Text>().text;
        }
        GameMode_Unlocker();

        LoadCustomOptions();
        //Set a default for custom 
        OnCustomModeUpdated();

        if (PlayerPrefs.GetInt("Played Before", 0) == 1)
        {
            Destroy(HTPEmphasizer);
        }
    }

    private void Update()
    {
        //Back or escape key compatibility
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if the main screen is active, just exit the application
            if (gameModeLayer.activeSelf)
            {
                Application.Quit();
            }
            else
            {
                Goto_Game_Mode_Layer();
            }
        }
    }

    //loads the proper game mode into the playerprefs for the game scene to read, then loads the game scene
    //This is where the UI for the transition would go
    public void Load_Game()
    {
        activeMode = gameModeScroller.CurrentPage;
        if (activeMode == 0)
        {
            Goto_Credits_Layer();
        }
        else
        {

            // TODO Extract out Game modes, fix mis-matching due to reworking menu order on title Screen
            switch (activeMode)
            {
                case 5:
                    Game_Mode_Helper.ActiveRuleSet = Game_Mode_Helper.AllRuleSets[(int)GameMode.Wiz];
                    break;
                case 4:
                    Game_Mode_Helper.ActiveRuleSet = Game_Mode_Helper.AllRuleSets[(int)GameMode.Quick];
                    break;
                case 3:
                    Game_Mode_Helper.ActiveRuleSet = Game_Mode_Helper.AllRuleSets[(int)GameMode.Wit];
                    break;
                case 2:
                    Game_Mode_Helper.ActiveRuleSet = Game_Mode_Helper.AllRuleSets[(int)GameMode.Holy];
                    break;
                case 1:
                    Game_Mode_Helper.ActiveRuleSet = Game_Mode_Helper.AllRuleSets[(int)GameMode.Custom];
                    break;
            }

            if (Game_Mode_Helper.isGamemodeUnlocked(Game_Mode_Helper.ActiveRuleSet.Mode))
            {
                StartCoroutine("GameTransition");
            }
        }
    }

    //loads the game mode layer and unloads the other layers
    public void Goto_Game_Mode_Layer()
    {
        gameModeLayer.SetActive(true);
        howToPlayLayer.SetActive(false);
        creditsLayer.SetActive(false);
        optionsLayer.SetActive(false);
        highScoreLayer.SetActive(false);
        achievementLayer.SetActive(false);
    }

    //loads the How to Play Layer and unloads the other layers
    public void Goto_How_To_Play_Layer()
    {
        howToPlayLayer.SetActive(true);
        gameModeLayer.SetActive(false);
        creditsLayer.SetActive(false);
        optionsLayer.SetActive(false);
        highScoreLayer.SetActive(false);
        achievementLayer.SetActive(false);
        PlayerPrefs.SetInt("Played Before", 1);
        if (HTPEmphasizer != null)
        {
            Destroy(HTPEmphasizer);
        }
    }

    //loads the Credits layer and unloads the other layers
    public void Goto_Credits_Layer()
    {
        creditsLayer.SetActive(true);
        gameModeLayer.SetActive(false);
        howToPlayLayer.SetActive(false);
        optionsLayer.SetActive(false);
        highScoreLayer.SetActive(false);
        achievementLayer.SetActive(false);
    }

    public void Goto_Options_Layer()
    {
        optionsLayer.SetActive(true);
        howToPlayLayer.SetActive(false);
        gameModeLayer.SetActive(false);
        creditsLayer.SetActive(false);
        highScoreLayer.SetActive(false);
        achievementLayer.SetActive(false);
    }

    public void Goto_High_Score_Layer()
    {
        highScoreLayer.SetActive(true);
        optionsLayer.SetActive(false);
        howToPlayLayer.SetActive(false);
        gameModeLayer.SetActive(false);
        creditsLayer.SetActive(false);
        achievementLayer.SetActive(false);
    }

    public void Goto_Achievement_Layer()
    {
        gameModeLayer.SetActive(false);
        howToPlayLayer.SetActive(false);
        creditsLayer.SetActive(false);
        optionsLayer.SetActive(false);
        highScoreLayer.SetActive(false);
        achievementLayer.SetActive(true);
    }

    public void Reset_High_Scores()
    {
        if (resetPresses == 0)
        {
            Text rhst = GameObject.Find("Reset High Scores Text").GetComponent<Text>();
            resetPresses++;
            rhst.text = "Are you sure?";
        }
        else
        {
            highScoreCalculator.Reset_All_Scores();
            Text rhst = GameObject.Find("Reset High Scores Text").GetComponent<Text>();
            foreach (High_Score_Displayer hsd in hsds)
            {
                hsd.update_scores();
            }
            rhst.text = "High Scores Reset!";
        }
    }

    public IEnumerator GameTransition()
    {
        shutter.Begin_Vertical_Close();
        mc.Stop_Music();
        AsyncOperation async = SceneManager.LoadSceneAsync("Game Scene");
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(2f);
        async.allowSceneActivation = true;
        yield return async;
    }

    private void GameMode_Unlocker()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!Game_Mode_Helper.isGamemodeUnlocked((GameMode)i))
            {
                GameButtons[i].GetComponent<Image>().sprite = lockedSprite;
                Descriptions[i].GetComponent<Text>().text = "Score in the previous Game Mode to unlock this one!";
                Scores[i].GetComponent<Text>().text = "";
            }
            else
            {
                GameButtons[i].GetComponent<Image>().sprite = OrigButtonSprite[i];
                Descriptions[i].GetComponent<Text>().text = OrigDescText[i];
                Scores[i].BroadcastMessage("update_scores", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void OnGameModeSelectionChanged()
    {
        if (gameModeLayer.activeSelf)
        {
            activeMode = gameModeScroller.CurrentPage;
            if (activeMode == gameModeScroller.ChildObjects.Length - 1 && prevMode == gameModeScroller.ChildObjects.Length - 2)
            {
                ScrollUp.BroadcastMessage("FadeOut", null, SendMessageOptions.DontRequireReceiver);
            }
            else if (prevMode == gameModeScroller.ChildObjects.Length - 1 && activeMode != prevMode)
            {
                ScrollUp.BroadcastMessage("FadeIn", null, SendMessageOptions.DontRequireReceiver);
            }
            else if (activeMode == 0 && prevMode == 1)
            {
                playButtonText.text = "VIEW";
                ScrollDown.BroadcastMessage("FadeOut", null, SendMessageOptions.DontRequireReceiver);
            }
            else if (prevMode == 0 && activeMode != prevMode)
            {
                playButtonText.text = "PLAY";
                ScrollDown.BroadcastMessage("FadeIn", null, SendMessageOptions.DontRequireReceiver);
            }
            prevMode = activeMode;
        }
    }

    public void LoadCustomOptions()
    {
        RuleSet rulesetToLoad;
        if (Game_Mode_Helper.AllRuleSets[(int)GameMode.Custom] == null)
        {
            rulesetToLoad = Game_Mode_Helper.AllRuleSets[(int)GameMode.Wiz];
        }
        else
        {
            rulesetToLoad = Game_Mode_Helper.AllRuleSets[(int)GameMode.Custom];
        }

        CustomSpellToggle.isOn = rulesetToLoad.HasSpells;

        if (rulesetToLoad.TimedCrunch)
        {
            sideOptionIndex = 1;
            secondsPerCrunch = (int)rulesetToLoad.TimePerCrunch.TotalSeconds;
            CustomTimedSidesOptions.SetActive(true);
            CustomSplitSidesOptions.SetActive(false);
        }
        else if (rulesetToLoad.TurnedCrunch)
        {
            sideOptionIndex = 2;
            splitsPerCrunch = rulesetToLoad.SplitsPerCrunch;
            CustomSplitSidesOptions.SetActive(true);
            CustomTimedSidesOptions.SetActive(false);
        }
        else
        {
            sideOptionIndex = 0;
            CustomSplitSidesOptions.SetActive(false);
            CustomTimedSidesOptions.SetActive(false);
        }

        CustomSidesText.text = SideOptions[sideOptionIndex];
    }

    /// <summary>
    /// Updates the Sides options in code and the text shown on screen to match. Called when the left/right side buttons for custom sides are pressed
    /// </summary>
    /// <param name="isLeft"> bool - call with true if this is the left button, denoting that they want to go to a previous option</param>
    public void OnSideOptionChanged(bool isLeft)
    {
        if (isLeft)
        {
            if (sideOptionIndex == 0)
            {
                sideOptionIndex = SideOptions.Length - 1;
            }
            else
            {
                sideOptionIndex--;
            }
        }
        else
        {
            if (sideOptionIndex == SideOptions.Length - 1)
            {
                sideOptionIndex = 0;
            }
            else
            {
                sideOptionIndex++;
            }
        }

        CustomSidesText.text = SideOptions[sideOptionIndex];
        if (sideOptionIndex == 1)
        {
            CustomTimedSidesOptions.SetActive(true);
            CustomSplitSidesOptions.SetActive(false);
        }
        else if (sideOptionIndex == 2)
        {
            CustomSplitSidesOptions.SetActive(true);
            CustomTimedSidesOptions.SetActive(false);
        }
        else
        {
            CustomSplitSidesOptions.SetActive(false);
            CustomTimedSidesOptions.SetActive(false);
        }

        OnCustomModeUpdated();
    }

    public void OnSplitsCrunchButtonClicked(bool isLeft)
    {
        if (isLeft)
        {
            if (splitsPerCrunch > 1)
            {
                splitsPerCrunch--;
            }
        }
        else
        {
            if (splitsPerCrunch < 999)
            {
                splitsPerCrunch++;
            }
        }

        OnCustomModeUpdated();
    }

    public void OnTimedCrunchButtonClicked(bool isLeft)
    {
        if (isLeft)
        {
            if (secondsPerCrunch > 1)
            {
                secondsPerCrunch--;
            }
        }
        else
        {
            if (secondsPerCrunch < 999)
            {
                secondsPerCrunch++;
            }
        }

        OnCustomModeUpdated();
    }

    /// <summary>
    /// Updates the number of Unlocked Piece colors in code and the text shown on screen to match. Called when the left/right side buttons for custom sides are pressed
    /// </summary>
    /// <param name="isLeft"> bool - call with true if this is the left button, denoting that they want to go to a previous option</param>
    public void OnUnlockedColorCountChanged(bool isLeft)
    {
        if (isLeft)
        {
            if (unlockedColorCount > 3)
            {
                unlockedColorCount--;
            }
        }
        else if (unlockedColorCount < 8)
        {
            unlockedColorCount++;
        }

        CustomUnlockedColorCountText.text = unlockedColorCount.ToString();
        OnCustomModeUpdated();
    }

    public void OnCustomModeUpdated()
    {
        RuleSet customRuleSet = new RuleSet();
        customRuleSet.Mode = GameMode.Custom;
        customRuleSet.HasSpells = CustomSpellToggle.isOn;
        if (CustomUnlockOverTimeToggle.isOn)
        {
            customRuleSet.SplitsToUnlock = 77;
        }
        else
        {
            customRuleSet.SplitsToUnlock = 0;
        }

        switch (sideOptionIndex)
        {
            default:
            case 0:
                customRuleSet.TurnedCrunch = false;
                customRuleSet.TimedCrunch = false;
                break;
            case 1:
                customRuleSet.TurnedCrunch = false;
                customRuleSet.TimedCrunch = true;
                customRuleSet.TimePerCrunch = new System.TimeSpan(0, 0, secondsPerCrunch);
                CustomSecondsSelector.text = secondsPerCrunch.ToString();
                break;
            case 2:
                customRuleSet.TimedCrunch = false;
                customRuleSet.TurnedCrunch = true;
                customRuleSet.SplitsPerCrunch = splitsPerCrunch;
                CustomSplitsSelector.text = splitsPerCrunch.ToString();
                break;
        }

        customRuleSet.UnlockedPieces = unlockedColorCount;

        Game_Mode_Helper.AllRuleSets[(int)GameMode.Custom] = customRuleSet;
    }

    //for the pumpkin pieceset
    public void code(string dir)
    {
        if (achievementHandler.is_Pieceset_Unlocked(PieceSets.Pumpkin))
        {
            return;
        }

        switch (codeStage)
        {
            case 0:
                if (dir == "Up")
                {
                    codeStage++;
                }
                else
                {
                    codeStage = 0;
                }
                break;
            case 1:
                if (dir == "Up")
                {
                    codeStage++;
                }
                else
                {
                    codeStage = 0;
                }
                break;
            case 2:
                if (dir == "Down")
                {
                    codeStage++;
                }
                else
                {
                    codeStage = 0;
                }
                break;
            case 3:
                if (dir == "Down")
                {
                    codeStage++;
                }
                else
                {
                    codeStage = 0;
                }
                break;
            case 4:
                if (dir == "Left")
                {
                    codeStage++;
                }
                else
                {
                    codeStage = 0;
                }
                break;
            case 5:
                if (dir == "Right")
                {
                    codeStage++;
                }
                else
                {
                    codeStage = 0;
                }
                break;
            case 6:
                if (dir == "Left")
                {
                    codeStage++;
                }
                else
                {
                    codeStage = 0;
                }
                break;
            case 7:
                if (dir == "Right")
                {
                    achievementHandler.Unlock_Pieceset(PieceSets.Pumpkin);
                }
                else
                {
                    codeStage = 0;
                }
                break;
        }
    }
}