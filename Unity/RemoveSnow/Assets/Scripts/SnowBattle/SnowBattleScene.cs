using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 対戦ゲームの進行管理
/// </summary>
public class SnowBattleScene : MonoBehaviour {

	/// <summary>
	/// フェーダー
	/// </summary>
	[SerializeField]
	private Fade fader;

	/// <summary>
	/// テロップ群
	/// </summary>
	[SerializeField]
	private GameObject[] subtitles;

	/// <summary>
	/// SEグループ
	/// </summary>
	[SerializeField]
	private AudioSource[] seGroup;

	/// <summary>
	/// ゲームが開始したかどうか
	/// </summary>
	static public bool IsStarted {
		get;
		private set;
	}

	/// <summary>
	/// プレイヤーごとの初期位置と初期状態の向きを定義した空のゲームオブジェクト群
	/// </summary>
	[SerializeField]
	private GameObject[] spawnPositions;

	/// <summary>
	/// ブレード型除雪車プレイヤーごとのプレハブ群
	/// </summary>
	[SerializeField]
	private GameObject[] snowplowsNormal;

	/// <summary>
	/// ロータリー型除雪車プレイヤーごとのプレハブ群
	/// </summary>
	[SerializeField]
	private GameObject[] snowplowsSurvival;

	/// <summary>
	/// シーン開始と同時にフェードインします。
	/// </summary>
	public void Start() {
		// 初期化
		PlayerScore.Init(SelectModeScene.Players);
		SnowBattleScene.IsStarted = false;

		// モードによる設定: バトルモードでUI表示、除雪車の種類を切り替える
		switch(SelectModeScene.BattleMode) {
			case SelectModeScene.BattleModes.ShavedIce:
				// HPメーターを無効化
				var meters = GameObject.FindGameObjectsWithTag("MeterUI");
				foreach(var meter in meters) {
					meter.SetActive(false);
				}

				// 通常の除雪車を配置
				for(int i = 0; i < SelectModeScene.Players; i++) {
					GameObject.Instantiate(
						this.snowplowsNormal[i],
						this.spawnPositions[i].transform.position,
						this.spawnPositions[i].transform.rotation
					);
				}
				break;

			case SelectModeScene.BattleModes.SnowFight:
				// 点数表示を無効化
				var scores = GameObject.FindGameObjectsWithTag("ScoreUI");
				foreach(var score in scores) {
					score.GetComponent<Text>().enabled = false;
				}

				// ロータリー除雪車を配置
				for(int i = 0; i < SelectModeScene.Players; i++) {
					GameObject.Instantiate(
						this.snowplowsSurvival[i],
						this.spawnPositions[i].transform.position,
						this.spawnPositions[i].transform.rotation
					);
				}
				break;
		}

		// ビルド後は開始直後にフェーダーを使うとNullReferenceExceptionが出るため、遅延呼び出しする
		this.Invoke("fadeIn", 0.5f);

		// 人数による設定
		switch(SelectModeScene.Players) {
			case 1:
				// 2Pの表示を消す
				if(SelectModeScene.BattleMode == SelectModeScene.BattleModes.ShavedIce) {
					GameObject.Find("CameraFor1P").SetActive(true);
				}
				GameObject.Find("CameraPlayer1P").SetActive(false);
				GameObject.Find("Text Score Player2").SetActive(false);
				GameObject.Find("Image player2").SetActive(false);
				break;

			case 2:
				// 1Pプレイ専用のカメラを消す
				if(SelectModeScene.BattleMode == SelectModeScene.BattleModes.ShavedIce) {
					GameObject.Find("CameraFor1P").SetActive(false);
				}
				break;
		}
	}

	/// <summary>
	/// 遅延処理用：フェードインしてシーン開始
	/// </summary>
	private void fadeIn() {
		this.fader.FadeIn(0, () => {
			GameObject.Find("StartingMask").SetActive(false);

			this.fader.FadeOut(1.0f, () => {
				// フェード完了後、Ready-Go表示をして開始する
				this.StartCoroutine(this.startingSubtitleControllerDescription());
			});
		});
	}

	/// <summary>
	/// コルーチン：操作説明を開始
	/// </summary>
	private IEnumerator startingSubtitleControllerDescription() {
		yield return new WaitForEndOfFrame();

		// iTweenによるアニメーション表示
		this.subtitles[0].SetActive(true);
		this.subtitles[0].transform.localScale = Vector3.zero;
		iTween.ScaleTo(
			this.subtitles[0],
			iTween.Hash(
				"x", 1,
				"y", 1,
				"z", 1,
				"time", 0.5f,
				"delay", 0.01f,
				"easeType", iTween.EaseType.easeOutQuint,
				"oncomplete", new Action<object>((param) => {
					this.StartCoroutine(this.closingSubtitleControllerDescription());
				}),
				"oncompletetarget", this.subtitles[0]
			)
		);
	}

