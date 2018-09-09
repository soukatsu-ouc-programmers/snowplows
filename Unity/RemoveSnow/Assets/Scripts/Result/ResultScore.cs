using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト画面の得点表示
/// </summary>
public class ResultScore : MonoBehaviour {

	/// <summary>
	/// プレイヤーの得点を設定
	/// </summary>
	public void Start() {
		var textScoreLabel = GameObject.Find("TextScoreLabel").GetComponent<Text>();
		this.GetComponent<Text>().text = "";
		textScoreLabel.text = "";

		if(SelectModeScene.BattleMode == SelectModeScene.BattleModes.SnowFight) {
			// サバイバルモードのときは何も表示しない
			return;
		}

		// 点数をプレイヤーカラーに応じて色分けして表示
		for(int i = 0; i < PlayerScore.Scores.Length; i++) {
			textScoreLabel.text += "Player " + (i + 1) + "   Score";
			this.GetComponent<Text>().text += PlayerScore.Scores[i].ToString("<color=" + PlayerScore.PlayerColorNames[i] + ">0</color>");

			if(i < PlayerScore.PlayerIndexMap.Count - 1) {
				// 末尾以外は改行を付ける
				this.GetComponent<Text>().text += "\r\n";
				textScoreLabel.text += "\r\n";
			}
		}
	}

}
