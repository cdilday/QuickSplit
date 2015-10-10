using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems; 

public class SFX_Slider : MonoBehaviour, IPointerUpHandler {

	GameObject MC;
	AudioSource MCsource;
	Slider mySlider;
	AudioSource sampleSource;
	// Use this for initialization
	void Awake () {
		MC = GameObject.Find ("Music Controller");
		mySlider = gameObject.GetComponent<Slider> ();
		MCsource = MC.GetComponent<AudioSource> ();
		mySlider.value = PlayerPrefs.GetFloat ("SFX Volume", 1);
		mySlider.onValueChanged.AddListener (delegate{onValueChanged ();});

		sampleSource = gameObject.GetComponent<AudioSource> ();
	}
	
	void onValueChanged()
	{
		MC.GetComponent<Music_Controller> ().SFXVolume = mySlider.value;
		PlayerPrefs.SetFloat ("SFX Volume", mySlider.value);
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		sampleSource.volume = PlayerPrefs.GetFloat ("SFX Volume", 1);
		sampleSource.Play ();
	}
}
