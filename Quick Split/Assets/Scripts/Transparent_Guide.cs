using UnityEngine;
using UnityEngine.UI;

public class Transparent_Guide : MonoBehaviour
{
    // TODO: Make this based off the current pieceset in order to be more color-blind friendly
    private Image image;
    private Color defaultColor;
    private RectTransform rectTransform;
    private Camera mainCamera;
    private bool beingTouched;

    // Use this for initialization
    private void Start()
    {
        if (PlayerPrefs.GetInt("Region Guide", 1) == 0 || PlayerPrefs.GetString("Controls", "Follow") != "Regions")
        {
            Destroy(gameObject);
        }
        image = GetComponent<Image>();
        defaultColor = image.color;
        rectTransform = GetComponent<RectTransform>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //put resizing code here
    }

    // Update is called once per frame
    private void Update()
    {
        beingTouched = false;
        foreach (Touch poke in Input.touches)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, poke.position, mainCamera))
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
                beingTouched = true;
            }
        }
        if (!beingTouched)
        {
            image.color = defaultColor;
        }
    }
}
