using UnityEngine;
using UnityEngine.UI.Extensions;

public class CustomModeMusicSelector : MonoBehaviour
{
    public HorizontalScrollSnap horizontalScrollSnap;

    private void Awake()
    {
        horizontalScrollSnap.StartingScreen = PlayerPrefs.GetInt(Constants.CustomModeMusic, 0);
    }

    /// <summary>
    /// Sets the option for the custom music. Music available is from each game mode (title music lacks ticking tracks)
    /// </summary>
    /// <param name="musicMode">GameMode - mode whose music plays in custom</param>
    public void SetCustomMusic(HorizontalScrollSnap musicSelector)
    {
        Debug.Log("Custom music set to " + musicSelector.CurrentPage);
        PlayerPrefs.SetInt(Constants.CustomModeMusic, musicSelector.CurrentPage);
    }
}