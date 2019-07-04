using UnityEngine;
using System.Collections;

public class Holder_Script : MonoBehaviour {

	//This handles the pieces in the holder above the game grid

	public Transform[,] holder = new Transform[3,2];

	public GameController gameController;
	public Splitter_script splitScript;

	// Use this for initialization
	void Start () {
		//getting the splitter object for easy use later in the code
		GameObject splitterObject = GameObject.FindWithTag ("Splitter");
		if (splitterObject != null) {
			splitScript = splitterObject.GetComponent <Splitter_script>();
		}
		//getting the gamecontroller for easy reference later in the code
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();
		}

		for (int slot = 0; slot < 3; slot++) {
			int left = Random.Range (0, gameController.availableCount);
			int right = Random.Range (0, gameController.availableCount);
			holder[slot, 0] = Instantiate (splitScript.pieces [left], new Vector2 ( -1 - slot * 2.5f, transform.position.y), Quaternion.identity) as Transform;
			holder[slot, 1] = Instantiate (splitScript.pieces [right], new Vector2 ( 0 - slot * 2.5f, transform.position.y), Quaternion.identity) as Transform;
			holder[slot, 0].GetComponent<Piece> ().locked = true;
			holder[slot, 0].GetComponent<Piece> ().inHolder = true;
			holder[slot, 0].GetComponent<Piece> ().lockPos = new Vector2 ( -1 - slot * 2.5f, transform.position.y);
			holder[slot, 1].GetComponent<Piece> ().locked = true;
			holder[slot, 1].GetComponent<Piece> ().inHolder = true;
			holder[slot, 1].GetComponent<Piece> ().lockPos = new Vector2 ( 0 - slot * 2.5f, transform.position.y);
		}
	}

	//handles moving the pieces along and loading the next pieces into the splitter
	public void getNextPiece()
	{
		splitScript.leftSlot = holder [0, 0];
		splitScript.rightSlot = holder [0, 1];
		holder [0, 0] = holder [1, 0];
		holder[0, 0].GetComponent<Piece> ().lockPos = new Vector2 ( -1, transform.position.y);
		holder [0, 1] = holder [1, 1];
		holder[0, 1].GetComponent<Piece> ().lockPos = new Vector2 ( 0, transform.position.y);
		holder [1, 0] = holder [2, 0];
		holder[1, 0].GetComponent<Piece> ().lockPos = new Vector2 ( -1 - 2.5f, transform.position.y);
		holder [1, 1] = holder [2, 1];
		holder[1, 1].GetComponent<Piece> ().lockPos = new Vector2 ( 0 - 2.5f, transform.position.y);

		//make new pieces for the missing slot
		int left = Random.Range (0, gameController.availableCount);
		int right = Random.Range (0, gameController.availableCount);
		holder[2, 0] = Instantiate (splitScript.pieces [left], new Vector2 ( -1 - 2 * 2.5f, transform.position.y), Quaternion.identity) as Transform;
		holder[2, 1] = Instantiate (splitScript.pieces [right], new Vector2 ( 0 - 2 * 2.5f, transform.position.y), Quaternion.identity) as Transform;
		holder[2, 0].GetComponent<Piece> ().locked = true;
		holder[2, 0].GetComponent<Piece> ().inHolder = true;
		holder[2, 0].GetComponent<Piece> ().lockPos = new Vector2 ( -1 - 2 * 2.5f, transform.position.y);
		holder[2, 1].GetComponent<Piece> ().locked = true;
		holder[2, 1].GetComponent<Piece> ().inHolder = true;
		holder[2, 1].GetComponent<Piece> ().lockPos = new Vector2 ( 0 - 2 * 2.5f, transform.position.y);
	}
}
