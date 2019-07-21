using System.Collections.Generic;
using UnityEngine;

public class BitPool : MonoBehaviour
{

    //This script is for proper tracking and efficient handling of score bits

    public int scoreBitMax;

    public GameController gameController;
    private List<GameObject> availableBits;
    private List<GameObject> activeBits;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        //instantiate all the bits, with the max possible amount of bits being max bits per piece * every slot on the grid.
        availableBits = new List<GameObject>();
        int maxPossible = scoreBitMax * 128;
        for (int i = 0; i < maxPossible; i++)
        {
            GameObject newbit = Instantiate((GameObject)Resources.Load("Score Bit"));
            availableBits.Add(newbit);
            newbit.transform.SetParent(transform);
            newbit.name = ("Score Bit " + i);
        }

        activeBits = new List<GameObject>();
    }

    //this spawns the correct number of bits by repurposing bits not active in the pool
    public void spawnBits(int score, Vector3 spawnLoc, PieceColor color)
    {
        int indivalue = score / scoreBitMax;
        int leftover = score % scoreBitMax;
        for (int i = 0; i < scoreBitMax; i++)
        {
            if (indivalue == 0 && leftover == 0)
            {
                break;
            }
            else
            {
                GameObject newbit = availableBits[0];
                availableBits.RemoveAt(0);
                newbit.SetActive(true);
                newbit.GetComponent<ScoreBit>().changeColor(color);
                newbit.transform.position = spawnLoc;
                newbit.GetComponent<ScoreBit>().target = gameController.scoreText.transform.position;
                newbit.GetComponent<ScoreBit>().value = indivalue;
                if (leftover > 0)
                {
                    leftover--;
                    newbit.GetComponent<ScoreBit>().value++;
                }

                activeBits.Add(newbit);

                newbit.GetComponent<ScoreBit>().beginJourney();
            }
        }
    }

    //loads an old bit back into the pool of ready to use bits, and repositions it instantly offscreen
    public void returnToPool(GameObject scoreBit)
    {
        activeBits.Remove(scoreBit);
        availableBits.Add(scoreBit);
        scoreBit.transform.position = transform.position;
    }

    /// <summary>
    /// Puts all active bits in the score, called when a game over occurs and there might've been a combo recently
    /// </summary>
    public void cashInAllBits ()
    {
        while (activeBits.Count > 0)
        {
            activeBits[0].GetComponent<ScoreBit>().endJourney();
            returnToPool(activeBits[0]);
        }
    }

}