using UnityEngine;

public enum PieceColor
{
    Empty = -1,
    Red = 0,
    Orange = 1,
    Yellow = 2,
    Green = 3,
    Blue = 4,
    Purple = 5,
    Cyan = 6,
    White = 7,
}

public class Piece : MonoBehaviour
{
    //this script is attatched to every piece and handles their movement, state, and functions

    #region Static

    public static readonly string[] PieceColorString =
    {
        "Red",
        "Orange",
        "Yellow",
        "Green",
        "Blue",
        "Purple",
        "Cyan",
        "White",
    };

    public static readonly Color[] PieceColorValues =
    {
            Color.red,
            new Color(1f, 0.5f, 0f),
            Color.yellow,
            Color.green,
            Color.blue,
            new Color(0.6f, 0, 0.6f),
            Color.cyan,
            Color.white,
    };

    #endregion

    public PieceColor pieceColor;
    public bool inSplitter;
    public bool inHolder;
    public bool inSideHolder;
    public bool isBomb;

    public GameObject scoreTextPrefab;

    public bool locked;
    public Vector2 lockPos;
    public Vector2 gridPos;
    private Vector2 prevPos;
    private Vector2 moveToPos;
    private int moveProgress;
    private float moveStepx;
    private float moveStepy;
    private bool isMoving;

    public bool selectable = false;

    public Sprite[] sprites = new Sprite[8];
    //value assigned to each piece that shows how many pieces are in a group of adjacent stuff.
    public int groupValue;

    //stores multiplier to reflect accurate score;
    public float multiplier;

    public GameController gameController;

    public Splitter splitter;

    public SpellHandler spellHandler;
    private BitPool BitPool;
    private GameObject clacker;
    private PieceSplitterAssetHelper spriteHolder;
    private RuntimeAnimatorController[] animations;
    private int prevColorNum;
    private bool hasPlayedAnim;
    private float animStartTime;

    // Use this for initialization
    private void Start()
    {
        isBomb = false;
        isMoving = false;
        groupValue = 1;
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        GameObject splitterObject = GameObject.Find("Splitter");
        if (splitterObject != null)
        {
            splitter = splitterObject.GetComponent<Splitter>();
        }
        GameObject spellHandlerObject = GameObject.Find("Spell Handler");
        if (spellHandlerObject != null)
        {
            spellHandler = spellHandlerObject.GetComponent<SpellHandler>();
        }
        //set grid position to -3,-3 until it's locked to prevent accidental cancelling.
        gridPos = new Vector2(-3, -3);

        GameObject BitPoolObject = GameObject.Find("Bit Pool");
        if (BitPoolObject == null)
        {
            Debug.LogError("Piece Error: Cannot find the Bit Pool");
        }
        else
        {
            BitPool = BitPoolObject.GetComponent<BitPool>();
        }

        clacker = GameObject.Find("Clacker");

        //time to set up piece visuals
        spriteHolder = GameObject.Find("Piece Sprite Holder").GetComponent<PieceSplitterAssetHelper>();
        sprites = spriteHolder.GetSprites();
        animations = spriteHolder.GetPieceSetAnimations();
        if (animations == null)
        {
            Destroy(gameObject.GetComponent<Animator>());
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)pieceColor];
        if (animations != null)
        {
            gameObject.GetComponent<Animator>().runtimeAnimatorController = animations[(int)pieceColor];
        }

