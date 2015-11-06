using UnityEngine;
using System.Collections;

public class Layer_Resizer : MonoBehaviour {

	public RectTransform parentCanvas;
	RectTransform rectTransform;

	// Use this for initialization
	void Start () {
		rectTransform = gameObject.GetComponent<RectTransform> ();
		rectTransform.position = parentCanvas.position;
		rectTransform.sizeDelta = parentCanvas.sizeDelta;

	}
}
