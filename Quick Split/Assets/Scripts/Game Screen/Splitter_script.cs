using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splitter_script : MonoBehaviour {

	//This script handles controlling the splitter and its states

	//check this if trying to emulate mobile controls on computer
	public bool mobileDebugging;

	public class State{
		public bool isMoving; // checks if the splitter is in the middle of moving to the next grid spot
		public bool canShoot;
		public bool isActive;
		public bool mouseControl;
		public bool touchControl;
		public bool inTransition;
		public bool yellowReady;
	}

	State splitState = new State ();
	
	public int moveDirection; // 1 if it's moving upwards, -1 if downwards, 0 if not currently moving
	int moveTarget;
	float moveStartTime;
	const float moveDuration = 0.08f;
	float speed = 1;

	//prefabs containing all the different colored pieces
	public Transform[] pieces;

	//pieces currently in the splitter
	public Transform leftSlot;
	public Transform rightSlot;
	
	Vector3 mouseLocation;

	public bool overrideControlType;
	public string controlType;

	//objects the splitter will need to use
	public Holder_Script holder;
	public GameController gameController;
	public Camera mainCamera;

	Piece_Sprite_Holder spriteHolder;
	AudioSource FireSFX;

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
		if (gameController.gameType != "Quick") {
			splitState.canShoot = true;
		} else {
			splitState.canShoot = false;
		}
		splitState.isActive = true;
		splitState.inTransition = false;
		splitState.yellowReady = false;

		gameObject.GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Get_Splitter ();
		FireSFX = GetComponent<AudioSource> ();

		if (!overrideControlType) 
			controlType = PlayerPrefs.GetString("Controls", "Follow");
		else
			PlayerPrefs.SetString("Controls", controlType);

		if (controlType == "Follow"){
			speed = 2;
		}
		else
			speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (!splitState.isActive || gameController.isPaused || gameController.gameOver)
			return;
		//player Input
		//checks if the player is playing on a mobile phone, if not activate mouse control
		if (!Application.isMobilePlatform && !mobileDebugging) {
			//uncomment to constantly see mouse position.
			//Debug.Log ("Mouse Position: X:" + Input.mousePosition.x + "    Y: " + Input.mousePosition.y);
			mouseLocation = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//Debug.Log ("Mouse Position: X:" + mouseLocation.x + "    Y: " + mouseLocation.y);
			//checking that the mouse is within the grid
			if (mouseLocation.x <= 7 && mouseLocation.x >= -8 && mouseLocation.y >= -0.5 && mouseLocation.y <= 7.5) {
				if ((mouseLocation.y > transform.position.y + 0.5f) && !splitState.isMoving && transform.position.y < 7) {
					MoveUp();
				}
				//Moving downwards if the mouse is below the splitter's hitbox
				if ((mouseLocation.y < transform.position.y - 0.5f) && !splitState.isMoving && transform.position.y > 0) {
					MoveDown();
				}
				//Swapping pieces with right click if on PC
				if (Input.GetMouseButtonDown (1) ) {
					swap ();
				}
				//launching pieces with Left click while over the board
				else if (Input.GetMouseButtonDown (0) && moveDirection == 0 && rightSlot != null && leftSlot != null && splitState.canShoot 
				         && !splitState.inTransition && splitState.isActive) {
					if(splitState.yellowReady == true){
						GameObject.Find ("Spell Handler").BroadcastMessage("YellowActivate");
					}
					else{
						StartCoroutine (fire ());
						splitState.canShoot = false;
						gameController.movesMade++;
						gameController.updateMoves ();
					}
				}
			}
		}
		/*else
		{
			bool hasMoveTouch = false;
			bool hasFireTouch = false;
			//touch controls
			//Region Control Type
			if(controlType == "Regions"){
				foreach (Touch poke in Input.touches) {
					//we only want to put in one movement touch per frame, so take the first one and listen to that
					if( !hasMoveTouch)
					{
						//moving up if user has touched top of play field
						if (poke.position.y / Screen.height > 0.66f && !splitState.isMoving && transform.position.y < 7) {
							MoveUp();
							hasMoveTouch = true;
							continue;
						}
						//moving down if user has touched bottom of play field
						if (poke.position.y / Screen.height < 0.27f && !splitState.isMoving && transform.position.y > 0) {
							MoveDown ();
							hasMoveTouch = true;
							continue;
						}
					}		

					//Vector2 pokeLocation = Camera.main.ScreenToWorldPoint (poke.position);
					if(!hasFireTouch && poke.phase == TouchPhase.Began)
					{
						//check to make sure they touched an area of the screen devoted to actions
						if(poke.position.y / Screen.height <= 0.66f && poke.position.y / Screen.height >= 0.27f)
						{
							if(poke.position.x / Screen.width < 0.5f)
							{
								swap ();
								hasFireTouch = true;
							}
							else if(moveDirection == 0 && rightSlot != null && leftSlot != null && splitState.canShoot && !splitState.inTransition && !hasMoveTouch) {
								if(splitState.yellowReady == true){
									GameObject.Find ("Spell Handler").BroadcastMessage("YellowActivate");
								}
								else{
									StartCoroutine (fire ());
									splitState.canShoot = false;
									gameController.movesMade++;
									gameController.updateMoves ();
									hasFireTouch = true;
								}
							}
						}
					}
				}
			}
			//follow controls
			else if (controlType == "Follow"){
				foreach (Touch poke in Input.touches) {		
					//begin tracking fingers
					if(poke.phase == TouchPhase.Began){
						idStartPos[poke.fingerId] = poke.position;
						idStartTimes[poke.fingerId] = Time.time;
						idIsTap[poke.fingerId] = true;
						idIsSwipe[poke.fingerId] = false;
						idIsDrag[poke.fingerId] = false;
					}

					if((!idIsTap[poke.fingerId] && !idIsSwipe[poke.fingerId]) || idIsDrag[poke.fingerId]){
						Vector3 pokeLocation = mainCamera.ScreenToWorldPoint(poke.position);
						idIsDrag[poke.fingerId] = true;
						if (pokeLocation.x <= 7 && pokeLocation.x >= -8 && pokeLocation.y >= -0.5 && pokeLocation.y <= 7.5) {
							if ((pokeLocation.y > transform.position.y + 0.5f) && !splitState.isMoving && transform.position.y < 7) {
								MoveUp();
							}
							//Moving downwards if the mouse is below the splitter's hitbox
							if ((pokeLocation.y < transform.position.y - 0.5f) && !splitState.isMoving && transform.position.y > 0) {
								MoveDown();
							}
						}
					}

					if(!idIsSwipe[poke.fingerId] && !idIsDrag[poke.fingerId]){


						if(Mathf.Abs (idStartPos[poke.fingerId].x- poke.position.x) >= DisplayMetricsAndroid.XDPI/8) {
							swap ();
							idIsTap[poke.fingerId] = false;
							idIsSwipe[poke.fingerId] = true;
						}
					}

					if(poke.phase == TouchPhase.Ended){
						//gotta check to make sure they tapped the board to split in order to prevent confusion with the pause or spell interactables
						Vector3 pokeLocation = mainCamera.ScreenToWorldPoint(poke.position);
						if(idIsTap[poke.fingerId] && pokeLocation.y < 8 && pokeLocation.y >= -0.5 && rightSlot != null && leftSlot != null && splitState.canShoot && !splitState.inTransition){
							//tap
							if(splitState.yellowReady == true){
								GameObject.Find ("Spell Handler").BroadcastMessage("YellowActivate");
							}
							else{
								StartCoroutine (fire ());
								splitState.canShoot = false;
								gameController.movesMade++;
								gameController.updateMoves ();
							}
						}
					}

					if(poke.phase == TouchPhase.Moved || Mathf.Abs(Time.time - idStartTimes[poke.fingerId]) > 0.16f ){
						idIsTap[poke.fingerId] = false;
					}
				}
			}
		}*/

		//moving upwards with keys W or Up
		if ((Input.GetKey ("w") || Input.GetKey ("up")) && !splitState.isMoving && transform.position.y < 7) {
			MoveUp ();
		}
		//moving downwards with keys S or Down
		if ((Input.GetKey ("s") || Input.GetKey ("down")) && !splitState.isMoving && transform.position.y > 0) {
			MoveDown ();
		}
		//swapping pieces with keys A, D, Left, or Right
		if (Input.GetKeyDown ("a") || Input.GetKeyDown ("d") || Input.GetKeyDown ("left") || Input.GetKeyDown ("right")) {
			swap ();
		}
		//launching pieces with key Space
		if (Input.GetKeyDown ("space") && moveDirection == 0 && rightSlot != null && leftSlot != null && splitState.canShoot && !splitState.inTransition) {
			if(splitState.yellowReady == true){
				GameObject.Find ("Spell Handler").BroadcastMessage("YellowActivate");
			}
			else{
				StartCoroutine (fire ());
				splitState.canShoot = false;
				gameController.movesMade++;
				gameController.updateMoves ();
			}
		}

		if(Input.GetMouseButtonUp(0))
		{
			splitState.inTransition = false;
		}
		//}//ending bracket for mouse/keyboard exclusivity

		//some debug keys
		if (Input.GetKeyDown (KeyCode.Keypad0)) {
				gameController.addSideColumns ();
		}

	}

	void FixedUpdate(){
		//checks if the splitter is currently between grid movement. 
		if(splitState.isMoving)
		{
			//check to see if the movement time is up. If it is, put it to it's proper location
			if(Mathf.Abs(moveStartTime - Time.time) > (moveDuration / speed))
			{
				moveDirection = 0;
				splitState.isMoving = false;
				transform.position = new Vector3(transform.position.x, moveTarget,transform.position.z);
				gameObject.BroadcastMessage ("Stopping", null, SendMessageOptions.DontRequireReceiver);
			}
			else //visually move it
			{
				transform.position = new Vector3(transform.position.x, transform.position.y + (moveDirection * (0.25f * speed)) , transform.position.z);
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
		if (splitState.isMoving && transform.position.y % 1 == 0) {
			moveDirection = 0;
		}
	}

	void MoveUp()
	{
		//first check to make sure it's possible
		if (splitState.isMoving || transform.position.y >= 6.9f) {
			return;
		}
		
		int currentLoc = (int) transform.position.y;
		moveTarget = currentLoc + 1;
		
		moveDirection = 1;
		moveStartTime = Time.time;
		splitState.isMoving = true;
	}

	void MoveDown()
	{
		//first check to make sure it's possible
		if (splitState.isMoving || transform.position.y <= 0.1f) {
			return;
		}

		int currentLoc = (int) transform.position.y;
		moveTarget = currentLoc - 1;

		moveDirection = -1;
		moveStartTime = Time.time;
		splitState.isMoving = true;
	}
	//swaps the left and right slot
	public void swap()
	{
		if(leftSlot != null && rightSlot != null){
			Transform temp = leftSlot;
			leftSlot = rightSlot;
			rightSlot = temp;
			leftSlot.transform.position = new Vector2(-1, transform.position.y);
			rightSlot.transform.position = new Vector2(0, transform.position.y);
		}
	}

	//shoots the pieces in the correct directions
	public IEnumerator fire()
	{
		FireSFX.volume = PlayerPrefs.GetFloat ("SFX Volume", 1);
		FireSFX.Play ();
		//tell the wedges that it has fired
		gameObject.BroadcastMessage ("Has_Fired", null, SendMessageOptions.DontRequireReceiver);
		GameObject tempSH = GameObject.Find ("Spell Handler");
		if (tempSH != null)
			tempSH.BroadcastMessage ("split", null, SendMessageOptions.DontRequireReceiver);
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
		piece_script[] slots = new piece_script[2] {leftSlot.GetComponent<piece_script> (),rightSlot.GetComponent<piece_script> ()};
		for (int i = 0; i < 2; i++) {
			slots[i].locked = false;
			slots[i].inHolder = false;
			slots[i].inSplitter = true;
		}
	}

	public State getState()
	{
		return splitState;
	}

	public bool getState(string name)
	{
		switch (name){
		case "isMoving":
			return splitState.isMoving;
		case "canShoot":
			return splitState.canShoot;
		case "isActive":
			return splitState.isActive;
		case "mouseControl":
			return splitState.mouseControl;
		case "touchControl":
			return splitState.touchControl;
		case "inTransition":
			return splitState.inTransition;
		case "yellowReady":
			return splitState.yellowReady;
		}
		Debug.LogError ("State error: no state of name " + name + " detected.");

		return false;
	}

	public bool setState(string name, bool value)
	{
		switch (name){
		case "isMoving":
			splitState.isMoving = value;
			return true;
		case "canShoot":
			splitState.canShoot = value;
			return true;
		case "isActive":
			splitState.isActive = value;
			return true;
		case "mouseControl":
			splitState.mouseControl = value;
			return true;
		case "touchControl":
			splitState.touchControl = value;
			return true;
		case "inTransition":
			splitState.inTransition = value;
			return true;
		case "yellowReady":
			splitState.yellowReady = value;
			return true;
		}

		Debug.LogError ("State error: no state of name " + name + " detected.");
		return false;
	}
}
