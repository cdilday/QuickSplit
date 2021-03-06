﻿using UnityEngine;
using UnityEngine.UI;

public class AchievementListItem : MonoBehaviour
{
    //This script handles an individual item on the achievement list on the main menu and updates it accordingly

    public string unlockable;
    public bool isSplitter;
    private Text NameText;
    private Text HintText;
    private Image UnlockableImage;

    public string AchievementName;
    public string AchievementHint;
    public string AchievementCondition;
    public Sprite UnlockableSprite;
    private ScoreAndAchievementHandler achievementHandler;
    private bool unlocked = false;

    // Use this for initialization
    private void OnEnable()
    {
        if (achievementHandler == null)
        {
            achievementHandler = GameObject.Find("Achievement Handler").GetComponent<ScoreAndAchievementHandler>();
        }

        NameText = transform.GetChild(0).GetComponent<Text>();
        HintText = transform.GetChild(1).GetComponent<Text>();
        UnlockableImage = transform.GetChild(2).GetComponent<Image>();

        //check if it's unlocked depending on what it is
        if (isSplitter)
        {
            unlocked = achievementHandler.isSplitterUnlocked(Splitter_Helper.Get_SplitterType_Enum(unlockable));
        }
        else
        {
            unlocked = achievementHandler.isPiecesetUnlocked(Piece_Set_Helper.Get_Pieceset_Enum(unlockable));
        }

        //if it's unlocked, update with the right info
        if (unlocked)
        {
            NameText.text = AchievementName;
            HintText.text = AchievementCondition;
            UnlockableImage.sprite = UnlockableSprite;
            UnlockableImage.color = new Color(1, 1, 1, 1);
        }
        //if it isn't unlocked, hide it and give the player the hint found in the unity GUI for this gameobject
        else
        {
            NameText.text = "???????";
            HintText.text = AchievementHint;
            UnlockableImage.sprite = UnlockableSprite;
            UnlockableImage.color = new Color(0, 0, 0, 1);
        }
    }

}
