using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour {

	/// <summary>
	/// 一周する角度
	/// </summary>
	const int AllRoundAngle = 360;

	/// <summary>
	/// ステージの数
	/// </summary>
	const int StageCount = 2;

	/// <summary>
	/// １フレーム当たりの回転角度
	/// </summary>
	const float AndleDelta = 5.0f;

	/// <summary>
	/// 現在の回転角度
	/// </summary>
	float CurrentAngle;

	/// <summary>
	/// フェーダー
	/// </summary>
	[SerializeField]
	Fade[] Faders;

	/// <summary>
	/// ステージ名のテキスト
	/// </summary>
	[SerializeField]
	Text StageName;

	/// <summary>
	/// ステージの説明テキスト
	/// </summary>
	[SerializeField]
	Text StageDescription;

	/// <summary>
	/// ステージ名の一覧
	/// </summary>
	[SerializeField]
	string[] StageNames;

	/// <summary>
	/// ステージ説明の一覧（ステージ名と同順）
	/// </summary>
	[SerializeField]
	string[] StageDescriptions;

	/// <summary>
	/// ボタン類一式
	/// </summary>
	[SerializeField]
	GameObject[] Buttons;

	/// <summary>
	/// 現在選択中のステージインデックス
	/// </summary>
	static public int StageIndex {
		get;
		private set;
	}

	/// <summary>
	/// 初期化
	/// </summary>
	void Start() {
		CurrentAngle = float.NaN;

		// フェードイン
		this.Faders[0].gameObject.SetActive(true);
		this.Faders[0].FadeIn(0, () => {
			GameObject.Find("StartingMask").SetActive(false);

			StageIndex = 0;
			StageName.text = this.StageNames[0];
			StageDescription.text = this.StageDescriptions[0];
			var stages = GameObject.Find("Stages");
			for(int i = 0; i < stages.transform.childCount; i++) {
				if(i == 0) {
					// デフォルトで先頭を選択状態
					stages.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
				} else {
					// 非選択状態
					stages.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
				}
			}

			this.Faders[0].FadeOut(1.0f);
		});
	}

	/// <summary>
	/// プレイヤー操作受付
	/// </summary>
	void Update() {
		if(float.IsNaN(CurrentAngle) == false) {
			// ステージ回転中は入力を受け付けない
			return;
		}

		// ステージ選択
		if(Input.GetKeyDown(KeyCode.RightArrow)) {
			this.SelectNextStage();
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			this.SelectPreviousStage();
		}

		// ステージ決定
		if(Input.GetKeyDown(KeyCode.Return)) {
			this.StartGame();
		}
	}

	/// <summary>
	/// 前のステージを選択する
	/// </summary>
	public void SelectPreviousStage() {
		if(float.IsNaN(CurrentAngle) == false) {
			// ステージ回転中は入力を受け付けない
			return;
		}

		// ボタン類一式を無効化
		foreach(var button in this.Buttons) {
			button.SetActive(false);
		}

		GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
		this.StartCoroutine(ChangeStageRight());
	}

	/// <summary>
	/// 前のステージを選択するコルーチン
	/// </summary>
	IEnumerator ChangeStageLeft() {
		StageIndex -= 1;
		if(StageIndex < 0) {
			StageIndex = StageCount - 1;
		}

		for(CurrentAngle = 0; CurrentAngle > -AllRoundAngle / StageCount; CurrentAngle -= AndleDelta) {
			this.transform.Rotate(new Vector3(0, -AndleDelta, 0));

			// ステージ個別のオブジェクトは常に正面を向く
			for(int i = 0; i < this.transform.childCount; i++) {
				Transform obj = this.transform.GetChild(i);
				obj.Rotate(new Vector3(0, AndleDelta, 0));

				// 選択中のステージ以外はグレーアウトする
				if(StageIndex == i) {
					obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
				} else {
					obj.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
				}
			}
			yield return new WaitForEndOfFrame();
		}

		// ステージ名と説明を更新
		this.StageName.text = this.StageNames[StageIndex];
		this.StageDescription.text = this.StageDescriptions[StageIndex];
		this.StageDescription.gameObject.GetComponent<Animator>().Play("StageDescription", 0, 0);

		// ボタン類一式を有効化
		foreach(var button in this.Buttons) {
			button.SetActive(true);
		}

		CurrentAngle = float.NaN;
	}

	/// <summary>
	/// 次のステージを選択する
	/// </summary>
	public void SelectNextStage() {
		if(float.IsNaN(CurrentAngle) == false) {
			// ステージ回転中は入力を受け付けない
			return;
		}

		// ボタン類一式を無効化
		foreach(var button in this.Buttons) {
			button.SetActive(false);
		}

		GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
		this.StartCoroutine(ChangeStageLeft());
	}

	/// <summary>
	/// 次のステージを選択するコルーチン
	/// </summary>
	IEnumerator ChangeStageRight() {
		StageIndex += 1;
		if(StageIndex >= StageCount) {
			StageIndex = 0;
		}

		for(CurrentAngle = 0; CurrentAngle < AllRoundAngle / StageCount; CurrentAngle += AndleDelta) {
			this.transform.Rotate(new Vector3(0, AndleDelta, 0));

			// ステージ個別のオブジェクトは常に正面を向く
			for(int i = 0; i < this.transform.childCount; i++) {
				Transform obj = this.transform.GetChild(i);
				obj.Rotate(new Vector3(0, -AndleDelta, 0));

				// 選択中のステージ以外はグレーアウトする
				if(StageIndex == i) {
					obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
				} else {
					obj.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
				}
			}
			yield return new WaitForEndOfFrame();
		}

		// ステージ名と説明を更新
		this.StageName.text = this.StageNames[StageIndex];
		this.StageDescription.text = this.StageDescriptions[StageIndex];
		this.StageDescription.gameObject.GetComponent<Animator>().Play("StageDescription", 0, 0);

		// ボタン類一式を有効化
		foreach(var button in this.Buttons) {
			button.SetActive(true);
		}

		CurrentAngle = float.NaN;
	}

	/// <summary>
	/// ゲームを開始する
	/// </summary>
	public void StartGame() {
		GameObject.Find("DecideSE").GetComponent<AudioSource>().Play();

		// ボタン類一式を無効化
		foreach(var button in this.Buttons) {
			button.SetActive(false);
		}

		// フェードアウトしてシーン遷移
		this.Faders[1].gameObject.SetActive(true);
		this.Faders[1].FadeIn(1.0f, () => {
			switch(StageIndex) {
				case 0:
					SceneManager.LoadScene(2);
					break;
				case 1:
					SceneManager.LoadScene(4);
					break;
			}
		});
	}

}
