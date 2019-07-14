using UnityEngine;

public class HorizontalBarMover : MonoBehaviour
{
    //this script moves the Bars on the title screen properly

    public bool isHorizontal;
    public float upLimit;
    public float leftLimit;
    private RectTransform rectTransform;

    public float speed;
    public Vector3 ResetPosition;

    // Use this for initialization
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //if this bar is moving right to left
        if (isHorizontal)
        {
            //if it's hit the boundary;
            if (rectTransform.localPosition.x < leftLimit)
            {
                rectTransform.localPosition = ResetPosition;
            }
            else
            {
                //move it left slightly
                rectTransform.Translate(speed, 0, 0);
            }
        }
        // else this bar is moving up and down
        else
        {
            //if it's hit the boundary;
            if (rectTransform.localPosition.y >= upLimit)
            {
                rectTransform.localPosition = ResetPosition;
            }
            else
            {
                //move it left slightly
                rectTransform.Translate(0, speed, 0);
            }
        }
    }
}
