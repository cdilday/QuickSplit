using UnityEngine;

public class Selectable_Color : MonoBehaviour
{

    //This script is part of the color Selector game object that pops up during certain spells

    public PieceColor pieceColor;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponentInParent<Color_Selector>().colorSelected(pieceColor);
        }
    }

}
