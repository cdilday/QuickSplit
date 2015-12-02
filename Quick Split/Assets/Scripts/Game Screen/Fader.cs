using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

	public bool isFadingIn;
	public bool isFadingOut;
	Image blackScreen;
	public float duration;
	float startTime;
	Color defaultColor;

	// Use this for initialization
	void Awake () {
		isFadingIn = false;
		isFadingOut = false;
		blackScreen = gameObject.GetComponent<Image> ();
		if (blackScreen.color.a != 0) {
			defaultColor = blackScreen.color;
		}else{
			defaultColor = new Color(0,0,0,1f);
			blackScreen.color = new Color (0, 0, 0, 0);
		}
	}
	
	void FixedUpdate()
	{
		if (isFadingIn) {
			if (Time.time > startTime + duration)
			{
				blackScreen.color = new Color(defaultColor.r,defaultColor.g,defaultColor.b,1f);
				isFadingIn = false;
			}
			else
			{
				blackScreen.color = new Color(defaultColor.r,defaultColor.g,defaultColor.b, (Time.time - startTime) / duration);
			}
		}
		else if (isFadingOut)
		{
			if (Time.time > startTime + duration)
			{
				blackScreen.color = new Color(defaultColor.r,defaultColor.g,defaultColor.b,0);
				isFadingOut = false;
			}
			else
			{
				blackScreen.color = new Color(defaultColor.r,defaultColor.g,defaultColor.b, 1f - ((Time.time - startTime) / duration));
			}
		}
	}

	public void FadeIn()
	{
		if (isFadingIn || isFadingOut)
			return;
		isFadingIn = true;
		startTime = Time.time;
	}

	public void FadeOut()
	{
		if (isFadingIn || isFadingOut)
			return;
		isFadingOut = true;
		startTime = Time.time;
	}
}
