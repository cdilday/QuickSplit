using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{

    //This script handles controlling the splitter and its states

    //check this if trying to emulate mobile controls on computer
    public bool mobileDebugging;

    public enum SplitterStates
    {
        isMoving = 0,
        canShoot = 1,
        isActive = 2,
        mouseControl = 3,
        touchControl = 4,
        inTransition = 5,
        yellowReady = 6,
    }

    public class State
    {
        public bool isMoving; // checks if the splitter is in the middle of moving to the next grid spot
        public bool canShoot;
        public bool isActive;
        public bool mouseControl;
        public bool touchControl;
        public bool inTransition;
        public bool yellowReady;
    }

    private State splitState = new State();

    public int moveDirection; // 1 if it's moving upwards, -1 if downwards, 0 if not currently moving
    private int moveTarget;
    private float moveStartTime;
    private const float moveDuration = 0.08f;
    private float speed = 1;

    //prefabs containing all the different colored pieces
    public Transform[] pieces;

    //pieces currently in the splitter
    public Transform leftSlot;
    public Transform rightSlot;
    private Vector3 mouseLocation;

    public bool overrideControlType;
    public string controlType;

    //this is used for follow controls so the fingerID's startTimes don't get messed up
    private Dictionary<int, float> idStartTimes;

    // same but for if it's been moved or not
    private Dictionary<int, Vector2> idStartPos;

    //this just defines if it was a tap or not
    private Dictionary<int, bool> idIsTap;

    //for swipes
    private Dictionary<int, bool> idIsSwipe;

    //for drag
    private Dictionary<int, bool> idIsDrag;

    //objects the splitter will need to use
    public Holder_Script holder;
    public GameController gameController;
    public Camera mainCamera;
    private Piece_Sprite_Holder spriteHolder;
    private AudioSource FireSFX;

    // Use this for initialization
    private void Start()
    {
        //get the gamecontroller
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        //get the holder
        GameObject holderObject = GameObject.FindWithTag("Holder");
        if (holderObject != null)
        {
            holder = holderObject.GetComponent<Holder_Script>();
        }
        //start the game with random pieces in the holder
        int left = Random.Range(0, gameController.availableCount);
        int right = Random.Range(0, gameController.availableCount);
        leftSlot = Instantiate(pieces[left], new Vector2(-1, transform.position.y), Quaternion.identity) as Transform;
        rightSlot = Instantiate(pieces[right], new Vector2(0, transform.position.y), Quaternion.identity) as Transform;
        leftSlot.GetComponent<Piece>().inSplitter = true;
        rightSlot.GetComponent<Piece>().inSplitter = true;
        if (gameController.gameMode != GameMode.Quick)
        {
            splitState.canShoot = true;
        }
        else
        {
            splitState.canShoot = false;
        }
        splitState.isActive = true;
        splitState.inTransition = false;
        splitState.yellowReady = false;

        gameObject.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder>().Get_Splitter();
        FireSFX = GetComponent<AudioSource>();

        if (!overrideControlType)
        {
            controlType = PlayerPrefs.GetString("Controls", "Follow");
        }
        else
        {
            PlayerPrefs.SetString("Controls", controlType);
        }

        if (controlType == "Follow")
        {
            idStartTimes = new Dictionary<int, float>();
            idStartPos = new Dictionary<int, Vector2>();
            idIsTap = new Dictionary<int, bool>();
            idIsSwipe = new Dictionary<int, bool>();
            idIsDrag = new Dictionary<int, bool>();
            speed = 2;
        }
        else
        {
            speed = 1;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!splitState.isActive || gameController.isPaused || gameController.gameOver)
        {
            return;
        }
        //player Input
        //checks if the player is playing on a mobile phone, if not activate mouse control
        if (!Application.isMobilePlatform && !mobileDebugging)
        {
            //uncomment to constantly see mouse position.
            //Debug.Log ("Mouse Position: X:" + Input.mousePosition.x + "    Y: " + Input.mousePosition.y);
            mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log ("Mouse Position: X:" + mouseLocation.x + "    Y: " + mouseLocation.y);
            //checking that the mouse is within the grid
            if (mouseLocation.x <= 7 && mouseLocation.x >= -8 && mouseLocation.y >= -0.5 && mouseLocation.y <= 7.5)
            {
                if ((mouseLocation.y > transform.position.y + 0.5f) && !splitState.isMoving && transform.position.y < 7)
                {
                    MoveUp();
                }
                //Moving downwards if the mouse is below the splitter's hitbox
                if ((mouseLocation.y < transform.position.y - 0.5f) && !splitState.isMoving && transform.position.y > 0)
                {
                    MoveDown();
                }
                //Swapping pieces with right click if on PC
                if (Input.GetMouseButtonDown(1))
                {
                    swap();
                }
                //launching pieces with Left click while over the board
                else if (Input.GetMouseButtonDown(0) && moveDirection == 0 && rightSlot != null && leftSlot != null && splitState.canShoot
                         && !splitState.inTransition && splitState.isActive)
                {
                    if (splitState.yellowReady == true)
                    {
                        GameObject.Find("Spell Handler").BroadcastMessage("YellowActivate");
                    }
                    else
                    {
                        StartCoroutine(fire());
                        splitState.canShoot = false;
                        gameController.movesMade++;
                        gameController.updateMoves();
                    }
                }
            }
        }

        //moving upwards with keys W or Up
        if ((Input.GetKey("w") || Input.GetKey("up")) && !splitState.isMoving && transform.position.y < 7)
        {
            MoveUp();
        }
        //moving downwards with keys S or Down
        if ((Input.GetKey("s") || Input.GetKey("down")) && !splitState.isMoving && transform.position.y > 0)
        {
            MoveDown();
        }
        //swapping pieces with keys A, D, Left, or Right
        if (Input.GetKeyDown("a") || Input.GetKeyDown("d") || Input.GetKeyDown("left") || Input.GetKeyDown("right"))
        {
            swap();
        }
        //launching pieces with key Space
        if (Input.GetKeyDown("space") && moveDirection == 0 && rightSlot != null && leftSlot != null && splitState.canShoot && !splitState.inTransition)
        {
            if (splitState.yellowReady == true)
            {
                GameObject.Find("Spell Handler").BroadcastMessage("YellowActivate");
            }
            else
            {
                StartCoroutine(fire());
                splitState.canShoot = false;
                gameController.movesMade++;
                gameController.updateMoves();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            splitState.inTransition = false;
        }
        //}//ending bracket for mouse/keyboard exclusivity

        //some debug keys
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            gameController.addSideColumns();
        }

    }

    private void FixedUpdate()
    {
        //checks if the splitter is currently between grid movement. 
        if (splitState.isMoving)
        {
            //check to see if the movement time is up. If it is, put it to it's proper location
            if (Mathf.Abs(moveStartTime - Time.time) > (moveDuration / speed))
            {
                moveDirection = 0;
                splitState.isMoving = false;
                transform.position = new Vector3(transform.position.x, moveTarget, transform.position.z);
                gameObject.BroadcastMessage("Stopping", null, SendMessageOptions.DontRequireReceiver);
            }
            else //visually move it
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (moveDirection * (0.25f * speed)), transform.position.z);
            }
        }

        //keps the stored pieces in the correct positions in the splitter
        if (leftSlot != null && rightSlot != null)
        {
            leftSlot.transform.position = new Vector3(-1, transform.position.y, 0);
            rightSlot.transform.position = new Vector3(0, transform.position.y, 0);
        }

        //bugfix to ensure that both slots are full
        if (leftSlot == null && rightSlot != null)
        {
            leftSlot = Instantiate(pieces[Random.Range(0, gameController.availableCount)], new Vector2(-1, transform.position.y), Quaternion.identity) as Transform;
        }
        if (rightSlot == null && leftSlot != null)
        {
            rightSlot = Instantiate(pieces[Random.Range(0, gameController.availableCount)], new Vector2(0, transform.position.y), Quaternion.identity) as Transform;
        }

        //checks if it's reached its next spot by seeing if it's y position is a whole number. 
        if (splitState.isMoving && transform.position.y % 1 == 0)
        {
            moveDirection = 0;
        }
    }

    private void MoveUp()
    {
        //first check to make sure it's possible
        if (splitState.isMoving || transform.position.y >= 6.9f)
        {
            return;
        }

        int currentLoc = (int)transform.position.y;
        moveTarget = currentLoc + 1;

        moveDirection = 1;
        moveStartTime = Time.time;
        splitState.isMoving = true;
    }

    private void MoveDown()
    {
        //first check to make sure it's possible
        if (splitState.isMoving || transform.position.y <= 0.1f)
        {
            return;
        }

        int currentLoc = (int)transform.position.y;
        moveTarget = currentLoc - 1;

        moveDirection = -1;
        moveStartTime = Time.time;
        splitState.isMoving = true;
    }
    //swaps the left and right slot
    public void swap()
    {
        if (leftSlot != null && rightSlot != null)
        {
            Transform temp = leftSlot;
            leftSlot = rightSlot;
            rightSlot = temp;
            leftSlot.transform.position = new Vector2(-1, transform.position.y);
            rightSlot.transform.position = new Vector2(0, transform.position.y);
        }
    }

    //shoots the pieces in the correct directions
    public IEnumerator fire()
    {
        FireSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        FireSFX.Play();
        //tell the wedges that it has fired
        gameObject.BroadcastMessage("Has_Fired", null, SendMessageOptions.DontRequireReceiver);
        GameObject tempSH = GameObject.Find("Spell Handler");
        if (tempSH != null)
        {
            tempSH.BroadcastMessage("split", null, SendMessageOptions.DontRequireReceiver);
        }

        leftSlot.GetComponent<Piece>().inSplitter = false;
        rightSlot.GetComponent<Piece>().inSplitter = false;
        Transform lefttemp = leftSlot;
        Transform righttemp = rightSlot;
        leftSlot = null;
        rightSlot = null;
        lefttemp.GetComponent<Rigidbody2D>().velocity = new Vector2(-20f, 0);
        righttemp.GetComponent<Rigidbody2D>().velocity = new Vector2(20f, 0);
        yield return new WaitForSeconds(0.07f);
        refill();
    }

    //refills the splitter with two new pieces
    public void refill()
    {
        holder.getNextPiece();
        Piece[] slots = new Piece[2] { leftSlot.GetComponent<Piece>(), rightSlot.GetComponent<Piece>() };
        for (int i = 0; i < 2; i++)
        {
            slots[i].locked = false;
            slots[i].inHolder = false;
            slots[i].inSplitter = true;
        }
    }

    public State getState()
    {
        return splitState;
    }

    public bool getState(SplitterStates state)
    {
        switch (state)
        {
            case SplitterStates.isMoving:
                return splitState.isMoving;
            case SplitterStates.canShoot:
                return splitState.canShoot;
            case SplitterStates.isActive:
                return splitState.isActive;
            case SplitterStates.mouseControl:
                return splitState.mouseControl;
            case SplitterStates.touchControl:
                return splitState.touchControl;
            case SplitterStates.inTransition:
                return splitState.inTransition;
            case SplitterStates.yellowReady:
                return splitState.yellowReady;
        }
        Debug.LogError("State error: no state of name " + name + " detected.");

        return false;
    }

    public bool setState(SplitterStates state, bool value)
    {
        switch (state)
        {
            case SplitterStates.isMoving:
                splitState.isMoving = value;
                return true;
            case SplitterStates.canShoot:
                splitState.canShoot = value;
                return true;
            case SplitterStates.isActive:
                splitState.isActive = value;
                return true;
            case SplitterStates.mouseControl:
                splitState.mouseControl = value;
                return true;
            case SplitterStates.touchControl:
                splitState.touchControl = value;
                return true;
            case SplitterStates.inTransition:
                splitState.inTransition = value;
                return true;
            case SplitterStates.yellowReady:
                splitState.yellowReady = value;
                return true;
        }

        Debug.LogError("State error: no state of name " + state + " detected.");
        return false;
    }
}
