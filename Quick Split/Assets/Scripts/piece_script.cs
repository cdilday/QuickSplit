using UnityEngine;
using System.Collections;

public class piece_script : MonoBehaviour {

	public string pieceColor;
	public bool inSplitter;
	public bool inHolder;
	public bool inSideHolder;
	public bool isBomb;

	public GameObject scoreTextPrefab;

	public bool locked;
	public Vector2 lockPos;
	public Vector2 gridPos;
	Vector2 prevPos;

	//maximum number of scorebits that can be spawned per piece. 2-3 is your best bet for performance
	int scoreBitMax = 2;

	Vector2 moveToPos;
	int moveProgress;
	float moveStepx;
	float moveStepy;
	bool isMoving;

	public bool selectable = false;

	public Sprite[] sprites = new Sprite[8];
	//value assigned to each piece that shows how many pieces are in a group of adjacent stuff.
	public int groupValue;

	//stores multiplier to reflect accurate score;
	public int multiplier;

	public GameController gameController;

	public Splitter_script splitter;

	public SpellHandler spellHandler;

	// Use this for initialization
	void Start () {
		isBomb = false;
		isMoving = false;
		groupValue = 1;
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		GameObject splitterObject = GameObject.Find("Splitter");
		if (splitterObject != null) {
			splitter = splitterObject.GetComponent <Splitter_script>();
		}
		GameObject spellHandlerObject = GameObject.Find("Spell Handler");
		if (spellHandlerObject != null) {
			spellHandler = spellHandlerObject.GetComponent <SpellHandler>();
		}
		//set grid position to -3,-3 until it's locked to prevent accidental cancelling.
		gridPos = new Vector2 (-3, -3);
		//multiplier = 1;
	}

	void FixedUpdate () {
		//this code is to ensure collisions don't offset piece's individual positions
		bool changedPos = false;
		if (transform.position.x != prevPos.x || transform.position.y != prevPos.y) {
			changedPos = true;
			prevPos = transform.position;
		}
		if (!isMoving && locked && lockPos != prevPos) {
			transform.position = lockPos;
			if(!inHolder && !inSideHolder){
				gridPos = new Vector2((int)lockPos.y, (int)lockPos.x + 8);
			}
		}

		if(isMoving && moveProgress < 10)
		{
			transform.position = new Vector2(transform.position.x + (moveStepx), transform.position.y + (moveStepy));
			moveProgress++;
		}
		else if(isMoving)
		{
			transform.position = moveToPos;
			isMoving = false;
		}

		if(changedPos)
		{
			if(!inSplitter && !inHolder)
				this.name = pieceColor + " piece (" + gridPos.x + ", " + gridPos.y + ")";
			else if (inHolder)
			{
				this.name = pieceColor + " in Holder";
			}
			else if (inSplitter)
				this.name = pieceColor + " in Splitter";
		}

	}
		
	//2D collision detection
	void OnTriggerEnter2D(Collider2D col)
	{
		//ignore score bits
		if (col.gameObject.tag == "Score Bit")
			return;
		//if the piece hasn't already been assigned a position, begin to assign it
		if(!isMoving && !locked && !inSplitter && !inSideHolder){
			piece_script colPiece = col.gameObject.GetComponent<piece_script> ();
			//if it collided with the side of a grid, place it in the grid
			if (colPiece == null) {
				transform.GetComponent<Rigidbody2D>().velocity = new Vector2 (0,0);
				GetComponent<Rigidbody2D>().isKinematic = false;
				locked = true;
				lockPos = new Vector2(Mathf.Round (transform.position.x), Mathf.Round (transform.position.y));
				transform.position = lockPos;
				gridPos = new Vector2((int)lockPos.y, (int)lockPos.x + 8);
				gameController.placePiece(gameObject);
			}
			//if it collided with another piece, determine where that piece is and place it relative to that piece
			else if(colPiece.locked == true && !colPiece.inSplitter)
			{
				//check if it was fired left
				if(transform.GetComponent<Rigidbody2D>().velocity.x < 0 && !colPiece.inSplitter && !inSplitter)
				{
					transform.GetComponent<Rigidbody2D>().velocity = new Vector2 (0,0);
					GetComponent<Rigidbody2D>().isKinematic = false;
					locked = true;
					lockPos = new Vector2(Mathf.Round (col.transform.position.x + 1), Mathf.Round(col.transform.position.y));
					transform.position = lockPos;
					gridPos = new Vector2((int)lockPos.y, (int)lockPos.x + 8);
				}
				//check if it was fired right
				else if(transform.GetComponent<Rigidbody2D>().velocity.x > 0 && !colPiece.inSplitter && !inSplitter)
				{
					transform.GetComponent<Rigidbody2D>().velocity = new Vector2 (0,0);
					GetComponent<Rigidbody2D>().isKinematic = false;
					locked = true;
					lockPos = new Vector2(Mathf.Round (col.transform.position.x - 1), Mathf.Round(col.transform.position.y));
					transform.position = lockPos;
					gridPos = new Vector2((int)lockPos.y, (int)lockPos.x + 8);
				}
				//places the piece in the grid upkept by the game controller
				gameController.placePiece(gameObject);
			}
			if(isBomb)
				Destroy (gameObject);
		}
	}

