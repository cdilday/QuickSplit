using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour
{
    //This script handles the color selecor object as a whole as used by certain spells

    public Text selectionText;
    private Splitter splitter;
    private SpellHandler spellHandler;

    // Use this for initialization
    private void Awake()
    {
        selectionText = GameObject.Find("Color Selector Text").GetComponent<Text>();
        //selectionText.pixelOffset = new Vector2 (Screen.width / 2f, Screen.height / 2f);
        splitter = GameObject.FindGameObjectWithTag("Splitter").GetComponent<Splitter>();
        splitter.setState(Splitter.SplitterStates.isActive, false);
        spellHandler = GameObject.Find("Spell Handler").GetComponent<SpellHandler>();
    }

    /// <summary>
    /// Fills in the player-facing text for the color selector, explaining why they're selecting the color
    /// </summary>
    /// <param name="purpose"> string - text you want the player to see in the color selector</param>
    public void givePurpose(string purpose)
    {
        selectionText.text = purpose;
    }

    /// <summary>
    /// called when the player selects a color, and passes that to the SpellHandler
    /// </summary>
    /// <param name="color"> PieceColor - color that was selected by the player</param>
    public void colorSelected(PieceColor color)
    {
        spellHandler.colorSelected(color);
        splitter.setState(Splitter.SplitterStates.inTransition, true);
        selectionText.text = "";
        Destroy(gameObject);
    }

    private void onDestroy()
    {
        selectionText.text = "";
    }

}