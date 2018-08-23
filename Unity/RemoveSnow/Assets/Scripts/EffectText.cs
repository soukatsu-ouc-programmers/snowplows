using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectText : MonoBehaviour {

	public Color TextColor;

	void Start(){
		StartCoroutine ("FadeOut");
	}

	IEnumerator FadeOut(){
		for (float i = 0; i <= 30.0f; i++) {
			this.GetComponent<Text> ().color = new Color (TextColor.r, TextColor.g, TextColor.b, 1.0f - i/30.0f);
			this.transform.parent.transform.Translate (transform.up * 0.01f);
			yield return null;
		}
		Destroy (this.transform.parent.gameObject);
	}
}
