using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	//grid stores the pieces themselves, while colorGrid only stores the color, allowing for easier access for calculations
	public GameObject[,] grid = new GameObject[8,16];
	public string[,] colorGrid = new string[8, 16];

	//Pieces available
	public int availableCount;

	//checkgrid keeps track of what tiles have been checked
	public bool[,] checkGrid = new bool[8,16];
	//keeps track of every piece in a cluster during checks.
	public GameObject[] cluster = new GameObject[16];

	public SideColumn[] sideColumns = new SideColumn[2];

	//text that pops up during a game over
	public GUIText gameOverText;

	//numbers and text for statistics the player should know
	public int movesMade;
	public GUIText movesText;
	public int score;
	public GUIText scoreText;
	
	//booleans and GuIText for the powers
	#region
	public bool redReady;
	public bool orangeReady;
	public bool yellowReady;
	public bool greenReady;
	public bool blueReady;
	public bool purpleReady;
	public bool greyReady;
	public bool whiteReady;
	public GUIText redText;
	public GUIText orangeText;
	public GUIText yellowText;
	public GUIText greenText;
	public GUIText blueText;
	public GUIText purpleText;
	public GUIText greyText;
	public GUIText whiteText;
	#endregion

	//pieces places is used to ensure both pieces have landed before checking the grid in Update()
	public int piecesPlaced;
	//checkFlag will make sure that a boardcheck is run when necessary
	public bool checkFlag;

	//Splitter keeps track of the splitter for easy access
	public Splitter_script splitter;

	//true if the player's lost
	public bool gameOver;

	//keeps track of the current score multiplier during checks
	public int multiplier;

	//set to true to check for game over in the update loop
	bool checkGameOver;

	public string gameType;

	// Use this for initialization
	void Awake () {
		//instantiate the grids with their appropriate starting values
		for(int r = 0; r <=7; r++ )
		{
			for(int c = 0; c<= 15; c++)
			{
				grid[r,c] = null;
				colorGrid[r,c] = null;
				checkGrid[r,c] = false;
			}
			cluster[r] = null;
		}

		multiplier = 1;
		checkGameOver = false;
		piecesPlaced = 0;
		checkFlag = false;
		movesMade = 0;
		score = 0;
		//load the splitter with the spawned splitter object
		GameObject splitterObject = GameObject.Find("Splitter");
		if (splitterObject != null) {
			splitter = splitterObject.GetComponent <Splitter_script>();
		}
		gameOver = false;
		sideColumns [0] = null;
		sideColumns [1] = null;
		//load the side columns if they exist
		GameObject[] scols = GameObject.FindGameObjectsWithTag("Side Column");
		if(gameType == "Wit"){

			availableCount = 8;

			//Wit does not use the sidecolumns, get rid of them
			Destroy(scols[1]);
			Destroy(scols[0]);
		}
		else if(gameType == "Quick")
		{	
			availableCount = 4;
			if (scols [0] != null && scols [1] != null) {
				//make sure they're loaded properly, left is 0, right is 1
				if(scols[0].GetComponent <SideColumn>().sideInt == 0)
				{
					sideColumns[0] = scols[0].GetComponent<SideColumn>();
					sideColumns[1] = scols[1].GetComponent<SideColumn>();
				}
				else{
					sideColumns[0] = scols[1].GetComponent<SideColumn>();
					sideColumns[1] = scols[0].GetComponent<SideColumn>();
				}
			}
		}

		if (gameType == "Wit") {
			redText.text = "";
			orangeText.text = "";
			yellowText.text = "";
			greenText.text = "";
			blueText.text = "";
			purpleText.text = "";
			greyText.text = "";
			whiteText.text = "";
		}

		//initially update the moves and scores
		updateMoves ();
		updateScore ();
	}
	
	// Update is called once per frame
	void Update () {
		//check to see if a piece is in the splitter area after each board check
		if(checkGameOver){
			for (int c = 7; c <= 8; c++) {
				for (int r = 0; r <= 7; r++){
					if(colorGrid[r,c] != null && grid[r,c] != null){
						gameOverText.text = "Game Over\nPress R to Restart";
						gameOver = true;
						splitter.canShoot = false;
					}
				}
			}
			checkGameOver = false;
		}

		//code for restarting the game after a game over
		if (gameOver && Input.GetKeyDown ("r")) {
			Application.LoadLevel(Application.loadedLevel);
		}

		//if both pieces have been placed, set the checkGrid to false and check the board
		if(piecesPlaced >= 2)
		{
			piecesPlaced = 0;
			checkGameOver = true;
			if(checkFlag)
			{
				checkBoard ();
				checkFlag = false;
			}
			else{
				splitter.canShoot = true;
			}
		}
	}

	//puts the pieces in the grid after they've settled into their place
	public void placePiece (GameObject piece)
	{
		piece_script pieceStats = piece.GetComponent<piece_script> ();
		int thisX = (int)pieceStats.gridPos.x;
		int thisY = (int)pieceStats.gridPos.y;
		//check to make sure the piece is in the grid
		if(pieceStats.gridPos.x >= 0 && pieceStats.gridPos.x < 8 && pieceStats.gridPos.y >= 0 && pieceStats.gridPos.x < 16)
		{
			//uncomment this to show what the grid position of the newly placed piece is
			//Debug.Log ("Grid Position is" +(int)pieceStats.gridPos.x +" " + (int)pieceStats.gridPos.y);
			colorGrid [(int)pieceStats.gridPos.x, (int)pieceStats.gridPos.y] = pieceStats.pieceColor;
			grid [(int)pieceStats.gridPos.x, (int)pieceStats.gridPos.y] = piece;
			//uncomment this to show what the color of the newly placed piece is
			//if(colorGrid[(int)pieceStats.gridPos.x, (int)pieceStats.gridPos.y] != null)
			//	Debug.Log ("inserted " +grid [(int)pieceStats.gridPos.x, (int)pieceStats.gridPos.y].GetComponent<piece_script>().pieceColor + " piece into colorgrid position " +(int)pieceStats.gridPos.x +" " + (int)pieceStats.gridPos.y);
			//This if statement will check if a check board is necessary, for optimization.
			//Debug.Log ("Beginning check");
			if( ((thisX+1 < 8) && (grid [thisX+1, thisY] != null && colorGrid[thisX+1, thisY] == pieceStats.pieceColor)) || //right
			   ((thisY+1 < 16) && (grid [thisX, thisY+1] != null && colorGrid[thisX, thisY+1] == pieceStats.pieceColor)) || //up
			   ((thisX-1 >= 0) && (grid [thisX-1, thisY] != null && colorGrid[thisX-1, thisY] == pieceStats.pieceColor)) || //left
			   ((thisY-1 >= 0) && ( grid [thisX, thisY-1] != null && colorGrid[thisX, thisY-1] == pieceStats.pieceColor)) ){ //below
				checkFlag = true;
				//Debug.Log ("Matching adjacent piece detected");
			}
			piecesPlaced++;
		}
		// if it isn't in the grid, throw an error up and delete the offending piece.
		else
		{
			Debug.Log ("Error in placing " + " piece with position" +(int)pieceStats.gridPos.x +" " + (int)pieceStats.gridPos.y);
			Destroy(piece);
		}

	}

	/* checkBoard() begins the recursive checkBoard check. The recursive sister method, scanner(), will check off some
	 * of the pieces but not all, so checkboard calls scanner on every piece not yet checked in its main loop. any groups
	 * are stored for easier deletion post-check and the deletion check calculates scores based off of group size and
	 * current multiplier.*/
	public void checkBoard()
	{
		for(int r = 0; r <=7; r++ )
		{
			for(int c = 0; c<= 15; c++)
			{
				checkGrid[r,c] = false;
			}
		}
		int groupCount = 0;
		bool groupIncreased = false;
		//nested for loops for checking the grid
		for(int r = 0; r <=7; r++ )
		{
			for(int c = 0; c<= 15; c++)
			{
				//check if current piece has already been checked
				if(checkGrid[r,c] == false && grid [r,c] != null)
				{
					//uncomment this to see what pieces the function is checking as it checks them
					//Debug.Log ("Checking piece in position " + r + " " + c);
					checkGrid[r,c] = true;
					cluster[0] = grid[r,c];
					//begin the recursion
					int temp = scanner (r,c, grid[r,c].GetComponent<piece_script>().pieceColor, 1);
					//loop that assigns each piece in the group the group value to be scanned afterwards
					for(int i = 0; i < temp; i++)
					{
						cluster[i].GetComponent<piece_script>().groupValue = temp;
					}
					//if a group has hit the deletion count, and it's not the first, add to the multiplier
					if(temp >= 4)
					{
						groupCount++;
						groupIncreased = true;
					}
					if(groupCount >= 2 && groupIncreased)
					{
						multiplier++;
					}
				}
				else
					checkGrid[r,c] = true;
				//reset group increased after doing a scanner check in preperation for later checks
				groupIncreased = false;
			}
		}

		//now check if groups need to be removed
		bool deleted = false;
		for(int r = 0; r <=7; r++ )
		{
			for(int c = 0; c <= 15; c++)
			{
				//if the value is high enough it's in a group big enough to delete, so delete it
				if(grid[r,c] != null && grid[r,c].GetComponent<piece_script>().groupValue >= 4)
				{
					score += grid[r,c].GetComponent<piece_script>().groupValue * multiplier;
					updateScore();
					//delete piece, mark that something was deleted
					Destroy(grid[r,c]);
					grid[r,c] = null;
					deleted = true;
				}
			}
		}


		//if the board changed, collapse & check it again
		if (deleted) {
			collapse ();
			StartCoroutine(boardWaiter());
		}
		else {
			multiplier = 1;
			checkGameOver = true;
			if(!gameOver){
				splitter.canShoot = true;
			}
		}
	}

	//collapses the pieces where they belong
	public void collapse()
	{
		int tempCol;
		bool adjusted = false;
		//check the left grid
		for(int r = 0; r <8; r++)
		{
			tempCol = 0;
			adjusted = false;
			for (int c = 0; c < 8; c++)
			{
				if(grid[r,c] == null && !adjusted)
				{
					tempCol = c;
					adjusted = true;
				}
				else if(adjusted && grid[r,c] != null)
				{
					//change the piece's stats to reflect the new position
					grid[r,c].GetComponent<piece_script>().movePiece(new Vector2(tempCol - 8, r));
					//re-assign all grids to fit the new position, add 1 to tempCol
					grid[r,tempCol] = grid[r,c];
					grid[r,c] = null;
					colorGrid[r,tempCol] = colorGrid[r,c];
					colorGrid[r,c] = null;
					tempCol++;
					//set c to TempCol to restart the check for later things in the column
					c = tempCol;
				}
			}
		}
		
		//check the right grid
		for(int r = 0; r <8; r++)
		{
			tempCol = 0;
			adjusted = false;
			for (int c = 15; c > 8; c--)
			{
				if(grid[r,c] == null && !adjusted)
				{
					tempCol = c;
					adjusted = true;
				}
				else if(adjusted && grid[r,c] != null)
				{
					//change the piece's stats to reflect the new position
					grid[r,c].GetComponent<piece_script>().movePiece(new Vector2(tempCol - 8, r));
					//re-assign all grids to fit the new position, subtract to tempCol
					grid[r,tempCol] = grid[r,c];
					grid[r,c] = null;
					colorGrid[r,tempCol] = colorGrid[r,c];
					colorGrid[r,c] = null;
					tempCol--;
					//set c to TempCol to restart the check for later things in the column
					c = tempCol;
				}
			}
		}
	}

	//scanner goes through and checks every adjacent piece recursively, then returns the amount of pieces in a cluster.
	public int scanner(int x, int y, string color, int adj)
	{
		//mark current as checked
		checkGrid[x,y] = true;
		//check right of piece
		if ((x + 1 < 8) && (checkGrid [x + 1, y] == false && grid [x + 1, y] != null && grid [x + 1, y].GetComponent<piece_script> ().pieceColor == color)) {
			//check to make sure the piece actually has a grid position and isn't in the splitter
			if(grid [x + 1, y].GetComponent<piece_script>().locked){
				adj++;
				//add to group cluster
				cluster[adj-1] = grid[x+1,y];
				adj = scanner (x+1, y, color, adj);
			}
		}
		//check up
		if (y + 1 < 16 && checkGrid [x, y+1] == false && grid [x, y+1] != null && grid [x, y+1].GetComponent<piece_script> ().pieceColor == color) {
			//check to make sure the piece actually has a grid position and isn't in the splitter
			if(grid [x, y + 1].GetComponent<piece_script>().locked){
				adj++;
				//add to group cluster
				cluster[adj-1] = grid[x,y+1];
				adj = scanner (x, y+1, color, adj);
			}
		}
		//check left
		if (x - 1 >= 0 && checkGrid [x - 1, y] == false && grid [x - 1, y] != null && grid [x - 1, y].GetComponent<piece_script> ().pieceColor == color) {
			//check to make sure the piece actually has a grid position and isn't in the splitter
			if(grid [x - 1, y].GetComponent<piece_script>().locked){
				adj++;
				//add to group cluster
				cluster[adj-1] = grid[x-1,y];
				adj = scanner (x-1, y, color, adj);
			}
		}
		//check below
		if (y - 1 >= 0 && checkGrid [x, y-1] == false && grid [x, y-1] != null && grid [x, y-1].GetComponent<piece_script> ().pieceColor == color) {
			//check to make sure the piece actually has a grid position and isn't in the splitter
			if(grid [x, y-1 ].GetComponent<piece_script>().locked){
				adj++;
				//add to group cluster
				cluster[adj-1] = grid[x,y-1];
				adj = scanner (x, y-1, color, adj);
			}
		}

		//base case, return the cluster value found so far
		return adj;
	}

	//call this when the move counter needs to change
	public void updateMoves()
	{
		movesText.text = "Splits made: " + movesMade;
	}
	//call this when the score counter needs to be updated
	public void updateScore()
	{
		scoreText.text = "Score: " + score;
	}

	//Red attack: Burns a layer off the top of each side, specifically deleting the block in each row closest to the center
	public void RedPower()
	{
		/*Deletion loops work by going to the splitter's columns outwards and deleting the first piece it comes across before moving on
		 * likely the player would only use this ability when on the brink of losing, so this is better than going from outwards in.
		 * Once the loop deletes the first thing it comes accross, it exits the inner loop to move onto the next row.*/
		//left grid loop
		for (int r = 0; r < 8; r++) {
			for (int c = 7; c >=0; c--){
				//Debug.Log ("Checking position R: " + r + " C: " + c);
				if(grid[r,c] != null)
				{
					Destroy (grid[r,c]);
					colorGrid[r,c] = null;
					grid[r,c] = null;
					c = -1;
				}
			}
		}
		//right grid loop
		for (int r = 0; r < 8; r++) {
			for (int c = 8; c < 16; c++){
				//Debug.Log ("Checking position R: " + r + " C: " + c);
				if(grid[r,c] != null)
				{
					Destroy (grid[r,c]);
					colorGrid[r,c] = null;
					grid[r,c] = null;
					c = 16;
				}
			}
		}
	}
	//Orange atttack: switches all the pieces of a single color on one side with all the pieces of a different single color on the other side
	//deletes leftover pieces if the switch is uneven.
	public void OrangePower()
	{
		
	}
	//Yellow attack: launches a single lightning bolt to each side that removes any blocks in the splitter's row
	public void YellowPower()
	{
		//get the row the splitter is in
		int row = (int) splitter.transform.position.y;
		//loop that deletes everything in the row
		for (int c = 0; c < 16; c++) {

			if (colorGrid[row,c] != null)
			{
				colorGrid[row,c] = null;
				Destroy(grid[row,c]);
				grid[row,c] = null;
			}
		}
		//you shouldn't need to check the board after this. 
	}
	//Green attack: change the color of three pieces currently in holder or splitter to any color the player chooses
	public void GreenPower()
	{
		
	}
	//Blue attack: rearrange every splitter/holder piece to any arrangement the player chooses
	public void BluePower()
	{
		
	}
	//Purple attack: turns pieces in holder/splitter into "special" pieces
	//special pieces are specially marked and do things such as increase score and multiplier
	public void PurplePower()
	{
		
	}
	//Grey Power: the splitter pieces turn to "bombs" which explode and destroy any pieces that come into contact with the explosion when launched
	public void GreyPower()
	{
		
	}
	//White Power: Sorts the board from rainbow down
	public void WhitePower() //consider renaming to ability, in hindsight I probably should've looked at that first
	{
		
	}

	// MoveInward will move every piece towards the center and create free columns near the edges
	public void MoveInward()
	{
		if(gameType != "Wit"){
			//First check to see if this action would createa gameover
			for (int r = 0; r <= 7; r++){
				if((colorGrid[r,6] != null && grid[r,6] != null) ||
				   (colorGrid[r,9] != null && grid[r,9] != null)){
					gameOverText.text = "Game Over\nPress R to Restart";
					gameOver = true;
					splitter.canShoot = false;
				}
			}

			//next we iterate through the left side moving everything forward a column
			for (int c = 6; c >= 0; c--) {
				for(int r = 0; r <= 7; r++){
					if(colorGrid[r,c] != null && grid[r,c] != null){
						//piece exits, more rightward making sure to
						//change the piece's stats to reflect the new position
						grid[r,c].GetComponent<piece_script>().movePiece(new Vector2((c+1) - 8, r));
						//re-assign all grids to fit the new position, add 1 to tempCol
						grid[r,c+1] = grid[r,c];
						grid[r,c] = null;
						colorGrid[r,(c+1)] = colorGrid[r,c];
						colorGrid[r,c] = null;
					}
				}
			}

			//and now iterate through the right
			//next we iterate through the left side moving everything forward a column
			for (int c = 9; c <= 15; c++) {
				for(int r = 0; r <= 7; r++){
					if(colorGrid[r,c] != null && grid[r,c] != null){
						//piece exits, more rightward making sure to
						//change the piece's stats to reflect the new position
						grid[r,c].GetComponent<piece_script>().movePiece(new Vector2((c-1) - 8, r));
						//re-assign all grids to fit the new position, add 1 to tempCol
						grid[r,c-1] = grid[r,c];
						grid[r,c] = null;
						colorGrid[r,(c-1)] = colorGrid[r,c];
						colorGrid[r,c] = null;
					}
				}
			}
		}
	}

	//adds the stored side column pieces to the board.
	public void addSideColumns()
	{
		if(gameType != "Wit"){
			if (sideColumns [0] == null || sideColumns [1] == null) {
				Debug.Log("GameController Error: Attempting to add nonexistant side columns");
				return;
			}

			//make room for the new columns
			MoveInward ();

			//loading left
			for (int r = 0; r < 8; r++) {
				colorGrid[r, 0] = sideColumns[0].colorColumn[r];
				grid[r,0] = sideColumns[0].column[r];
				grid[r,0].GetComponent<piece_script>().movePiece(new Vector2(-8, r));
			}
			sideColumns[0].empty();
			sideColumns [0].reload ();

			//loading right
			for (int r = 0; r <8; r++) {
				colorGrid[r, 15] = sideColumns[1].colorColumn[r];
				grid[r,15] = sideColumns[1].column[r];
				grid[r,15].GetComponent<piece_script>().movePiece(new Vector2(15-8, r));
			}
			sideColumns[1].empty();
			sideColumns [1].reload ();

			//and finally check to get rid of new matches.
			StartCoroutine(boardWaiter());
		}
	}
	public IEnumerator boardWaiter()
	{
		yield return new WaitForSeconds (0.25f);
		checkBoard ();
	}
}
