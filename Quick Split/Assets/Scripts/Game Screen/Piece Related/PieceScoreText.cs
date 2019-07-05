using UnityEngine;
using UnityEngine.UI;

public class PieceScoreText : MonoBehaviour
{

    //this script is attatched to piece score text objecs that pop up when pieces are cleared and handles all that

    public PieceColor pieceColor;
    public Color textColor;
    public int scoreValue = 1;
    private Text text;
    private int liveCount = 180;

    // Use this for initialization
    private void Start()
    {
        text = GetComponent<Text>();
        textColor = Piece.PieceColorValues[(int)pieceColor];
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        text.text = "" + scoreValue;
        text.color = textColor;
        liveCount--;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.001f);
        if (liveCount % 20 == 0 && liveCount >= 60)
        {
            if (textColor.a == 1f)
            {
                textColor = new Color(textColor.r, textColor.g, textColor.b, 0.5f);
            }
            else
            {
                textColor = new Color(textColor.r, textColor.g, textColor.b, 1f);
            }
        }

        if (liveCount < 60)
        {
            textColor = new Color(textColor.r, textColor.g, textColor.b, 1f * (liveCount / 60f));
        }

        if (liveCount <= 0)
        {
            Destroy(transform.gameObject);
        }
    }

}