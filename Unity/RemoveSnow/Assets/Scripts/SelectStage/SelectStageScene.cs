﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ステージ選択
/// </summary>
public class SelectStageScene : MonoBehaviour {

	/// <summary>
	/// 一周する角度
	/// </summary>
	public const int AllRoundAngle = 360;

	/// <summary>
	/// ステージの数
	/// </summary>
	public const int StageCount = 2;

	/// <summary>
	/// １フレーム当たりの回転角度
	/// </summary>
	public const float AndleDelta = 5.0f;

	/// <summary>
	/// 現在の回転角度
	/// </summary>
	private float currentAngle;

	/// <summary>
	/// フェーダー
	/// </summary>
	[SerializeField]
	private Fade[] faders;

	/// <summary>
	/// ステージ名のテキスト
	/// </summary>
	[SerializeField]
	private Text stageName;

	/// <summary>
	/// ステージの説明テキスト
	/// </summary>
	[SerializeField]
	private Text stageDescription;

	/// <summary>
	/// ステージ名の一覧
	/// </summary>
	[SerializeField]
	private string[] stageNames;

	/// <summary>
	/// ステージ説明の一覧（ステージ名と同順）
	/// </summary>
	[SerializeField]
	private string[] stageDescriptions;

	/// <summary>
	/// ボタン類一式
	/// </summary>
	[SerializeField]
	private GameObject[] buttons;

	/// <summary>
	/// ステージインデックスに応じたシーン番号
	/// </summary>
	[SerializeField]
	private int[] stageSceneIDs;

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
	public void Start() {
		this.currentAngle = float.NaN;

		// フェードイン
		this.faders[0].gameObject.SetActive(true);
		this.faders[0].FadeIn(0, () => {
			GameObject.Find("StartingMask").SetActive(false);

			// 初期化
			SelectStageScene.StageIndex = 0;
			this.stageName.text = this.stageNames[0];
			this.stageDescription.text = this.stageDescriptions[0];
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

			this.faders[0].FadeOut(1.0f);
		});
	}

	/// <summary>
	/// プレイヤー操作受付
	/// </summary>
	public void Update() {
		if(float.IsNaN(this.currentAngle) == false) {
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
		if(float.IsNaN(this.currentAngle) == false) {
			// ステージ回転中は入力を受け付けない
			return;
		}

		// ボタン類一式を無効化
		foreach(var button in this.buttons) {
			button.SetActive(false);
		}

		GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
		this.StartCoroutine(changeStageRight());
	}

	/// <summary>
	/// コルーチン：前のステージを選択します。
	/// </summary>
	private IEnumerator changeStageLeft() {
		StageIndex -= 1;
		if(StageIndex < 0) {
			StageIndex = StageCount - 1;
		}

		for(this.currentAngle = 0; this.currentAngle > -AllRoundAngle / StageCount; this.currentAngle -= AndleDelta) {
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
		this.stageName.text = this.stageNames[StageIndex];
		this.stageDescription.text = this.stageDescriptions[StageIndex];
		this.stageDescription.gameObject.GetComponent<Animator>().Play("StageDescription", 0, 0);

		// ボタン類一式を有効化
		foreach(var button in this.buttons) {
			button.SetActive(true);
		}

		this.currentAngle = float.NaN;
	}

	/// <summary>
	/// 次のステージを選択する
	/// </summary>
	public void SelectNextStage() {
		if(float.IsNaN(this.currentAngle) == false) {
			// ステージ回転中は入力を受け付けない
			return;
		}

		// ボタン類一式を無効化
		foreach(var button in this.buttons) {
			button.SetActive(false);
		}

		GameObject.Find("SelectSE").GetComponent<AudioSource>().Play();
		this.StartCoroutine(changeStageLeft());
	}

	/// <summary>
	/// コルーチン：次のステージを選択します。
	/// </summary>
	private IEnumerator changeStageRight() {
		StageIndex += 1;
		if(StageIndex >= StageCount) {
			StageIndex = 0;
		}

		for(this.currentAngle = 0; this.currentAngle < AllRoundAngle / StageCount; this.currentAngle += AndleDelta) {
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
		this.stageName.text = this.stageNames[StageIndex];
		this.stageDescription.text = this.stageDescriptions[StageIndex];
		this.stageDescription.gameObject.GetComponent<Animator>().Play("StageDescription", 0, 0);

		// ボタン類一式を有効化
		foreach(var button in this.buttons) {
			button.SetActive(true);
		}

		this.currentAngle = float.NaN;
	}

	/// <summary>
	/// ゲームを開始する
	/// </summary>
	public void StartGame() {
		GameObject.Find("DecideSE").GetComponent<AudioSource>().Play();

		// ボタン類一式を無効化
		foreach(var button in this.buttons) {
			button.SetActive(false);
		}

		// フェードアウトしてシーン遷移
		this.faders[1].gameObject.SetActive(true);
		this.faders[1].FadeIn(1.0f, () => {
			SceneManager.LoadScene(this.stageSceneIDs[SelectStageScene.StageIndex]);
		});
	}

}