	//takes in a vector2 for the new location and does the appropriate changes
	public void movePiece(Vector2 newLoc)
	{
		moveStepx = (newLoc.x - transform.position.x) / 10f;
		moveStepy = (newLoc.y - transform.position.y) / 10f;
		isMoving = true;
		locked = false;
		moveToPos = newLoc;
		moveProgress = 0;
		lockPos = newLoc;
		// the strange vector2 is because the grid has no negatives and the x/y are switched
		gridPos = new Vector2(newLoc.y, newLoc.x + 8);
		locked = true;

		//update the gamecontroller's grid
		gameController.grid [(int)gridPos.x, (int)gridPos.y] = gameObject;
		gameController.colorGrid [(int)gridPos.x, (int)gridPos.y] = pieceColor;
	}

	public void ConvertColor(string newColor)
	{
		if (newColor == pieceColor){
			return;
		}

		pieceColor = newColor;
		if(!inHolder && !inSplitter){
			gameController.colorGrid [(int)gridPos.x, (int)gridPos.y] = newColor;
		}
		switch (newColor)
		{
		case "Red":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
			break;
		case "Orange":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
			break;
		case "Yellow":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
			break;
		case "Green":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
			break;
		case "Blue":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[4];
			break;
		case "Purple":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[5];
			break;
		case "Grey":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[6];
			break;
		case "White":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[7];
			break;
		}
	}

	void OnDestroy()
	{
		//this means that the game just ended, don't spawn stuff
		if (gameController.isQuitting || gameController.gameOver)
			return;
		//spawn a GUI text prefab that shows what number the square was worth
		Vector2 spawnPoint = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>().WorldToViewportPoint (transform.position);
		GameObject scoreText = Instantiate (scoreTextPrefab) as GameObject;
		scoreText.transform.position = spawnPoint;
		PieceScoreText thistext = scoreText.GetComponent<PieceScoreText> ();
		thistext.pieceColor = pieceColor;
		if (multiplier != 0)
			thistext.scoreValue = groupValue * multiplier;
		else
			thistext.scoreValue = groupValue;
		//gameController.score += thistext.scoreValue;
		//gameController.updateScore();
		int indivalue = thistext.scoreValue / scoreBitMax;
		int leftover = thistext.scoreValue % scoreBitMax;
		for (int i = 0; i < scoreBitMax; i++) {
			if(indivalue == 0 && leftover == 0)
				break;
			else{
				GameObject newbit = Instantiate((GameObject) Resources.Load ("Score Bit"));
				newbit.transform.position = transform.position;
				newbit.GetComponent<ScoreBit>().target = gameController.scoreText.transform.GetComponent<BoxCollider2D>().offset 
															+ new Vector2( gameController.scoreText.transform.position.x,
					              											gameController.scoreText.transform.position.y);
				newbit.GetComponent<ScoreBit>().changeColor(pieceColor);
				newbit.GetComponent<ScoreBit>().value = indivalue;
				if(leftover > 0)
				{
					leftover--;
					newbit.GetComponent<ScoreBit>().value++;
				}
			}

		}
		if (isBomb) {
			for(int r = 0; r < 3; r++)
			{
				for (int c = 0; c < 3; c++)
				{
					//check to make sure it's a valid move
					if((int)(gridPos.x-1+r) >= 0 && (int)(gridPos.x-1+r) <= 7 && (int)gridPos.y-1+c >= 0 && (int)gridPos.y-1+c <= 15 &&
					   gameController.grid[(int)gridPos.x - 1 + r, (int) gridPos.y - 1 + c] != null)
					{
						Destroy(gameController.grid[(int)gridPos.x - 1 + r, (int) gridPos.y - 1 + c]);
						gameController.grid[(int)gridPos.x - 1 + r, (int) gridPos.y - 1 + c] = null;
						gameController.colorGrid[(int)gridPos.x - 1 + r, (int) gridPos.y - 1 + c] = null;
					}
				}
			}
			gameController.collapse ();
		}
	}

	void OnMouseOver(){
		if (selectable && Input.GetMouseButtonDown (0)) {
			if(spellHandler.spellColor == "Green" || spellHandler.spellColor == "Blue")
			{
				GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
				picker.GetComponent<Color_Selector> ().givePurpose ("Select a color to change this piece to");
				spellHandler.selectedPiece = this;
				selectable = false;
			}
			//gameObject.GetComponentInParent<Color_Selector>().colorSelected(pieceColor);
		}
	}
	
	
}
