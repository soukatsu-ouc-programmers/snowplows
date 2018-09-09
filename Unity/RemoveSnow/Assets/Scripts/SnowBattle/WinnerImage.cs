using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// スコアの優勢・劣勢をグラフィカルに表示
/// BattleCanvasにアタッチ
/// </summary>
public class WinnerImage : MonoBehaviour {

	/// <summary>
	/// プレイヤーのかき氷アイコン
	/// </summary>
	[SerializeField]
	private GameObject[] playerIcons;

	/// <summary>
	/// 勝敗状態
	/// </summary>
	private int winnerPlayerIndex;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Start() {
		this.winnerPlayerIndex = -1;
	}

	/// <summary>
	/// 勝敗状態が変わったらアニメーションする
	/// </summary>
	public void Update() {
		// 注意：Animatorのトリガーセットするとき、他のトリガーをすべて解除しないとタイミングがずれておかしなことになる

		if(SelectModeScene.BattleMode == SelectModeScene.BattleModes.ShavedIce) {
			if(PlayerScore.IsScoreHidden == true) {
				if(this.winnerPlayerIndex != -1) {
					// Debug.Log("プレイヤー 優劣非表示");

					this.resetAnimatorTriggers();
					foreach(var icon in this.playerIcons) {
						icon.GetComponent<Animator>().SetTrigger("Nutral");
					}
					this.winnerPlayerIndex = -1;
				}

				return;
			}
		}

		// 暫定トップのプレイヤーを取得
		List<int> scores = null;
		switch(SelectModeScene.BattleMode) {
			case SelectModeScene.BattleModes.ShavedIce:
				scores = new List<int>(PlayerScore.Scores);
				break;

			case SelectModeScene.BattleModes.SnowFight:
				scores = new List<int>(PlayerScore.HPs);
				break;

			default:
				return;
		}
		var winnerPlayers = scores.Select((value, index) => new {
			index,
			value
		})
			.Where(item => item.value == scores.Max())
			.Select(item => item.index)
			.ToList();

		if(winnerPlayers.Count == 1) {
			if(this.winnerPlayerIndex != winnerPlayers[0]) {
				// var playerNumber = winnerPlayers[0] + 1;
				// Debug.Log("プレイヤー" + playerNumber + " 優勢");

				this.resetAnimatorTriggers();
				foreach(var icon in this.playerIcons) {
					if(icon.Equals(this.playerIcons[winnerPlayers[0]]) == true) {
						icon.GetComponent<Animator>().SetTrigger("Winner");
					} else {
						icon.GetComponent<Animator>().SetTrigger("Loser");
					}
				}
				this.winnerPlayerIndex = winnerPlayers[0];
			}
		} else {
			if(this.winnerPlayerIndex != -1) {
				// Debug.Log("プレイヤー 拮抗");

				this.resetAnimatorTriggers();
				foreach(var icon in this.playerIcons) {
					icon.GetComponent<Animator>().SetTrigger("Nutral");
				}
				this.winnerPlayerIndex = -1;
			}
		}
	}

	/// <summary>
	/// すべてのプレイヤーのAnimatorのトリガーをすべて初期化します
	/// </summary>
	private void resetAnimatorTriggers() {
		foreach(var icon in this.playerIcons) {
			var animator = icon.GetComponent<Animator>();

			animator.ResetTrigger("Loser");
			animator.ResetTrigger("Winner");
			animator.ResetTrigger("Nutral");
		}
	}

}
