using UnityEngine;

public class Guide : MonoBehaviour
{

    //this script handles the guide circles on the splitter, and is attatched to each one individually

    private bool isRight;
    private Splitter splitter;
    private GameController gameController;
    private string pieceColor;
    private SpriteRenderer spriteRenderer;


    // Use this for initialization
    private void Start()
    {
        //get rid of the guide piece if the player has opted out on the option screen
        if (PlayerPrefs.GetInt("Guide", 1) == 0)
        {
            Destroy(gameObject);
            return;
        }

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        splitter = GameObject.FindGameObjectWithTag("Splitter").GetComponent<Splitter>();

        //assign which side it's on
        if (transform.localPosition.x > 0)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isRight)
        {
            if (splitter.rightSlot)
            {
                spriteRenderer.sprite = splitter.rightSlot.GetComponent<SpriteRenderer>().sprite;
            }

            //check which row/column it should be in relative to the grid info in the gamecontroller
            int row = (int)transform.position.y;
            if (gameController.grid[row, 9] != null)
            {
                spriteRenderer.color = new Color(1f, 0.5f, 0f, 0);
            }
            else
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.4f);
                bool hitEnd = true;
                for (int c = 10; c < 16; c++)
                {
                    if (gameController.grid[row, c] != null)
                    {
                        transform.localPosition = new Vector2((float)c - 8.5f, transform.localPosition.y);
                        hitEnd = false;
                        break;
                    }
                }
                //if it's hit the end, put it in the final columns
                if (hitEnd)
                {
                    transform.localPosition = new Vector2(7.5f, transform.localPosition.y);
                }
            }
        }
        //similar to right side, but just on the left
        else
        {
            if (splitter.leftSlot)
            {
                spriteRenderer.sprite = splitter.leftSlot.GetComponent<SpriteRenderer>().sprite;
            }

            int row = (int)transform.position.y;
            if (gameController.grid[row, 6] != null)
            {
                spriteRenderer.color = new Color(1f, 0.5f, 0f, 0);
            }
            else
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.25f);
                bool hitEnd = true;
                for (int c = 5; c >= 0; c--)
                {
                    if (gameController.grid[row, c] != null)
                    {
                        transform.localPosition = new Vector2((float)c - 6.5f, transform.localPosition.y);
                        hitEnd = false;
                        break;
                    }
                }
                if (hitEnd)
                {
                    transform.localPosition = new Vector2(-7.5f, transform.localPosition.y);
                }
            }
        }
    }

}