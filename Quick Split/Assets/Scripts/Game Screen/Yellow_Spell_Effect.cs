using UnityEngine;
using System.Collections;

public class Yellow_Spell_Effect : MonoBehaviour {

	//This script handles the Yellow Spell's effect and animations

	GameController gameController;
	SpellHandler spellHandler;

	int row;
	float startTime;
	float dynamicStartTime;

	float initialCatalystIncrement = 0;
	public float increment;

	bool activated = false;
	bool isChaining = false;
	bool finishedGrowing = false;
	int ChainStage = 0;
	// this is assigned through IDE. 0 is the left origin, 1-7 are the other effects going right to left
	// 8 is the right origin, 9-15 are the other effects going left to right
	public GameObject[] yellowSpellEffects = new GameObject[16];

	public AudioSource YellowChargeSFX;
	public AudioSource YellowFireSFX;

	Achievement_Script achievementHandler;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		if(gameController.gameMode != GameMode.Wiz && gameController.gameMode != GameMode.Holy)
        {
			Destroy(gameObject);
			return;
		}

		foreach (GameObject yse in yellowSpellEffects) {
			yse.GetComponent<SpriteRenderer>().sprite = null;
		}

		achievementHandler = GameObject.FindGameObjectWithTag ("Achievement Handler").GetComponent<Achievement_Script> ();

		spellHandler = GameObject.Find ("Spell Handler").GetComponent<SpellHandler> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (activated) {
			if(!isChaining && initialCatalystIncrement + startTime < Time.time){
				isChaining = true;
				ChainStage = 2;
				yellowSpellEffects [1].GetComponent<Animator> ().SetBool ("inActive", false);
				yellowSpellEffects [1].GetComponent<Animator> ().SetBool ("isGrowing", true);
				yellowSpellEffects [9].GetComponent<Animator> ().SetBool ("inActive", false);
				yellowSpellEffects [9].GetComponent<Animator> ().SetBool ("isGrowing", true);
				increment = ((yellowSpellEffects [1].GetComponent<Animator> ().GetCurrentAnimatorStateInfo(0).length * (1/yellowSpellEffects [1].GetComponent<Animator> ().speed)) / 6f);
				startTime = Time.time;
				YellowFireSFX.volume = PlayerPrefs.GetFloat ("SFX Volume", 1);
				YellowFireSFX.Play ();
			}
			else if(isChaining){
				if( !finishedGrowing && ChainStage < 8 && increment + startTime <Time.time){
					yellowSpellEffects [ChainStage].GetComponent<Animator> ().SetBool ("inActive", false);
					yellowSpellEffects [ChainStage].GetComponent<Animator> ().SetBool ("isGrowing", true);
					yellowSpellEffects [8 + ChainStage].GetComponent<Animator> ().SetBool ("inActive", false);
					yellowSpellEffects [8 + ChainStage].GetComponent<Animator> ().SetBool ("isGrowing", true);
					ChainStage++;
					startTime = Time.time;
					dynamicStartTime = Time.time;
				}
				else if( !finishedGrowing && ChainStage == 8){
					finishedGrowing = true;
					int deletedNum = 0;
					//delete pieces in row
					row = (int) gameController.splitter.transform.position.y;
					//loop that deletes everything in the row
					for (int c = 0; c < 16; c++) {	
						if (gameController.colorGrid[row,c] != null){
							gameController.colorGrid[row,c] = null;
							Destroy(gameController.grid[row,c]);
							gameController.grid[row,c] = null;
							deletedNum++;
						}
					}

					if(!achievementHandler.is_Splitter_Unlocked(SplitterType.Yellow) && deletedNum == 14)
						achievementHandler.Unlock_Splitter(SplitterType.Yellow);

					//make it so the splitter can't continually fire yellow spells
					gameController.splitter.setState ("yellowReady", false);
					//check the board to update group values
					gameController.checkBoard ();
					yellowSpellEffects [0].GetComponent<Animator> ().SetInteger ("LeftStage", 2);
					yellowSpellEffects [8].GetComponent<Animator> ().SetInteger ("RightStage", 2);
					startTime = Time.time;
					ChainStage--;
				}
				else if (finishedGrowing){
					if(ChainStage > 0 && increment + dynamicStartTime <Time.time){
						yellowSpellEffects [8 - ChainStage].GetComponent<Animator> ().SetBool ("isGrowing", false);
						yellowSpellEffects [(8 - ChainStage) + 8].GetComponent<Animator> ().SetBool ("isGrowing", false);
						ChainStage--;
						dynamicStartTime = Time.time;
					}
					for(int i = 0; i < 16; i++){
						float animPlayLength = yellowSpellEffects[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * (1/yellowSpellEffects[i].GetComponent<Animator>().speed);
						if(animPlayLength + startTime + ((increment*1.1f) * (i%8)) < Time.time){
							yellowSpellEffects[i].GetComponent<Animator>().SetBool("inActive", true);
							if(i == 0 || i == 8){
								yellowSpellEffects[i].GetComponent<Animator>().SetInteger("RightStage", 0);
								yellowSpellEffects[i].GetComponent<Animator>().SetInteger("LeftStage", 0);
							}
							if( i == 15){
								reset();
							}
						}
					}
				}
			}
		
		}
		//in here we will go down the line and trigger the animations as they go depending on the time passed since the first
		// this will have to be calculated on the fly, as animations are likely to have their speed changed later in tweaking
		// once animations in index 7 and 15 are complete, destroy all pieces in that row
		// then go down the line, triggering the shrinking as the animations go as well
		// once each individual animation has finished, set the animator to inactive and replace the sprite with null
		// once animations in index 7 and 15 have finished, let the player use the splitter again

		//the rest start after the previous hits the 2/5th frame
	}

	//begins the effect & animations
	public void Activate(){
		yellowSpellEffects [0].GetComponent<Animator> ().SetBool ("inActive", false);
		yellowSpellEffects [0].GetComponent<Animator> ().SetInteger ("LeftStage", 1);
		yellowSpellEffects [8].GetComponent<Animator> ().SetBool ("inActive", false);
		yellowSpellEffects [8].GetComponent<Animator> ().SetInteger ("RightStage", 1);

		startTime = Time.time;

		//the initial animation after the source begins on the 11/14th frame
		if(initialCatalystIncrement == 0)
			initialCatalystIncrement = 11f *(((yellowSpellEffects [0].GetComponent<Animator> ().GetCurrentAnimatorStateInfo(0).length * (1/yellowSpellEffects [0].GetComponent<Animator> ().speed)) / 14f));
		activated = true;
		YellowChargeSFX.volume = PlayerPrefs.GetFloat ("SFX Volume", 1);
		YellowChargeSFX.Play ();
	}

	//This resets it back to a state where it can be reused
	void reset()
	{
		gameController.splitter.setState ("isActive", true);
		activated = false;
		isChaining = false;
		finishedGrowing = false;
		ChainStage = 0;
		spellHandler.spellWorking = false;
	}

}