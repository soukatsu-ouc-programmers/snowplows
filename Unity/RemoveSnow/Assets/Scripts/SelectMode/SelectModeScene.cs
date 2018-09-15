using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ゲーム設定
/// </summary>
public class SelectModeScene : MonoBehaviour {

	/// <summary>
	/// 最大プレイヤー人数
	/// </summary>
	public const int MaxPlayerCount = 2;

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
	/// 設定項目ごとのカーソルアニメーター
	/// </summary>
	[SerializeField]
	private Animator[] cursorAnimators;

	/// <summary>
	/// ドロップダウンリスト：制限時間
	/// </summary>
	[SerializeField]
	private Dropdown timerDropdown;

	/// <summary>
	/// ドロップダウンリスト：プレイヤー人数
	/// </summary>
	[SerializeField]
	private Dropdown playersDropdown;

	/// <summary>
	/// ドロップダウンリスト：バトルモード
	/// </summary>
	[SerializeField]
	private Dropdown battleModeDropdown;

	/// <summary>
	/// ドロップダウンリスト：アイテムモード
	/// </summary>
	[SerializeField]
	private Dropdown itemModeDropdown;

	/// <summary>
	/// すべてのドロップダウン設定項目
	/// </summary>
	private Dropdown[] dropdowns;

	/// <summary>
	/// 現在設定中の設定項目の番号（0=制限時間・1=人数・2=バトルモード・3=アイテムモード）
	/// これに該当する設定項目がキーボードから変更できるようにします。
	/// </summary>
	private int settingIndex {
		get {
			return this._settingIndex;
		}
		set {
			if(value < -1) {
				return;
			}
			if(this.dropdowns.Length <= value) {
				return;
			}

			int beforeIndex = this._settingIndex;
			this._settingIndex = value;

			// アニメーション更新
			foreach(var animator in this.cursorAnimators) {
				animator.ResetTrigger("CursorActive");
				animator.ResetTrigger("CursorNonActive");
			}
			if(beforeIndex != -1) {
				this.cursorAnimators[beforeIndex].SetTrigger("CursorNonActive");
			}
			if(value != -1) {
				this.cursorAnimators[value].SetTrigger("CursorActive");
			}
		}
	}
	private int _settingIndex = -1;

	/// <summary>
	/// 選択された制限時間のインデックス
	/// </summary>
	static private int selectedTimeIndex = 0;

	/// <summary>
	/// 現在選択中のプレイヤー人数のインデックス
	/// </summary>
	static private int selectedPeopleIndex = 1;

	/// <summary>
	/// 選択されたバトルモードのインデックス
	/// </summary>
	static private int selectedBattleModeIndex = 0;

	/// <summary>
	/// 選択されたアイテムモードのインデックス
	/// </summary>
	static private int selectedItemModeIndex = 0;

	/// <summary>
	/// 現在選択中の制限時間：分
	/// </summary>
	static public int TimeMinutes {
		get {
			return SelectModeScene._timeMinutes;
		}
		private set {
			SelectModeScene._timeMinutes = value;
		}
	}
	static private int _timeMinutes = 1;

	/// <summary>
	/// 現在選択中の制限時間：秒
	/// </summary>
	static public int TimeSeconds {
		get {
			return SelectModeScene._timeSeconds;
		}
		private set {
			SelectModeScene._timeSeconds = value;
		}
	}
	static private int _timeSeconds = 30;

	/// <summary>
	/// 現在選択中のプレイヤー人数
	/// </summary>
	static public int Players {
		get {
			return SelectModeScene._players;
		}
		private set {
			SelectModeScene._players = value;
		}
	}
	static private int _players = 2;

	/// <summary>
	/// 現在選択中のバトルモード
	/// </summary>
	static public BattleModes BattleMode {
		get {
			return SelectModeScene._battleMode;
		}
		private set {
			SelectModeScene._battleMode = value;
		}
	}
	static private BattleModes _battleMode = BattleModes.ShavedIce;

	/// <summary>
	/// バトルモード
	/// </summary>
	public enum BattleModes {
		ShavedIce,  // かき氷集め＝ブレード型除雪車
		SnowFight,  // 雪合戦＝ロータリー型除雪車
	}

	/// <summary>
	/// 現在選択中のアイテムモード
	/// </summary>
	static public ItemModes ItemMode {
		get {
			return SelectModeScene._itemMode;
		}
		set {
			SelectModeScene._itemMode = value;
		}
	}
	static private ItemModes _itemMode = ItemModes.Normal;

