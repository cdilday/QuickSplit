using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles updating the Achivement tally on the achievement screen
/// </summary>
public class AchievementTally : MonoBehaviour
{ 
    public Text UnlockedText;
    public Text OutOfText;
    private ScoreAndAchievementHandler achievementHandler;

    private void OnEnable()
    {
        if (achievementHandler == null)
        {
            achievementHandler = GameObject.Find("Achievement Handler").GetComponent<ScoreAndAchievementHandler>();
        }
        //minus 3 because the default pieceset, splitter, and symbol piecesets are unlocked at the start
        int totalTally = achievementHandler.splittersUnlocked.Length + achievementHandler.piecesetsUnlocked.Length - 3;
        int unlockedTally = -3;
        foreach (bool isUnlocked in achievementHandler.splittersUnlocked)
        {
            if (isUnlocked)
            {
                unlockedTally++;
            }
        }
        foreach (bool isUnlocked in achievementHandler.piecesetsUnlocked)
        {
            if (isUnlocked)
            {
                unlockedTally++;
            }
        }
        UnlockedText.text = unlockedTally.ToString();
        OutOfText.text = totalTally.ToString();
    }

}