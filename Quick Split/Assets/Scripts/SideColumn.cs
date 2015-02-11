using UnityEngine;
using System.Collections;

public class SideColumn : MonoBehaviour {

	//List of piece prefabs to randomize from
	public GameObject[] pieces;

	//will track if it's left or right, then keep its proper X value
	public string side;
	public int sideInt;
	float sideXValue;

	Vector2 permPosition;

	public bool isShaking;

	// for moving the pieces closer and how many moves are required before adding this row
	public int stepValue;

	//Will contain the gameobjects, similar to the grids in the gamecontroller
	public GameObject[] column = new GameObject[8];
	public string[] colorColumn = new string[8];

	public GameController gameController;

	// Use this for initialization
	void Start () {

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();
		}

		// begin by poisitioning these at the right locations
		if (side == "Left" || side == "left" || side == "L" || side == "l") {
			Vector2 tempPos = new Vector2(-9.5f, 0);
			this.transform.position = tempPos;
			sideXValue = -9.5f;
			sideInt = 0;
		}
		else if (side == "Right" || side == "right" || side == "R" || side == "R")
		{
			Vector2 tempPos = new Vector2(8.5f, 0);
			this.transform.position = tempPos;
			sideXValue = 8.5f;
			sideInt = 1;
		}
		else
		{
			//stupid mistake was made
			Debug.Log ("Side Column Error: side not assigned with a valid position string");
			Destroy(this);
			return;
		}

		//now load it up
		empty ();
		reload ();
		isShaking = false;

		permPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(isShaking)
			shake ();
	}

	//empties the column, ideally after the objects have been taken from it already
	public void empty()
	{
		for (int row = 0; row < 8; row++) {
			column[row] = null;
			colorColumn[row] = null;
		}
	}

	public void shake ()
	{
		transform.position = new Vector2 (Random.Range (-0.1f, 0.1f) + permPosition.x, 
		                                  Random.Range (-0.1f, 0.1f) + permPosition.y);
	}

	//reloads column after it's been taken by the grid. Only for use by the gamecontroller
	public void reload()
	{
		if(column[0] != null || colorColumn[0] != null)
		{
			Debug.Log ("Side Column Error: Trying to reload a loaded column");
			return;
		}

		for (int row = 0; row < 8; row++) {
			int randPiece = Random.Range (0, gameController.availableCount);
			column[row] = Instantiate (pieces[randPiece], new Vector2 ( sideXValue, row), Quaternion.identity) as GameObject;
			//column[row].GetComponent<piece_script> ().locked = true;
			column[row].GetComponent<piece_script> ().lockPos = new Vector2 ( sideXValue, row);
			column[row].GetComponent<piece_script> ().inSideHolder = true;
			column[row].transform.parent = transform;
			colorColumn[row] = column[row].GetComponent<piece_script>().pieceColor;
		}
	}
}
