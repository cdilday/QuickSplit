using UnityEngine;

public class SideColumn : MonoBehaviour
{

    //This Script handles the sidecolumns that contain the pieces to be moved in on the sides of the grid

    //List of piece prefabs to randomize from
    public GameObject[] pieces;

    //will track if it's left or right, then keep its proper X value
    public bool isRight;
    public int sideInt;
    private float sideXValue;

    public Vector2 permPosition;
    private float shakeValue = 0.03f;
    public int shakeStage = 0;
    public bool isShaking;
    public bool ready;

    // for moving the pieces closer and how many moves are required before adding this row
    public int stepValue;

    //Will contain the gameobjects, similar to the grids in the gamecontroller
    public GameObject[] column = new GameObject[8];
    public PieceColor[] colorColumn = new PieceColor[8];

    public GameController gameController;

    // Use this for initialization
    private void Start()
    {

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        // begin by poisitioning these at the right locations
        if (!isRight)
        {
            Vector2 tempPos = new Vector2(-9f, 0);
            this.transform.position = tempPos;
            sideXValue = -9f;
            sideInt = 0;
        }
        else if (isRight)
        {
            Vector2 tempPos = new Vector2(8f, 0);
            this.transform.position = tempPos;
            sideXValue = 8f;
            sideInt = 1;
        }

        //now load it up
        empty();
        reload();
        isShaking = false;
        shakeStage = 0;
        ready = false;
        permPosition = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isShaking)
        {
            shake();
        }
        else if (ready)
        {
            pulse();
        }
        else
        {
            transform.position = permPosition;
        }
    }

    //empties the column, ideally after the objects have been taken from it already
    public void empty()
    {
        for (int row = 0; row < 8; row++)
        {
            column[row] = null;
            colorColumn[row] = PieceColor.Empty;
        }
    }

    public void shake()
    {
        //use shake stage to make a sense of progression.
        transform.position = new Vector2(Random.Range(-1 * shakeStage * shakeValue, shakeStage * shakeValue) + permPosition.x,
                                          Random.Range(-1 * shakeStage * shakeValue, shakeStage * shakeValue) + permPosition.y);
    }

    public void pulse()
    {
        //pulse will make the columns pulse around the grid signify they're ready to enter
        if (sideInt == 0)
        {
            transform.position = new Vector2((Mathf.Sin(Time.time * 4) * 0.25f) + (permPosition.x - 0.25f), permPosition.y);
        }
        else if (sideInt == 1)
        {
            transform.position = new Vector2((Mathf.Sin((Time.time * 4) + Mathf.PI) * 0.25f) + (permPosition.x + 0.25f), permPosition.y);
        }
    }

    //reloads column after it's been taken by the grid. Only for use by the gamecontroller
    public void reload()
    {
        if (column[0] != null || colorColumn[0] != PieceColor.Empty)
        {
            Debug.LogError("Side Column Error: Trying to reload a loaded column");
            return;
        }

        isShaking = false;
        ready = false;
        transform.position = permPosition;

        for (int row = 0; row < 8; row++)
        {
            int randPiece = Random.Range(0, gameController.availableCount);
            column[row] = Instantiate(pieces[randPiece], new Vector2(sideXValue, row), Quaternion.identity) as GameObject;
            //column[row].GetComponent<piece_script> ().locked = true;
            column[row].GetComponent<Piece>().lockPos = new Vector2(sideXValue, row);
            column[row].GetComponent<Piece>().inSideHolder = true;
            column[row].transform.parent = transform;
            colorColumn[row] = column[row].GetComponent<Piece>().pieceColor;
        }
    }

}