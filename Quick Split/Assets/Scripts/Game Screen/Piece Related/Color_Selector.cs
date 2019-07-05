using UnityEngine;
using UnityEngine.UI;

public class Color_Selector : MonoBehaviour
{

    //This script handles the color selecor object as a whole as used by certain spells

    public Text selectionText;
    private Splitter_script splitter;
    private SpellHandler spellHandler;

    // Use this for initialization
    private void Awake()
    {
        selectionText = GameObject.Find("Color Selector Text").GetComponent<Text>();
        //selectionText.pixelOffset = new Vector2 (Screen.width / 2f, Screen.height / 2f);
        splitter = GameObject.FindGameObjectWithTag("Splitter").GetComponent<Splitter_script>();
        splitter.setState("isActive", false);
        spellHandler = GameObject.Find("Spell Handler").GetComponent<SpellHandler>();
    }

    public void givePurpose(string purpose)
    {
        selectionText.text = purpose;
    }

    public void colorSelected(PieceColor color)
    {
        spellHandler.colorSelected(color);
        splitter.setState("inTransition", true);
        selectionText.text = "";
        Destroy(gameObject);
    }

    private void onDestroy()
    {
        selectionText.text = "";
    }

}