using UnityEngine;
using System.Collections;

public class Splitter_script : MonoBehaviour {
	
	public bool isMoving; // checks if the splitter is in the middle of moving to the next grid spot
	public int moveDirection; // 1 if it's moving upwards, -1 if downwards, 0 if not currently moving

	//prefabs containing all the different colored pieces
	public Transform[] pieces;

	//pieces currently in the splitter
	public Transform leftSlot;
	public Transform rightSlot;
	public bool canShoot;

	public bool mouseControl;
	Vector3 mouseLocation;

	//objects the splitter will need to use
	public Holder_Script holder;
	public GameController gameController;

	// Use this for initialization
	void Start () {
		//get the gamecontroller
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		//get the holder
		GameObject holderObject = GameObject.FindWithTag ("Holder");
		if (holderObject != null) {
			holder = holderObject.GetComponent <Holder_Script>();
		}
		//start the game with random pieces in the holder
		int left = Random.Range (0, gameController.availableCount);
		int right = Random.Range (0, gameController.availableCount);
		leftSlot = Instantiate(pieces[left], new Vector2(-1, transform.position.y), Quaternion.identity) as Transform;
		rightSlot = Instantiate(pieces[right], new Vector2(0, transform.position.y), Quaternion.identity) as Transform;
		leftSlot.GetComponent<piece_script> ().inSplitter = true;
		rightSlot.GetComponent<piece_script> ().inSplitter = true;
		canShoot = true;
		//make a camera for mouse control to use
	}
	
