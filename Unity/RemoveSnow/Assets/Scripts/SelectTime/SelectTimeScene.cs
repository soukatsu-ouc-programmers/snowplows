using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 制限時間選択
/// </summary>
public class SelectTimeScene : MonoBehaviour {

	/// <summary>
	/// フェードインが完了したかどうか
	/// </summary>
	private bool fadeInCompleted;

	/// <summary>
	/// フェーダー
	/// </summary>
	[SerializeField]
	private Fade fader;

	/// <summary>
	/// ボタン類一式
	/// </summary>
	[SerializeField]
	private GameObject[] buttons;

	/// <summary>
	/// ドロップダウンリスト
	/// </summary>
	[SerializeField]
	private Dropdown timerDropdown;

	/// <summary>
	/// 選択された制限時間のインデックス
	/// </summary>
	static private int selectedIndex;

	/// <summary>
	/// 現在選択中の制限時間：分
	/// </summary>
	static public int TimeMinutes {
		get;
		private set;
	}

	/// <summary>
	/// 現在選択中の制限時間：秒
	/// </summary>
	static public int TimeSeconds {
		get;
		private set;
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public void Start() {
		this.fadeInCompleted = false;
		this.timerDropdown.value = SelectTimeScene.selectedIndex;

		// ビルド後は開始直後にフェーダーを使うとNullReferenceExceptionが出るため、遅延呼び出しする
		this.Invoke("fadeIn", 0.5f);
	}

	/// <summary>
	/// 遅延処理用：フェードインしてシーン開始
	/// </summary>
	private void fadeIn() {
		this.fader.gameObject.SetActive(true);
		this.fader.FadeIn(0, () => {
			GameObject.Find("StartingMask").SetActive(false);
			this.fader.FadeOut(1.0f, () => {
				this.fadeInCompleted = true;
			});
		});
	}

	/// <summary>
	/// プレイヤー操作受付
	/// </summary>
	public void Update() {
		if(this.fadeInCompleted == false) {
			// フェードインが終わっていないときは操作不能にする
			return;
		}
		if(this.buttons.Length > 0 && this.buttons[0].activeSelf == false) {
			// ボタンが無効化されている場合も操作を受け付けない
			return;
		}

		// ステージ選択
		var inputHorizontal = Input.GetAxisRaw("Horizontal");
		if(Input.GetKeyDown(KeyCode.UpArrow) == true
		|| inputHorizontal > 0) {
			if(this.timerDropdown.value > 0) {
				this.timerDropdown.value--;
			}
		}
		if(Input.GetKeyDown(KeyCode.DownArrow) == true
		|| inputHorizontal < 0) {
			if(this.timerDropdown.value < this.timerDropdown.options.Count - 1) {
				this.timerDropdown.value++;
			}
		}

		// 制限時間決定
		if(Input.GetKeyDown(KeyCode.Return) == true
		|| Input.GetKeyDown(KeyCode.Joystick1Button0) == true) {
			this.NextScene();
		}
	}

	/// <summary>
	/// 時間が選択されたときの処理
	/// </summary>
	/// <param name="index"></param>
	public void OnTimeChanged(Dropdown dropdown) {
		if(this.fadeInCompleted == true) {
			GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
			SelectTimeScene.selectedIndex = dropdown.value;
			// Debug.Log("選択index: " + dropdown.value);
		}
	}

	/// <summary>
	/// ゲームを開始する
	/// </summary>
	public void NextScene() {
		if(this.fadeInCompleted == false) {
			// フェードインが終わっていないときは操作不能にする
			return;
		}

		GameObject.Find("DecideSE").GetComponent<AudioSource>().Play();

		// ボタン類一式を無効化
		foreach(var button in this.buttons) {
			button.SetActive(false);
		}

		// 現在選択された制限時間で確定する
		switch(SelectTimeScene.selectedIndex) {
			case 0:
				SelectTimeScene.TimeMinutes = 1;
				SelectTimeScene.TimeSeconds = 30;
				break;

			case 1:
				SelectTimeScene.TimeMinutes = 2;
				SelectTimeScene.TimeSeconds = 0;
				break;

			case 2:
				SelectTimeScene.TimeMinutes = 3;
				SelectTimeScene.TimeSeconds = 0;
				break;

			case 3:
				SelectTimeScene.TimeMinutes = 4;
				SelectTimeScene.TimeSeconds = 0;
				break;

			case 4:
				SelectTimeScene.TimeMinutes = 5;
				SelectTimeScene.TimeSeconds = 0;
				break;

			default:
				SelectTimeScene.TimeMinutes = 1;
				SelectTimeScene.TimeSeconds = 0;
				break;
		}
		// Debug.Log("制限時間 = " + SelectTimeScene.TimeMinutes + ":" + SelectTimeScene.TimeSeconds.ToString("00"));

		// フェードアウトしてシーン遷移
		this.fader.FadeIn(1.0f, () => {
			SceneManager.LoadScene(2);
		});
	}

}
