using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attatched to and is used for the volume sliders which control what volume the music is played at
/// </summary>
public class MusicSlider : MonoBehaviour
{
    private GameObject MC;
    private Slider mySlider;

    // Use this for initialization
    private void Start()
    {
        MC = GameObject.Find("Music Controller");
        mySlider = gameObject.GetComponent<Slider>();
        mySlider.value = PlayerPrefs.GetFloat(Constants.MusicVolumeLookup, 1);
        mySlider.onValueChanged.AddListener(delegate { onValueChanged(); });
    }

    //update the volume while the value is being changed
    private void onValueChanged()
    {
        MC.GetComponent<MusicController>().ChangeMusicVolume(mySlider.value);
        PlayerPrefs.SetFloat(Constants.MusicVolumeLookup, mySlider.value);
    }

}