using UnityEngine;
using UnityEngine.UI;

public class MultiplierDisplayer : MonoBehaviour
{
    //This displays the current multiplier on the Game Scene

    private GameController gameController;
    private Text text;

    // Use this for initialization
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        text.text = "x" + gameController.multiplier;
    }

}