using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

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
	/// 前回update時の秒数
	/// </summary>
	private float oldSeconds;

	/// <summary>
	/// timerText　UI
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
	void Start() {
		totalTime = minute * 60 + seconds;
		oldSeconds = 0f;
		timerText = GetComponentInChildren<Text>();
		timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
	}

	/// <summary>
	/// タイマーカウント実行
	/// </summary>
	void Update() {
		if(BattleGameMaster.IsStarted == false) {
			// ゲームが開始していないときはカウントしない
			return;
		}
		if(totalTime <= 0f) {
			// 制限時間が０秒以下なら何もしない
			return;
		}

		// 一旦トータルの制限時間を計測
		totalTime = minute * 60 + seconds;
		totalTime -= Time.deltaTime;

		// 再設定
		minute = (int)totalTime / 60;
		seconds = totalTime - minute * 60;

		if((int)seconds != (int)oldSeconds) {
			// タイマー表示用UIテキストに時間を表示する
			timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
		}
		oldSeconds = seconds;
		if(totalTime <= 0f) {
			// 制限時間終了
			if(this.ZeroTimerEvent != null) {
				this.ZeroTimerEvent.Invoke();
				this.gameObject.SetActive(false);
			}
		}
	}

}

