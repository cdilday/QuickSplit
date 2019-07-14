using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is for selecting your control scheme, leftover from mobile
/// </summary>
public class ControlSelector : MonoBehaviour
{
    public Text controlText;
    public GameObject[] controlObjects;

    // Use this for initialization
    private void Start()
    {
        //WebGL/standalone don't have complicated or conflicting control schemes, and therefore have no reason for changing
        if (!Application.isMobilePlatform)
        {
            foreach (GameObject control in controlObjects)
            {
                Destroy(control);
            }

            PlayerPrefs.SetString("Controls", "Follow");
            Destroy(gameObject);
        }
        else
        {
            controlText.text = PlayerPrefs.GetString("Controls", "Regions");
        }
    }

    public void OnButtonClick()
    {
        if (controlText.text == "Regions")
        {
            PlayerPrefs.SetString("Controls", "Follow");
            controlText.text = "Follow";
        }
        else
        {
            PlayerPrefs.SetString("Controls", "Regions");
            controlText.text = "Regions";
        }
    }
}
