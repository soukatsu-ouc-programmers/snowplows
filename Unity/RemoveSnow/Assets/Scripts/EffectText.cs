using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectText : MonoBehaviour {

	[SerializeField]
	private Color[] textColor;

	private int colorNumber;

	void Start(){
		switch (this.gameObject.transform.parent.name) {
		case "Text SpeedUp(Clone)":
			colorNumber = 0;
			break;
		case "Text Cannon(Clone)":
			colorNumber = 1;
			break;
		case "Text SizeUp(Clone)":
			colorNumber = 2;
			break;
		case "Text Puzzle(Clone)":
			colorNumber = 3;
			break;
		}
		StartCoroutine ("FadeOut");
	}

	IEnumerator FadeOut(){
		for (float i = 0; i <= 30.0f; i++) {
			this.GetComponent<Text> ().color = new Color (textColor[colorNumber].r, textColor[colorNumber].g, textColor[colorNumber].b, 1.0f - i/30.0f);
			this.transform.parent.transform.Translate (transform.up * 0.01f);
			yield return null;
		}
		Destroy (this.transform.parent.gameObject);
	}
}
