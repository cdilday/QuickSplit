using UnityEngine;

public class Gears_Script : MonoBehaviour
{

    //this is true if the gear is on the right side of the splitter. The rotation changes depending on its location
    private bool isRight;
    private Animator animator;

    public Sprite[] gears;
    private SpriteRenderer spriteRenderer;
    private Splitter splitter;

    // Use this for initialization
    private void Start()
    {
        splitter = transform.parent.GetComponent<Splitter>();
        if (transform.position.x < 0)
        {
            isRight = false;
        }
        else
        {
            isRight = true;
        }
    }

    private void FixedUpdate()
    {
        if (splitter.getState(Splitter.SplitterStates.isMoving))
        {
            //it's moving, find direction
            if (splitter.moveDirection == 1)
            {
                //it's going up
                if (isRight)
                {
                    transform.Rotate(new Vector3(0, 0, -20f));
                }
                else
                {
                    transform.Rotate(new Vector3(0, 0, 20f));
                }
            }
            else
            {
                //it's going down
                if (isRight)
                {
                    transform.Rotate(new Vector3(0, 0, 20f));
                }
                else
                {
                    transform.Rotate(new Vector3(0, 0, -20f));
                }
            }
        }
    }

}