using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackTitle : MonoBehaviour {

	/// <summary>
	/// フェーダー
	/// </summary>
	[SerializeField] Fade Fader;

	private void Start() {
		// フェード解除
		this.Fader.FadeIn(0, () => {
			GameObject.Find("StartingMask").SetActive(false);
			this.Fader.FadeOut(1.0f, null);
		});
	}

	public void GoTitle() {
		// フェード処理
		GameObject.Find("Decide").GetComponent<AudioSource>().Play();
		this.Fader.FadeIn(1.0f, () => {
			SceneManager.LoadScene("TitleScene");
		});
	}

}
