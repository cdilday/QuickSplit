using UnityEngine;

public class SpellText : MonoBehaviour
{
    //this script handles the spell progress text on the bottom of the game screen

    public string spellColor;
    private GameObject description;
    private Vector3 startPos;

    private void Start()
    {
        description = transform.GetChild(0).gameObject;
        startPos = description.transform.position;
        description.transform.position = new Vector3(startPos.x + 20, startPos.y, startPos.z);
    }

    private void OnMouseOver()
    {
        description.transform.position = startPos;
    }

    private void OnMouseExit()
    {
        description.transform.position = new Vector3(startPos.x + 20, startPos.y, startPos.z);
    }

}