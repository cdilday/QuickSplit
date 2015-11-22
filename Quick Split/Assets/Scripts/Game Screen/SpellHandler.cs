using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class SpellHandler : MonoBehaviour {

	GameController gameController;
	Splitter_script splitter;
	Holder_Script holder;

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
	public Text redText;
	public Text orangeText;
	public Text yellowText;
	public Text greenText;
	public Text blueText;
	public Text purpleText;
	public Text greyText;
	public Text whiteText;
	#endregion

	public string spellColor;
	public piece_script selectedPiece;
	string pickedColor1;
	string pickedColor2;
	int spellLimit = 0;

	//this is to tell scorebits that are created as a result of spells not to charge them
	public bool spellActive;
	int splitsNeeded = 0;

	bool[] spellsUsed = new bool[8] {false, false, false, false, false, false, false, false};

	public float wizMultiplier;
	public float holyMultiplier;
	float chargeMultiplier;

	GameObject[] RedSpellEffects;
	GameObject YellowSpellEffect;

	// Use this for initialization
	void Start () {
		pickedColor1 = null;
		pickedColor2 = null;
		GameObject gameControllerobject = GameObject.FindGameObjectWithTag ("GameController");
		RedSpellEffects = GameObject.FindGameObjectsWithTag ("Red Spell Effect");
		YellowSpellEffect = GameObject.Find ("Yellow Spell Handler");
		foreach (GameObject rse in RedSpellEffects) {
			rse.SetActive(false);
		}

		if (gameControllerobject == null) {
			Debug.LogError("spell Handler Error: cannot find game controller");
		}
		else{
			gameController = gameControllerobject.GetComponent<GameController>();
		}

		GameObject splitterObject = GameObject.FindGameObjectWithTag ("Splitter");
		if (splitterObject == null) {
			Debug.LogError("spell Handler Error: cannot find Splitter");
		}
		else{
			splitter = splitterObject.GetComponent<Splitter_script>();
		}

		GameObject holderObject = GameObject.FindGameObjectWithTag ("Holder");
		if (holderObject == null) {
			Debug.LogError("spell Handler Error: cannot find holder");
		}
		else{
			holder = holderObject.GetComponent<Holder_Script>();
		}

		/*redReady = false;
		orangeReady = false;
		yellowReady = false;
		greenReady = false;
		blueReady = false;
		purpleReady = false;
		greyReady = false;
		whiteReady = false;*/

		redText.text = "";
		orangeText.text = "";
		yellowText.text = "";
		greenText.text = "";
		blueText.text = "";
		purpleText.text = "";
		greyText.text = "";
		whiteText.text = "";

		selectedPiece = null;
		if (gameController.gameType == "Wiz")
			chargeMultiplier = wizMultiplier;
		else
			chargeMultiplier = holyMultiplier;


		spellActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		//attacks, only activatable one at a time
		if(spellColor == null || spellColor == ""){
			//red
			if (Input.GetKeyDown ("1") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && redReady) {
				Redspell ();
			}
			//orange
			if (Input.GetKeyDown ("2") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && orangeReady) {
				Orangespell ();
			}
			//yellow
			if (Input.GetKeyDown ("3") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && yellowReady) {
				Yellowspell ();
			}
			//green
			if (Input.GetKeyDown ("4") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && greenReady) {
				Greenspell ();
			}
			//blue
			if (Input.GetKeyDown ("5") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && blueReady) {
				Bluespell ();
			}
			//purple
			if (Input.GetKeyDown ("6") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && purpleReady) {
				Purplespell ();
			}
			//grey
			if (Input.GetKeyDown ("7") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && greyReady) {
				Greyspell ();
			}
			//white
			if (Input.GetKeyDown ("8") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && whiteReady) {
				Whitespell ();
			}
		}
	}

	//Red attack: Burns a layer off the top of each side, specifically deleting the block in each row closest to the center
	public void Redspell()
	{
		Spell_Used (0);

		/*Deletion loops work by going to the splitter's columns outwards and deleting the first piece it comes across before moving on
		 * likely the player would only use this ability when on the brink of losing, so this is better than going from outwards in.
		 * Once the loop deletes the first thing it comes accross, it exits the inner loop to move onto the next row.*/
		/*//left grid loop
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
		gameController.checkBoard ();*/
		
		foreach(GameObject rse in RedSpellEffects)
		{
			rse.SetActive(true);
			rse.BroadcastMessage("Activate", null, SendMessageOptions.DontRequireReceiver);
		}
		spellLimit = 16;
		splitter.setState ("isActive", false);
	}

	public void Red_Spell_Helper()
	{
		spellLimit--;
		if (spellLimit == 0) {
			gameController.checkBoard ();
			splitter.setState ("isActive", true);
		}
	}

	//Orange attack: switches all the pieces of a single color on one side with all the pieces of a different single color on the other side
	//deletes leftover pieces if the switch is uneven.
	public void Orangespell()
	{
		Spell_Used (1);
		spellColor = "Orange";
		GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
		picker.GetComponent<Color_Selector> ().givePurpose ("Select a color to switch with on the left side");
	}
	void OrangeHelper ()
	{

		List<GameObject> leftPieces = new List<GameObject>();
		List<GameObject> rightPieces = new List<GameObject>();

		//go through left side, store all pieces of color 1 in an array
		for (int r = 0; r < 8; r++) {
			for (int c = 7; c >=0; c--){
				//Debug.Log ("Checking position R: " + r + " C: " + c);
				if(gameController.grid[r,c] != null)
				{
					if(gameController.colorGrid[r,c] == pickedColor1)
					{
						leftPieces.Add (gameController.grid[r,c]);
					}
				}
			}
		}
		//same with right
		for (int r = 0; r < 8; r++) {
			for (int c = 8; c < 16; c++){
				//Debug.Log ("Checking position R: " + r + " C: " + c);
				if(gameController.grid[r,c] != null)
				{
					if(gameController.colorGrid[r,c] == pickedColor2)
					{
						rightPieces.Add (gameController.grid[r,c]);
					}
				}
			}
		}
		//get both lengths and store the smallest size
		int smallestSize = Mathf.Min (leftPieces.Count, rightPieces.Count);
		int largestSize = Mathf.Max (leftPieces.Count, rightPieces.Count);
		int difference = largestSize - smallestSize;
		bool leftLarger = (leftPieces.Count >= rightPieces.Count);
		//delete the remainder on the bigger side randomly
		bool deleted = false;
		if(leftLarger){
			for (; difference > 0; difference--)
			{
				int randPiece = (int) Random.Range(0, leftPieces.Count);
				int r =(int) leftPieces[randPiece].GetComponent<piece_script>().gridPos.x;
				int c =(int) leftPieces[randPiece].GetComponent<piece_script>().gridPos.y;
				leftPieces.RemoveAt(randPiece);
				gameController.grid[r,c].BroadcastMessage("Activate_Orange", "dead", SendMessageOptions.DontRequireReceiver);
			}
		}
		else {
			for (; difference > 0; difference--)
			{
				int randPiece = (int) Random.Range(0, rightPieces.Count);
				int r = (int) rightPieces[randPiece].GetComponent<piece_script>().gridPos.x;
				int c = (int) rightPieces[randPiece].GetComponent<piece_script>().gridPos.y;
				rightPieces.RemoveAt(randPiece);
				gameController.grid[r,c].BroadcastMessage("Activate_Orange", "dead", SendMessageOptions.DontRequireReceiver);
			}
		}
		//swap colors in each array
		for (int i = 0; i < leftPieces.Count; i++) {
			leftPieces[i].BroadcastMessage("Activate_Orange", pickedColor2, SendMessageOptions.DontRequireReceiver);
			if((i == leftPieces.Count - 1) && (rightPieces.Count == 0))
			{
				leftPieces[i].GetComponentInChildren<Piece_Spell_Effect>().lastPiece = true;
			}
		}
		for (int i = 0; i < rightPieces.Count; i++) {
			if(i == rightPieces.Count - 1)
			{
				rightPieces[i].GetComponentInChildren<Piece_Spell_Effect>().lastPiece = true;
			}
		}
		//check the board
		pickedColor1 = null;
		pickedColor2 = null;
		spellColor = null;
	}
	//Yellow attack: launches a single lightning bolt to each side that removes any blocks in the splitter's row
	//this method loads the splitter with the power to activate it on the next fire
	public void Yellowspell()
	{
		Spell_Used (2);
		splitter.setState ("yellowReady", true);
		//recolor splitter to show it's ready to fire
		splitter.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 0, 1);
	}

	public void YellowActivate()
	{
		//set splitter to default color
		splitter.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
		splitter.setState ("isActive", false);
		YellowSpellEffect.BroadcastMessage ("Activate", null, SendMessageOptions.DontRequireReceiver);
		splitter.setState ("yellowReady", false);
		//get the row the splitter is in
		/*int row = (int) gameController.splitter.transform.position.y;
		//loop that deletes everything in the row
		for (int c = 0; c < 16; c++) {
			
			if (gameController.colorGrid[row,c] != null)
			{
				gameController.colorGrid[row,c] = null;
				Destroy(gameController.grid[row,c]);
				gameController.grid[row,c] = null;
			}
		}
		//make it so the splitter can't continually fire yellow spells
		splitter.setState ("yellowReady", false);
		//check the board to update group values
		gameController.checkBoard ();*/
	}
	//Green attack: change the color of three pieces currently in holder or splitter to any color the player chooses
	public void Greenspell()
	{
		Spell_Used (3);
		spellColor = "Green";
		spellLimit = 3;
		splitter.leftSlot.GetComponent<piece_script> ().selectable = true;
		splitter.rightSlot.GetComponent<piece_script> ().selectable = true;
		for (int r = 0; r < 3; r++) {
			for (int c = 0; c < 2; c++){
				holder.holder[r,c].gameObject.GetComponent<piece_script>().selectable = true;
			}
		}
		gameController.gameOverText.text = "Select pieces in the holder/splitter to change";
		splitter.setState ("isActive", false);
	}
	public void GreenHelper()
	{
		spellLimit--;

		if (spellLimit <= 0) {
			splitter.leftSlot.GetComponent<piece_script> ().selectable = false;
			splitter.rightSlot.GetComponent<piece_script> ().selectable = false;

			for (int r = 0; r < 3; r++) {
				for (int c = 0; c < 2; c++){
					holder.holder[r,c].GetComponent<piece_script>().selectable = false;
				}
			}
			pickedColor1 = null;
			pickedColor2 = null;
			spellColor = null;
			selectedPiece = null;
			spellLimit = 0;
			gameController.gameOverText.text = "";
		}
	}

	//Blue attack: recolor any 3 pieces on the board
	public void Bluespell()
	{
		int boardPieceCount = 0;
		foreach (GameObject piece in gameController.grid)
		{
			if(piece != null)
			{
				boardPieceCount++;
				if(boardPieceCount == 3)
					break;
			}
		}
		//TODO: Make a message that tells the player there aren't enough pieces on the board to change
		if (boardPieceCount < 3)
			return;

		Spell_Used (4);
		spellColor = "Blue";
		spellLimit = 3;
		gameController.gameOverText.text = "Select pieces on the board to change";
		GameObject[] allPieces = GameObject.FindGameObjectsWithTag ("Piece");
		foreach (GameObject piece in allPieces) {
			piece_script temp = piece.GetComponent<piece_script>();
			if(!temp.inSideHolder && !temp.inHolder && !temp.inSplitter)
			{
				temp.selectable = true;
			}
		}

		splitter.setState ("isActive", false);
	}
	void BlueHelper(){
		spellLimit--;
		
		if (spellLimit <= 0) {
			splitter.leftSlot.GetComponent<piece_script> ().selectable = false;
			splitter.rightSlot.GetComponent<piece_script> ().selectable = false;
			
			GameObject[] allPieces = GameObject.FindGameObjectsWithTag ("Piece");
			foreach (GameObject piece in allPieces) {
				piece_script temp = piece.GetComponent<piece_script>();
				if(!temp.inSideHolder && !temp.inHolder && !temp.inSplitter)
				{
					temp.selectable = false;
				}
			}
			pickedColor1 = null;
			pickedColor2 = null;
			spellColor = null;
			selectedPiece = null;
			spellLimit = 0;
			gameController.gameOverText.text =  "";
		}
	}


	//Purple attack: deletes all pieces of the selected color on the board
	public void Purplespell()
	{
		Spell_Used (5);
		spellColor = "Purple";
		GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
		picker.GetComponent<Color_Selector> ().givePurpose ("Select a color to eliminate from the board");
		splitter.setState ("isActive", false);
	}
	void PurpleHelper()
	{
		StartCoroutine (Purple_Activator());
	}

	IEnumerator Purple_Activator(){
		int rowLeft = 7, rowRight = 7;
		int colLeft = 0, colRight = 15;
		int tempr, tempc;
		bool leftDone = false, rightDone = false;
		bool empty = true;

		//TODO: Comment out this code because it is overly convulted and covers to many edge cases to be self-documenting
		while (!leftDone || !rightDone) {

			//we'll traverse the Left first. This will be one long if else chain
			if (!leftDone)
			{
				if(gameController.grid[rowLeft,colLeft] != null){
					empty = false;
					gameController.grid[rowLeft,colLeft].BroadcastMessage("Activate_Purple", pickedColor1, SendMessageOptions.DontRequireReceiver);
					tempr = rowLeft;
					tempc = colLeft;
				}
				else
				{
					tempr = -1;
					tempc = -1;
				}
				do
				{
					if(rowLeft % 2 == 1)
					{
						if(colLeft < 6)
						{
							colLeft++;
						}
						else
						{
							rowLeft--;
						}
					}
					else
					{
						if(colLeft > 0)
						{
							colLeft--;
						}
						else
						{
							if(rowLeft == 0)
							{
								//we've hit the bottom left corner, should mark left as completed
								leftDone = true;
							}
							else
							{
								rowLeft--;
							}
						}
					}
				} while(!leftDone && gameController.grid[rowLeft,colLeft] == null);
				if(tempr != -1 && leftDone && rightDone)
				{
					gameController.grid[tempr, tempc].GetComponentInChildren<Piece_Spell_Effect>().lastPiece = true;
				}
			}

			//now Right
			if (!rightDone)
			{
				if(gameController.grid[rowRight,colRight] != null){
					empty = false;
					gameController.grid[rowRight,colRight].BroadcastMessage("Activate_Purple", pickedColor1, SendMessageOptions.DontRequireReceiver);
					tempr = rowRight;
					tempc = colRight;
				}
				else
				{
					tempr = -1;
					tempc = -1;
				}
				do
				{
					if(rowRight % 2 == 1)
					{
						if(colRight > 9)
						{
							colRight--;
						}
						else
						{
							rowRight--;
						}
					}
					else
					{
						if(colRight < 15 )
						{
							colRight++;
						}
						else
						{
							if(rowRight == 0)
							{
								//we've hit the bottom right corner, should mark right as completed
								rightDone = true;
							}
							else
							{
								rowRight--;
							}
						}
					}
				}while (!rightDone && gameController.grid[rowRight,colRight] == null);
				if(tempr != -1 && leftDone && rightDone)
				{
					gameController.grid[tempr, tempc].GetComponentInChildren<Piece_Spell_Effect>().lastPiece = true;
				}
			}

			yield return new WaitForSeconds(0.03f);
		}

		pickedColor1 = null;
		spellColor = null;
		if(empty)
			splitter.setState("isActive", true);
	}
	//Grey spell: the splitter pieces turn to "bombs" which explode and destroy any pieces that come into contact with the explosion when launched
	public void Greyspell()
	{
		Spell_Used (6);
		splitter.rightSlot.GetComponent<piece_script> ().isBomb = true;
		splitter.rightSlot.BroadcastMessage ("Activate_Grey", null, SendMessageOptions.DontRequireReceiver);
		splitter.leftSlot.GetComponent<piece_script> ().isBomb = true;
		splitter.leftSlot.BroadcastMessage ("Activate_Grey", null, SendMessageOptions.DontRequireReceiver);
	}
	//White spell: Sorts the board from rainbow down
	public void Whitespell()
	{
		Spell_Used (7);
		for (int r = 0; r < 8; r++) {
			for (int c = 0; c < 16; c++){
				if( gameController.grid[r,c] != null)
				{
					gameController.grid[r,c].BroadcastMessage("Activate_White", null, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		splitter.setState ("isActive", false);
		StartCoroutine (WhiteHelper ());
	}

	IEnumerator WhiteHelper()
	{
		yield return new WaitForSeconds (1f);
		//get all pieces on left side
		List<GameObject> leftPieces = new List<GameObject>();
		for (int r = 0; r < 8; r++) {
			for (int c = 7; c >=0; c--){
				//Debug.Log ("Checking position R: " + r + " C: " + c);
				if(gameController.grid[r,c] != null)
				{
					leftPieces.Add (gameController.grid[r,c]);
				}
			}
		}
		//sort them by color, alphabetical will do
		IEnumerable<GameObject> sorter = leftPieces;
		sorter = sorter.OrderBy(colorName => colorName.GetComponent<piece_script>().pieceColor);
		leftPieces = sorter.ToList ();
		//make sure not to game over
		int rowHeight;
		if (leftPieces.Count % 8 == 0)
			rowHeight = leftPieces.Count / 8;
		else
			rowHeight = (leftPieces.Count / 8) + 1;
		//put them back evenly in sorted over
		for (int r = 0; r < 8; r++) {
			for (int c = 0; c < rowHeight; c++){
				if(leftPieces.Count == 0)
					break;
				else{
					leftPieces[0].GetComponent<piece_script>().movePiece(new Vector2((float) (c - 8),(float)r));
					leftPieces.RemoveAt(0);
				}
			}
		}
		
		//now for the right
		//get all pieces on left side
		List<GameObject> rightPieces = new List<GameObject>();
		for (int r = 0; r < 8; r++) {
			for (int c = 8; c < 16; c++){
				//Debug.Log ("Checking position R: " + r + " C: " + c);
				if(gameController.grid[r,c] != null)
				{
					rightPieces.Add (gameController.grid[r,c]);
				}
			}
		}
		//sort them by color, alphabetical will do
		sorter = rightPieces;
		sorter = sorter.OrderBy(colorName => colorName.GetComponent<piece_script>().pieceColor);
		rightPieces = sorter.ToList ();
		//make sure not to game over
		if (rightPieces.Count % 8 == 0)
			rowHeight = rightPieces.Count / 8;
		else
			rowHeight = (rightPieces.Count / 8) + 1;
		//put them back evenly in sorted over
		for (int r = 0; r < 8; r++) {
			for (int c = 15; c > (15 - rowHeight); c--){
				if(rightPieces.Count == 0)
					break;
				else{
					rightPieces[0].GetComponent<piece_script>().movePiece(new Vector2((float) (c - 8),(float)r));
					rightPieces.RemoveAt(0);
				}
			}
		}
		
		//check the board
		gameController.recalculateBoard ();
		StartCoroutine (gameController.boardWaiter ());
		splitter.setState ("isActive", true);

	}

	public void colorSelected(string color)
	{
		switch (spellColor) {
		case "Orange":
			if(pickedColor1 == null)
			{
				pickedColor1 = color;
				GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
				picker.GetComponent<Color_Selector> ().givePurpose ("Select a color to switch with on the right side");
				//go through left side, activate
				for (int r = 0; r < 8; r++) {
					for (int c = 7; c >=0; c--){
						if(gameController.grid[r,c] != null)
						{
							if(gameController.colorGrid[r,c] == pickedColor1)
							{
								gameController.grid[r,c].BroadcastMessage("Activate_Orange", "left", SendMessageOptions.DontRequireReceiver);
							}
						}
					}
				}
			}
			else if(pickedColor2 == null)
			{
				pickedColor2 = color;
				for (int r = 0; r < 8; r++) {
					for (int c = 8; c < 16; c++){
						//Debug.Log ("Checking position R: " + r + " C: " + c);
						if(gameController.grid[r,c] != null)
						{
							if(gameController.colorGrid[r,c] == pickedColor2)
							{
								gameController.grid[r,c].BroadcastMessage("Activate_Orange", pickedColor1, SendMessageOptions.DontRequireReceiver);
							}
						}
					}
				}
				OrangeHelper();
			}
			break;
		case "Green":
			selectedPiece.BroadcastMessage("Activate_Green", color, SendMessageOptions.DontRequireReceiver);
			if (spellLimit == 1)
				selectedPiece.GetComponentInChildren<Piece_Spell_Effect>().lastPiece = true;
			GreenHelper();
			break;
		case "Blue":
			selectedPiece.BroadcastMessage("Activate_Blue", color, SendMessageOptions.DontRequireReceiver);
			if (spellLimit == 1)
				selectedPiece.GetComponentInChildren<Piece_Spell_Effect>().lastPiece = true;
			BlueHelper();
			break;
		case "Purple":
			pickedColor1 = color;
			PurpleHelper();
			break;
		}
	}

	public void addBit(string colorOfBit)
	{
		if (spellActive)
			return;
		switch (colorOfBit) {
		case "Red":
			redProgress++;
			if(redProgress >= redGoal || redReady){
				redProgress = redGoal;
				redReady = true;
				redText.text = "100%";
				redText.gameObject.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
			}
			else{
				redText.text = ((int) (((float)redProgress/(float)redGoal) * 100f)) + "%";
			}
			break;
		case "Orange":
			orangeProgress++;
			if(orangeProgress >= orangeGoal || orangeReady){
				orangeProgress = orangeGoal;
				orangeReady = true;
				orangeText.text = "100%";
				orangeText.gameObject.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
			}
			else{
				orangeText.text = ((int) (((float)orangeProgress/(float)orangeGoal) * 100f)) + "%";
			}
			break;
		case "Yellow":
			yellowProgress++;
			if(yellowProgress >= yellowGoal || yellowReady){
				yellowProgress = yellowGoal;
				yellowReady = true;
				yellowText.text = "100%";
				yellowText.gameObject.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
			}
			else{
				yellowText.text = ((int) (((float)yellowProgress/(float)yellowGoal) * 100f)) + "%";
			}
			break;
		case "Green":
			greenProgress++;
			if(greenProgress >= greenGoal || greenReady){
				greenProgress = greenGoal;
				greenReady = true;
				greenText.text = "100%";
				greenText.gameObject.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
			}
			else{
				greenText.text = ((int) (((float)greenProgress/(float)greenGoal)* 100f)) + "%";
			}
			break;
		case "Blue":
			blueProgress++;
			if(blueProgress >= blueGoal || blueReady){
				blueProgress = blueGoal;
				blueReady = true;
				blueText.text = "100%";
				blueText.gameObject.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
			}
			else{
				blueText.text = ((int) (((float)blueProgress/(float)blueGoal) * 100f)) + "%";
			}
			break;
		case "Purple":
			purpleProgress++;
			if(purpleProgress >= purpleGoal || purpleReady){
				purpleProgress = purpleGoal;
				purpleReady = true;
				purpleText.text = "100%";
				purpleText.gameObject.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
			}
			else{
				purpleText.text = ((int) (((float)purpleProgress/(float)purpleGoal) * 100f)) + "%";
			}
			break;
		case "Grey":
			greyProgress++;
			if(greyProgress >= greyGoal || greyReady){
				greyProgress = greyGoal;
				greyReady = true;
				greyText.text = "100%";
				greyText.gameObject.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
			}
			else{
				greyText.text = ((int) (((float)greyProgress/(float)greyGoal) * 100f)) + "%";
			}
			break;
		case "White":
			whiteProgress++;
			if(whiteProgress >= whiteGoal || whiteReady){
				whiteProgress = whiteGoal;
				whiteReady = true;
				whiteText.text = "100%";
				whiteText.gameObject.BroadcastMessage ("FadeOut", null, SendMessageOptions.DontRequireReceiver);
			}
			else{
				whiteText.text = ((int) (((float)whiteProgress/(float)whiteGoal)* 100f)) + "%";
			}
			break;
		}
	}

	//Call whenever a spell is used. This handles the stuff every spell does.
	void Spell_Used(int spellNum)
	{
		spellsUsed [spellNum] = true;
		for (int i = 0; i < 8; i++) {
			if(!spellsUsed[i])
			{
				break;
			}
			else if (i == 7 && gameController.gameType == "Wiz")
			{
				//TODO: Achievement Notifications for ArcanePieceset
				PlayerPrefs.SetInt ("Arcane Pieceset unlocked", 1);
			}
		}

		spellActive = true;
		splitsNeeded = 1;

		switch (spellNum) {
		case 0:
			redReady = false;
			redProgress = 0;
			redGoal = (int) (redGoal * chargeMultiplier);
			redText.text = "0%";
			redText.gameObject.BroadcastMessage ("FadeIn", null, SendMessageOptions.DontRequireReceiver);
			break;
		case 1:
			orangeReady = false;
			orangeProgress = 0;
			orangeGoal = (int) (orangeGoal * chargeMultiplier);
			orangeText.text = "0%";
			orangeText.gameObject.BroadcastMessage ("FadeIn", null, SendMessageOptions.DontRequireReceiver);
			break;
		case 2:
			yellowReady = false;
			yellowProgress = 0;
			yellowGoal = (int) (yellowGoal * chargeMultiplier);
			yellowText.text = "0%";
			yellowText.gameObject.BroadcastMessage ("FadeIn", null, SendMessageOptions.DontRequireReceiver);
			break;
		case 3:
			greenReady = false;
			greenProgress = 0;
			greenGoal = (int) (greenGoal * chargeMultiplier);
			greenText.text = "0%";
			greenText.gameObject.BroadcastMessage ("FadeIn", null, SendMessageOptions.DontRequireReceiver);
			break;
		case 4:
			blueReady = false;
			blueProgress = 0;
			blueGoal = (int) (blueGoal * chargeMultiplier);
			blueText.text = "0%";
			blueText.gameObject.BroadcastMessage ("FadeIn", null, SendMessageOptions.DontRequireReceiver);
			break;
		case 5:
			purpleReady = false;
			purpleProgress = 0;
			purpleGoal = (int) (purpleGoal * chargeMultiplier);
			purpleText.text = "0%";
			purpleText.gameObject.BroadcastMessage ("FadeIn", null, SendMessageOptions.DontRequireReceiver);
			break;
		case 6:
			greyReady = false;
			greyProgress = 0;
			greyGoal = (int) (greyGoal * chargeMultiplier);
			greyText.text = "0%";
			greyText.gameObject.BroadcastMessage ("FadeIn", null, SendMessageOptions.DontRequireReceiver);
			splitsNeeded = 2;
			break;
		case 7:
			whiteReady = false;
			whiteProgress = 0;
			whiteGoal = (int) (whiteGoal * chargeMultiplier);
			whiteText.text = "0%";
			whiteText.gameObject.BroadcastMessage ("FadeIn", null, SendMessageOptions.DontRequireReceiver);
			break;
		}
	}

	//called when the splitter splits. Is used to tell spellhandler to deactive spellActive for scorebits to begin charging
	public void split()
	{
		if (spellActive) {
			splitsNeeded--;
			if(splitsNeeded <= 0 )
				spellActive = false;
		}

	}

	public bool Used_Spells()
	{
		foreach (bool spell in spellsUsed) {
			if(spell)
				return true;
		}
		return false;
	}
}
