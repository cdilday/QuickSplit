﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.SceneManagement;

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
    int resetPresses = 0;

    Achievement_Script achievementHandler;
    Music_Controller mc;
    High_Score_Calculator highScoreCalculator;

    //gameobjects needed for transitions b/w game mode select and description scenes
    public GameObject[] GameButtons = new GameObject[4];
    Sprite[] OrigButtonSprite = new Sprite[4];
    public Sprite lockedSprite;
    public GameObject[] Descriptions = new GameObject[4];
    string[] OrigDescText = new string[4];
    public GameObject[] Scores = new GameObject[4];

    public GameObject PlayButton;
    public GameObject BackButton;

    bool isInPlayScreen;

    public int activeMode;
    int prevMode;

    //Specifically for the Pumpkin Achievement code
    int codeStage = 0;

    public GameObject HTPEmphasizer;

    // Use this for initialization
    void Start()
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

        mc.Play_Music("Menu");

        //just in case this is the first time playing, set Wiz to be for sure unlocked
        PlayerPrefs.SetInt("Wiz unlocked", 1);
        //tell achievmement handler to check gamemodes that are supposed to be active
        achievementHandler.Check_Gamemode_Unlocked();

        activeMode = 4;
        prevMode = 4;
        ScrollUp.BroadcastMessage("FadeOut", null, SendMessageOptions.DontRequireReceiver);
        for (int i = 0; i < 4; i++)
        {
            OrigButtonSprite[i] = GameButtons[i].GetComponent<Image>().sprite;
            OrigDescText[i] = Descriptions[i].GetComponent<Text>().text;
        }
        GameMode_Unlocker();

        if (PlayerPrefs.GetInt("Played Before", 0) == 1)
        {
            Destroy(HTPEmphasizer);
        }
    }

    void FixedUpdate()
    {

    }

    void Update()
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
            int gameType = 0;
            switch (activeMode)
            {
                case 4:
                    gameType = 0;
                    break;
                case 3:
                    gameType = 1;
                    break;
                case 2:
                    gameType = 2;
                    break;
                case 1:
                    gameType = 3;
                    break;
            }
            if (gameNum_unlock_checker(gameType))
            {
                PlayerPrefs.SetInt("Mode", gameType);
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

    bool gameNum_unlock_checker(int gameNum)
    {
        switch (gameNum)
        {
            case 0:
                return achievementHandler.is_Gamemode_Unlocked("Wiz");
            case 1:
                return achievementHandler.is_Gamemode_Unlocked("Quick");
            case 2:
                return achievementHandler.is_Gamemode_Unlocked("Wit");
            case 3:
                return achievementHandler.is_Gamemode_Unlocked("Holy");
        }
        return false;
    }

    void GameMode_Unlocker()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!gameNum_unlock_checker(i))
            {
                GameButtons[i].GetComponent<Image>().sprite = lockedSprite;
                Descriptions[i].GetComponent<Text>().text = "Score in the last Game Mode to unlock this one!";
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
            if (activeMode == 4 && prevMode == 3)
            {
                ScrollUp.BroadcastMessage("FadeOut", null, SendMessageOptions.DontRequireReceiver);
            }
            else if (prevMode == 4 && activeMode != prevMode)
            {
                ScrollUp.BroadcastMessage("FadeIn", null, SendMessageOptions.DontRequireReceiver);
            }
            else if (activeMode == 0 && prevMode == 1)
            {
                ScrollDown.BroadcastMessage("FadeOut", null, SendMessageOptions.DontRequireReceiver);
            }
            else if (prevMode == 0 && activeMode != prevMode)
            {
                ScrollDown.BroadcastMessage("FadeIn", null, SendMessageOptions.DontRequireReceiver);
            }
            prevMode = activeMode;
        }
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