using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The transparent overlay on the game screen for mobile when the regions game mode is in use
/// </summary>
public class TransparentGuide : MonoBehaviour
{
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
