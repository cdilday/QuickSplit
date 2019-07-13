using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    //for sfx that need to play often in relation to unstable objects

    //attatch as many audiosources as the maximum number of sounds  may need toplay at once
    private List<AudioSource> aSources;
    //marks which ones are and aren't busy
    public List<bool> availableSource;

    //for pitch variations
    public bool variation;
    public float variationAmount;
    private GameController gameController;

    // Use this for initialization
    private void Start()
    {
        aSources = new List<AudioSource>();
        availableSource = new List<bool>();
        //get all attatched sources
        AudioSource[] attatchedSources = gameObject.GetComponents<AudioSource>();

        GameObject temp = GameObject.FindGameObjectWithTag("GameController");
        if (temp != null)
        {
            gameController = temp.GetComponent<GameController>();
        }

        //load them up properly
        foreach (AudioSource source in attatchedSources)
        {
            aSources.Add(source);
            availableSource.Add(true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // upkeep for available sources
        for (int i = 0; i < aSources.Count(); i++)
        {
            if (!availableSource.ElementAt(i))
            {
                if (!aSources.ElementAt(i).isPlaying)
                {
                    availableSource[i] = true;
                }
            }
        }
    }

    //Plays the sound with the proper pitch and volume
    private void PlaySound()
    {

        if (gameController.gameOver)
        {
            return;
        }

        for (int i = 0; i < aSources.Count(); i++)
        {
            //find first available audiosource
            if (availableSource.ElementAt(i))
            {
                availableSource[i] = false;
                //set proper volume
                aSources[i].volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);

                //add variation if needed
                if (variation)
                {
                    aSources[i].pitch = 1f + Random.Range(-1f * variationAmount, variationAmount);
                }
                aSources.ElementAt(i).Play();
                return;
            }
        }
    }

}