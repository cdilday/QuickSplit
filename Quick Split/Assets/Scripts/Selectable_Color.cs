using UnityEngine;
using System.Collections;

public class Selectable_Color : MonoBehaviour {

	public string pieceColor;
 
	void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			gameObject.GetComponentInParent<Color_Selector>().colorSelected(pieceColor);
		}
	}
}
