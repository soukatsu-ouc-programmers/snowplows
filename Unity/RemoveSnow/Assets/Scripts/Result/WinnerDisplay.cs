using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// リザルト画面の勝敗表示
/// </summary>
public class WinnerDisplay : MonoBehaviour {

	/// <summary>
	/// 引き分けの文字色
	/// </summary>
	static public readonly Color DrawTextColor = Color.blue;

	/// <summary>
	/// 勝敗結果を表示
	/// </summary>
	public void Start() {
		// 勝敗を出力
		var scores = new List<int>(PlayerScore.Scores);
		var winnerPlayers = scores.Select((value, index) => new {
			index,
			value
		})
			.Where(item => item.value == scores.Max())
			.Select(item => item.index)
			.ToList();

		if(winnerPlayers.Count == 1) {
			// 勝者が一人
			var playerNumber = winnerPlayers[0] + 1;
			this.GetComponent<Text>().text = "Player " + playerNumber + "\r\nWin !";
			this.GetComponent<Text>().color = PlayerScore.PlayerColors[winnerPlayers[0]];

			// 敗者のかき氷を非表示にする
			var shavedIcesTransform = GameObject.Find("ShavedIces").transform;
			for(int i = 0; i < shavedIcesTransform.childCount; i++) {
				var child = shavedIcesTransform.GetChild(i);
				if(child.name.IndexOf(PlayerScore.PlayerColorNames[winnerPlayers[0]]) == -1) {
					GameObject.Find(PlayerScore.PlayerColorNames[i] + "ShavedIce").SetActive(false);
				}
			}
		} else {
			// 引き分け
			this.GetComponent<Text>().text = "Draw !";
			this.GetComponent<Text>().color = WinnerDisplay.DrawTextColor;

			// トップでないかき氷を非表示にする
			var shavedIcesTransform = GameObject.Find("ShavedIces").transform;
			for(int i = 0; i < shavedIcesTransform.childCount; i++) {
				if(i >= PlayerScore.Scores.Length) {
					break;
				}

				var isWinner = (PlayerScore.Scores[i] == scores.Max());
				if(isWinner == false) {
					GameObject.Find(PlayerScore.PlayerColorNames[i] + "ShavedIce").SetActive(false);
				}
			}
		}
	}

}
