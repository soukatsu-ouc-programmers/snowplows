using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectText : MonoBehaviour {

	[SerializeField]
	private Color[] textColor;

	private int colorNumber;

	/// <summary>
	/// エフェクトフレーム時間
	/// </summary>
	public const int EffectTimeFrames = 90;

	void Start() {
		switch(this.gameObject.transform.parent.name) {
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
		StartCoroutine("FadeOut");
	}

	IEnumerator FadeOut() {
		// 初回色設定
		this.GetComponent<Text>().color = new Color(
			textColor[colorNumber].r,
			textColor[colorNumber].g,
			textColor[colorNumber].b,
			1.0f
		);

		for(int i = 0; i <= EffectTimeFrames; i += 1) {
			if(i >= EffectTimeFrames / 2) {
				// アニメーション後半からフェードさせる
				this.GetComponent<Text>().color = new Color(
					textColor[colorNumber].r,
					textColor[colorNumber].g,
					textColor[colorNumber].b,
					1.0f - (i / 2) / (EffectTimeFrames / 2.0f)
				);
			}
			this.transform.parent.transform.Translate(transform.up * 0.0025f);
			yield return null;
		}
		Destroy(this.transform.parent.gameObject);
	}
}
