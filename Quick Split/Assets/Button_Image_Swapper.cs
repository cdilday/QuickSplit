using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Button_Image_Swapper : MonoBehaviour {

	Image buttonImage;
	public Sprite[] images;

	void Awake()
	{
		//Get the Image script on the button
		Transform temp = transform.GetChild (0);
		buttonImage = temp.gameObject.GetComponent<Image> ();
	}

	public void Change_Image(int index){
		if (index >= 0 && index < images.Length) {
			buttonImage.sprite = images[index];
		}
	}
}
