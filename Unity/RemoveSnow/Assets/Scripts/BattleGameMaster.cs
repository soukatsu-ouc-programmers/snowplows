using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 対戦ゲームの進行管理を行います。
/// </summary>
public class BattleGameMaster : MonoBehaviour {

	/// <summary>
	/// フェーダー
	/// </summary>
	[SerializeField]
	Fade Fader;

	/// <summary>
	/// テロップ群
	/// </summary>
	[SerializeField]
	GameObject[] Subtitles;

	/// <summary>
	/// SEグループ
	/// </summary>
	[SerializeField]
	AudioSource[] SEGroup;

	/// <summary>
	/// ゲームが開始したかどうか
	/// </summary>
	static public bool IsStarted;

	/// <summary>
	/// 最初の処理
	/// </summary>
	void Start() {
		BattleGameMaster.IsStarted = false;

		this.Fader.FadeIn(0, () => {
			GameObject.Find("StartingMask").SetActive(false);
			this.Fader.FadeOut(1.0f, () => {
				// フェード完了後、Ready-Go表示をして開始する
				this.StartCoroutine(this.StartingSubtitle1());
			});
		});
	}

	/// <summary>
	/// テロップ：Ready？
	/// </summary>
	/// <returns></returns>
	IEnumerator StartingSubtitle1() {
		yield return new WaitForEndOfFrame();

		// SE再生
		this.SEGroup[0].Play();

		this.Subtitles[0].SetActive(true);
		this.Subtitles[0].transform.localScale = Vector3.zero;
		iTween.ScaleTo(
			this.Subtitles[0],
			iTween.Hash(
				"x", 1,
				"y", 1,
				"z", 1,
				"time", 0.5f,
				"delay", 0.01f,
				"easeType", iTween.EaseType.easeOutQuint,
				"oncomplete", new Action<object>((param) => {
					this.StartCoroutine(this.StartingSubtitle2());
				}),
				"oncompletetarget", this.Subtitles[0]
			)
		);
	}

	/// <summary>
	/// テロップ：GO!
	/// </summary>
	/// <returns></returns>
	IEnumerator StartingSubtitle2() {
		// 一定時間待つ
		yield return new WaitForSeconds(3.0f);

		// SE再生
		this.SEGroup[1].Play();

		this.Subtitles[0].SetActive(false);
		this.Subtitles[1].SetActive(true);
		this.Subtitles[1].transform.localScale = Vector3.zero;
		iTween.ScaleTo(
			this.Subtitles[1],
			iTween.Hash(
				"x", 1,
				"y", 1,
				"z", 1,
				"time", 0.5f,
				"delay", 0.01f,
				"easeType", iTween.EaseType.easeOutQuint,
				"oncomplete", new Action<object>((param) => {
					this.StartCoroutine(this.StartingSubtitleClose());
				}),
				"oncompletetarget", this.Subtitles[1]
			)
		);

	}

	/// <summary>
	/// 開始テロップ解除
	/// </summary>
	IEnumerator StartingSubtitleClose() {
		// 一定時間待つ
		yield return new WaitForSeconds(1.0f);

		iTween.ScaleTo(
			this.Subtitles[1],
			iTween.Hash(
				"x", 0,
				"y", 0,
				"z", 0,
				"time", 0.5f,
				"delay", 0.01f,
				"easeType", iTween.EaseType.easeOutQuint,
				"oncomplete", new Action<object>((param) => {
					this.Subtitles[1].SetActive(false);

					// プレイヤー入力を許可
					BattleGameMaster.IsStarted = true;
				}),
				"oncompletetarget", this.Subtitles[1]
			)
		);
	}

	/// <summary>
	/// タイマーがゼロになったときに終了処理を行う
	/// </summary>
	public void EndTimer() {

		IsStarted = false;

		// SE再生
		this.SEGroup[2].Play();

		this.Subtitles[2].SetActive(true);
		this.Subtitles[2].transform.localScale = Vector3.zero;
		iTween.ScaleTo(
			this.Subtitles[2],
			iTween.Hash(
				"x", 1,
				"y", 1,
				"z", 1,
				"time", 0.5f,
				"delay", 0.01f,
				"easeType", iTween.EaseType.easeOutQuint,
				"oncomplete", new Action<object>((param) => {
					this.StartCoroutine(this.EndingSubtitleClose());
				}),
				"oncompletetarget", this.Subtitles[1]
			)
		);
	}

	/// <summary>
	/// Finish表示の解除
	/// </summary>
	IEnumerator EndingSubtitleClose() {
		yield return new WaitForSeconds(3.0f);

		this.Fader.FadeIn(1.0f, () => {
			SceneManager.LoadScene(3);
		});
	}

}
