﻿using UnityEngine;
using UnityEngine.UI;

public class Pulser : MonoBehaviour
{
    //This is attatched to the score on the Game Screen and is responsible for handling the pulse that happens on update

    private Text thisText;
    public float defaultSize;

    public bool pulsing;
    public bool growing;
    public int counter;
    private AudioSource scoreBlip;
    private GameController gameController;

    // Use this for initialization
    private void Start()
    {
        thisText = gameObject.GetComponent<Text>();
        defaultSize = thisText.fontSize;
        pulsing = false;
        growing = false;
        GameObject temp = GameObject.FindGameObjectWithTag("GameController");
        if (temp != null)
        {
            gameController = temp.GetComponent<GameController>();
        }
        scoreBlip = gameObject.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (pulsing)
        {
            handlePulse();
        }
    }

    private void beginPulse()
    {
        if (gameController != null && !gameController.gameOver)
        {
            scoreBlip.volume = 0.5f * (PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1));
            scoreBlip.pitch = 1f + (Mathf.Sin(Time.fixedUnscaledTime * 3f ) / 8);
            scoreBlip.Play();
        }
        if (pulsing)
        {
            counter = 10;
            thisText.fontSize = (int)(defaultSize + ((float)counter / 2.5f));
        }
        else
        {
            counter = 0;
            thisText.fontSize = (int)defaultSize;
        }
        pulsing = true;
        growing = true;
    }

    private void handlePulse()
    {
        if (growing)
        {
            counter += 6;
            thisText.fontSize = (int)(defaultSize + ((float)counter / 2.5f));
            if (counter >= 10)
            {
                growing = false;
            }
        }
        else
        {
            counter--;
            thisText.fontSize = (int)(defaultSize + ((float)counter / 2.5f));
            if (counter <= 0)
            {
                pulsing = false;
                thisText.fontSize = (int)defaultSize;
            }
        }
    }

}