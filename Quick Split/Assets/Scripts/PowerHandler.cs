using UnityEngine;
using System.Collections;

public class PowerHandler : MonoBehaviour {

	GameController gameController;
	Splitter_script splitter;

	#region
	public bool redReady;
	public bool orangeReady;
	public bool yellowReady;
	public bool greenReady;
	public bool blueReady;
	public bool purpleReady;
	public bool greyReady;
	public bool whiteReady;
	#endregion

	#region
	public int redProgress;
	public int redGoal = 100;
	public int orangeProgress;
	public int orangeGoal = 100;
	public int yellowProgress;
	public int yellowGoal = 100;
	public int greenProgress;
	public int greenGoal = 100;
	public int blueProgress;
	public int blueGoal = 100;
	public int purpleProgress;
	public int purpleGoal = 100;
	public int greyProgress;
	public int greyGoal = 100;
	public int whiteProgress;
	public int whiteGoal = 100;
	#endregion

	#region
	public GUIText redText;
	public GUIText orangeText;
	public GUIText yellowText;
	public GUIText greenText;
	public GUIText blueText;
	public GUIText purpleText;
	public GUIText greyText;
	public GUIText whiteText;
	#endregion
	// Use this for initialization
	void Start () {

		GameObject gameControllerobject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerobject == null) {
			Debug.LogError("Power Handler Error: cannot find game controller");
		}
		else{
			gameController = gameControllerobject.GetComponent<GameController>();
		}

		GameObject splitterObject = GameObject.FindGameObjectWithTag ("Splitter");
		if (splitterObject == null) {
			Debug.LogError("Power Handler Error: cannot find Splitter");
		}
		else{
			splitter = splitterObject.GetComponent<Splitter_script>();
		}

		redReady = false;
		orangeReady = false;
		yellowReady = false;
		greenReady = false;
		blueReady = false;
		purpleReady = false;
		greyReady = false;
		whiteReady = false;

