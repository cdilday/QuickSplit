using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Image_Spriter : MonoBehaviour {

	//This entirely wasteful script bypasses Unity's terrible UI image not working with given sprite animators.
	Image image;
	SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		image.sprite = spriteRenderer.sprite;
	}

}