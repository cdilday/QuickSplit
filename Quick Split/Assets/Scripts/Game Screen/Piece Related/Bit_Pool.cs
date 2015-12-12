using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bit_Pool : MonoBehaviour {

	//This script is for proper tracking and efficient handling of score bits

	public int scoreBitMax;

	public GameController gameController;

	List<GameObject> availableBits;
	
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

	//this spawns the correct number of bits by repurposing bits not active in the pool
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
				if(leftover > 0){
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