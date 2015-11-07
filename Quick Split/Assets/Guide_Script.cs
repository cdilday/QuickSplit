using UnityEngine;
using System.Collections;

public class Guide_Script : MonoBehaviour {

	bool isRight;
	Splitter_script splitter;
	GameController gameController;

	string pieceColor;

	SpriteRenderer spriteRenderer;


	// Use this for initialization
	void Start () {
		//get rid of the guide piece if 
		if(PlayerPrefs.GetInt("Guide", 1) == 0)
		{
			Destroy (gameObject);
		}

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		splitter = GameObject.FindGameObjectWithTag ("Splitter").GetComponent<Splitter_script> ();

		if (transform.localPosition.x > 0) {
			isRight = true;
		}
		else{
			isRight = false;
		}

		spriteRenderer = GetComponent<SpriteRenderer> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isRight) {
			//get the color to match
			if(splitter.rightSlot){
				switch(splitter.rightSlot.GetComponent<piece_script>().pieceColor){
				case "Red":
					spriteRenderer.color = Color.red;
					break;
				case "Orange":
					spriteRenderer.color = new Color(1f, 0.5f, 0f);
					break;
				case "Yellow":
					spriteRenderer.color = Color.yellow;
					break;
				case "Green":
					spriteRenderer.color = Color.green;
					break;
				case "Blue":
					spriteRenderer.color = Color.blue;
					break;
				case "Purple":
					spriteRenderer.color = new Color(0.6f, 0, 0.6f);
					break;
				case "Grey":
					spriteRenderer.color = Color.cyan;
					break;
				case "White":
					spriteRenderer.color = Color.white;
					break;

				}
			}

			int row = (int) transform.position.y;
			if(gameController.grid[row,9] != null)
			{
				spriteRenderer.color = new Color(1f, 0.5f, 0f, 0);
			}
			else{
				bool hitEnd = true;
				for(int c = 10; c < 16; c++)
				{
					if(gameController.grid[row,c] != null)
					{
						transform.localPosition = new Vector2((float) c - 8.5f, transform.localPosition.y);
						hitEnd = false;
						break;
					}
				}
				if(hitEnd)
				{
					transform.localPosition = new Vector2(7.5f, transform.localPosition.y);
				}
			}


		}
		else
		{
			if(splitter.leftSlot){
				switch(splitter.leftSlot.GetComponent<piece_script>().pieceColor){
				case "Red":
					spriteRenderer.color = Color.red;
					break;
				case "Orange":
					spriteRenderer.color = new Color(1f, 0.5f, 0f);
					break;
				case "Yellow":
					spriteRenderer.color = Color.yellow;
					break;
				case "Green":
					spriteRenderer.color = Color.green;
					break;
				case "Blue":
					spriteRenderer.color = Color.blue;
					break;
				case "Purple":
					spriteRenderer.color = new Color(0.6f, 0, 0.6f);
					break;
				case "Grey":
					spriteRenderer.color = Color.cyan;
					break;
				case "White":
					spriteRenderer.color = Color.white;
					break;
				}
			}

			int row = (int) transform.position.y;
			if(gameController.grid[row,6] != null)
			{
				spriteRenderer.color = new Color(1f, 0.5f, 0f, 0);
			}
			else{
				bool hitEnd = true;
				for(int c = 5; c >= 0; c--)
				{
					if(gameController.grid[row,c] != null)
					{
						transform.localPosition = new Vector2((float) c - 6.5f, transform.localPosition.y);
						hitEnd = false;
						break;
					}
				}
				if(hitEnd)
				{
					transform.localPosition = new Vector2(-7.5f, transform.localPosition.y);
				}
			}
		}
	}
}
