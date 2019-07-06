using System.Collections;
using UnityEngine;

public class Achievement_Script : MonoBehaviour
{
    //this is for checking specifically for the cyan splitter unlock, as it requires timing
    private bool cyanCheck;
    private float startTime;

    public bool[] splittersUnlocked;
    public bool[] piecesetsUnlocked;

    public Achievement_Notification notification;

    // Use this for initialization
    private void Awake()
    {
        //there should only ever be 1 of these
        DontDestroyOnLoad(transform.gameObject);
        //get rid of redundant Achievement Handlers
        GameObject[] mcs = GameObject.FindGameObjectsWithTag("Achievement Handler");
        if (mcs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        //make sure default sets are always unlocked at start of game to prevent crashes
        PlayerPrefs.SetInt("Wiz unlocked", 1);
        PlayerPrefs.SetInt(SplitterType.Default + " Splitter unlocked", 1);
        PlayerPrefs.SetInt(PieceSets.Default +" Pieceset unlocked", 1);
        PlayerPrefs.SetInt(PieceSets.Symbol + " Pieceset unlocked", 1);

        splittersUnlocked = new bool[SplitterHelper.SplitterStrings.Length];
        piecesetsUnlocked = new bool[PieceSetHelper.PieceSetStrings.Length];

        //load the arrays for unlocks with the proper values already saved in prefs. This prevents longer lookups later
        for (int i = 0; i < splittersUnlocked.Length; i++)
        {
            if (PlayerPrefs.GetInt((SplitterType)i + " Splitter unlocked", 0) == 0)
            {
                splittersUnlocked[i] = false;
            }
            else
            {
                splittersUnlocked[i] = true;
            }
        }

        for (int i = 0; i < piecesetsUnlocked.Length; i++)
        {
            if (PlayerPrefs.GetInt((PieceSets)i + " Pieceset unlocked", 0) == 0)
            {
                piecesetsUnlocked[i] = false;
            }
            else
            {
                piecesetsUnlocked[i] = true;
            }
        }
        //the programmer unlocks require all other things to be unlocked, so do that check now
        if (PlayerPrefs.GetInt(SplitterType.Programmer + " Splitter unlocked", 0) == 0)
        {
            bool check = true;
            for (int i = 0; i < splittersUnlocked.Length; i++)
            {
                if (splittersUnlocked[i] == false && (SplitterType)i != SplitterType.Programmer)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                Unlock_Splitter(SplitterType.Programmer);
            }
        }

        if (PlayerPrefs.GetInt(PieceSets.Programmer + " Pieceset unlocked", 0) == 0)
        {
            bool check = true;
            for (int i = 0; i < piecesetsUnlocked.Length; i++)
            {
                if (piecesetsUnlocked[i] == false && (PieceSets)i != PieceSets.Programmer)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                Unlock_Pieceset(PieceSets.Programmer);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //debug buttons
        // numpad 5 resets all the unlocks
        // numpad 7 unlocks all the things
        // numpad 6 unlocks the pumpkin pieceset
        if (Debug.isDebugBuild)
        {
            if (Input.GetKey(KeyCode.Keypad5))
            {
                PlayerPrefs.SetInt(GameMode.Wiz + " unlocked", 1);
                PlayerPrefs.SetInt(GameMode.Quick + " unlocked", 0);
                PlayerPrefs.SetInt(GameMode.Wit + " unlocked", 0);
                PlayerPrefs.SetInt(GameMode.Holy + " unlocked", 0);
                PlayerPrefs.SetInt(GameMode.Custom + " unlocked", 0);
                Debug.Log("Unlocked GameModes Reset!");
                for (int i = 1; i < SplitterHelper.SplitterStrings.Length; i++)
                {
                    splittersUnlocked[i] = false;
                    PlayerPrefs.SetInt((SplitterType)i + " Splitter unlocked", 0);
                }
                PlayerPrefs.SetInt(SplitterType.Default + " Splitter unlocked", 1);
                Debug.Log("Unlocked Splitters Reset!");
                for (int i = 1; i < PieceSetHelper.PieceSetStrings.Length; i++)
                {
                    piecesetsUnlocked[i] = false;
                    PlayerPrefs.SetInt((PieceSets)i + " Pieceset unlocked", 0);
                }
                PlayerPrefs.SetInt(PieceSets.Default + " Pieceset unlocked", 1);
                Unlock_Pieceset(PieceSets.Symbol);
                Debug.Log("Unlocked Piecesets Reset!");

            }
            else if (Input.GetKey(KeyCode.Keypad7))
            {
                PlayerPrefs.SetInt(GameMode.Wiz + " unlocked", 1);
                PlayerPrefs.SetInt(GameMode.Quick + " unlocked", 1);
                PlayerPrefs.SetInt(GameMode.Wit + " unlocked", 1);
                PlayerPrefs.SetInt(GameMode.Holy + " unlocked", 1);
                PlayerPrefs.SetInt(GameMode.Custom + " unlocked", 1);
                Debug.Log("All GameModes Unlocked!");
                for (int i = 0; i < SplitterHelper.SplitterStrings.Length; i++)
                {
                    splittersUnlocked[i] = true;
                    PlayerPrefs.SetInt((SplitterType)i + " Splitter unlocked", 1);
                }
                Debug.Log("All Splitters Unlocked!");
                for (int i = 0; i < PieceSetHelper.PieceSetStrings.Length; i++)
                {
                    piecesetsUnlocked[i] = true;
                    PlayerPrefs.SetInt((PieceSets)i + " Pieceset unlocked", 1);
                }
                Debug.Log("All Piecesets Unlocked!");
            }
            else if (Input.GetKey(KeyCode.Keypad6))
            {
                Unlock_Pieceset(PieceSets.Pumpkin);
            }
            else if (Input.GetKey(KeyCode.Keypad1))
            {
                Debug.Log("Wiz " + PlayerPrefs.GetInt("Wiz unlocked", 1));
                Debug.Log("Quick " + PlayerPrefs.GetInt("Quick unlocked", 0));
                Debug.Log("Wit " + PlayerPrefs.GetInt("Wit unlocked", 0));
                Debug.Log("Holy " + PlayerPrefs.GetInt("Holy unlocked", 0));
                Debug.Log("Custom " + PlayerPrefs.GetInt("Custom unlocked", 0));
            }
        }
    }

    //use this to add new high scores
    public void Add_Score(GameMode gameMode, int score)
    {
        //you need to actually score to save a high score
        if (score == 0)
        {
            return;
        }

        int tempScore = score;

        for (int i = 0; i < 15; i++)
        {
            int currScore = PlayerPrefs.GetInt(gameMode + " score " + i, 0);
            if (currScore == 0)
            {
                //We've hit the end of the list. Place the score here and exit
                PlayerPrefs.SetInt(gameMode + " score " + i, tempScore);
                break;
            }
            else if (currScore <= tempScore)
            {
                //continue looking through the list
                PlayerPrefs.SetInt(gameMode + " score " + i, tempScore);
                tempScore = currScore;
            }
        }
    }

    private void FixedUpdate()
    {
        //More cyan unlock stuff. This resets it if the time has passed
        if (cyanCheck && startTime + 3f < Time.time)
        {
            cyanCheck = false;
        }
    }

    //unlocks the splitter with the given name
    public void Unlock_Splitter(SplitterType splitterType)
    {
        splittersUnlocked[(int)splitterType] = true;
        PlayerPrefs.SetInt(name + " Splitter unlocked", 1);
        if (notification != null)
        {
            notification.Achievement_Unlocked(SplitterHelper.Get_Splitter_Name(splitterType), "Splitter");
        }
    }

    //returns true if the given splitter name is unlocked
    public bool is_Splitter_Unlocked(SplitterType splitter)
    {
        return splittersUnlocked[(int)splitter];
    }

    //unlocks the pieceset with the given name
    public void Unlock_Pieceset(PieceSets pieceSet)
    {
        piecesetsUnlocked[(int)pieceSet] = true;
        PlayerPrefs.SetInt(pieceSet + " Pieceset unlocked", 1);

        if (notification != null)
        {
            notification.Achievement_Unlocked(PieceSetHelper.Get_Pieceset_Name(pieceSet), "Pieceset");
        }
    }

    //returns true if the given pieceset name is unlocked
    public bool is_Pieceset_Unlocked(PieceSets pieceSet)
    {
        return piecesetsUnlocked[(int)pieceSet];
    }

    //unlocks gamemodes as the player scores in previous ones
    public void Check_Gamemode_Unlocked()
    {
        //the unlock order goes from Wiz -> Quick -> Wit -> Holy
        if (PlayerPrefs.GetInt("Wiz score 0", 0) > 0)
        {
            PlayerPrefs.SetInt(GameMode.Quick + " unlocked", 1);
        }

        if (PlayerPrefs.GetInt("Quick score 0", 0) > 0)
        {
            PlayerPrefs.SetInt(GameMode.Wit + " unlocked", 1);
        }

        if (PlayerPrefs.GetInt("Wit score 0", 0) > 0)
        {
            PlayerPrefs.SetInt(GameMode.Holy + " unlocked", 1);
        }

        if (PlayerPrefs.GetInt("Holy score 0", 0) > 0)
        {
            if (is_Pieceset_Unlocked(PieceSets.Present) == false)
            {
                Unlock_Pieceset(PieceSets.Present);
            }
            PlayerPrefs.SetInt(GameMode.Custom + " unlocked", 1);
        }
    }

    //this is for checking if the cyan splitter conditions for unlock have been met
    public void Cyan_Splitter_Checker()
    {
        if (splittersUnlocked[(int)SplitterType.Cyan])
        {
            return;
        }
        else if (cyanCheck)
        {
            Unlock_Splitter(SplitterType.Cyan);
        }
        else
        {
            startTime = Time.time;
            cyanCheck = true;
        }
    }

    //this is for handling the purple splitter's conditions for unlocking
    public IEnumerator Purple_Splitter_Checker(int oldDangerPieces)
    {
        if (!is_Splitter_Unlocked(SplitterType.Purple) && oldDangerPieces < 3)
        {
            yield return new WaitForSeconds(3);
            if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Get_Danger_Pieces() == 0)
            {
                Unlock_Splitter(SplitterType.Purple);
            }
        }
    }

    //this is for handling the blue splitter's conditions for unlocking
    public IEnumerator Blue_Splitter_Checker()
    {
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        int oldCount = 0;
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 16; c++)
            {
                if (gameController.grid[r, c] != null)
                {
                    oldCount++;
                }
            }
        }
        yield return new WaitForSeconds(2f);
        int newCount = 0;
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 16; c++)
            {
                if (gameController.grid[r, c] != null)
                {
                    newCount++;
                }
            }
        }

        if (oldCount - newCount >= 16)
        {
            Unlock_Splitter(SplitterType.Blue);
        }
    }

}
