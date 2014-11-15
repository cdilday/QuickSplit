using UnityEngine;
using System.Collections;

public class piece_script : MonoBehaviour {

	public string pieceColor;
	public bool inSplitter;
	public bool inHolder;

	public bool locked;
	public Vector2 lockPos;
	public Vector2 gridPos;
	public Vector2 prevPos;

	//value assigned to each piece that shows how many pieces are in a group of adjacent stuff.
	public int groupValue;

	public GameController gameController;

	public Splitter_script splitter;

	// Use this for initialization
	void Start () {
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
	}
	
	// Update is called once per frame
	void Update () {
		//this code is to ensure collisions don't offset piece's individual positions
		prevPos = transform.position;
		if (locked && lockPos != prevPos) {
			transform.position = lockPos;
			if(!inHolder)
				gridPos = new Vector2((int)lockPos.y + 8, (int)lockPos.y);
		}

	}
		
	//2D collision detection
	void OnTriggerEnter2D(Collider2D col)
	{
		//if the piece hasn't already been assigned a position, begin to assign it
		if(!locked && !inSplitter){
			piece_script colPiece = col.gameObject.GetComponent<piece_script> ();
			//if it collided with the side of a grid, place it in the grid
			if (colPiece == null) {
				rigidbody2D.isKinematic = false;
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
				if(transform.rigidbody2D.velocity.x < 0 && !colPiece.inSplitter && !inSplitter)
				{
					transform.rigidbody2D.velocity = new Vector2 (0,0);
					rigidbody2D.isKinematic = false;
					locked = true;
					lockPos = new Vector2(Mathf.Round (col.transform.position.x + 1), Mathf.Round(col.transform.position.y));
					transform.position = lockPos;
					gridPos = new Vector2((int)lockPos.y, (int)lockPos.x + 8);
				}
				//check if it was fired right
				else if(transform.rigidbody2D.velocity.x > 0 && !colPiece.inSplitter && !inSplitter)
				{
					transform.rigidbody2D.velocity = new Vector2 (0,0);
					rigidbody2D.isKinematic = false;
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
}
