using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// リザルト画面
/// </summary>
public class ResultScene : MonoBehaviour {

	/// <summary>
	/// メインカメラ
	/// </summary>
	[SerializeField]
	private Camera mainCamera;

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
	/// シーン開始と同時にフェードインします。
	/// </summary>
	private void Start() {
		this.fadeInCompleted = false;

		// ビルド後は開始直後にフェーダーを使うとNullReferenceExceptionが出るため、遅延呼び出しする
		this.Invoke("fadeIn", 0.5f);

		if(SelectModeScene.Players == 1) {
			// 一人用のときはアングルを変更する
			this.mainCamera.transform.rotation = Quaternion.Euler(
				this.mainCamera.transform.rotation.eulerAngles.x,
				-13.31f,
				this.mainCamera.transform.rotation.eulerAngles.z
			);

			if(PlayerScore.HighScore < PlayerScore.Scores[0]) {
				// ハイスコア更新
				PlayerPrefs.SetInt("HighScore-" + SelectModeScene.TimeMinutes + ":" + SelectModeScene.TimeSeconds, PlayerScore.Scores[0]);
			}
		}
	}

	/// <summary>
	/// キー入力でも次のシーンへ移る
	/// </summary>
	public void Update() {
		if(this.fadeInCompleted == false) {
			// フェードインが終わっていないときは操作不能にする
			return;
		}
		if(GameObject.Find("BackToTitleButton").GetComponent<Button>().interactable == false) {
			// ボタンが押せなくなっているときはこちらも操作不能にする
			return;
		}

		if(Input.GetKeyDown(KeyCode.Joystick1Button0) == true || Input.GetKeyDown(KeyCode.Return) == true) {
			this.GoTitle();
		}
	}

	/// <summary>
	/// 遅延処理用：フェードインしてシーン開始
	/// </summary>
	private void fadeIn() {
		this.fader.FadeIn(0, () => {
			// フェーダーの初期暗転が完了してからマスクオブジェクトを取ってフェードインを開始する
			GameObject.Find("StartingMask").SetActive(false);

			// 結果発表アナウンスを待ってから開始する
			this.Invoke("FadeIn", 1.5f);
		});
	}

	/// <summary>
	/// 遅延処理用：フェーダー的にはフェードアウトがフェードイン（イミフ）
	/// </summary>
	private void FadeIn() {
		// BGM再生
		GameObject.Find("Canvas").GetComponent<AudioSource>().Play();
		this.fader.FadeOut(1.0f, () => {
			this.fadeInCompleted = true;
		});
	}

	/// <summary>
	/// タイトルに戻ります。
	/// </summary>
	public void GoTitle() {
		if(this.fadeInCompleted == false) {
			// フェードインが終わっていないときは操作不能にする
			return;
		}

		// ボタンを一度押したら押せないようする
		GameObject.Find("BackToTitleButton").GetComponent<Button>().interactable = false;

		// 暗転してからシーン遷移
		GameObject.Find("Decide").GetComponent<AudioSource>().Play();
		this.fader.FadeIn(1.0f, () => {
			SceneManager.LoadScene(0);
		});
	}

}
