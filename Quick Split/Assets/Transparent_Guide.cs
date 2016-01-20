using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Transparent_Guide : MonoBehaviour {

	Image image;
	Color defaultColor;
	RectTransform rectTransform;
	Camera mainCamera;

	bool beingTouched;
	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
		defaultColor = image.color;
		rectTransform = GetComponent<RectTransform> ();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		//put resizing code here
	}
	
	// Update is called once per frame
	void Update () {
		beingTouched = false;
		foreach (Touch poke in Input.touches) {
			if(RectTransformUtility.RectangleContainsScreenPoint(rectTransform, poke.position, mainCamera)){
				image.color = new Color(image.color.r, image.color.g, image.color.b, 0.33f);
				beingTouched = true;
			}
		}
		if (!beingTouched) {
			image.color = defaultColor;
		}
	}
}
