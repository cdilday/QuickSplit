using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Text_Fader : MonoBehaviour {

	//this is used to fade the given text in and out. This is not a uniform script that can be used in all cases, but can be altered to be

	public bool isFadingIn;
	public bool isFadingOut;
	Color originalColor;
	Text text;
	public float duration;
	float startTime;
	
	// Use this for initialization
	void Awake () {
		isFadingIn = false;
		isFadingOut = false;
		text = gameObject.GetComponent<Text> ();
		originalColor = text.color;
	}
	
	void FixedUpdate()
	{
		if (isFadingIn) {
			if (Time.time > startTime + duration){
				text.color = new Color(originalColor.r,originalColor.g,originalColor.b,1f);
				isFadingIn = false;
			}
			else{
				text.color = new Color(originalColor.r,originalColor.g,originalColor.b, (Time.time - startTime) / duration);
			}
		}
		else if (isFadingOut){
			if (Time.time > startTime + duration){
				text.color = new Color(originalColor.r,originalColor.g,originalColor.b,0);
				isFadingOut = false;
			}
			else{
				text.color = new Color(originalColor.r,originalColor.g,originalColor.b, 1f - ((Time.time - startTime) / duration));
			}
		}
	}

	//starts the fading in
	public void FadeIn()
	{
		//cannot happen if it is already busy
		if (isFadingIn || isFadingOut || text.color.a == 1)
			return;
		isFadingIn = true;
		startTime = Time.time;
	}

	//starts the fading out
	public void FadeOut()
	{
		//cannot happen if it is already busy
		if (isFadingIn || isFadingOut || text.color.a == 0)
			return;
		isFadingOut = true;
		startTime = Time.time;
	}

}