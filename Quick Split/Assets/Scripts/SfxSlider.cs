using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// attached to the SFX sliders and is used for updating their volumes
/// </summary>
public class SfxSlider : MonoBehaviour, IPointerUpHandler
{
    private GameObject MC;

    //AudioSource MCsource;
    private Slider mySlider;
    private AudioSource sampleSource;

    // Use this for initialization
    private void Awake()
    {
        MC = GameObject.Find("Music Controller");
        mySlider = gameObject.GetComponent<Slider>();
        mySlider.value = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        mySlider.onValueChanged.AddListener(delegate { onValueChanged(); });

        sampleSource = gameObject.GetComponent<AudioSource>();
    }

    //update the value as soon as it changes
    private void onValueChanged()
    {
        MC.GetComponent<MusicController>().SFXVolume = mySlider.value;
        PlayerPrefs.SetFloat(Constants.SfxVolumeLookup, mySlider.value);
    }

    //this will play a sound to indicate what it will sound like at the new volume
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        sampleSource.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        sampleSource.Play();
    }
}
