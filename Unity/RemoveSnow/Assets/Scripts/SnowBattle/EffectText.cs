using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アイテム取得時の演出：アイテム名表示
/// </summary>
public class EffectText : MonoBehaviour {

	/// <summary>
	/// エフェクトフレーム時間
	/// </summary>
	public const int EffectTimeFrames = 90;

	/// <summary>
	/// アイテムごとの文字色
	/// </summary>
	[SerializeField]
	private Color[] textColors;

	/// <summary>
	/// 文字色インデックス
	/// </summary>
	private int colorIndex;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Start() {
		switch(this.gameObject.transform.parent.name) {
			case "Text SpeedUp(Clone)":
				this.colorIndex = 0;
				break;
			case "Text Cannon(Clone)":
				this.colorIndex = 1;
				break;
			case "Text SizeUp(Clone)":
				this.colorIndex = 2;
				break;
			case "Text Puzzle(Clone)":
				this.colorIndex = 3;
				break;
		}
		this.StartCoroutine(this.fadeOut());
	}

	/// <summary>
	/// コルーチン：徐々にフェードアウト＋上昇させます。
	/// </summary>
	private IEnumerator fadeOut() {
		// 初回色設定
		this.GetComponent<Text>().color = new Color(
			this.textColors[this.colorIndex].r,
			this.textColors[this.colorIndex].g,
			this.textColors[this.colorIndex].b,
			1.0f
		);

		// フレーム時間で徐々に変えていく
		for(int i = 0; i <= EffectTimeFrames; i += 1) {
			if(i >= EffectTimeFrames / 2) {
				// アニメーション後半からフェードさせる
				this.GetComponent<Text>().color = new Color(
					this.textColors[this.colorIndex].r,
					this.textColors[this.colorIndex].g,
					this.textColors[this.colorIndex].b,
					1.0f - (i / 2) / (EffectTimeFrames / 2.0f)
				);
			}

			// 上昇
			this.transform.parent.transform.Translate(this.transform.up * 0.0025f);

			yield return new WaitForEndOfFrame();
		}

		Object.Destroy(this.transform.parent.gameObject);
	}

}
