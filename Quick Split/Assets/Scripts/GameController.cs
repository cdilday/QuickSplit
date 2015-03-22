using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	Camera mainCamera;

	//grid stores the pieces themselves, while colorGrid only stores the color, allowing for easier access for calculations
	public GameObject[,] grid = new GameObject[8,16];
	public string[,] colorGrid = new string[8, 16];

	//Pieces available to pull from
	public int availableCount;

	//checkgrid keeps track of what tiles have been checked
	public bool[,] checkGrid = new bool[8,16];
	//keeps track of every piece in a cluster during checks.
	public GameObject[] cluster = new GameObject[16];

	//columns that contain pieces to be pushed in at some point
	SideColumn[] sideColumns = new SideColumn[2];

	//Game Object for powers
	GameObject powerHolder;

	//text that pops up during a game over
	public GUIText gameOverText;

	//numbers and text for statistics the player should know
	public int movesMade;
	public GUIText movesText;
	public int score;
	public GUIText scoreText;

	//pieces places is used to ensure both pieces have landed before checking the grid in Update()
	int piecesPlaced;
	//checkFlag will make sure that a boardcheck is run when necessary
	bool checkFlag;

	//Splitter keeps track of the splitter for easy access
	public Splitter_script splitter;

	//true if the player's lost
	public bool gameOver;

	//keeps track of the current score multiplier during checks
	int multiplier;
	bool multiRun;

	//set to true to check for game over in the update loop
	bool checkGameOver;

	//gametype says what mode the board is in to easily set it up accordingly
	public string gameType;

	//how many moves until the sides are added onto the board
	public int sideMovesLimit = 16;
	bool sidesChecked;
	bool quickMoveSides;

	//tells if the application is in the process of quitting, for cleanup
	[HideInInspector]
	public bool isQuitting;
	
	void Awake () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();

		//reposition score to wherever it may be
		scoreText.transform.GetComponent<BoxCollider2D> ().offset = (mainCamera.ViewportToWorldPoint (scoreText.transform.position))
																		- scoreText.transform.position;

		multiRun = false;
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
		//load the side columns if they exist
		sideColumns [0] = null;
		sideColumns [1] = null;
		GameObject[] scols = GameObject.FindGameObjectsWithTag("Side Column");
		powerHolder = GameObject.Find ("Power Handler");
		if(gameType == "Wit"){

			availableCount = 8;

			//no powers in Wit
			Destroy (powerHolder);

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
			quickMoveSides = false;
			StartCoroutine("QuickSideTimer");

			// no powers in Quick, only Holy and Wiz
			Destroy (powerHolder);
		}
		else if(gameType == "Wiz")
		{
			//powers are in Wiz, start out with 5 kinds of blocks
			availableCount = 5;
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
		else if( gameType == "Holy")
		{
			//Holy has everything at once. Essentially hard mode
			availableCount = 8;
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

		sidesChecked = false;

		//initially update the moves and scores
		updateMoves ();
		updateScore ();
	}
	
	// Update is called once per frame
	void Update()
	{
		//code for restarting the game after a game over
		if (gameOver && Input.GetKeyDown ("r")) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}


	void FixedUpdate () {
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

		//if both pieces have been placed, set the checkGrid to false and check the board
		if(piecesPlaced >= 2)
		{
			piecesPlaced = 0;
			checkGameOver = true;
			if(checkFlag)
			{
				multiplier = 1;
				checkBoard ();
				checkFlag = false;
				//check to see if it's time to move the sides in
			}
			else{
				splitter.canShoot = true;
			}

			//here's where we do side-entering management
			if((gameType == "Wiz" || gameType == "Holy") && !sidesChecked){
				if( movesMade % sideMovesLimit == 0){
					addSideColumns();
					sideColumns[0].shakeStage = 0;
					sideColumns[1].shakeStage =	0;
				}
				else if(sideMovesLimit - (movesMade % sideMovesLimit) <= 4)
				{
					switch (sideMovesLimit - (movesMade % sideMovesLimit)){
					case 1:
						sideColumns[0].isShaking = false;
						sideColumns [1].isShaking = false;
						sideColumns[0].ready = true;
						sideColumns[1].ready = true;
						sideColumns[0].shakeStage = 0;
						sideColumns[1].shakeStage = 0;
						break;
					case 2:
						sideColumns[0].shakeStage = 3;
						sideColumns[1].shakeStage = 3;
						break;
					case 3:
						sideColumns[0].shakeStage = 2;
						sideColumns[1].shakeStage = 2;
						break;
					case 4:
						sideColumns[0].isShaking = true;
						sideColumns[1].isShaking = true;
						sideColumns[0].shakeStage = 1;
						sideColumns[1].shakeStage = 1;
						break;
					}
				}
				sidesChecked = true;
			}
			else if((gameType == "Quick") && !sidesChecked){
				//quick mode moves the sides in based off of time, not moves
				if(quickMoveSides)
				{
					addSideColumns();
					sideColumns[0].ready = false;
					sideColumns[1].ready = false;
					quickMoveSides = false;
					StartCoroutine("QuickSideTimer");
				}
				sidesChecked = true;
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
			Debug.LogError ("Error in placing piece with position" +(int)pieceStats.gridPos.x +" " + (int)pieceStats.gridPos.y);
			Destroy(piece);
		}

	}

	/* checkBoard() begins the recursive checkBoard check. The recursive sister method, scanner(), will check off some
	 * of the pieces but not all, so checkboard calls scanner on every piece not yet checked in its main loop. any groups
	 * are stored for easier deletion post-check and the deletion check calculates scores based off of group size and
	 * current multiplier.*/
	public void checkBoard()
	{
		//reset the bool board for the current run
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
		for(int r = 0; r <= 7; r++ )
		{
			for(int c = 0; c <= 15; c++)
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
					if((groupCount >= 2 || multiRun) && groupIncreased)
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
					grid[r,c].GetComponent<piece_script>().multiplier = multiplier;
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
			multiRun = true;
			StartCoroutine(boardWaiter());
		}
		else {
			multiRun = false;
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

		//this is the point where any post-move action should be taken
		if (gameType != "Wit") {
			sidesChecked = false;
		}

		if(movesMade % 77 == 0 && availableCount != 8 && movesMade != 0)
		{
			availableCount++;
		}

	}
	//call this when the score counter needs to be updated
	public void updateScore()
	{
		scoreText.text = "Score: " + score;
	}

	// MoveInward will move every piece towards the center and create free columns near the edges
	public void MoveInward()
	{
		if(gameType != "Wit"){
			//First check to see if this action would create a gameover
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
						grid[r,c].transform.parent = null;
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
				Debug.LogError ("GameController Error: Attempting to add nonexistant side columns");
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

			sideColumns[0].isShaking = false;
			sideColumns[1].isShaking = false;

			//and finally check to get rid of new matches.
			StartCoroutine(boardWaiter());
		}
	}

	//this is solely to let the pieces fall into place aesthetically
	public IEnumerator boardWaiter()
	{
		yield return new WaitForSeconds (0.25f);
		checkBoard ();
	}

	public IEnumerator QuickSideTimer()
	{
		yield return new WaitForSeconds (11f);
		sideColumns[0].isShaking = true;
		sideColumns[1].isShaking = true;
		sideColumns[0].shakeStage = 1;
		sideColumns[1].shakeStage = 1;
		yield return new WaitForSeconds (3f);
		sideColumns[0].shakeStage = 2;
		sideColumns[1].shakeStage = 2;
		yield return new WaitForSeconds (3f);
		sideColumns[0].shakeStage = 3;
		sideColumns[1].shakeStage = 3;
		yield return new WaitForSeconds (3f);
		sideColumns[0].isShaking = false;
		sideColumns [1].isShaking = false;
		sideColumns[0].ready = true;
		sideColumns[1].ready = true;
		sideColumns[0].shakeStage = 0;
		sideColumns[1].shakeStage = 0;
		quickMoveSides = true;
	}

	void OnApplicationQuit()
	{
		isQuitting = true;
	}

}
