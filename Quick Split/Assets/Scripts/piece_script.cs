using UnityEngine;
using System.Collections;

public class piece_script : MonoBehaviour {

	public string pieceColor;
	public bool inSplitter;
	public bool inHolder;
	public bool inSideHolder;

	public GameObject scoreTextPrefab;

	public bool locked;
	public Vector2 lockPos;
	public Vector2 gridPos;
	Vector2 prevPos;

	Vector2 moveToPos;
	int moveProgress;
	float moveStep;
	bool isMoving;

	//value assigned to each piece that shows how many pieces are in a group of adjacent stuff.
	public int groupValue;

	//stores multiplier to reflect accurate score;
	public int multiplier;

	public GameController gameController;

	public Splitter_script splitter;

	// Use this for initialization
	void Start () {
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
		//set grid position to -3,-3 until it's locked to prevent accidental cancelling.
		gridPos = new Vector2 (-3, -3);
		//multiplier = 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//this code is to ensure collisions don't offset piece's individual positions
		prevPos = transform.position;
		if (!isMoving && locked && lockPos != prevPos) {
			transform.position = lockPos;
			if(!inHolder)
				gridPos = new Vector2((int)lockPos.y + 8, (int)lockPos.y);
		}

		if(isMoving && moveProgress <= 10)
		{
			transform.position = new Vector2(transform.position.x + (moveStep), transform.position.y);
			moveProgress++;
		}
		else if(isMoving)
		{
			transform.position = moveToPos;
			isMoving = false;
		}

		if(!inSplitter && !inHolder)
			this.name = pieceColor + " piece (" + gridPos.x + ", " + gridPos.y + ")";
		else if (inHolder)
		{
			this.name = pieceColor + " in Holder";
		}
		else if (inSplitter)
			this.name = pieceColor + " in Splitter";

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
		}
	}

	//takes in a vector2 for the new location and does the appropriate changes
	public void movePiece(Vector2 newLoc)
	{
		moveStep = (newLoc.x - transform.position.x) / 10f;
		isMoving = true;
		locked = false;
		moveToPos = newLoc;
		moveProgress = 0;
		lockPos = newLoc;
		// the strange vector2 is because the grid has no negatives and the x/y are switched
		gridPos = new Vector2(newLoc.y, newLoc.x + 8);
		locked = true;
	}


	void OnDestroy()
	{
		//this means that the game just ended, don't spawn stuff
		if (gameController.isQuitting || gameController.gameOver)
			return;
		//spawn a GUI text prefab that shows what number the square was worth
		Camera tempCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		Vector2 spawnPoint = tempCamera.WorldToViewportPoint (transform.position);
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

		for (int i = 0; i < thistext.scoreValue; i++) {
			GameObject newbit = Instantiate((GameObject) Resources.Load ("Score Bit"));
			newbit.transform.position = transform.position;
			newbit.GetComponent<ScoreBit>().target = gameController.scoreText.transform.GetComponent<BoxCollider2D>().offset 
														+ new Vector2( gameController.scoreText.transform.position.x,
				              											gameController.scoreText.transform.position.y);
			newbit.GetComponent<ScoreBit>().changeColor(pieceColor);
		}
	}
	
	
}
