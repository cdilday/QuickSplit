using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Toggle for the region control scheme's transparent overlay being displayed (mobile only)
/// </summary>
public class RegionToggler : MonoBehaviour
{
    private Toggle toggle;

    // Use this for initialization
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        int guideOnInt = PlayerPrefs.GetInt("Region Guide", 1);
        if (guideOnInt == 1)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    public void OnToggle()
    {
        if (!toggle.isOn)
        {
            PlayerPrefs.SetInt("Region Guide", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Region Guide", 1);
        }
    }

}
