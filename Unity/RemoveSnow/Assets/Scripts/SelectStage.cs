using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		StageIndex = 0;
		CurrentAngle = float.NaN;

		// フェードイン
		this.Faders[0].gameObject.SetActive(true);
		this.Faders[0].FadeIn(0, () => {
			GameObject.Find("StartingMask").SetActive(false);
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
			this.StartCoroutine(ChangeStageRight());
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			this.StartCoroutine(ChangeStageLeft());
		}

		// ステージ決定
		if(Input.GetKeyDown(KeyCode.Return)) {
			// SE再生
			GameObject.Find("DecideSE").GetComponent<AudioSource>().Play();

			// フェードアウトしてシーン遷移
			this.Faders[1].gameObject.SetActive(true);
			this.Faders[1].FadeIn(1.0f, () => {
				switch(StageIndex){
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

	/// <summary>
	/// 次のステージを選択する
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

		CurrentAngle = float.NaN;
	}

	/// <summary>
	/// 前のステージを選択する
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

		CurrentAngle = float.NaN;
	}

}
