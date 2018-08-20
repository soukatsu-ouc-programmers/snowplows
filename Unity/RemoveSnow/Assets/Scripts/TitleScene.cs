using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour {

	/// <summary>
	/// タイトル画面のBGMが一周したら自動的に次のシーンに移る
	/// </summary>
	void Start() {
		this.Invoke("ChangeScene", 90f);
	}
	
	/// <summary>
	/// 次のシーンへ移る
	/// </summary>
	public void ChangeScene() {
		// SE再生
		GameObject.Find("DecideSE").GetComponent<AudioSource>().Play();

		// フェードアウトしてシーン遷移
		GameObject.Find("FadeCanvas").GetComponent<Fade>().FadeIn(1.0f, () => {
			this.StartCoroutine(this.SEWait(1.0f, () => {
				SceneManager.LoadScene(1);
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
