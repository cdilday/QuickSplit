using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{

    //this is used for fading images in and out, but is not a universal script

    public bool isFadingIn;
    public bool isFadingOut;
    private Image blackScreen;
    public float duration;
    private float startTime;
    private Color defaultColor;

    // Use this for initialization
    private void Awake()
    {
        isFadingIn = false;
        isFadingOut = false;
        blackScreen = gameObject.GetComponent<Image>();
        if (blackScreen.color.a != 0)
        {
            defaultColor = blackScreen.color;
        }
        else
        {
            defaultColor = new Color(0, 0, 0, 1f);
            blackScreen.color = new Color(0, 0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        if (isFadingIn)
        {
            if (Time.time > startTime + duration)
            {
                blackScreen.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 1f);
                isFadingIn = false;
            }
            else
            {
                blackScreen.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, (Time.time - startTime) / duration);
            }
        }
        else if (isFadingOut)
        {
            if (Time.time > startTime + duration)
            {
                blackScreen.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
                isFadingOut = false;
            }
            else
            {
                blackScreen.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 1f - ((Time.time - startTime) / duration));
            }
        }
    }

    //begins the fading in
    public void FadeIn()
    {
        //cannot happen if it is already busy
        if (isFadingIn || isFadingOut)
        {
            return;
        }

        isFadingIn = true;
        startTime = Time.time;
    }

    //begins the fading out
    public void FadeOut()
    {
        //cannot happen if it is already busy
        if (isFadingIn || isFadingOut)
        {
            return;
        }

        isFadingOut = true;
        startTime = Time.time;
    }

}