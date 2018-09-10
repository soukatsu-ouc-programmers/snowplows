using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
	/// HPメーター
	/// </summary>
	[SerializeField]
	private Slider HPMeter;

	/// <summary>
	/// HPがゼロになったときに発動するイベント
	/// イベントハンドラーはインスペクターで設定して下さい。
	/// </summary>
	public UnityEvent ZeroHPEvent;

	/// <summary>
	/// HPがゼロになったときに発動するイベントが実行されたかどうか
	/// </summary>
	private bool zeroHPEventDone;

	/// <summary>
	/// 最初に文字色を設定
	/// </summary>
	public void Start() {
		this.GetComponent<Text>().color = PlayerScore.PlayerColors[this.playerIndex];
		this.HPMeter.maxValue = PlayerScore.MaxHP;
		this.HPMeter.value = PlayerScore.MaxHP;
		this.zeroHPEventDone = false;
	}

	/// <summary>
	/// スコア表示を更新
	/// </summary>
	public void Update() {
		switch(SelectModeScene.BattleMode) {
			case SelectModeScene.BattleModes.ShavedIce:
				// 除雪モード
				if(PlayerScore.IsScoreHidden == false) {
					this.GetComponent<Text>().text = PlayerScore.Scores[this.playerIndex].ToString("0");
				} else {
					this.GetComponent<Text>().text = "????";
				}
				break;

			case SelectModeScene.BattleModes.SnowFight:
				// サバイバルモード
				this.HPMeter.value = PlayerScore.HPs[this.playerIndex];

				// ゲーム終了判定
				if(SnowBattleScene.IsStarted == false) {
					break;
				}
				if(this.zeroHPEventDone == true) {
					break;
				}
				if(0 < this.HPMeter.value) {
					break;
				}
				if(this.ZeroHPEvent == null) {
					break;
				}

				// HPがゼロになったときのイベントを発動させる
				// TODO: ３人以上でプレイできるようにする場合、残されたプレイヤーがあと１人or０人になっているかどうかを確認する必要がある
				this.ZeroHPEvent.Invoke();
				this.zeroHPEventDone = true;
				break;
		}
	}

}
