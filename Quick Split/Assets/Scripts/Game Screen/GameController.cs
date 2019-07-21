using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Camera mainCamera;

    //grid stores the pieces themselves, while colorGrid only stores the color, allowing for easier access for calculations
    public GameObject[,] grid = new GameObject[8, 16];
    public PieceColor[,] colorGrid = new PieceColor[8, 16];

    /// <summary>
    /// Number of unlocked piece colors (Note : the pieces are unlocked in a different order thant their position in the PieceColor enum)
    /// </summary>
    public int availableCount;

    /// <summary>
    /// This is used to keep track of which tiles have been checked during the checking process, since groups cause it to check out of order
    /// </summary>
    public bool[,] checkGrid = new bool[8, 16];

    /// <summary>
    /// Every cluster made while checking the board. This is be used to clear them and count combos
    /// </summary>
    public GameObject[] cluster = new GameObject[16];

    //columns that contain pieces to be pushed in at some point
    private SideColumn[] sideColumns = new SideColumn[2];
    private TimeSpan timePerCrunch;
    private TimeSpan lightIncrementTiming;
    private TimeSpan timeBeforeShaking;

    //Script for Spells
    private SpellHandler spellHandler;

    //text that pops up during a game over. also used for the pause screen and other alerts
    public Text gameOverText;
    //text that gives a tip at gameover
    public Text tipText;

    //numbers and text for statistics the player should know
    public int movesMade;
    private int splitsToUnlock;
    public Text movesText;
    public int score;
    public Text scoreText;
    private bool newHighScore;
    public Text HighScoreText;
    public GameObject Score_Text_Canvas;

    /// <summary>
    /// After each split piece lands this is incremented, so every 2 pieces the board needs to see if it should be checked in 
    /// FixedUpdate using this value
    /// </summary>
    private int piecesPlaced;

    //Splitter keeps track of the splitter for easy access
    public Splitter splitter;

    //true if the player's lost
    public bool gameOver;
    public GameObject GameOverLayer;

    //keeps track of the current score multiplier during checks
    public float multiplier;
    private bool multiRun;
    private bool piecesDeletedThisSplit;
    private bool clearedLastTurn;

    //gametype says what mode the board is in to easily set it up accordingly
    public GameMode gameMode;

    //how many moves until the sides are added onto the board
    public int sideMovesLimit;
    private bool quickMoveSides;
    private bool sidesChecked = false;

    //tells if the application is in the process of quitting, for cleanup
    [HideInInspector]
    public bool isQuitting;

    public bool isPaused;
    public GameObject pauseLayer;
    public ShutterHandler shutter;
    private MusicController mc;
    private ScoreAndAchievementHandler achievementHandler;

    public AudioSource PauseSFX;
    public AudioSource UnpauseSFX;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Split Title Scene")
        {
            SceneManager.UnloadSceneAsync("Split Title Scene");
        }
        achievementHandler = GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<ScoreAndAchievementHandler>();

        gameMode = Game_Mode_Helper.ActiveRuleSet.Mode;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //let's grab the music controller
        GameObject MCobject = GameObject.FindGameObjectWithTag("Music Controller");
        mc = MCobject.GetComponent<MusicController>();

        //reposition score to wherever it may be
        scoreText.transform.GetComponent<BoxCollider2D>().offset = (mainCamera.ViewportToWorldPoint(scoreText.transform.position))
                                                                        - scoreText.transform.position;

        multiRun = false;
        //instantiate the grids with their appropriate starting values
        for (int r = 0; r <= 7; r++)
        {
            for (int c = 0; c <= 15; c++)
            {
                grid[r, c] = null;
                colorGrid[r, c] = PieceColor.Empty;
                checkGrid[r, c] = false;
            }
            cluster[r] = null;
        }

        multiplier = 1;
        piecesPlaced = 0;
        movesMade = 0;
        score = 0;
        //load the splitter with the spawned splitter object
        GameObject splitterObject = GameObject.Find("Splitter");
        if (splitterObject != null)
        {
            splitter = splitterObject.GetComponent<Splitter>();
        }

        gameOver = false;

        loadGameMode(Game_Mode_Helper.ActiveRuleSet);

        //initially update the moves and scores
        updateMoves();
        updateScore();

        //get the pause stuff in order
        isPaused = false;
        GameOverLayer.SetActive(false);
        shutter.BeginHorizontalOpen();

        //high score stuff
        newHighScore = false;
        HighScoreText.text = "";
        tipText.text = "";
        Score_Text_Canvas = GameObject.Find("Score Text Canvas");
    }

    /// <summary>
    /// Loads the game scene with the given ruleset
    /// </summary>
    /// <param name="activeRuleSet"> RuleSet - the ruleset to load the game with</param>
    private void loadGameMode(RuleSet activeRuleSet)
    {
        //load the side columns if they exist
        sideColumns[0] = null;
        sideColumns[1] = null;
        GameObject[] scols = GameObject.FindGameObjectsWithTag("Side Column");
        GameObject spellHandlerObject = GameObject.Find("Spell Handler");
        spellHandler = spellHandlerObject.GetComponent<SpellHandler>();

        availableCount = activeRuleSet.UnlockedPieces;
        splitsToUnlock = activeRuleSet.SplitsToUnlock;

        if (activeRuleSet.UsesSides)
        {
            if (scols[0] != null && scols[1] != null)
            {
                //make sure they're loaded properly, left is 0, right is 1
                if (scols[0].GetComponent<SideColumn>().sideInt == 0)
                {
                    sideColumns[0] = scols[0].GetComponent<SideColumn>();
                    sideColumns[1] = scols[1].GetComponent<SideColumn>();
                }
                else
                {
                    sideColumns[0] = scols[1].GetComponent<SideColumn>();
                    sideColumns[1] = scols[0].GetComponent<SideColumn>();
                }
            }

            // Possibility that we got them out of order, swap them if so
            if (sideColumns != null && sideColumns[0].isRight)
            {
                SideColumn temp = sideColumns[0];
                sideColumns[0] = sideColumns[1];
                sideColumns[1] = temp;
            }

            if (activeRuleSet.TimedCrunch)
            {
                timePerCrunch = activeRuleSet.TimePerCrunch;

                if (timePerCrunch == null || timePerCrunch.TotalSeconds == 0)
                {
                    timePerCrunch = Game_Mode_Helper.AllRuleSets[(int)GameMode.Quick].TimePerCrunch;
                }

                quickMoveSides = false;
                StartCoroutine("StartingCountDown");
                splitter.setState(Splitter.SplitterStates.canShoot, false);
            }
            else
            {
                sideMovesLimit = activeRuleSet.SplitsPerCrunch;
                //backup default
                if (sideMovesLimit == 0)
                {
                    sideMovesLimit = Game_Mode_Helper.AllRuleSets[(int)GameMode.Wiz].SplitsPerCrunch;
                }
            }
        }
        else
        {
            Destroy(scols[1]);
            Destroy(scols[0]);
        }

        // There iss a countdown for timed crunching before game modes, so music is handled after that countdown completes in a seperate thread
        if (activeRuleSet.TimedCrunch == false)
        {
            mc.PlayMusic(gameMode);
        }

        if (activeRuleSet.HasSpells)
        {
            //TODO: Make unlocking pieces and piece order generic; might add option to customize unlock order but this is messy as a result of bad old code.
            int unlockedPieces = activeRuleSet.UnlockedPieces;

            // obviously need more than one
            spellHandler.SpellReady[(int)PieceColor.Red] = true;
            spellHandler.SpellReady[(int)PieceColor.Orange] = unlockedPieces >= 6;
            spellHandler.SpellReady[(int)PieceColor.Yellow] = unlockedPieces >= 4;
            spellHandler.SpellReady[(int)PieceColor.Green] = unlockedPieces >= 3;
            spellHandler.SpellReady[(int)PieceColor.Blue] = unlockedPieces >= 2;
            spellHandler.SpellReady[(int)PieceColor.Purple] = unlockedPieces >= 5;
            spellHandler.SpellReady[(int)PieceColor.Cyan] = unlockedPieces >= 7;
            spellHandler.SpellReady[(int)PieceColor.White] = unlockedPieces >= 8;
        }
        else
        {
            Destroy(spellHandlerObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //code for restarting the game after a game over
        if (gameOver && Input.GetKeyDown("r"))
        {
            StartCoroutine(ReloadScene());
        }
        else if (gameOver && Input.GetKeyDown(KeyCode.Backspace))
        {
            StartCoroutine(TitleTransition());
        }

        if (Input.GetKeyDown("0"))
        {
            recalculateBoard();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                TogglePause();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!gameOver && Get_Danger_Pieces() > 0)
        {
            mc.StartSlowTick();
        }
        else
        {
            mc.StopSlowTick();
        }
    }

    /// <summary>
    /// checks to see if a game over has occurred
    /// </summary>
    public void checkForGameOver()
    {
        //check to see if a piece is in the splitter area after each board check
        if (!gameOver)
        {
            for (int c = 7; c <= 8; c++)
            {
                for (int r = 0; r <= 7; r++)
                {
                    if (colorGrid[r, c] != PieceColor.Empty && grid[r, c] != null)
                    {
                        if (Game_Mode_Helper.ActiveRuleSet.HasSpells && spellHandler.spellWorking)
                        {
                            //there's a cyan bomb and the game should've ended. Wait it out, it will be re-checked after the bombs clear
                            Debug.Log("cyan bomb on field while a game over should've occurred, deactivating splitter until it's cleared");
                            splitter.setState(Splitter.SplitterStates.canShoot, false);
                            return;
                        }

                        startGameOver();
                        return;
                    }
                }
            }
            if (!gameOver)
            {
                splitter.setState(Splitter.SplitterStates.canShoot, true);
            }
        }
    }

    /// <summary>
    /// Puts the game in its game over state and sets appropriate gameobject status
    /// </summary>
    public void startGameOver()
    {
        //things that should only happen once at gameover
        gameOver = true;
        gameOverText.text = "Game Over";
        GameOverLayer.SetActive(true);
        mc.StopMusic();

        BitPool bitPool = GameObject.Find("Bit Pool").GetComponent<BitPool>();
        bitPool.cashInAllBits();

        achievementHandler.AddScore(Game_Mode_Helper.ActiveRuleSet, score);
        GameObject goBlackScreen = GameObject.Find("GO Black Screen");
        goBlackScreen.SetActive(true);
        goBlackScreen.GetComponent<Fader>().FadeIn();
        tipText.text = Constants.Hints[UnityEngine.Random.Range(0, Constants.Hints.Count())];
        mc.PlayMusic("Gameover");

        splitter.setState(Splitter.SplitterStates.canShoot, false);
        //unlocking candy cane splitter
        if (!achievementHandler.isSplitterUnlocked(SplitterType.CandyCane) && score > 0 && score < 200)
        {
            achievementHandler.UnlockSplitter(SplitterType.CandyCane);
        }

        if (!achievementHandler.isSplitterUnlocked(SplitterType.Dark) && (gameMode == GameMode.Wiz || gameMode == GameMode.Holy))
        {
            if (!spellHandler.Used_Spells() && score > 1000)
            {
                achievementHandler.UnlockSplitter(SplitterType.Dark);
            }
        }

        if (!achievementHandler.isSplitterUnlocked(SplitterType.Caution) || !achievementHandler.isPiecesetUnlocked(PieceSets.Blob))
        {
            int dangerPieces = Get_Danger_Pieces();
            if (!achievementHandler.isSplitterUnlocked(SplitterType.Caution) && gameMode != GameMode.Holy && gameMode != GameMode.Wiz && dangerPieces >= 5 && dangerPieces == 0)
            {
                achievementHandler.UnlockSplitter(SplitterType.Caution);
            }
            if (!achievementHandler.isPiecesetUnlocked(PieceSets.Blob) && gameMode == GameMode.Holy && dangerPieces == 16)
            {
                achievementHandler.UnlockPieceset(PieceSets.Blob);
            }
        }
    }

    //puts the pieces in the grid after they've settled into their place
    public void placePiece(GameObject piece)
    {
        Piece pieceStats = piece.GetComponent<Piece>();
        int thisX = (int)pieceStats.gridPos.x;
        int thisY = (int)pieceStats.gridPos.y;
        //check to make sure the piece is in the grid
        if (pieceStats.gridPos.x >= 0 && pieceStats.gridPos.x < 8 && pieceStats.gridPos.y >= 0 && pieceStats.gridPos.x < 16)
        {
            //uncomment this to show what the grid position of the newly placed piece is
            //Debug.Log ("Grid Position is" +(int)pieceStats.gridPos.x +" " + (int)pieceStats.gridPos.y);
            colorGrid[(int)pieceStats.gridPos.x, (int)pieceStats.gridPos.y] = pieceStats.pieceColor;
            grid[(int)pieceStats.gridPos.x, (int)pieceStats.gridPos.y] = piece;
            
            if (piecesPlaced > 0)
            {
                BeginBoardChecking();
            }
            else
            {
                piecesPlaced++;
            }
        }
        // if it isn't in the grid, throw an error up and delete the offending piece.
        else
        {
            Debug.LogError("Error in placing piece with position" + (int)pieceStats.gridPos.x + " " + (int)pieceStats.gridPos.y);
            Destroy(piece);
        }

    }

    /// <summary>
    /// Starts off board checking after the splitter's pieces have been settled
    /// </summary>
    public void BeginBoardChecking()
    {
        // if both pieces have been placed, set the checkGrid to false and check the board
        clearedLastTurn = piecesDeletedThisSplit;
        piecesDeletedThisSplit = false;
        piecesPlaced = 0;
        checkBoardLoop();
    }

    /// <summary>
    /// The board needs to be re-checked every time something changes on it
    /// </summary>
    public void checkBoardLoop()
    {
        bool deleted = checkForMatches();
        piecesDeletedThisSplit = piecesDeletedThisSplit || deleted;

        if (deleted)
        {
            collapse();
            StartCoroutine(boardWaiter());
        }
        else if (Game_Mode_Helper.ActiveRuleSet.UsesSides && !sidesChecked)
        {
            if (checkSideStatus())
            {
                sidesChecked = true;
                StartCoroutine(boardWaiter());
            }
            else
            {
                endTurn();
            }
        }
        else
        {
            endTurn();
        }
    }

    /// <summary>
    /// Updates the sides as needed, adds them and returns true if they're ready to be added
    /// </summary>
    /// <returns> bool - true if the sides were added to the board, false otherwise</returns>
    public bool checkSideStatus()
    {
        GameObject[] sidebars = GameObject.FindGameObjectsWithTag("Sidebar");
        //here's where we do side-entering management
        if (Game_Mode_Helper.ActiveRuleSet.TurnedCrunch)
        {
            if (movesMade % sideMovesLimit == 0)
            {
                addSideColumns();
                sideColumns[0].shakeStage = 0;
                sideColumns[1].shakeStage = 0;
                sidebars[0].BroadcastMessage("Reset");
                sidebars[1].BroadcastMessage("Reset");
                mc.StopFastTick();
                return true;
            }
            else if (sideMovesLimit - (movesMade % sideMovesLimit) <= 8)
            {
                int splitsLeft = sideMovesLimit - (movesMade % sideMovesLimit);
                switch (splitsLeft)
                {
                    case 1:
                        sideColumns[0].isShaking = false;
                        sideColumns[1].isShaking = false;
                        sideColumns[0].ready = true;
                        sideColumns[1].ready = true;
                        sideColumns[0].shakeStage = 0;
                        sideColumns[1].shakeStage = 0;
                        break;
                    case 2:
                        sideColumns[0].shakeStage = 3;
                        sideColumns[1].shakeStage = 3;
                        sidebars[0].BroadcastMessage("IncrementLights");
                        sidebars[1].BroadcastMessage("IncrementLights");
                        break;
                    case 3:
                        sideColumns[0].shakeStage = 2;
                        sideColumns[1].shakeStage = 2;
                        sidebars[0].BroadcastMessage("IncrementLights");
                        sidebars[1].BroadcastMessage("IncrementLights");
                        break;
                    case 4:
                        sideColumns[0].isShaking = true;
                        sideColumns[1].isShaking = true;
                        sideColumns[0].shakeStage = 1;
                        sideColumns[1].shakeStage = 1;
                        break;
                }

                int turnToTick = Math.Min((int)(0.5 * Game_Mode_Helper.ActiveRuleSet.SplitsPerCrunch), 4);

                if (splitsLeft <= turnToTick && !mc.IsFastTicking)
                {
                    mc.StartFastTick();
                }

                sidebars[0].BroadcastMessage("SetLightStage", 8 - splitsLeft);
                sidebars[1].BroadcastMessage("SetLightStage", 8 - splitsLeft);

                return false;
            }
        }
        else if (Game_Mode_Helper.ActiveRuleSet.TimedCrunch)
        {
            //quick mode moves the sides in based off of time, not moves
            if (quickMoveSides)
            {
                addSideColumns();
                sideColumns[0].ready = false;
                sideColumns[1].ready = false;
                quickMoveSides = false;
                StartCoroutine("QuickSideTimer");
                mc.StopFastTick();
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// checkForMatches() begins the recursive checkBoard check. The recursive sister method, scanner(), will check off some
	/// of the pieces but not all, so checkboard calls scanner on every piece not yet checked in its main loop.any groups
	/// are stored for easier deletion post-check and the deletion check calculates scores based off of group size and
    /// current multiplier.
    /// </summary>
    /// <returns> bool - true if any combos were found, false if nothing's changed</returns>
    public bool checkForMatches()
    {
        recalculateBoard();
        //reset the bool board for the current run
        for (int r = 0; r <= 7; r++)
        {
            for (int c = 0; c <= 15; c++)
            {
                checkGrid[r, c] = false;
            }
        }
        int groupCount = 0;
        bool groupIncreased = false;

        //nested for loops for checking the grid
        for (int r = 0; r <= 7; r++)
        {
            for (int c = 0; c <= 15; c++)
            {
                //check if current piece has already been checked
                if (checkGrid[r, c] == false && grid[r, c] != null)
                {
                    //uncomment this to see what pieces the function is checking as it checks them
                    //Debug.Log ("Checking piece in position " + r + " " + c);
                    checkGrid[r, c] = true;
                    cluster[0] = grid[r, c];
                    //begin the recursion
                    int temp = scanner(r, c, grid[r, c].GetComponent<Piece>().pieceColor, 1);
                    //loop that assigns each piece in the group the group value to be scanned afterwards
                    for (int i = 0; i < temp; i++)
                    {
                        cluster[i].GetComponent<Piece>().groupValue = temp;
                    }
                    //if a group has hit the deletion count, and it's not the first, add to the multiplier
                    if (temp >= 4)
                    {
                        groupCount++;
                        groupIncreased = true;
                    }
                    //if the previous turn had a clear, add to the multiplier
                    if (groupIncreased && groupCount == 1 && clearedLastTurn && !multiRun)
                    {
                        multiplier++;
                    }

                    if ((groupCount >= 2 || multiRun) && groupIncreased)
                    {
                        multiplier++;
                    }
                }
                else
                {
                    checkGrid[r, c] = true;
                }
                //reset group increased after doing a scanner check in preperation for later checks
                groupIncreased = false;
            }
        }

        //now check if groups need to be removed
        bool deleted = false;
        for (int r = 0; r <= 7; r++)
        {
            for (int c = 0; c <= 15; c++)
            {
                //if the value is high enough it's in a group big enough to delete, so delete it
                if (grid[r, c] != null && grid[r, c].GetComponent<Piece>().groupValue >= 4)
                {
                    grid[r, c].GetComponent<Piece>().multiplier = multiplier;
                    updateScore();
                    //delete piece, mark that something was deleted
                    Destroy(grid[r, c]);
                    grid[r, c] = null;
                    deleted = true;
                }
            }
        }

        return deleted;
    }

    /// <summary>
    /// Does the combo/game over checking, as well as re-allowing the splitter to shoot if needed. Called after the sides and board were
    /// checked and nothing was changed on the board
    /// </summary>
    public void endTurn()
    {
        checkForGameOver();

        if (piecesDeletedThisSplit)
        {
            multiRun = true;
        }
        else
        {
            multiRun = false;

            if (!gameOver)
            {
                if (!piecesDeletedThisSplit)
                {
                    multiplier = 1;
                }
            }
                
            clearedLastTurn = piecesDeletedThisSplit;
        }

        if (gameMode != GameMode.Holy && gameMode != GameMode.Wiz && !achievementHandler.isPiecesetUnlocked(PieceSets.Domino) && multiplier >= 9)
        {
            achievementHandler.UnlockPieceset(PieceSets.Domino);
        }

        if (!gameOver)
        {
            splitter.setState(Splitter.SplitterStates.canShoot, true);
            sidesChecked = false;
        }
    }

    /// <summary>
    /// Collapses the game board, which makes pieces on the left that have no piece left of them will fall as far left as they can
    /// and any pieces on the right will fall to the right if they have no pieces to the right. Visually this will take a moment,
    /// so call the board waiter after every call to this
    /// </summary>
    public void collapse()
    {
        int tempCol;
        bool adjusted = false;
        //check the left grid
        for (int r = 0; r < 8; r++)
        {
            tempCol = 0;
            adjusted = false;
            for (int c = 0; c < 8; c++)
            {
                if (grid[r, c] == null && !adjusted)
                {
                    tempCol = c;
                    adjusted = true;
                }
                else if (adjusted && grid[r, c] != null)
                {
                    //change the piece's stats to reflect the new position
                    grid[r, c].GetComponent<Piece>().movePiece(new Vector2(tempCol - 8, r));
                    //re-assign all grids to fit the new position, add 1 to tempCol
                    grid[r, tempCol] = grid[r, c];
                    grid[r, c] = null;
                    colorGrid[r, tempCol] = colorGrid[r, c];
                    colorGrid[r, c] = PieceColor.Empty;
                    tempCol++;
                    //set c to TempCol to restart the check for later things in the column
                    c = tempCol;
                }
            }
        }

        //check the right grid
        for (int r = 0; r < 8; r++)
        {
            tempCol = 0;
            adjusted = false;
            for (int c = 15; c > 7; c--)
            {
                if (grid[r, c] == null && !adjusted)
                {
                    tempCol = c;
                    adjusted = true;
                }
                else if (adjusted && grid[r, c] != null)
                {
                    //change the piece's stats to reflect the new position
                    grid[r, c].GetComponent<Piece>().movePiece(new Vector2(tempCol - 8, r));
                    //re-assign all grids to fit the new position, subtract to tempCol
                    grid[r, tempCol] = grid[r, c];
                    grid[r, c] = null;
                    colorGrid[r, tempCol] = colorGrid[r, c];
                    colorGrid[r, c] = PieceColor.Empty;
                    tempCol--;
                    //set c to TempCol to restart the check for later things in the column
                    c = tempCol;
                }
            }
        }
    }

    /// <summary>
    /// Begins/continues the Board check -> collapse -> wait -> Board check loop without checking for sides, or affecting the combo
    /// </summary>
    public void lazyBoardLoop()
    {
        bool deleted = checkForMatches();

        if (deleted)
        {
            collapse();
            StartCoroutine(lazyBoardWaiter());
        }
        else
        {
            checkForGameOver();
            if (!gameOver)
            {
                splitter.setState(Splitter.SplitterStates.isActive, true);
                splitter.setState(Splitter.SplitterStates.canShoot, true);
            }
        }
    }

    /// <summary>
    /// to continue the checkboard-> collapse -> checkboard loop without checking sides, game overs, or affecting piece combos
    /// </summary>
    public IEnumerator lazyBoardWaiter()
    {
        yield return new WaitForSeconds(0.3f);
        lazyBoardLoop();
    }

    //scanner goes through and checks every adjacent piece recursively, then returns the amount of pieces in a cluster.
    public int scanner(int x, int y, PieceColor color, int adj)
    {
        //mark current as checked
        checkGrid[x, y] = true;
        //check right of piece
        if ((x + 1 < 8) && 
            (checkGrid[x + 1, y] == false && grid[x + 1, y] != null && grid[x + 1, y].GetComponent<Piece>().pieceColor == color))
        {
            //check to make sure the piece actually has a grid position and isn't in the splitter
            if (grid[x + 1, y].GetComponent<Piece>().locked &&
                grid[x + 1, y].GetComponent<Piece>().isBomb == false)
            {
                adj++;
                //add to group cluster
                cluster[adj - 1] = grid[x + 1, y];
                adj = scanner(x + 1, y, color, adj);
            }
        }
        //check up
        if (y + 1 < 16 && checkGrid[x, y + 1] == false && grid[x, y + 1] != null && grid[x, y + 1].GetComponent<Piece>().pieceColor == color)
        {
            //check to make sure the piece actually has a grid position and isn't in the splitter
            if (grid[x, y + 1].GetComponent<Piece>().locked &&
                grid[x, y + 1].GetComponent<Piece>().isBomb == false)
            {
                adj++;
                //add to group cluster
                cluster[adj - 1] = grid[x, y + 1];
                adj = scanner(x, y + 1, color, adj);
            }
        }
        //check left
        if (x - 1 >= 0 && checkGrid[x - 1, y] == false && grid[x - 1, y] != null && grid[x - 1, y].GetComponent<Piece>().pieceColor == color)
        {
            //check to make sure the piece actually has a grid position and isn't in the splitter
            if (grid[x - 1, y].GetComponent<Piece>().locked &&
                grid[x - 1, y].GetComponent<Piece>().isBomb == false)
            {
                adj++;
                //add to group cluster
                cluster[adj - 1] = grid[x - 1, y];
                adj = scanner(x - 1, y, color, adj);
            }
        }
        //check below
        if (y - 1 >= 0 && checkGrid[x, y - 1] == false && grid[x, y - 1] != null && grid[x, y - 1].GetComponent<Piece>().pieceColor == color)
        {
            //check to make sure the piece actually has a grid position and isn't in the splitter
            if (grid[x, y - 1].GetComponent<Piece>().locked &&
                grid[x, y - 1].GetComponent<Piece>().isBomb == false)
            {
                adj++;
                //add to group cluster
                //FIXME: using the white Spell on a full board in Wiz cause a game-breaking index out of range error. Cannot easily replicate
                cluster[adj - 1] = grid[x, y - 1];
                adj = scanner(x, y - 1, color, adj);
            }
        }

        //base case, return the cluster value found so far
        return adj;
    }

    //call this when the move counter needs to change
    public void updateMoves()
    {
        movesText.text = "Splits made: " + movesMade;

        if (splitsToUnlock > 0 && 
            movesMade % splitsToUnlock == 0 && 
            availableCount != 8 && 
            movesMade != 0)
        {
            availableCount++;
            if (Game_Mode_Helper.ActiveRuleSet.HasSpells)
            {
                switch (availableCount)
                {
                    case 4:
                        spellHandler.SpellReady[(int)PieceColor.Yellow] = true;
                        break;
                    case 5:
                        spellHandler.SpellReady[(int)PieceColor.Purple] = true;
                        break;
                    case 6:
                        spellHandler.SpellReady[(int)PieceColor.Orange] = true;
                        break;
                    case 7:
                        spellHandler.SpellReady[(int)PieceColor.Cyan] = true;
                        break;
                    case 8:
                        spellHandler.SpellReady[(int)PieceColor.White] = true;
                        break;
                }
            }
            else if (!achievementHandler.isPiecesetUnlocked(PieceSets.Techno) && gameMode == GameMode.Quick && availableCount == 5)
            {
                achievementHandler.UnlockPieceset(PieceSets.Techno);
            }
        }

        if (!achievementHandler.isPiecesetUnlocked(PieceSets.Retro) && gameMode == GameMode.Wit && movesMade == 255)
        {
            achievementHandler.UnlockPieceset(PieceSets.Retro);
        }
    }

    //call this when the score counter needs to be updated
    public void updateScore()
    {
        if (!isQuitting)
        {
            scoreText.text = "Score:\n" + score;
        }
        //save current score
        if (PlayerPrefs.GetInt(Game_Mode_Helper.ActiveRuleSet.ToString() + Constants.TopScorePredicate, 0) < score)
        {
            if (!newHighScore)
            {
                HighScoreText.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
                HighScoreText.gameObject.GetComponent<AudioSource>().Play();
                newHighScore = true;
                HighScoreText.text = "New High Score!";
            }
        }
    }

    // MoveInward will move every piece towards the center and create free columns near the edges
    public void MoveInward()
    {
        if (Game_Mode_Helper.ActiveRuleSet.UsesSides)
        {
            // iterate through the left side moving everything forward a column
            for (int c = 6; c >= 0; c--)
            {
                for (int r = 0; r <= 7; r++)
                {
                    if (colorGrid[r, c] != PieceColor.Empty && grid[r, c] != null)
                    {
                        //piece exits, more rightward making sure to
                        //change the piece's stats to reflect the new position
                        grid[r, c].transform.parent = null;
                        grid[r, c].GetComponent<Piece>().movePiece(new Vector2((c + 1) - 8, r));
                        //re-assign all grids to fit the new position, add 1 to tempCol
                        grid[r, c + 1] = grid[r, c];
                        grid[r, c] = null;
                        colorGrid[r, (c + 1)] = colorGrid[r, c];
                        colorGrid[r, c] = PieceColor.Empty;
                    }
                }
            }

            //and now iterate through the right
            //next we iterate through the left side moving everything forward a column
            for (int c = 9; c <= 15; c++)
            {
                for (int r = 0; r <= 7; r++)
                {
                    if (colorGrid[r, c] != PieceColor.Empty && grid[r, c] != null)
                    {
                        //piece exits, more rightward making sure to
                        //change the piece's stats to reflect the new position
                        grid[r, c].GetComponent<Piece>().movePiece(new Vector2((c - 1) - 8, r));
                        //re-assign all grids to fit the new position, add 1 to tempCol
                        grid[r, c - 1] = grid[r, c];
                        grid[r, c] = null;
                        colorGrid[r, (c - 1)] = colorGrid[r, c];
                        colorGrid[r, c] = PieceColor.Empty;
                    }
                }
            }
        }
    }

    //adds the stored side column pieces to the board.
    public void addSideColumns()
    {
        if (Game_Mode_Helper.ActiveRuleSet.UsesSides)
        {
            if (sideColumns[0] == null || sideColumns[1] == null)
            {
                Debug.LogError("GameController Error: Attempting to add nonexistant side columns");
                return;
            }

            //make room for the new columns
            MoveInward();

            //loading left
            for (int r = 0; r < 8; r++)
            {
                colorGrid[r, 0] = sideColumns[0].colorColumn[r];
                grid[r, 0] = sideColumns[0].column[r];
                grid[r, 0].GetComponent<Piece>().movePiece(new Vector2(-8, r));
                grid[r, 0].GetComponent<Piece>().inSideHolder = false;
                grid[r, 0].transform.parent = null;
            }
            sideColumns[0].empty();
            sideColumns[0].reload();

            //loading right
            for (int r = 0; r < 8; r++)
            {
                colorGrid[r, 15] = sideColumns[1].colorColumn[r];
                grid[r, 15] = sideColumns[1].column[r];
                grid[r, 15].GetComponent<Piece>().movePiece(new Vector2(15 - 8, r));
                grid[r, 15].GetComponent<Piece>().inSideHolder = false;
                grid[r, 15].transform.parent = null;
            }
            sideColumns[1].empty();
            sideColumns[1].reload();

            sideColumns[0].isShaking = false;
            sideColumns[1].isShaking = false;
        }
    }

    //this is solely to let the pieces fall into place aesthetically
    public IEnumerator boardWaiter()
    {
        yield return new WaitForSeconds(0.3f);
        checkBoardLoop();
    }

    //this begins the countdown at the start of Quick mode
    public IEnumerator StartingCountDown()
    {
        // need to calculate stage timing 
        double startSeconds = Math.Min(timePerCrunch.TotalSeconds * 0.35f, 7f);
        lightIncrementTiming = new TimeSpan((long)(Math.Min(startSeconds / 7, 1f) * TimeSpan.TicksPerSecond));
        timeBeforeShaking = timePerCrunch.Subtract(new TimeSpan((long)(startSeconds * TimeSpan.TicksPerSecond)));
        yield return new WaitForSeconds(4f);
        splitter.setState(Splitter.SplitterStates.canShoot, true);
        mc.PlayMusic(gameMode);
        StartCoroutine("QuickSideTimer");
        yield return new WaitForSeconds(1f);
        gameOverText.text = "";
    }

    //this is the timer for the side columns in quick mode
    public IEnumerator QuickSideTimer()
    {
        GameObject[] sidebars = GameObject.FindGameObjectsWithTag("Sidebar");
        sidebars[0].BroadcastMessage("Reset");
        sidebars[1].BroadcastMessage("Reset");
        yield return new WaitForSeconds((float)timeBeforeShaking.TotalSeconds);
        sideColumns[0].isShaking = true;
        sideColumns[1].isShaking = true;
        yield return new WaitForSeconds((float)lightIncrementTiming.TotalSeconds);
        sidebars[0].BroadcastMessage("IncrementLights");
        sidebars[1].BroadcastMessage("IncrementLights");
        yield return new WaitForSeconds((float)lightIncrementTiming.TotalSeconds);
        sideColumns[0].shakeStage = 1;
        sideColumns[1].shakeStage = 1;
        if (!gameOver)
        {
            mc.StartFastTick();
        }

        sidebars[0].BroadcastMessage("IncrementLights");
        sidebars[1].BroadcastMessage("IncrementLights");
        yield return new WaitForSeconds((float)lightIncrementTiming.TotalSeconds);
        sidebars[0].BroadcastMessage("IncrementLights");
        sidebars[1].BroadcastMessage("IncrementLights");
        yield return new WaitForSeconds((float)lightIncrementTiming.TotalSeconds);
        sidebars[0].BroadcastMessage("IncrementLights");
        sidebars[1].BroadcastMessage("IncrementLights");
        yield return new WaitForSeconds((float)lightIncrementTiming.TotalSeconds);
        sideColumns[0].shakeStage = 2;
        sideColumns[1].shakeStage = 2;
        sidebars[0].BroadcastMessage("IncrementLights");
        sidebars[1].BroadcastMessage("IncrementLights");
        yield return new WaitForSeconds((float)lightIncrementTiming.TotalSeconds);
        sidebars[0].BroadcastMessage("IncrementLights");
        sidebars[1].BroadcastMessage("IncrementLights");
        yield return new WaitForSeconds((float)lightIncrementTiming.TotalSeconds);
        sidebars[0].BroadcastMessage("IncrementLights");
        sidebars[1].BroadcastMessage("IncrementLights");
        yield return new WaitForSeconds((float)lightIncrementTiming.TotalSeconds);
        sideColumns[0].isShaking = false;
        sideColumns[1].isShaking = false;
        sideColumns[0].ready = true;
        sideColumns[1].ready = true;
        sideColumns[0].shakeStage = 0;
        sideColumns[1].shakeStage = 0;
        quickMoveSides = true;
        sidebars[0].BroadcastMessage("IncrementLights");
        sidebars[1].BroadcastMessage("IncrementLights");
    }

    //use this to double check to make sure the arrays are accurate;
    public void recalculateBoard()
    {
        grid = new GameObject[8, 16];
        colorGrid = new PieceColor[8, 16];
        GameObject[] allPieces = GameObject.FindGameObjectsWithTag("Piece");
        List<GameObject> offendingPieces = new List<GameObject>();
        foreach (GameObject piece in allPieces)
        {
            Piece temp = piece.GetComponent<Piece>();
            //if it's on the grid, add it to the grid
            if (temp != null && !temp.inHolder && !temp.inSplitter && !temp.inSideHolder)
            {
                //it's just been shot, ignore it for now
                if (!(temp.gridPos.x < 0))
                {
                    if (grid[(int)temp.gridPos.x, (int)temp.gridPos.y] == null)
                    {
                        grid[(int)temp.gridPos.x, (int)temp.gridPos.y] = piece;
                        colorGrid[(int)temp.gridPos.x, (int)temp.gridPos.y] = temp.pieceColor;
                    }
                    else
                    {
                        offendingPieces.Add(piece);
                    }
                }
            }
        }
        foreach (GameObject piece in offendingPieces)
        {
            Destroy(piece);
        }
        collapse();
    }

    //this handles all pausing transitions
    public void TogglePause()
    {
        if (gameOver)
        {
            return;
        }

        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
            gameOverText.text = "PAUSED";
            pauseLayer.SetActive(true);
            splitter.setState(Splitter.SplitterStates.isActive, false);
            GameObject.Find("Pause Button Text").GetComponent<Text>().text = "Unpause";
            mc.PauseMusic();
            PauseSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
            PauseSFX.Play();
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1;
            gameOverText.text = "";
            splitter.setState(Splitter.SplitterStates.isActive, true);
            pauseLayer.SetActive(false);
            GameObject.Find("Pause Button Text").GetComponent<Text>().text = "Pause";
            mc.ResumeMusic();
            UnpauseSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
            UnpauseSFX.Play();
        }
    }

    //returns to the main menu
    public void LoadMainMenu()
    {
        //save current score
        if (PlayerPrefs.GetInt(gameMode.ToString(), 0) < score)
        {
            PlayerPrefs.SetInt(gameMode.ToString(), score);
        }
        isQuitting = true;
        //remember to properly reload time
        Time.timeScale = 1;
        //stop the music
        mc.StopMusic();
        //load the main menu
        StartCoroutine(TitleTransition());
    }

    //restarts the game scene
    public void Retry()
    {
        StartCoroutine("ReloadScene");
    }

    //begins the reloading of the current scene
    private IEnumerator ReloadScene()
    {
        if (PlayerPrefs.GetInt(gameMode.ToString(), 0) < score)
        {
            PlayerPrefs.SetInt(gameMode.ToString(), score);
        }
        isQuitting = true;
        //remember to properly reload time
        Time.timeScale = 1;
        mc.StopMusic();
        shutter.BeginHorizontalClose();
        AsyncOperation async = SceneManager.LoadSceneAsync("Game Scene");
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(2f);
        async.allowSceneActivation = true;
        yield return async;
    }

    //transitions to the title screen
    private IEnumerator TitleTransition()
    {
        mc.StopMusic();
        shutter.BeginHorizontalClose();
        AsyncOperation async = SceneManager.LoadSceneAsync("Split Title Scene");
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(2f);
        async.allowSceneActivation = true;
        yield return async;
    }

    //used for handling some gameobject that don't know when to stop
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    //returns the number of pieces in danger (the columns next to the splitter)
    public int Get_Danger_Pieces()
    {
        int dangerPieces = 0;
        for (int r = 0; r < 8; r++)
        {
            if (grid[r, 6] != null)
            {
                dangerPieces++;
            }

            if (grid[r, 9] != null)
            {
                dangerPieces++;
            }
        }
        return dangerPieces;
    }

    //now for android related things
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && !isPaused)
        {
            TogglePause();
        }
    }
}