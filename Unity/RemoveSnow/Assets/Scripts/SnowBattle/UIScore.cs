using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコアUI
/// </summary>
public class UIScore : MonoBehaviour {

	/// <summary>
	/// この得点の対象となっているプレイヤーのインデックス
	/// </summary>
	[SerializeField]
	private int playerIndex;

	/// <summary>
	/// 最初に文字色を設定
	/// </summary>
	public void Start() {
		this.GetComponent<Text>().color = PlayerScore.PlayerColors[this.playerIndex];
	}

	/// <summary>
	/// スコア表示を更新
	/// </summary>
	public void Update() {
		if(PlayerScore.IsScoreHidden == false) {
			this.GetComponent<Text>().text = PlayerScore.Scores[this.playerIndex].ToString("0");
		} else {
			this.GetComponent<Text>().text = "????";
		}
	}

}
