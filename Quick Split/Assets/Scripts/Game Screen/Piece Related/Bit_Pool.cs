using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bit_Pool : MonoBehaviour {

	public int scoreBitMax;

	public GameController gameController;

	List<GameObject> availableBits;

	/* Things this needs:*/
	// - An Arraylist of score bits to pull from
	// - A way of knowing how many bits it has
	// - the ability to turn scorebits on and off in terms of whether or not they're active
	// - To instantiate the maximum amount of bits possible on start, then immediately make them inactive 
	// - a function that takes in a number, V3 position, and color, then moves that number of bits to the location and makes them active
	//   - this will also remove the bits from the inactive arraylist
	// - a function called by the bit on collision with the score that will return the bit to its original, inactive state and into the inactive arraylist
	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		//instantiate all the bits, with the max possible amount of bits being max bits per piece * every slot on the grid.
		availableBits = new List<GameObject>();
		int maxPossible = scoreBitMax * 128;
		for (int i = 0; i < maxPossible; i++) {
			GameObject newbit = Instantiate((GameObject) Resources.Load ("Score Bit"));
			availableBits.Add(newbit);
			newbit.transform.SetParent(transform);
			newbit.name = ("Score Bit " + i);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void spawn_bits(int score, Vector3 spawnLoc, string color){
		int indivalue = score / scoreBitMax;
		int leftover = score % scoreBitMax;
		for (int i = 0; i < scoreBitMax; i++) {
			if(indivalue == 0 && leftover == 0)
				break;
			else{
				GameObject newbit = availableBits[0];
				availableBits.RemoveAt(0);
				newbit.SetActive(true);
				newbit.GetComponent<ScoreBit>().changeColor(color);
				newbit.transform.position = spawnLoc;
				newbit.GetComponent<ScoreBit>().target = gameController.scoreText.transform.position;
				newbit.GetComponent<ScoreBit>().value = indivalue;
				if(leftover > 0)
				{
					leftover--;
					newbit.GetComponent<ScoreBit>().value++;
				}
				newbit.GetComponent<ScoreBit>().Begin_Journey();
			}
		}
	}

	//loads an old bit back into the pool of ready to use bits, and repositions it instantly offscreen
	public void return_to_pool(GameObject scoreBit)
	{
		availableBits.Add (scoreBit);
		scoreBit.transform.position = transform.position;
	}
}