		redText.text = "Not Ready";
		orangeText.text = "Not Ready";
		yellowText.text = "Not Ready";
		greenText.text = "Not Ready";
		blueText.text = "Not Ready";
		purpleText.text = "Not Ready";
		greyText.text = "Not Ready";
		whiteText.text = "Not Ready";

	}
	
	// Update is called once per frame
	void Update () {
		//attacks
		//red
		if (Input.GetKeyDown ("1") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && redReady) {
			redReady = false;
			redProgress = 0;
			redGoal = (int) (redGoal * 1.5);
			redText.text = "Not Ready";
			RedPower ();
		}
		//orange
		if (Input.GetKeyDown ("2") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && orangeReady) {
			orangeReady = false;
			orangeProgress = 0;
			orangeGoal = (int) (orangeGoal * 1.5);
			orangeText.text = "Not Ready";
			OrangePower ();
		}
		//yellow
		if (Input.GetKeyDown ("3") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && yellowReady) {
			yellowReady = false;
			yellowProgress = 0;
			yellowGoal = (int) (yellowGoal * 1.5);
			yellowText.text = "Not Ready";
			YellowPower ();
		}
		//green
		if (Input.GetKeyDown ("4") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && greenReady) {
			greenReady = false;
			greenProgress = 0;
			greenGoal = (int) (greenGoal * 1.5);
			greenText.text = "Not Ready";
			GreenPower ();
		}
		//blue
		if (Input.GetKeyDown ("5") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && blueReady) {
			blueReady = false;
			blueProgress = 0;
			blueGoal = (int) (blueGoal * 1.5);
			blueText.text = "Not Ready";
			BluePower ();
		}
		//purple
		if (Input.GetKeyDown ("6") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && purpleReady) {
			purpleReady = false;
			purpleProgress = 0;
			purpleGoal = (int) (purpleGoal * 1.5);
			purpleText.text = "Not Ready";
			PurplePower ();
		}
		//grey
		if (Input.GetKeyDown ("7") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && greyReady) {
			greyReady = false;
			greyProgress = 0;
			greyGoal = (int) (greyGoal * 1.5);
			greyText.text = "Not Ready";
			GreyPower ();
		}
		//white
		if (Input.GetKeyDown ("8") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && whiteReady) {
			whiteReady = false;
			whiteProgress = 0;
			whiteGoal = (int) (whiteGoal * 1.5);
			whiteText.text = "Not Ready";
			WhitePower ();
		}
	}

	//Red attack: Burns a layer off the top of each side, specifically deleting the block in each row closest to the center
	public void RedPower()
	{
		/*Deletion loops work by going to the splitter's columns outwards and deleting the first piece it comes across before moving on
		 * likely the player would only use this ability when on the brink of losing, so this is better than going from outwards in.
		 * Once the loop deletes the first thing it comes accross, it exits the inner loop to move onto the next row.*/
		//left grid loop
		for (int r = 0; r < 8; r++) {
			for (int c = 7; c >=0; c--){
				//Debug.Log ("Checking position R: " + r + " C: " + c);
				if(gameController.grid[r,c] != null)
				{
					Destroy (gameController.grid[r,c]);
					gameController.colorGrid[r,c] = null;
					gameController.grid[r,c] = null;
					c = -1;
				}
			}
		}
		//right grid loop
		for (int r = 0; r < 8; r++) {
			for (int c = 8; c < 16; c++){
				//Debug.Log ("Checking position R: " + r + " C: " + c);
				if(gameController.grid[r,c] != null)
				{
					Destroy (gameController.grid[r,c]);
					gameController.colorGrid[r,c] = null;
					gameController.grid[r,c] = null;
					c = 16;
				}
			}
		}
		//check the board to update group values
		gameController.checkBoard ();
	}
	//Orange atttack: switches all the pieces of a single color on one side with all the pieces of a different single color on the other side
	//deletes leftover pieces if the switch is uneven.
	public void OrangePower()
	{
		
	}
	//Yellow attack: launches a single lightning bolt to each side that removes any blocks in the splitter's row
	public void YellowPower()
	{
		//get the row the splitter is in
		int row = (int) gameController.splitter.transform.position.y;
		//loop that deletes everything in the row
		for (int c = 0; c < 16; c++) {
			
			if (gameController.colorGrid[row,c] != null)
			{
				gameController.colorGrid[row,c] = null;
				Destroy(gameController.grid[row,c]);
				gameController.grid[row,c] = null;
			}
		}
		//check the board to update group values
		gameController.checkBoard ();
	}
	//Green attack: change the color of three pieces currently in holder or splitter to any color the player chooses
	public void GreenPower()
	{
		
	}
	//Blue attack: rearrange every splitter/holder piece to any arrangement the player chooses
	public void BluePower()
	{
		
	}
	//Purple attack: turns pieces in holder/splitter into "special" pieces
	//special pieces are specially marked and do things such as increase score and multiplier
	public void PurplePower()
	{
		
	}
	//Grey Power: the splitter pieces turn to "bombs" which explode and destroy any pieces that come into contact with the explosion when launched
	public void GreyPower()
	{
		
	}
	//White Power: Sorts the board from rainbow down
	public void WhitePower() //consider renaming to ability, in hindsight I probably should've looked at that first
	{
		
	}

	public void addBit(string colorOfBit)
	{
		switch (colorOfBit) {
		case "Red":
			redProgress++;
			if(redProgress >= redGoal){
				redProgress = redGoal;
				redReady = true;
				redText.text = "Ready!";
			}
			break;
		case "Orange":
			orangeProgress++;
			if(orangeProgress >= orangeGoal){
				orangeProgress = orangeGoal;
				orangeReady = true;
				orangeText.text = "Ready!";
			}
			break;
		case "Yellow":
			yellowProgress++;
			if(yellowProgress >= yellowGoal){
				yellowProgress = yellowGoal;
				yellowReady = true;
				yellowText.text = "Ready!";
			}
			break;
		case "Green":
			greenProgress++;
			if(greenProgress >= greenGoal){
				greenProgress = greenGoal;
				greenReady = true;
				greenText.text = "Ready!";
			}
			break;
		case "Blue":
			blueProgress++;
			if(blueProgress >= blueGoal){
				blueProgress = blueGoal;
				blueReady = true;
				blueText.text = "Ready!";
			}
			break;
		case "Purple":
			purpleProgress++;
			if(purpleProgress >= purpleGoal){
				purpleProgress = purpleGoal;
				purpleReady = true;
				purpleText.text = "Ready!";
			}
			break;
		case "Grey":
			greyProgress++;
			if(greyProgress >= greyGoal){
				greyProgress = greyGoal;
				greyReady = true;
				greyText.text = "Ready!";
			}
			break;
		case "White":
			whiteProgress++;
			if(whiteProgress >= whiteGoal){
				whiteProgress = whiteGoal;
				whiteReady = true;
				whiteText.text = "Ready!";
			}
			break;
		}
	}
}
