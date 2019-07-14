using UnityEngine;

public class CircleRotater : MonoBehaviour
{
    //this Script rotates the circles on the title scene

    private RectTransform rectTransform;

    public float speed;
    public bool randomize;

    public bool Clockwise;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (randomize)
        {
            speed = Random.Range(-10f, 10f);
        }
        if (speed == 0)
        {
            speed = 1f;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Clockwise)
        {
            rectTransform.Rotate(new Vector3(0, 0, speed));
        }
        else
        {
            rectTransform.Rotate(new Vector3(0, 0, speed));
        }
    }
}
