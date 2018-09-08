using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	/// リザルトシーン番号
	/// </summary>
	[SerializeField]
	private int resultSceneID;

	/// <summary>
	/// ゲームが開始したかどうか
	/// </summary>
	static public bool IsStarted {
		get;
		private set;
	}

	/// <summary>
	/// シーン開始と同時にフェードインします。
	/// </summary>
	public void Start() {
		// 初期化
		// TODO: プレイヤーの人数が増える場合はここを修正する必要がある
		PlayerScore.Init(2);
		SnowBattleScene.IsStarted = false;

		// ビルド後は開始直後にフェーダーを使うとNullReferenceExceptionが出るため、遅延呼び出しする
		this.Invoke("fadeIn", 0.5f);
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
			SceneManager.LoadScene(this.resultSceneID);
		});
	}

}