	/// <summary>
	/// アイテムモード
	/// </summary>
	public enum ItemModes {
		Normal,     // 通常の時間間隔
		Chaos,      // 短い時間間隔
		None,       // 出てこない
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public void Start() {
		this.fadeInCompleted = false;

		// 選択状態を初期化 or 前回のものを引き継ぐ
		this.timerDropdown.value = SelectModeScene.selectedTimeIndex;
		this.OnTimeChanged(this.timerDropdown.value);
		this.playersDropdown.value = SelectModeScene.selectedPeopleIndex;
		this.OnPlayersChanged(this.playersDropdown.value);
		this.battleModeDropdown.value = SelectModeScene.selectedBattleModeIndex;
		this.OnBattleModeChanged(this.battleModeDropdown.value);
		this.itemModeDropdown.value = SelectModeScene.selectedItemModeIndex;
		this.OnItemModeChanged(this.itemModeDropdown.value);

		// すべてのドロップダウン設定項目をまとめる
		this.dropdowns = new Dropdown[] {
			this.timerDropdown,
			this.playersDropdown,
			this.battleModeDropdown,
			this.itemModeDropdown,
		};
		this.settingIndex = 0;

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

		// 現在有効な設定項目を変更
		var inputVertical = Input.GetAxisRaw("Vertical");
		if(Input.GetKeyDown(KeyCode.LeftArrow) == true
		|| inputVertical > 0) {
			if(this.dropdowns[this.settingIndex].value > 0) {
				this.dropdowns[this.settingIndex].value--;
			}
		}
		if(Input.GetKeyDown(KeyCode.RightArrow) == true
		|| inputVertical < 0) {
			if(this.dropdowns[this.settingIndex].value < this.dropdowns[this.settingIndex].options.Count - 1) {
				this.dropdowns[this.settingIndex].value++;
			}
		}

		// 選択項目の切り替え
		var inputHorizontal = Input.GetAxisRaw("Horizontal");
		if(Input.GetKeyDown(KeyCode.DownArrow) == true
		|| inputHorizontal > 0) {
			if(this.settingIndex + 1 < this.dropdowns.Length) {
				// 次の選択項目へ
				this.settingIndex++;
				GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
			}
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) == true
		|| inputHorizontal < 0) {
			if(this.settingIndex - 1 >= 0) {
				// 前の設定項目へ
				this.settingIndex--;
				GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
			}
		}

		// ステージ選択へ
		if(Input.GetKeyDown(KeyCode.Return) == true
		|| Input.GetKeyDown(KeyCode.Joystick1Button0) == true) {
			this.NextScene();
		}
	}

	/// <summary>
	/// 制限時間が直接選択されたときの処理
	/// </summary>
	/// <param name="value">選択後の値</param>
	public void OnTimeChanged(int value) {
		EventSystem.current.SetSelectedGameObject(null);
		if(this.fadeInCompleted == true) {
			GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
			SelectModeScene.selectedTimeIndex = value;
		}

		// ハイスコアを読み込む
		int highScore = PlayerPrefs.GetInt("HighScore-" + this.getTimeMinutes() + ":" + this.getTimeSeconds(), 0);
		GameObject.Find("HighScore").GetComponent<Text>().text = highScore.ToString();

		// Debug.Log("制限時間変更 -> 選択index: " + value);
	}

	/// <summary>
	/// プレイヤー人数が直接選択されたときの処理
	/// </summary>
	/// <param name="value">選択後の値</param>
	public void OnPlayersChanged(int value) {
		EventSystem.current.SetSelectedGameObject(null);
		if(this.fadeInCompleted == true) {
			GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
			SelectModeScene.selectedPeopleIndex = value;
		}

		if(SelectModeScene.selectedPeopleIndex == 0
		&& SelectModeScene.selectedBattleModeIndex == (int)BattleModes.SnowFight) {
			// 一人なのにサバイバルモードのときはバトルモードを変える
			Debug.Log("バトルモード強制変更");
			this.battleModeDropdown.value = (int)BattleModes.ShavedIce;
		}

		if(SelectModeScene.selectedPeopleIndex == 0
		&& SelectModeScene.selectedItemModeIndex != (int)ItemModes.Normal) {
			// 一人なのに通常モード以外のときはアイテムモードを変える
			Debug.Log("アイテムモード強制変更");
			this.itemModeDropdown.value = (int)ItemModes.Normal;
		}

		var highScorePanel = GameObject.Find("HighScorePanel");
		if(SelectModeScene.selectedPeopleIndex == 0) {
			// 一人用のときはハイスコアを表示
			highScorePanel.transform.localScale = Vector3.one;
		} else {
			// 複数人のときはハイスコアを非表示
			highScorePanel.transform.localScale = Vector3.zero;
		}

		// Debug.Log("プレイヤー人数変更 -> 選択index: " + value);
	}