        prevColorNum = ((int)Time.time) % 8;
        //multiplier = 1;
        hasPlayedAnim = false;
    }

    private void FixedUpdate()
    {
        //this code is to ensure collisions don't offset piece's individual positions
        bool changedPos = false;
        if (transform.position.x != prevPos.x || transform.position.y != prevPos.y)
        {
            changedPos = true;
            prevPos = transform.position;
        }
        if (!isMoving && locked && lockPos != prevPos)
        {
            transform.position = lockPos;
            if (!inHolder && !inSideHolder)
            {
                gridPos = new Vector2((int)lockPos.y, (int)lockPos.x + 8);
            }
        }

        if (isMoving && moveProgress < 10)
        {
            transform.position = new Vector2(transform.position.x + (moveStepx), transform.position.y + (moveStepy));
            moveProgress++;
        }
        else if (isMoving)
        {
            transform.position = moveToPos;
            isMoving = false;
        }

        if (changedPos)
        {
            if (!inSplitter && !inHolder)
            {
                this.name = pieceColor + " piece (" + gridPos.x + ", " + gridPos.y + ")";
            }
            else if (inHolder)
            {
                this.name = pieceColor + " in Holder";
            }
            else if (inSplitter)
            {
                this.name = pieceColor + " in Splitter";
            }
        }

        //animations
        if (animations != null)
        {
            if (((int)Time.time) % 8 != prevColorNum && ((int)Time.time) % 8 == (int)pieceColor && !hasPlayedAnim)
            {
                gameObject.GetComponent<Animator>().SetBool("isPlaying", true);
                hasPlayedAnim = true;
                animStartTime = Time.time;
            }
            else if (hasPlayedAnim && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + animStartTime < Time.time)
            {
                gameObject.GetComponent<Animator>().SetBool("isPlaying", false);
                hasPlayedAnim = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)pieceColor];
            }

            prevColorNum = ((int)Time.time) % 8;
        }
    }

    //2D collision detection
    private void OnTriggerEnter2D(Collider2D col)
    {
        //ignore score bits
        if (col.gameObject.tag == "Score Bit" || col.gameObject.tag == "Spell Tab")
        {
            return;
        }
        //if the piece hasn't already been assigned a position, begin to assign it
        if (!isMoving && !locked && !inSplitter && !inSideHolder)
        {
            Piece colPiece = col.gameObject.GetComponent<Piece>();
            //if it collided with the side of a grid, place it in the grid
            if (colPiece == null)
            {
                transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                GetComponent<Rigidbody2D>().isKinematic = false;
                locked = true;
                lockPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
                transform.position = lockPos;
                gridPos = new Vector2((int)lockPos.y, (int)lockPos.x + 8);
                gameController.placePiece(gameObject);
                clacker.BroadcastMessage("PlaySound");
            }
            //if it collided with another piece, determine where that piece is and place it relative to that piece
            else if (colPiece.locked == true && !colPiece.inSplitter)
            {
                //check if it was fired left
                if (transform.GetComponent<Rigidbody2D>().velocity.x < 0 && !colPiece.inSplitter && !inSplitter)
                {
                    transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    GetComponent<Rigidbody2D>().isKinematic = false;
                    locked = true;
                    lockPos = new Vector2(Mathf.Round(col.transform.position.x + 1), Mathf.Round(col.transform.position.y));
                    transform.position = lockPos;
                    gridPos = new Vector2((int)lockPos.y, (int)lockPos.x + 8);
                }
                //check if it was fired right
                else if (transform.GetComponent<Rigidbody2D>().velocity.x > 0 && !colPiece.inSplitter && !inSplitter)
                {
                    transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    GetComponent<Rigidbody2D>().isKinematic = false;
                    locked = true;
                    lockPos = new Vector2(Mathf.Round(col.transform.position.x - 1), Mathf.Round(col.transform.position.y));
                    transform.position = lockPos;
                    gridPos = new Vector2((int)lockPos.y, (int)lockPos.x + 8);
                }
                //places the piece in the grid upkept by the game controller
                gameController.placePiece(gameObject);
                clacker.BroadcastMessage("PlaySound");
            }
            if (isBomb)
            {
                gameObject.BroadcastMessage("ActivateCyan", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    //takes in a vector2 for the new location and does the appropriate changes
    public void movePiece(Vector2 newLoc)
    {
        moveStepx = (newLoc.x - transform.position.x) / 10f;
        moveStepy = (newLoc.y - transform.position.y) / 10f;
        isMoving = true;
        locked = false;
        moveToPos = newLoc;
        moveProgress = 0;
        lockPos = newLoc;
        // the strange vector2 is because the grid has no negatives and the x/y are switched
        gridPos = new Vector2(newLoc.y, newLoc.x + 8);
        locked = true;

        //update the gamecontroller's grid
        gameController.grid[(int)gridPos.x, (int)gridPos.y] = gameObject;
        gameController.colorGrid[(int)gridPos.x, (int)gridPos.y] = pieceColor;
    }

    public void ConvertColor(PieceColor newColor)
    {
        if (newColor == pieceColor)
        {
            return;
        }

        pieceColor = newColor;

        if (!inHolder && !inSplitter)
        {
            gameController.colorGrid[(int)gridPos.x, (int)gridPos.y] = newColor;
        }

        if (animations != null)
        {
            gameObject.GetComponent<Animator>().runtimeAnimatorController = animations[(int)pieceColor];
            gameObject.GetComponent<Animator>().SetBool("isPlaying", true);
            hasPlayedAnim = true;
            animStartTime = Time.time;
        }

        //make sure to change the color of the pulser as well
        PiecePulseEffect piecePulser = null;
        if (GetComponentInChildren<PiecePulseEffect>())
        {
            piecePulser = GetComponentInChildren<PiecePulseEffect>();
            piecePulser.spriteRenderer.sprite = sprites[(int)pieceColor];
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)pieceColor];
    }

    private void OnDestroy()
    {
        //this means that the game just ended, don't spawn stuff
        if (gameController.isQuitting || gameController.gameOver)
        {
            return;
        }
        //tell the score canvas to create a score text at this given location
        Vector2 spawnPoint = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToViewportPoint(transform.position);
        int scoreValue;
        if (multiplier != 0)
        {
            scoreValue = (int)(groupValue * multiplier);
        }
        else
        {
            scoreValue = groupValue;
        }

        gameController.Score_Text_Canvas.GetComponent<Score_Text_Layer>().Spawn_Score_Text(spawnPoint, pieceColor, scoreValue);

        BitPool.spawn_bits(scoreValue, transform.position, pieceColor);

    }

    private void OnMouseOver()
    {
        if (selectable && Input.GetMouseButtonDown(0))
        {
            if ((spellHandler.spellColor == PieceColor.Green || spellHandler.spellColor == PieceColor.Blue))
            {
                //check to see if they've already tapped a piece
                if (GameObject.Find("Color Selector(Clone)") == null)
                {
                    GameObject picker = (GameObject)Instantiate(Resources.Load("Color Selector"));
                    picker.GetComponent<ColorSelector>().givePurpose("Select a color to change this piece to");
                    spellHandler.selectedPiece = this;
                    selectable = false;
                }
                else
                {
                    //allow them to switch if they have
                    spellHandler.selectedPiece.selectable = true;
                    spellHandler.selectedPiece = this;
                    selectable = false;
                }
            }
        }
    }

}