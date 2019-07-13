using UnityEngine;

public class SplitterAnimatorScript : MonoBehaviour
{
    //This script applies the proper animations to the splitters and activates them on start

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private PieceSplitterAssetHelper pieceSpriteHolder;

    // Use this for initialization
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        pieceSpriteHolder = GameObject.Find("Piece Sprite Holder").GetComponent<PieceSplitterAssetHelper>();
        animator.runtimeAnimatorController = pieceSpriteHolder.Get_Splitter_Animation();
        if (animator.runtimeAnimatorController == null)
        {
            spriteRenderer.sprite = null;
        }
    }
}