	/// <summary>
	/// バトルモードが直接選択されたときの処理
	/// </summary>
	/// <param name="value">選択後の値</param>
	public void OnBattleModeChanged(int value) {
		EventSystem.current.SetSelectedGameObject(null);
		if(this.fadeInCompleted == true) {
			GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
			SelectModeScene.selectedBattleModeIndex = value;
		}

		// サバイバルモードなのに一人のときは人数を変える
		if(SelectModeScene.selectedPeopleIndex == 0
		&& SelectModeScene.selectedBattleModeIndex == (int)BattleModes.SnowFight) {
			Debug.Log("プレイヤー人数強制変更");
			this.playersDropdown.value = 1;
		}

		// Debug.Log("バトルモード変更 -> 選択index: " + value);
	}

	/// <summary>
	/// アイテムモードが直接選択されたときの処理
	/// </summary>
	/// <param name="value">選択後の値</param>
	public void OnItemModeChanged(int value) {
		EventSystem.current.SetSelectedGameObject(null);
		if(this.fadeInCompleted == true) {
			GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
			SelectModeScene.selectedItemModeIndex = value;
		}

		// 一人モードのときはNORMAL以外選択できないようにする
		if(SelectModeScene.selectedPeopleIndex == 0
		&& SelectModeScene.selectedItemModeIndex != (int)ItemModes.Normal) {
			Debug.Log("プレイヤー人数強制変更");
			this.playersDropdown.value = 1;
		}

		// Debug.Log("アイテムモード変更 -> 選択index: " + value);
	}

	/// <summary>
	/// ゲームを開始する
	/// </summary>
	public void NextScene() {
		EventSystem.current.SetSelectedGameObject(null);
		if(this.fadeInCompleted == false) {
			// フェードインが終わっていないときは操作不能にする
			return;
		}

		this.settingIndex = -1;
		GameObject.Find("DecideSE").GetComponent<AudioSource>().Play();

		// ボタン類一式を無効化
		foreach(var button in this.buttons) {
			button.SetActive(false);
		}

		// ドロップダウンを選択不能にする
		foreach(var dropdown in this.dropdowns) {
			dropdown.interactable = false;
		}

		// 現在選択されている制限時間で確定する
		SelectModeScene.TimeMinutes = this.getTimeMinutes();
		SelectModeScene.TimeSeconds = this.getTimeSeconds();

		// Debug.Log("制限時間 = " + SelectTimeScene.TimeMinutes + ":" + SelectTimeScene.TimeSeconds.ToString("00"));

		// 現在選択されているプレイヤー人数で確定する
		SelectModeScene.Players = SelectModeScene.selectedPeopleIndex + 1;
		// Debug.Log("プレイヤー人数 = " + SelectModeScene.Players);

		// 現在選択されているバトルモードで確定する
		SelectModeScene.BattleMode = (BattleModes)SelectModeScene.selectedBattleModeIndex;
		// Debug.Log("バトルモード = " + SelectModeScene.BattleMode);

		// 現在選択されているアイテムモードで確定する
		SelectModeScene.ItemMode = (ItemModes)SelectModeScene.selectedItemModeIndex;
		// Debug.Log("アイテムモード = " + SelectModeScene.ItemMode);

		// フェードアウトしてシーン遷移
		this.fader.FadeIn(1.0f, () => {
			SceneManager.LoadScene((int)SceneIDs.SelectStage);
		});
	}

	/// <summary>
	/// 現在選択されている制限時間の "分" を取得します。
	/// </summary>
	/// <returns>分数</returns>
	private int getTimeMinutes() {
		switch(SelectModeScene.selectedTimeIndex) {
			case 0:
				return 1;
			case 1:
				return 2;
			case 2:
				return 3;
			case 3:
				return 4;
			case 4:
				return 5;
			default:
				return 1;
		}
	}

	/// <summary>
	/// 現在選択されている制限時間の "秒" を取得します。
	/// </summary>
	/// <returns>秒数</returns>
	private int getTimeSeconds() {
		switch(SelectModeScene.selectedTimeIndex) {
			case 0:
				return 30;
			case 1:
				return 0;
			case 2:
				return 0;
			case 3:
				return 0;
			case 4:
				return 0;
			default:
				return 0;
		}
	}

}
