using UnityEngine;
using System.Collections;

public class Layer_Resizer : MonoBehaviour {

	//this resizes the layers within layers because unity can't do it by itself even though it's a basic hierarchy organizational function

	public RectTransform parentCanvas;
	RectTransform rectTransform;

	// Use this for initialization
	void Start () {
		rectTransform = gameObject.GetComponent<RectTransform> ();
		rectTransform.position = parentCanvas.position;
		rectTransform.sizeDelta = parentCanvas.sizeDelta;
	}

}