	// Update is called once per frame
	void Update () {

		//player Input
		//checks if the player has opted for mouse control, if not uses key input. Uncomment to make them exclusive
		//if (mouseControl) {
			//uncomment to constantly see mouse position.
			//Debug.Log ("Mouse Position: X:" + Input.mousePosition.x + "    Y: " + Input.mousePosition.y);
			mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//checking that the mouse is within the grid
			if(mouseLocation.x <= 7 && mouseLocation.x >= -8 && mouseLocation.y >= -0.5 && mouseLocation.y <= 7.5)
			{
				//Moving up if the mouse is above the splitter's hitbox
				if ((mouseLocation.y > transform.position.y + 0.5f) && !isMoving && transform.position.y < 7) {
					moveDirection = 1;
					StartCoroutine(MovementPause());
					isMoving = true;
				}
				//Moving downwards if the mouse is below the splitter's hitbox
				if ((mouseLocation.y < transform.position.y - 0.5f) && !isMoving && transform.position.y > 0) {
					moveDirection = -1;
					StartCoroutine(MovementPause());
					isMoving = true;
				}
				//Swapping pieces with right click
				if (Input.GetMouseButtonDown(1)) {
					swap ();
				}
				//launching pieces with Left click while over the board
				if (Input.GetMouseButtonDown(0) && moveDirection == 0 && rightSlot != null && leftSlot !=null && canShoot)
				{
					StartCoroutine(fire());
					canShoot = false;
					gameController.movesMade++;
					gameController.updateMoves();
				}
			}
		//}
		//else{
			//moving upwards with keys W or Up
			if ((Input.GetKey ("w") || Input.GetKey("up")) && !isMoving && transform.position.y < 7) {
				moveDirection = 1;
				StartCoroutine(MovementPause());
				isMoving = true;
			}
			//moving downwards with keys S or Down
			if ((Input.GetKey ("s") || Input.GetKey("down")) && !isMoving && transform.position.y > 0) {
				moveDirection = -1;
				StartCoroutine(MovementPause());
				isMoving = true;
			}
			//swapping pieces with keys A, D, Left, or Right
			if (Input.GetKeyDown ("a") || Input.GetKeyDown("d") || Input.GetKeyDown("left") || Input.GetKeyDown("right")) {
				swap ();
			}
			//launching pieces with key Space
			if (Input.GetKeyDown("space") && moveDirection == 0 && rightSlot != null && leftSlot !=null && canShoot)
			{
				StartCoroutine(fire());
				canShoot = false;
				gameController.movesMade++;
				gameController.updateMoves();
			}

			//attacks
			//red
			if(Input.GetKeyDown("1") && canShoot && !isMoving)
			{
				gameController.RedPower();
			}
			//orange
			if(Input.GetKeyDown("2") && canShoot && !isMoving)
			{
				gameController.OrangePower();
			}
			//yellow
			if(Input.GetKeyDown("3") && canShoot && !isMoving)
		   	{
				gameController.YellowPower();
			}
			//green
			if(Input.GetKeyDown("4") && canShoot && !isMoving)
			{
				gameController.GreenPower();
			}
			//blue
			if(Input.GetKeyDown("5") && canShoot && !isMoving)
			{
				gameController.BluePower();
			}
			//purple
			if(Input.GetKeyDown("6") && canShoot && !isMoving)
			{
				gameController.PurplePower();
			}
			//grey
			if(Input.GetKeyDown("7") && canShoot && !isMoving)
			{
				gameController.GreyPower();
			}
			//white
			if(Input.GetKeyDown("8") && canShoot && !isMoving)
			{
				gameController.WhitePower();
			}

		//}//ending bracket for mouse/keyboard exclusivity

		//some debug keys
		if(Input.GetKeyDown(KeyCode.Keypad0))
		{
			gameController.addSideColumns();
		}

		//checks if the splitter is currently between grid movement. 
		if(isMoving)
		{
			//move downwards if the direction is downwards
			if (moveDirection == -1)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f,-1);
			}
			else if (moveDirection == 1) //move upwards if the direction is upwards
			{
				transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f,-1);
			}
		}

		//keps the stored pieces in the correct positions in the splitter
		if(leftSlot != null && rightSlot != null)
		{
			leftSlot.transform.position = new Vector3(-1, transform.position.y, 0);
			rightSlot.transform.position = new Vector3(0, transform.position.y, 0);
		}

		//bugfix to ensure that both slots are full
		if (leftSlot == null && rightSlot != null) {
			leftSlot = Instantiate(pieces[Random.Range (0, gameController.availableCount)], new Vector2(-1, transform.position.y), Quaternion.identity) as Transform;
		}
		if (rightSlot == null && leftSlot != null) {
			rightSlot = Instantiate(pieces[Random.Range (0, gameController.availableCount)], new Vector2(0, transform.position.y), Quaternion.identity) as Transform;
		}

		//checks if it's reached its next spot by seeing if it's y position is a whole number. 
		if (isMoving && transform.position.y % 1 == 0) {
			moveDirection = 0;
		}
	}

	//swaps the left and right slot
	public void swap()
	{
		Transform temp = leftSlot;
		leftSlot = rightSlot;
		rightSlot = temp;
		leftSlot.transform.position = new Vector2(-1, transform.position.y);
		rightSlot.transform.position = new Vector2(0, transform.position.y);
	}

	//shoots the pieces in the correct directions
	public IEnumerator fire()
	{
		leftSlot.GetComponent<piece_script> ().inSplitter = false;
		rightSlot.GetComponent<piece_script> ().inSplitter = false;
		Transform lefttemp = leftSlot;
		Transform righttemp = rightSlot;
		leftSlot = null;
		rightSlot = null;
		lefttemp.GetComponent<Rigidbody2D>().velocity = new Vector2 (-20f, 0);
		righttemp.GetComponent<Rigidbody2D>().velocity = new Vector2 (20f, 0);
		yield return new WaitForSeconds (0.07f);
		refill ();
	}

	//refills the splitter with two new pieces
	public void refill()
	{
		holder.getNextPiece ();
		leftSlot.GetComponent<piece_script> ().locked = false;
		rightSlot.GetComponent<piece_script> ().locked = false;
		leftSlot.GetComponent<piece_script> ().inHolder = false;
		rightSlot.GetComponent<piece_script> ().inHolder = false;
		leftSlot.GetComponent<piece_script> ().inSplitter = true;
		rightSlot.GetComponent<piece_script> ().inSplitter = true;
	}

	//Adds a short pause to allow for more precise movement, then sets is moving to false to allow the player to move again
	public IEnumerator MovementPause()
	{
		yield return new WaitForSeconds (0.07f);
		isMoving = false;
	}
	
}
