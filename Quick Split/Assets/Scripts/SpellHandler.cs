using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
	public GUIText redText;
	public GUIText orangeText;
	public GUIText yellowText;
	public GUIText greenText;
	public GUIText blueText;
	public GUIText purpleText;
	public GUIText greyText;
	public GUIText whiteText;
	#endregion

	public string spellColor;
	public piece_script selectedPiece;
	string pickedColor1;
	string pickedColor2;
	int spellLimit = 0;
	// Use this for initialization
	void Start () {
		pickedColor1 = null;
		pickedColor2 = null;
		GameObject gameControllerobject = GameObject.FindGameObjectWithTag ("GameController");
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

		redReady = false;
		orangeReady = false;
		yellowReady = false;
		greenReady = false;
		blueReady = false;
		purpleReady = false;
		greyReady = false;
		whiteReady = false;

		redText.text = "0%";
		orangeText.text = "0%";
		yellowText.text = "0%";
		greenText.text = "0%";
		blueText.text = "0%";
		purpleText.text = "0%";
		greyText.text = "0%";
		whiteText.text = "0%";

		selectedPiece = null;
	}
	
	// Update is called once per frame
	void Update () {
		//attacks, only activatable one at a time
		if(spellColor == null || spellColor == ""){
			//red
			if (Input.GetKeyDown ("1") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && redReady) {
				redReady = false;
				redProgress = 0;
				redGoal = (int) (redGoal * 1.1);
				redText.text = "0%";
				Redspell ();
			}
			//orange
			if (Input.GetKeyDown ("2") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && orangeReady) {
				orangeReady = false;
				orangeProgress = 0;
				orangeGoal = (int) (orangeGoal * 1.1);
				orangeText.text = "0%";
				Orangespell ();
			}
			//yellow
			if (Input.GetKeyDown ("3") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && yellowReady) {
				yellowReady = false;
				yellowProgress = 0;
				yellowGoal = (int) (yellowGoal * 1.1);
				yellowText.text = "0%";
				Yellowspell ();
			}
			//green
			if (Input.GetKeyDown ("4") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && greenReady) {
				greenReady = false;
				greenProgress = 0;
				greenGoal = (int) (greenGoal * 1.1);
				greenText.text = "0%";
				Greenspell ();
			}
			//blue
			if (Input.GetKeyDown ("5") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && blueReady) {
				blueReady = false;
				blueProgress = 0;
				blueGoal = (int) (blueGoal * 1.1);
				blueText.text = "0%";
				Bluespell ();
			}
			//purple
			if (Input.GetKeyDown ("6") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && purpleReady) {
				purpleReady = false;
				purpleProgress = 0;
				purpleGoal = (int) (purpleGoal * 1.1);
				purpleText.text = "0%";
				Purplespell ();
			}
			//grey
			if (Input.GetKeyDown ("7") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && greyReady) {
				greyReady = false;
				greyProgress = 0;
				greyGoal = (int) (greyGoal * 1.1);
				greyText.text = "0%";
				Greyspell ();
			}
			//white
			if (Input.GetKeyDown ("8") && splitter.getState ("canShoot") && !splitter.getState ("isMoving") && whiteReady) {
				whiteReady = false;
				whiteProgress = 0;
				whiteGoal = (int) (whiteGoal * 1.1);
				whiteText.text = "0%";
				Whitespell ();
			}
		}
	}

	//Red attack: Burns a layer off the top of each side, specifically deleting the block in each row closest to the center
	public void Redspell()
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
	public void Orangespell()
	{
		spellColor = "Orange";
		GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
		picker.GetComponent<Color_Selector> ().givePurpose ("Select a color to switch with on the left side");
	}
	void OrangeHelper ()
	{
		if (pickedColor2 == null) {
			spellColor = "Orange";
			GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
			picker.GetComponent<Color_Selector> ().givePurpose ("Select a color to switch with on the right side");
			return;
		}

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
				Destroy(gameController.grid[r,c]);
				gameController.grid[r,c] = null;
				gameController.colorGrid[r,c] = null;
				deleted = true;
			}
		}
		else {
			for (; difference > 0; difference--)
			{
				int randPiece = (int) Random.Range(0, rightPieces.Count);
				int r = (int) rightPieces[randPiece].GetComponent<piece_script>().gridPos.x;
				int c = (int) rightPieces[randPiece].GetComponent<piece_script>().gridPos.y;
				rightPieces.RemoveAt(randPiece);
				Destroy(gameController.grid[r,c]);
				gameController.grid[r,c] = null;
				gameController.colorGrid[r,c] = null;
				deleted = true;
			}
		}
		if (deleted){
			gameController.collapse();
		}
		//swap colors in each array
		for (int i = 0; i < leftPieces.Count; i++) {
			leftPieces[i].GetComponent<piece_script>().ConvertColor(pickedColor2);
		}
		for (int i = 0; i < rightPieces.Count; i++) {
			rightPieces[i].GetComponent<piece_script>().ConvertColor(pickedColor1);
		}
		//check the board
		StartCoroutine (gameController.boardWaiter ());
		pickedColor1 = null;
		pickedColor2 = null;
		spellColor = null;
		splitter.setState ("isActive", true);
	}
	//Yellow attack: launches a single lightning bolt to each side that removes any blocks in the splitter's row
	//this method loads the splitter with the power to activate it on the next fire
	public void Yellowspell()
	{
		splitter.setState ("yellowReady", true);
		//recolor splitter to show it's ready to fire
		splitter.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 0, 1);
	}

	public void YellowActivate()
	{
		//set splitter to default color
		splitter.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
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
		//make it so the splitter can't continually fire yellow spells
		splitter.setState ("yellowReady", false);
		//check the board to update group values
		gameController.checkBoard ();
	}
	//Green attack: change the color of three pieces currently in holder or splitter to any color the player chooses
	public void Greenspell()
	{
		spellColor = "Green";
		spellLimit = 3;
		splitter.leftSlot.GetComponent<piece_script> ().selectable = true;
		splitter.rightSlot.GetComponent<piece_script> ().selectable = true;
		for (int r = 0; r < 3; r++) {
			for (int c = 0; c < 2; c++){
				holder.holder[r,c].gameObject.GetComponent<piece_script>().selectable = true;
			}
		}

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
			splitter.setState ("isActive", true);
		}
	}

	//Blue attack: rearrange every splitter/holder piece to any arrangement the player chooses
	public void Bluespell()
	{
		spellColor = "Blue";
		spellLimit = 3;

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
			splitter.setState ("isActive", true);
			gameController.checkBoard();
		}
	}


	//Purple attack: deletes all pieces of the selected color on the board
	public void Purplespell()
	{
		spellColor = "Purple";
		GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
		picker.GetComponent<Color_Selector> ().givePurpose ("Select a color to eliminate from the board");
		splitter.setState ("isActive", false);
	}
	void PurpleHelper()
	{
		GameObject[] allPieces = GameObject.FindGameObjectsWithTag ("Piece");
		foreach (GameObject piece in allPieces) {
			piece_script temp = piece.GetComponent<piece_script>();
			if(!temp.inSideHolder && !temp.inHolder && !temp.inSplitter && temp.pieceColor == pickedColor1)
			{
				gameController.grid[(int)temp.gridPos.x, (int)temp.gridPos.y] = null;
				gameController.colorGrid[(int)temp.gridPos.x, (int)temp.gridPos.y] = null;
				Destroy (piece);
			}
		}
		gameController.collapse ();
		StartCoroutine (gameController.boardWaiter ());
		pickedColor1 = null;
		spellColor = null;
		splitter.setState ("isActive", true);
	}
	//Grey spell: the splitter pieces turn to "bombs" which explode and destroy any pieces that come into contact with the explosion when launched
	public void Greyspell()
	{
		splitter.rightSlot.GetComponent<piece_script> ().isBomb = true;
		Vector3 hsv = RGBandHSVconverter.RGBtoHSV (splitter.rightSlot.GetComponent<SpriteRenderer> ().color);
		hsv = new Vector3(hsv.x, hsv.y, hsv.z - 0.5f);
  		splitter.rightSlot.GetComponent<SpriteRenderer> ().color = RGBandHSVconverter.HSVtoRGB(hsv);
		splitter.leftSlot.GetComponent<piece_script> ().isBomb = true;
		hsv = RGBandHSVconverter.RGBtoHSV (splitter.leftSlot.GetComponent<SpriteRenderer> ().color);
		hsv = new Vector3(hsv.x, hsv.y, hsv.z - 0.5f);
		splitter.leftSlot.GetComponent<SpriteRenderer> ().color = RGBandHSVconverter.HSVtoRGB(hsv);
	}
	//White spell: Sorts the board from rainbow down
	public void Whitespell() //consider renaming to ability, in hindsight I probably should've looked at that first
	{
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
			}
			else if(pickedColor2 == null)
			{
				pickedColor2 = color;
				OrangeHelper();
			}
			break;
		case "Green":
			selectedPiece.ConvertColor(color);
			GreenHelper();
			break;
		case "Blue":
			selectedPiece.ConvertColor(color);
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
		switch (colorOfBit) {
		case "Red":
			redProgress++;
			if(redProgress >= redGoal){
				redProgress = redGoal;
				redReady = true;
				redText.text = "Ready!";
			}
			else{
				redText.text = ((int) (((float)redProgress/(float)redGoal) * 100f)) + "%";
			}
			break;
		case "Orange":
			orangeProgress++;
			if(orangeProgress >= orangeGoal){
				orangeProgress = orangeGoal;
				orangeReady = true;
				orangeText.text = "Ready!";
			}
			else{
				orangeText.text = ((int) (((float)orangeProgress/(float)orangeGoal) * 100f)) + "%";
			}
			break;
		case "Yellow":
			yellowProgress++;
			if(yellowProgress >= yellowGoal){
				yellowProgress = yellowGoal;
				yellowReady = true;
				yellowText.text = "Ready!";
			}
			else{
				yellowText.text = ((int) (((float)yellowProgress/(float)yellowGoal) * 100f)) + "%";
			}
			break;
		case "Green":
			greenProgress++;
			if(greenProgress >= greenGoal){
				greenProgress = greenGoal;
				greenReady = true;
				greenText.text = "Ready!";
			}
			else{
				greenText.text = ((int) (((float)greenProgress/(float)greenGoal)* 100f)) + "%";
			}
			break;
		case "Blue":
			blueProgress++;
			if(blueProgress >= blueGoal){
				blueProgress = blueGoal;
				blueReady = true;
				blueText.text = "Ready!";
			}
			else{
				blueText.text = ((int) (((float)blueProgress/(float)blueGoal) * 100f)) + "%";
			}
			break;
		case "Purple":
			purpleProgress++;
			if(purpleProgress >= purpleGoal){
				purpleProgress = purpleGoal;
				purpleReady = true;
				purpleText.text = "Ready!";
			}
			else{
				purpleText.text = ((int) (((float)purpleProgress/(float)purpleGoal) * 100f)) + "%";
			}
			break;
		case "Grey":
			greyProgress++;
			if(greyProgress >= greyGoal){
				greyProgress = greyGoal;
				greyReady = true;
				greyText.text = "Ready!";
			}
			else{
				greyText.text = ((int) (((float)greyProgress/(float)greyGoal) * 100f)) + "%";
			}
			break;
		case "White":
			whiteProgress++;
			if(whiteProgress >= whiteGoal){
				whiteProgress = whiteGoal;
				whiteReady = true;
				whiteText.text = "Ready!";
			}
			else{
				whiteText.text = ((int) (((float)whiteProgress/(float)whiteGoal)* 100f)) + "%";
			}
			break;
		}
	}
}
