using UnityEngine;
using UnityEngine.UI;

public class PieceColorer : MonoBehaviour
{
    //This is used to recolor pieces as needed

    public PieceColor pieceColor;
    private Sprite[] sprites = new Sprite[8];

    public PieceSplitterAssetHelper spriteHolder;

    // Use this for initialization
    private void Start()
    {
        spriteHolder = GameObject.Find("Piece Sprite Holder").GetComponent<PieceSplitterAssetHelper>();
        updateColor();
    }

    /// <summary>
    /// Checks what color the piece should be, and updates it to fit
    /// </summary>
    public void updateColor()
    {
        sprites = spriteHolder.GetSprites();
        if (gameObject.GetComponent<SpriteRenderer>())
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)pieceColor];
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = sprites[(int)pieceColor];
        }
    }

}