using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// タイトル画面
/// </summary>
public class TitleScene : MonoBehaviour {

	/// <summary>
	/// フェーダー
	/// </summary>
	[SerializeField]
	private Fade fader;

	/// <summary>
	/// フェードインが完了したかどうか
	/// </summary>
	private bool fadeInCompleted;

	/// <summary>
	/// タイトル画面のBGMが一周したら自動的に次のシーンに移る
	/// </summary>
	public void Start() {
		this.fadeInCompleted = false;
		this.Invoke("ChangeScene", 90f);

		// ビルド後は開始直後にフェーダーを使うとNullReferenceExceptionが出るため、遅延呼び出しする
		this.Invoke("fadeIn", 0.5f);
	}

	/// <summary>
	/// キー入力でも次のシーンへ移る
	/// </summary>
	public void Update() {
		if(this.fadeInCompleted == false) {
			// フェードインが終わっていないときは操作不能にする
			return;
		}
		if(this.GetComponent<Button>().interactable == false) {
			// ボタンが押せなくなっているときはこちらも操作不能にする
			return;
		}

		if(Input.GetKeyDown(KeyCode.Joystick1Button0) == true
		|| Input.GetKeyDown(KeyCode.Return) == true) {
			this.ChangeScene();
		}
	}

	/// <summary>
	/// 遅延処理用：フェードインしてシーン開始
	/// </summary>
	private void fadeIn() {
		this.fader.FadeIn(0, () => {
			GameObject.Find("StartingMask").SetActive(false);
			this.fader.FadeOut(1.0f, () => {
				this.fadeInCompleted = true;
			});
		});
	}

	/// <summary>
	/// 次のシーンへ移る
	/// </summary>
	public void ChangeScene() {
		// SE再生
		GameObject.Find("DecideSE").GetComponent<AudioSource>().Play();

		// ボタンを一度押したら押せないようする
		this.GetComponent<Button>().interactable = false;

		// フェードアウトしてシーン遷移
		this.fader.FadeIn(1.0f, () => {
			this.StartCoroutine(this.SEWait(1.0f, () => {
				SceneManager.LoadScene((int)SceneIDs.SelectMode);
			}));
		});
	}

	/// <summary>
	/// 指定時間ウェイトしてから所定の処理を行います。
	/// </summary>
	/// <param name="waitSeconds">待機秒数</param>
	/// <param name="callback">ウェイト後に行う処理</param>
	IEnumerator SEWait(float waitSeconds, Action callback) {
		yield return new WaitForSeconds(waitSeconds);

		if(callback != null) {
			callback.Invoke();
		}
	}

}
