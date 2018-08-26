using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// タイマー
/// </summary>
public class Timer : MonoBehaviour {
	
	/// <summary>
	/// スコアを非表示にする残り秒数
	/// </summary>
	public const float ScoreHiddenRemainSeconds = 20.0f;

	/// <summary>
	/// トータル制限時間
	/// </summary>
	private float totalTime;

	/// <summary>
	/// 制限時間（分）
	/// </summary>
	[SerializeField]
	private int minute;

	/// <summary>
	/// 制限時間（秒）
	/// </summary>
	[SerializeField]
	private float seconds;

	/// <summary>
	/// 前回Update時の秒数
	/// </summary>
	private float oldSeconds;

	/// <summary>
	/// タイマーテキストUI
	/// </summary>
	private Text timerText;

	/// <summary>
	/// タイマーがゼロになったときに発動するイベント
	/// イベントハンドラーはインスペクターで設定して下さい。
	/// </summary>
	public UnityEvent ZeroTimerEvent;

	/// <summary>
	/// 初期設定
	/// </summary>
	public void Start() {
		this.totalTime = this.minute * 60 + this.seconds;
		this.oldSeconds = 0f;
		this.timerText = this.GetComponentInChildren<Text>();
		this.timerText.text = this.minute.ToString("00") + ":" + ((int)this.seconds).ToString("00");
	}

	/// <summary>
	/// タイマーカウント実行
	/// </summary>
	public void Update() {
		if(SnowBattleScene.IsStarted == false) {
			// ゲームが開始していないときはカウントしない
			return;
		}
		if(this.totalTime <= 0f) {
			// 制限時間が０秒以下なら何もしない
			return;
		}

		// 一旦トータルの制限時間を計測
		this.totalTime = this.minute * 60 + this.seconds;
		this.totalTime -= Time.deltaTime;

		// 再設定
		this.minute = (int)this.totalTime / 60;
		this.seconds = this.totalTime - this.minute * 60;

		if((int)this.seconds != (int)this.oldSeconds) {
			// タイマー表示用UIテキストに時間を表示する
			this.timerText.text = this.minute.ToString("00") + ":" + ((int)this.seconds).ToString("00");
		}
		this.oldSeconds = this.seconds;

		if(this.totalTime < Timer.ScoreHiddenRemainSeconds + 1f) {
			// 制限時間ギリギリになったらスコアを非表示に
			PlayerScore.IsScoreHidden = true;
		}

		if(this.totalTime <= 0f) {
			// 制限時間終了
			if(this.ZeroTimerEvent != null) {
				// イベントハンドラーを呼び出す
				this.ZeroTimerEvent.Invoke();
				this.gameObject.SetActive(false);
			}
		}
	}

}

