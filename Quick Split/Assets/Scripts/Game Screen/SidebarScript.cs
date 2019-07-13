using UnityEngine;

public class SidebarScript : MonoBehaviour
{

    //This Script handles the sidebars that seperate the game grid from the pieces in the side columns and lights up.

    public Sprite[] SequentialLights;
    public Sprite[] FlashingLights;
    private GameMode activeGameMode;
    private SpriteRenderer spriteRenderer;
    private int lightStage;
    private bool isFlashing;

    // Use this for initialization
    private void Start()
    {
        activeGameMode = Game_Mode_Helper.ActiveRuleSet.Mode;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = SequentialLights[0];
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
                spriteRenderer.sprite = FlashingLights[0];
            }
            else
            {
                spriteRenderer.sprite = FlashingLights[1];
            }
        }
    }

    //lights up an additional light
    public void IncrementLights()
    {
        if (Game_Mode_Helper.ActiveRuleSet.UsesSides)
        {
            if (lightStage == SequentialLights.Length - 1)
            {
                isFlashing = true;
            }
            else
            {
                lightStage++;
                spriteRenderer.sprite = SequentialLights[lightStage];
            }
        }
    }
    public void SetLightStage(int stage)
    {
        if (Game_Mode_Helper.ActiveRuleSet.UsesSides)
        {
            lightStage = stage;
        }

        if (lightStage == SequentialLights.Length - 1)
        {
            isFlashing = true;
        }
        else
        {
            spriteRenderer.sprite = SequentialLights[lightStage];
        }
    }

    //resets the sidebars back to their intial unlit state
    public void Reset()
    {
        isFlashing = false;
        lightStage = 0;
        spriteRenderer.sprite = SequentialLights[lightStage];
    }

}