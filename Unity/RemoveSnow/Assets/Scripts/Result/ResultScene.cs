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
	/// フェーダー
	/// </summary>
	[SerializeField]
	private Fade fader;

	/// <summary>
	/// シーン開始と同時にフェードインします。
	/// </summary>
	private void Start() {
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
		this.fader.FadeOut(1.0f, null);
	}

	/// <summary>
	/// タイトルに戻ります。
	/// </summary>
	public void GoTitle() {
		// ボタンを一度押したら押せないようする
		GameObject.Find("BackToTitleButton").GetComponent<Button>().interactable = false;

		// 暗転してからシーン遷移
		GameObject.Find("Decide").GetComponent<AudioSource>().Play();
		this.fader.FadeIn(1.0f, () => {
			SceneManager.LoadScene(0);
		});
	}

}
