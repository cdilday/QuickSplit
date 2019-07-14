using UnityEngine;

/// <summary>
/// Part of the color selector prefab attached to the individual pieces. Tells the selector when it's been clicked and which color
/// </summary>
public class SelectableColor : MonoBehaviour
{
    public PieceColor pieceColor;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponentInParent<ColorSelector>().colorSelected(pieceColor);
        }
    }

}
