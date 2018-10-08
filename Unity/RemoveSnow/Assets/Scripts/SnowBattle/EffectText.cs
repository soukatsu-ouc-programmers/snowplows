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
	/// アイテムオブジェクト名から文字色インデックスを紐づけるマップ
	/// </summary>
	static private readonly Dictionary<string, int> ItemNameToColorIndexMap = new Dictionary<string, int>() {
		{ "Turbo", 0 },
		{ "Cannon", 1 },
		{ "BigBull", 2 },
		{ "Puzzle", 3 },
		{ "Heal", 4 },
		{ "Minion", 5 },
	};

	/// <summary>
	/// アイテムオブジェクト名から文字色インデックスを返す
	/// </summary>
	/// <param name="name">アイテムオブジェクト名</param>
	/// <returns>対応する文字色インデックス。該当しない場合は-1</returns>
	static public int GetItemColorIndex(string name) {
		foreach(var key in EffectText.ItemNameToColorIndexMap.Keys) {
			if(name.ToLower().IndexOf(key.ToLower()) != -1) {
				return EffectText.ItemNameToColorIndexMap[key];
			}
		}
		return -1;
	}

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Start() {
		this.colorIndex = EffectText.GetItemColorIndex(this.gameObject.transform.parent.name);
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
