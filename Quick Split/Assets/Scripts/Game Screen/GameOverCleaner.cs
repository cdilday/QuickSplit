using UnityEngine;

/// <summary>
/// this script deletes objects that have no use after gameover. This cleanup is required, because despite being on a layer behind
/// the game over layer, on a layer behind the pause layer, and is physically behind it according to the Z-axis, the spell canvas 
/// draws infront of it during game over. Thanks Unity.
/// </summary>
public class GameOverCleaner : MonoBehaviour
{


    private GameController gameController;

    // Use this for initialization
    private void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("GameController");
        if (temp != null)
        {
            gameController = temp.GetComponent<GameController>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameController.gameOver)
        {
            Destroy(gameObject);
        }
    }
}
