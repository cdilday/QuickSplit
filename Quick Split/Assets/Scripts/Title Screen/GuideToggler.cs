using UnityEngine;
using UnityEngine.UI;

public class GuideToggler : MonoBehaviour
{
    //this handles the guide toggle on the options menu

    private Toggle toggle;

    // Use this for initialization
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        int guideOnInt = PlayerPrefs.GetInt(Constants.GuideOption, 1);
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
            PlayerPrefs.SetInt(Constants.GuideOption, 0);
        }
        else
        {
            PlayerPrefs.SetInt(Constants.GuideOption, 1);
        }
    }

}