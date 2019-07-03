using UnityEngine;

public class Sidebar_Script : MonoBehaviour
{

    //This Script handles the sidebars that seperate the game grid from the pieces in the side columns and lights up.

    public Sprite[] Sequential_Lights;
    public Sprite[] Colored_Lights;
    public Sprite[] Flashing_Lights;
    private GameMode activeGameMode;
    private SpriteRenderer spriteRenderer;
    private int lightStage;
    private bool isFlashing;

    // Use this for initialization
    private void Start()
    {
        activeGameMode = Game_Mode_Helper.ActiveRuleSet.Mode;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sequential_Lights[0];
        lightStage = 0;
        isFlashing = false;
    }

    private void FixedUpdate()
    {
        if (isFlashing)
        {
            int seconds = (int)Time.time;
            float tempTime = Time.time - seconds;
            if (tempTime < 0.25f || (tempTime >= 0.5f && tempTime < 0.75f))
            {
                spriteRenderer.sprite = Flashing_Lights[0];
            }
            else
            {
                spriteRenderer.sprite = Flashing_Lights[1];
            }
        }
    }

    //lights up an additional light
    public void Increment_Lights()
    {
        if (activeGameMode != GameMode.Wit)
        {
            if (lightStage == Sequential_Lights.Length - 1)
            {
                isFlashing = true;
            }
            else
            {
                lightStage++;
                spriteRenderer.sprite = Sequential_Lights[lightStage];
            }
        }
    }

    //resets the sidebars back to their intial unlit state
    public void Reset()
    {
        isFlashing = false;
        lightStage = 0;
        spriteRenderer.sprite = Sequential_Lights[lightStage];
    }

}