	/// <summary>
	/// コルーチン：操作説明を終了
	/// </summary>
	private IEnumerator closingSubtitleControllerDescription() {
		// 一定時間経過 or キー入力で次へ
		float startTime = Time.time;
		while(Input.anyKeyDown == false && Time.time - startTime <= 5.0f) {
			yield return null;
		}

		// iTweenによるアニメーション表示
		iTween.ScaleTo(
			this.subtitles[0],
			iTween.Hash(
				"x", 0,
				"y", 1,
				"z", 1,
				"time", 0.5f,
				"easeType", iTween.EaseType.easeOutQuint,
				"oncomplete", new Action<object>((param) => {
					this.StartCoroutine(this.startingSubtitleReady());
				}),
				"oncompletetarget", this.subtitles[0]
			)
		);
	}

	/// <summary>
	/// コルーチン：テロップ：Ready？
	/// </summary>
	private IEnumerator startingSubtitleReady() {
		yield return new WaitForEndOfFrame();

		// SE再生
		this.seGroup[0].Play();

		// iTweenによるアニメーション表示
		this.subtitles[0].SetActive(false);
		this.subtitles[1].SetActive(true);
		this.subtitles[1].transform.localScale = Vector3.zero;
		iTween.ScaleTo(
			this.subtitles[1],
			iTween.Hash(
				"x", 1,
				"y", 1,
				"z", 1,
				"time", 0.5f,
				"delay", 0.01f,
				"easeType", iTween.EaseType.easeOutQuint,
				"oncomplete", new Action<object>((param) => {
					this.StartCoroutine(this.startingSubtitleGo());
				}),
				"oncompletetarget", this.subtitles[1]
			)
		);
	}

	/// <summary>
	/// コルーチン：テロップ：GO!
	/// </summary>
	private IEnumerator startingSubtitleGo() {
		yield return new WaitForSeconds(2.0f);

		// SE再生
		this.seGroup[1].Play();

		// iTweenによるアニメーション表示
		this.subtitles[1].SetActive(false);
		this.subtitles[2].SetActive(true);
		this.subtitles[2].transform.localScale = Vector3.zero;
		iTween.ScaleTo(
			this.subtitles[2],
			iTween.Hash(
				"x", 1,
				"y", 1,
				"z", 1,
				"time", 0.5f,
				"delay", 0.01f,
				"easeType", iTween.EaseType.easeOutQuint,
				"oncomplete", new Action<object>((param) => {
					this.StartCoroutine(this.startingSubtitleClose());
				}),
				"oncompletetarget", this.subtitles[2]
			)
		);

	}

	/// <summary>
	/// コルーチン：開始テロップ解除
	/// </summary>
	private IEnumerator startingSubtitleClose() {
		yield return new WaitForSeconds(1.0f);

		// iTweenによるアニメーション表示
		iTween.ScaleTo(
			this.subtitles[2],
			iTween.Hash(
				"x", 0,
				"y", 0,
				"z", 0,
				"time", 0.5f,
				"delay", 0.01f,
				"easeType", iTween.EaseType.easeOutQuint,
				"oncomplete", new Action<object>((param) => {
					this.subtitles[2].SetActive(false);

					// プレイヤー入力を許可
					SnowBattleScene.IsStarted = true;
				}),
				"oncompletetarget", this.subtitles[2]
			)
		);
	}

	/// <summary>
	/// タイマーゼロイベントハンドラー：タイマーがゼロになったときに終了処理を行う
	/// </summary>
	public void EndTimer() {
		SnowBattleScene.IsStarted = false;

		// SE再生
		this.seGroup[2].Play();
		this.Invoke("delayFinishVoice", 1.5f);

		// iTweenによるアニメーション表示
		this.subtitles[3].SetActive(true);
		this.subtitles[3].transform.localScale = Vector3.zero;
		iTween.ScaleTo(
			this.subtitles[3],
			iTween.Hash(
				"x", 1,
				"y", 1,
				"z", 1,
				"time", 0.5f,
				"delay", 0.01f,
				"easeType", iTween.EaseType.easeOutQuint,
				"oncomplete", new Action<object>((param) => {
					// Finish表示を解除して次のシーンへ遷移
					this.StartCoroutine(this.endingSubtitleClose());
				}),
				"oncompletetarget", this.subtitles[3]
			)
		);
	}

	/// <summary>
	/// 遅延処理用：終了のアナウンス [ゲームセット] をホイッスルの後に続けます。
	/// </summary>
	private void delayFinishVoice() {
		this.seGroup[3].Play();
	}

	/// <summary>
	/// コルーチン：Finish表示を解除して次のシーンへ遷移します。
	/// </summary>
	private IEnumerator endingSubtitleClose() {
		yield return new WaitForSeconds(3.0f);

		this.fader.FadeIn(1.0f, () => {
			SceneManager.LoadScene((int)SceneIDs.Result);
		});
	}

}
