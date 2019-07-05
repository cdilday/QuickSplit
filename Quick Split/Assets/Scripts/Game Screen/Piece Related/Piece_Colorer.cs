using UnityEngine;
using UnityEngine.UI;

public class Piece_Colorer : MonoBehaviour
{

    //This is used to recolor pieces as needed

    public PieceColor pieceColor;
    private Sprite[] sprites = new Sprite[8];

    public Piece_Sprite_Holder spriteHolder;

    // Use this for initialization
    private void Start()
    {
        spriteHolder = GameObject.Find("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder>();
        updateColor();
    }

    //updates the color based on what color it is
    public void updateColor()
    {
        sprites = spriteHolder.Get_Sprites();
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