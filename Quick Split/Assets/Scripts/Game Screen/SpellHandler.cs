using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpellHandler : MonoBehaviour
{

    //This scripte handles all the spell stuff, including activating and tracking spells

    private GameController gameController;
    private Splitter splitter;
    private Holder_Script holder;
    private Achievement_Script achievementHandler;

    public bool[] SpellReady = new bool[8];
    public int[] SpellProgress = new int[8];
    public int[] SpellGoal = Enumerable.Repeat(100, 8).ToArray();
    public Text[] SpellText = new Text[8];

    public PieceColor spellColor;
    public Piece selectedPiece;
    private PieceColor pickedColor1 = PieceColor.Empty;
    private PieceColor pickedColor2 = PieceColor.Empty;
    private int spellLimit = 0;

    //this is to tell scorebits that are created as a result of spells not to charge them
    public bool spellActive;
    private int splitsNeeded = 0;

    //this is to make it so you can't activate multiple spells at once
    public bool spellWorking = false;
    private bool[] spellsUsed = new bool[8] { false, false, false, false, false, false, false, false };

    public float wizMultiplier;
    public float holyMultiplier;
    private float chargeMultiplier;
    private GameObject[] RedSpellEffects;
    private GameObject YellowSpellEffect;

    public AudioSource RedChargeSFX;
    public AudioSource PurpleEffectSFX;
    public AudioSource WhiteEffectSFX;

    // Use this for initialization
    private void Start()
    {
        pickedColor1 = PieceColor.Empty;
        pickedColor2 = PieceColor.Empty;
        GameObject gameControllerobject = GameObject.FindGameObjectWithTag("GameController");
        RedSpellEffects = GameObject.FindGameObjectsWithTag("Red Spell Effect");
        YellowSpellEffect = GameObject.Find("Yellow Spell Handler");
        foreach (GameObject rse in RedSpellEffects)
        {
            rse.SetActive(false);
        }

        if (gameControllerobject == null)
        {
            Debug.LogError("spell Handler Error: cannot find game controller");
        }
        else
        {
            gameController = gameControllerobject.GetComponent<GameController>();
        }

        GameObject splitterObject = GameObject.FindGameObjectWithTag("Splitter");
        if (splitterObject == null)
        {
            Debug.LogError("spell Handler Error: cannot find Splitter");
        }
        else
        {
            splitter = splitterObject.GetComponent<Splitter>();
        }

        GameObject holderObject = GameObject.FindGameObjectWithTag("Holder");
        if (holderObject == null)
        {
            Debug.LogError("spell Handler Error: cannot find holder");
        }
        else
        {
            holder = holderObject.GetComponent<Holder_Script>();
        }

        foreach (Text spellText in SpellText)
        {
            spellText.text = "";
        }

        selectedPiece = null;
        if (gameController.gameMode == GameMode.Wiz)
        {
            chargeMultiplier = wizMultiplier;
        }
        else
        {
            chargeMultiplier = holyMultiplier;
        }

        spellActive = false;
        achievementHandler = GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<Achievement_Script>();
    }

    // Update is called once per frame
    private void Update()
    {
        //attacks, only activatable one at a time
        if (spellColor == PieceColor.Empty &&
            splitter.getState(Splitter.SplitterStates.canShoot) &&
            !splitter.getState(Splitter.SplitterStates.isMoving))
        {
            //red
            if (Input.GetKeyDown("1") && SpellReady[(int)PieceColor.Red])
            {
                Redspell();
            }
            //orange
            if (Input.GetKeyDown("2")  && SpellReady[(int)PieceColor.Orange])
            {
                Orangespell();
            }
            //yellow
            if (Input.GetKeyDown("3")  && SpellReady[(int)PieceColor.Yellow])
            {
                Yellowspell();
            }
            //green
            if (Input.GetKeyDown("4") && SpellReady[(int)PieceColor.Green])
            {
                Greenspell();
            }
            //blue
            if (Input.GetKeyDown("5") && SpellReady[(int)PieceColor.Blue])
            {
                Bluespell();
            }
            //purple
            if (Input.GetKeyDown("6") && SpellReady[(int)PieceColor.Purple])
            {
                Purplespell();
            }
            //cyan
            if (Input.GetKeyDown("7") && SpellReady[(int)PieceColor.Cyan])
            {
                Cyanspell();
            }
            //white
            if (Input.GetKeyDown("8") && SpellReady[(int)PieceColor.White])
            {
                Whitespell();
            }
        }
    }

    //Red attack: Burns a layer off the top of each side, specifically deleting the block in each row closest to the center
    public void Redspell()
    {
        Spell_Used(PieceColor.Red);

        //Red Splitter Achievement
        if (!achievementHandler.is_Splitter_Unlocked(SplitterType.Red) && gameController.Get_Danger_Pieces() >= 8)
        {
            achievementHandler.Unlock_Splitter(SplitterType.Red);
        }

        RedChargeSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1) * 0.5f;
        RedChargeSFX.Play();

        //This spell uses the Red Spell Effect Gameobjects to do all the dirty work
        foreach (GameObject rse in RedSpellEffects)
        {
            rse.SetActive(true);
            rse.BroadcastMessage("Activate", null, SendMessageOptions.DontRequireReceiver);
        }
        //there are 16 RSE objects
        spellLimit = 16;
        splitter.setState(Splitter.SplitterStates.isActive, false);
    }

    public void Red_Spell_Helper()
    {
        //once all 16 RSE's have chimed in, then the red spell is over
        spellLimit--;
        if (spellLimit == 0)
        {
            gameController.checkBoard();
            splitter.setState(Splitter.SplitterStates.isActive, true);
            spellWorking = false;
        }
    }

    //Orange attack: switches all the pieces of a single color on one side with all the pieces of a different single color on the other side
    //deletes leftover pieces if the switch is uneven.
    public void Orangespell()
    {
        Spell_Used(PieceColor.Orange);
        spellColor = PieceColor.Orange;
        GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
        picker.GetComponent<ColorSelector>().givePurpose("Select a color to switch with on the left side");
    }

    //called after both selections are made
    private void OrangeHelper()
    {
        List<GameObject> leftPieces = new List<GameObject>();
        List<GameObject> rightPieces = new List<GameObject>();

        //Orange Splitter unlock code
        if (!achievementHandler.is_Splitter_Unlocked(SplitterType.Orange))
        {
            if ((leftPieces.Count == 0 && rightPieces.Count > 0) || (leftPieces.Count > 0 && rightPieces.Count > 0))
            {
                achievementHandler.Unlock_Splitter(SplitterType.Orange);
            }
        }

        //go through left side, store all pieces of color 1 in an array
        for (int r = 0; r < 8; r++)
        {
            for (int c = 7; c >= 0; c--)
            {
                //Debug.Log ("Checking position R: " + r + " C: " + c);
                if (gameController.grid[r, c] != null)
                {
                    if (gameController.colorGrid[r, c] == pickedColor1)
                    {
                        leftPieces.Add(gameController.grid[r, c]);
                    }
                }
            }
        }
        //same with right
        for (int r = 0; r < 8; r++)
        {
            for (int c = 8; c < 16; c++)
            {
                //Debug.Log ("Checking position R: " + r + " C: " + c);
                if (gameController.grid[r, c] != null)
                {
                    if (gameController.colorGrid[r, c] == pickedColor2)
                    {
                        rightPieces.Add(gameController.grid[r, c]);
                    }
                }
            }
        }
        //get both lengths and store the smallest size
        int smallestSize = Mathf.Min(leftPieces.Count, rightPieces.Count);
        int largestSize = Mathf.Max(leftPieces.Count, rightPieces.Count);
        int difference = largestSize - smallestSize;
        bool leftLarger = (leftPieces.Count >= rightPieces.Count);
        //delete the remainder on the bigger side randomly
        if (leftLarger)
        {
            for (; difference > 0; difference--)
            {
                int randPiece = (int)Random.Range(0, leftPieces.Count);
                int r = (int)leftPieces[randPiece].GetComponent<Piece>().gridPos.x;
                int c = (int)leftPieces[randPiece].GetComponent<Piece>().gridPos.y;
                leftPieces.RemoveAt(randPiece);
                //marking the object as dead so that the orangeActive state knows to delete it
                gameController.grid[r, c].BroadcastMessage("Activate_Orange", "dead", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            for (; difference > 0; difference--)
            {
                int randPiece = (int)Random.Range(0, rightPieces.Count);
                int r = (int)rightPieces[randPiece].GetComponent<Piece>().gridPos.x;
                int c = (int)rightPieces[randPiece].GetComponent<Piece>().gridPos.y;
                rightPieces.RemoveAt(randPiece);
                gameController.grid[r, c].BroadcastMessage("Activate_Orange", "dead", SendMessageOptions.DontRequireReceiver);
            }
        }
        //swap colors in each array
        for (int i = 0; i < leftPieces.Count; i++)
        {
            //recall Activate orange on the left side to update with the second picked color and get the animations going
            leftPieces[i].BroadcastMessage("Activate_Orange", pickedColor2.ToString(), SendMessageOptions.DontRequireReceiver);
        }

        for (int i = 0; i < rightPieces.Count; i++)
        {
            //recall Activate orange on the right side to update with the 1st picked color and get the animations going
            rightPieces[i].BroadcastMessage("Activate_Orange", pickedColor1.ToString(), SendMessageOptions.DontRequireReceiver);
        }
        pickedColor1 = PieceColor.Empty;
        pickedColor2 = PieceColor.Empty;
        spellColor = PieceColor.Empty;
        StartCoroutine(Orange_Waiter());
    }
    //Yellow attack: launches a single lightning bolt to each side that removes any blocks in the splitter's row
    //this method loads the splitter with the power to activate it on the next fire
    public void Yellowspell()
    {
        Spell_Used(PieceColor.Yellow);
        splitter.setState(Splitter.SplitterStates.yellowReady, true);
        //recolor splitter to show it's ready to fire
        splitter.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
    }

    public void YellowActivate()
    {
        //set splitter to default color
        splitter.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        splitter.setState(Splitter.SplitterStates.isActive, false);
        //this spell is also entirely handled by a seperate gameobject
        YellowSpellEffect.BroadcastMessage("Activate", null, SendMessageOptions.DontRequireReceiver);
        splitter.setState(Splitter.SplitterStates.yellowReady, false);
    }
    //Green attack: change the color of three pieces currently in holder or splitter to any color the player chooses
    public void Greenspell()
    {
        //green spell and helper tell other objects when to activate
        Spell_Used(PieceColor.Green);
        spellColor = PieceColor.Green;
        spellLimit = 3;
        splitter.leftSlot.GetComponent<Piece>().selectable = true;
        splitter.rightSlot.GetComponent<Piece>().selectable = true;
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 2; c++)
            {
                holder.holder[r, c].gameObject.GetComponent<Piece>().selectable = true;
            }
        }
        gameController.gameOverText.text = "Select pieces in the holder/splitter to change";
        splitter.setState(Splitter.SplitterStates.isActive, false);
    }
    public void GreenHelper()
    {
        spellLimit--;
        //if all 3 tries have been exhausted, then the spell is over
        if (spellLimit <= 0)
        {
            splitter.leftSlot.GetComponent<Piece>().selectable = false;
            splitter.rightSlot.GetComponent<Piece>().selectable = false;

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    holder.holder[r, c].GetComponent<Piece>().selectable = false;
                }
            }
            pickedColor1 = PieceColor.Empty;
            pickedColor2 = PieceColor.Empty;
            spellColor = PieceColor.Empty;
            selectedPiece = null;
            spellLimit = 0;
            gameController.gameOverText.text = "";
            spellWorking = false;
        }
    }

    //Blue attack: recolor any 3 pieces on the board
    public void Bluespell()
    {
        //blue spell is almost identical to green spell, just over a different space
        // but first we need to cover a case that makes it impossible to use the blue spell
        int boardPieceCount = 0;
        foreach (GameObject piece in gameController.grid)
        {
            if (piece != null)
            {
                boardPieceCount++;
                if (boardPieceCount == 3)
                {
                    break;
                }
            }
        }
        //TODO: Make a message that tells the player there aren't enough pieces on the board to change
        if (boardPieceCount < 3)
        {
            return;
        }

        Spell_Used(PieceColor.Blue);
        spellColor = PieceColor.Blue;
        spellLimit = 3;
        gameController.gameOverText.text = "Select pieces on the board to change";
        GameObject[] allPieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject piece in allPieces)
        {
            Piece temp = piece.GetComponent<Piece>();
            if (!temp.inSideHolder && !temp.inHolder && !temp.inSplitter)
            {
                temp.selectable = true;
            }
        }

        splitter.setState(Splitter.SplitterStates.isActive, false);
    }

    private void BlueHelper()
    {
        spellLimit--;
        //again, like the green spell it only has 3 uses
        if (spellLimit <= 0)
        {
            splitter.leftSlot.GetComponent<Piece>().selectable = false;
            splitter.rightSlot.GetComponent<Piece>().selectable = false;

            GameObject[] allPieces = GameObject.FindGameObjectsWithTag("Piece");
            foreach (GameObject piece in allPieces)
            {
                Piece temp = piece.GetComponent<Piece>();
                if (!temp.inSideHolder && !temp.inHolder && !temp.inSplitter)
                {
                    temp.selectable = false;
                }
            }
            pickedColor1 = PieceColor.Empty;
            pickedColor2 = PieceColor.Empty;
            spellColor = PieceColor.Empty;
            selectedPiece = null;
            spellLimit = 0;
            gameController.gameOverText.text = "";
            spellWorking = false;
            if (!achievementHandler.is_Splitter_Unlocked(SplitterType.Blue))
            {
                StartCoroutine(achievementHandler.Blue_Splitter_Checker());
            }
        }
    }


    //Purple attack: deletes all pieces of the selected color on the board
    public void Purplespell()
    {
        Spell_Used(PieceColor.Purple);
        spellColor = PieceColor.Purple;
        GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
        picker.GetComponent<ColorSelector>().givePurpose("Select a color to eliminate from the board");
        splitter.setState(Splitter.SplitterStates.isActive, false);
    }

    private IEnumerator Orange_Waiter()
    {
        yield return new WaitForSeconds(1.5f);
        gameController.collapse();
        StartCoroutine(gameController.boardWaiter());
        gameController.splitter.setState(Splitter.SplitterStates.isActive, true);
        spellWorking = false;
    }

    private void PurpleHelper()
    {
        //this needed to be a coroutine so it could wait and give a proper effect
        StartCoroutine(Purple_Activator());
        PurpleEffectSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        PurpleEffectSFX.Play();
    }

    private IEnumerator Purple_Activator()
    {
        int rowLeft = 7, rowRight = 7;
        int colLeft = 0, colRight = 15;
        int tempr, tempc;
        bool leftDone = false, rightDone = false;
        bool empty = true;

        //This is probably the most ridiculous looking traversal but both sides go opposite and alternate directions
        while (!leftDone || !rightDone)
        {
            //we'll traverse the Left first. This will be one long if else chain
            if (!leftDone)
            {
                //first, check to see if the last iteration of the loop found anything, or if there was anything to start
                if (gameController.grid[rowLeft, colLeft] != null)
                {
                    empty = false;
                    gameController.grid[rowLeft, colLeft].BroadcastMessage("Activate_Purple", pickedColor1, SendMessageOptions.DontRequireReceiver);
                    tempr = rowLeft;
                    tempc = colLeft;
                }
                else
                {
                    //if there wasn't, mark it
                    tempr = -1;
                    tempc = -1;
                }
                //this needs to happen at least once, therefore a do while loop is handy
                do
                {
                    //this will alternate direction based off row, starting with the upper left
                    if (rowLeft % 2 == 1)
                    {
                        //if it hasn't hit the end of the row, iterate
                        if (colLeft < 6)
                        {
                            colLeft++;
                        }
                        else
                        {
                            //move to next row
                            rowLeft--;
                        }
                    }
                    else
                    {
                        //if it hasn't hit the end of the row, iterate
                        if (colLeft > 0)
                        {
                            colLeft--;
                        }
                        else
                        {
                            //special case
                            if (rowLeft == 0)
                            {
                                //we've hit the bottom left corner, should mark left as completed
                                leftDone = true;
                            }
                            else
                            {
                                //move to next row
                                rowLeft--;
                            }
                        }
                    }
                } while (!leftDone && gameController.grid[rowLeft, colLeft] == null);
                //if this was marked and the previous checks marked it done, mark the lastpiece so it know to run a check
                if (tempr != -1 && leftDone && rightDone)
                {
                    gameController.grid[tempr, tempc].GetComponentInChildren<Piece_Spell_Effect>().lastPiece = true;
                }
            }

            //now Right, works similarly to left but in opposite directions
            if (!rightDone)
            {
                //need to check if the current one is valid
                if (gameController.grid[rowRight, colRight] != null)
                {
                    empty = false;
                    gameController.grid[rowRight, colRight].BroadcastMessage("Activate_Purple", pickedColor1, SendMessageOptions.DontRequireReceiver);
                    tempr = rowRight;
                    tempc = colRight;
                }
                else
                {
                    //mark if it isn't
                    tempr = -1;
                    tempc = -1;
                }
                do
                {
                    //every other row has a different direction of traversal
                    if (rowRight % 2 == 1)
                    {
                        if (colRight > 9)
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
                        if (colRight < 15)
                        {
                            colRight++;
                        }
                        else
                        {
                            if (rowRight == 0)
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
                } while (!rightDone && gameController.grid[rowRight, colRight] == null);
                if (tempr != -1 && leftDone && rightDone)
                {
                    gameController.grid[tempr, tempc].GetComponentInChildren<Piece_Spell_Effect>().lastPiece = true;
                }
            }

            yield return new WaitForSeconds(0.03f);
        }

        pickedColor1 = PieceColor.Empty;
        spellColor = PieceColor.Empty;
        if (empty)
        {
            splitter.setState(Splitter.SplitterStates.isActive, true);
        }
    }
    //cyan spell: the splitter pieces turn to "bombs" which explode and destroy any pieces that come into contact with the explosion when launched
    public void Cyanspell()
    {
        //again most of the hard work is done by the piece spell effect script, this just tells it when to activate
        Spell_Used(PieceColor.Cyan);
        splitter.rightSlot.GetComponent<Piece>().isBomb = true;
        splitter.rightSlot.BroadcastMessage("Activate_Cyan", null, SendMessageOptions.DontRequireReceiver);
        splitter.leftSlot.GetComponent<Piece>().isBomb = true;
        splitter.leftSlot.BroadcastMessage("Activate_Cyan", null, SendMessageOptions.DontRequireReceiver);
    }
    //White spell: Sorts the board from rainbow down
    public void Whitespell()
    {
        Spell_Used(PieceColor.White);
        //WhiteHelper does all of the technical stuff; this just tell the pieces to play an aesthetic animation
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 16; c++)
            {
                if (gameController.grid[r, c] != null)
                {
                    gameController.grid[r, c].BroadcastMessage("Activate_White", null, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        splitter.setState(Splitter.SplitterStates.isActive, false);
        StartCoroutine(WhiteHelper());
    }

    private IEnumerator WhiteHelper()
    {
        WhiteEffectSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        WhiteEffectSFX.Play();
        yield return new WaitForSeconds(1f);
        //get all pieces on left side
        List<GameObject> leftPieces = new List<GameObject>();
        for (int r = 0; r < 8; r++)
        {
            for (int c = 7; c >= 0; c--)
            {
                //Debug.Log ("Checking position R: " + r + " C: " + c);
                if (gameController.grid[r, c] != null)
                {
                    leftPieces.Add(gameController.grid[r, c]);
                }
            }
        }
        //sort them by color, alphabetical will do
        IEnumerable<GameObject> sorter = leftPieces;
        sorter = sorter.OrderBy(colorName => colorName.GetComponent<Piece>().pieceColor);
        leftPieces = sorter.ToList();
        bool empty = leftPieces.Count < 1;
        //make sure not to game over
        int rowHeight;
        if (leftPieces.Count % 8 == 0)
        {
            rowHeight = leftPieces.Count / 8;
        }
        else
        {
            rowHeight = (leftPieces.Count / 8) + 1;
        }
        //put them back evenly in sorted over
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < rowHeight; c++)
            {
                if (leftPieces.Count == 0)
                {
                    break;
                }
                else
                {
                    leftPieces[0].GetComponent<Piece>().movePiece(new Vector2((float)(c - 8), (float)r));
                    leftPieces.RemoveAt(0);
                }
            }
        }

        //now for the right
        //get all pieces on left side
        List<GameObject> rightPieces = new List<GameObject>();
        for (int r = 0; r < 8; r++)
        {
            for (int c = 8; c < 16; c++)
            {
                //Debug.Log ("Checking position R: " + r + " C: " + c);
                if (gameController.grid[r, c] != null)
                {
                    rightPieces.Add(gameController.grid[r, c]);
                }
            }
        }
        //sort them by color, alphabetical will do
        sorter = rightPieces;
        sorter = sorter.OrderBy(colorName => colorName.GetComponent<Piece>().pieceColor);
        rightPieces = sorter.ToList();
        empty = empty && rightPieces.Count < 1;
        //make sure not to game over
        if (rightPieces.Count % 8 == 0)
        {
            rowHeight = rightPieces.Count / 8;
        }
        else
        {
            rowHeight = (rightPieces.Count / 8) + 1;
        }
        //put them back evenly in sorted over
        for (int r = 0; r < 8; r++)
        {
            for (int c = 15; c > (15 - rowHeight); c--)
            {
                if (rightPieces.Count == 0)
                {
                    break;
                }
                else
                {
                    rightPieces[0].GetComponent<Piece>().movePiece(new Vector2((float)(c - 8), (float)r));
                    rightPieces.RemoveAt(0);
                }
            }
        }

        //check the board
        gameController.recalculateBoard();
        StartCoroutine(gameController.boardWaiter());
        splitter.setState(Splitter.SplitterStates.isActive, true);
        spellWorking = false;

        if (!achievementHandler.is_Splitter_Unlocked(SplitterType.White) && !empty)
        {
            yield return new WaitForSeconds(1.25f);
            for (int r = 0; r < 8; r++)
            {
                if (gameController.grid[r, 0] != null || gameController.grid[r, 15] != null)
                {
                    yield return true;
                }
            }
            achievementHandler.Unlock_Splitter(SplitterType.White);
        }
    }

    //this is what the color selectors spawned from certain colors calls when one is selected
    public void colorSelected(PieceColor color)
    {
        switch (spellColor)
        {
            case PieceColor.Orange:
                if (pickedColor1 == PieceColor.Empty)
                {
                    pickedColor1 = color;
                    GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
                    picker.GetComponent<ColorSelector>().givePurpose("Select a color to switch with on the right side");
                    //go through left side, activate
                    for (int r = 0; r < 8; r++)
                    {
                        for (int c = 7; c >= 0; c--)
                        {
                            if (gameController.grid[r, c] != null)
                            {
                                if (gameController.colorGrid[r, c] == pickedColor1)
                                {
                                    gameController.grid[r, c].BroadcastMessage("Activate_Orange", "left", SendMessageOptions.DontRequireReceiver);
                                }
                            }
                        }
                    }
                }
                else if (pickedColor2 == PieceColor.Empty)
                {
                    pickedColor2 = color;
                    for (int r = 0; r < 8; r++)
                    {
                        for (int c = 8; c < 16; c++)
                        {
                            //Debug.Log ("Checking position R: " + r + " C: " + c);
                            if (gameController.grid[r, c] != null)
                            {
                                if (gameController.colorGrid[r, c] == pickedColor2)
                                {
                                    gameController.grid[r, c].BroadcastMessage("Activate_Orange", pickedColor2.ToString(), SendMessageOptions.DontRequireReceiver);
                                }
                            }
                        }
                    }
                    OrangeHelper();
                }
                break;
            case PieceColor.Green:
                selectedPiece.BroadcastMessage("Activate_Green", color, SendMessageOptions.DontRequireReceiver);
                if (spellLimit == 1)
                {
                    selectedPiece.GetComponentInChildren<Piece_Spell_Effect>().lastPiece = true;
                }

                GreenHelper();
                break;
            case PieceColor.Blue:
                selectedPiece.BroadcastMessage("Activate_Blue", color, SendMessageOptions.DontRequireReceiver);
                if (spellLimit == 1)
                {
                    selectedPiece.GetComponentInChildren<Piece_Spell_Effect>().lastPiece = true;
                }

                BlueHelper();
                break;
            case PieceColor.Purple:
                pickedColor1 = color;
                PurpleHelper();
                break;
        }
    }

    //this is for score bits adding to the spells
    public void addBit(PieceColor colorOfBit)
    {
        if (spellActive || colorOfBit == PieceColor.Empty)
        {
            return;
        }

        SpellProgress[(int)colorOfBit] = SpellProgress[(int)colorOfBit] + 1;
        if (SpellReady[(int)colorOfBit] || 
            SpellProgress[(int)colorOfBit] >= SpellGoal[(int)colorOfBit])
        {
            SpellProgress[(int)colorOfBit] = SpellGoal[(int)colorOfBit];
            SpellReady[(int)colorOfBit] = true;
            SpellText[(int)colorOfBit].text = "100%";
            SpellText[(int)colorOfBit].gameObject.BroadcastMessage("FadeOut", null, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            SpellText[(int)colorOfBit].text = ((int)(((float)SpellProgress[(int)colorOfBit] / (float)SpellGoal[(int)colorOfBit]) * 100f)) + "%";
        }

        switch (colorOfBit)
        {
            case PieceColor.Orange:
                if (!achievementHandler.is_Splitter_Unlocked(SplitterType.Green) && spellsUsed[3] && gameController.availableCount < 6)
                {
                    achievementHandler.Unlock_Splitter(SplitterType.Green);
                }
                break;
            case PieceColor.Cyan:
                if (!achievementHandler.is_Splitter_Unlocked(SplitterType.Green) && spellsUsed[3] && gameController.availableCount < 7)
                {
                    achievementHandler.Unlock_Splitter(SplitterType.Green);
                }
                break;
            case PieceColor.White:
                if (!achievementHandler.is_Splitter_Unlocked(SplitterType.Green) && spellsUsed[3] && gameController.availableCount < 8)
                {
                    achievementHandler.Unlock_Splitter(SplitterType.Green);
                }
                break;
        }
    }

    //Call whenever a spell is used. This handles the stuff every spell does.
    private void Spell_Used(PieceColor spellColor)
    {
        spellsUsed[(int)spellColor] = true;
        if (!achievementHandler.is_Pieceset_Unlocked(PieceSets.Arcane))
        {
            for (int i = 0; i < 8; i++)
            {
                if (!spellsUsed[i])
                {
                    break;
                }
                else if (i == 7 && gameController.gameMode == GameMode.Wiz)
                {
                    achievementHandler.Unlock_Pieceset(PieceSets.Arcane);
                }
            }
        }

        spellActive = true;
        splitsNeeded = 1;

        spellWorking = true;

        SpellReady[(int)spellColor] = false;
        SpellProgress[(int)spellColor] = 0;
        SpellGoal[(int)spellColor] = (int)(SpellGoal[(int)spellColor] * chargeMultiplier);
        SpellText[(int)spellColor].text = "0%";
        SpellText[(int)spellColor].gameObject.BroadcastMessage("FadeIn", null, SendMessageOptions.DontRequireReceiver);
    }

    //called when the splitter splits. Is used to tell spellhandler to deactive spellActive for scorebits to begin charging
    public void split()
    {
        if (spellActive)
        {
            splitsNeeded--;
            if (splitsNeeded <= 0)
            {
                spellActive = false;
            }
        }

    }
    //this is solely for achievements to track which spells have been used in this game
    public bool Used_Spells()
    {
        foreach (bool spell in spellsUsed)
        {
            if (spell)
            {
                return true;
            }
        }
        return false;
    }

}