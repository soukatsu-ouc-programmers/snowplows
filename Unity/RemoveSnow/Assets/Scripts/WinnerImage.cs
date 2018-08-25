using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコアの優勢、劣勢をわかりやすくする。
/// BattleCanvasにアタッチ。
/// </summary>
public class WinnerImage : MonoBehaviour {

	[SerializeField]
	private GameObject playerOneImage;
	[SerializeField]
	private GameObject playerTwoImage;

	/// <summary>
	/// 勝敗状態
	/// </summary>
	private enum WinnerPlayerState {
		Draw,
		Player1,
		Player2,
	}

	/// <summary>
	/// 勝敗状態の管理
	/// </summary>
	private WinnerPlayerState state;

	private void Start() {
		state = WinnerPlayerState.Draw;
	}

	// Update is called once per frame
	void Update() {
		// 注意：Animatorのトリガーセットするとき、他のトリガーをすべて解除しないとタイミングがずれておかしなことになる

		if(Score.isHideScore == true) {
			if(state != WinnerPlayerState.Draw) {
				resetAnimatorTriggers(playerOneImage.GetComponent<Animator>());
				resetAnimatorTriggers(playerTwoImage.GetComponent<Animator>());
				playerOneImage.GetComponent<Animator>().SetTrigger("Nutral");
				playerTwoImage.GetComponent<Animator>().SetTrigger("Nutral");
				state = WinnerPlayerState.Draw;
				Debug.Log("プレイヤー 優劣非表示");
			}
			return;
		}

		if(RemoveSnow.ScoreOne > RemoveSnow.ScoreTwo) {
			if(state != WinnerPlayerState.Player1) {
				resetAnimatorTriggers(playerOneImage.GetComponent<Animator>());
				resetAnimatorTriggers(playerTwoImage.GetComponent<Animator>());
				playerOneImage.GetComponent<Animator>().SetTrigger("Winner");
				playerTwoImage.GetComponent<Animator>().SetTrigger("Loser");
				state = WinnerPlayerState.Player1;
				Debug.Log("プレイヤー1 優勢");
			}
		}
		if(RemoveSnow.ScoreOne < RemoveSnow.ScoreTwo) {
			if(state != WinnerPlayerState.Player2) {
				resetAnimatorTriggers(playerOneImage.GetComponent<Animator>());
				resetAnimatorTriggers(playerTwoImage.GetComponent<Animator>());
				playerOneImage.GetComponent<Animator>().SetTrigger("Loser");
				playerTwoImage.GetComponent<Animator>().SetTrigger("Winner");
				state = WinnerPlayerState.Player2;
				Debug.Log("プレイヤー2 優勢");
			}
		}
		if(RemoveSnow.ScoreOne == RemoveSnow.ScoreTwo) {
			if(state != WinnerPlayerState.Draw) {
				resetAnimatorTriggers(playerOneImage.GetComponent<Animator>());
				resetAnimatorTriggers(playerTwoImage.GetComponent<Animator>());
				playerOneImage.GetComponent<Animator>().SetTrigger("Nutral");
				playerTwoImage.GetComponent<Animator>().SetTrigger("Nutral");
				state = WinnerPlayerState.Draw;
				Debug.Log("プレイヤー 拮抗");
			}
		}
	}

	/// <summary>
	/// Animatorのトリガーをすべて初期化します
	/// </summary>
	/// <param name="animator">初期化対象のAnimator</param>
	private void resetAnimatorTriggers(Animator animator) {
		animator.ResetTrigger("Loser");
		animator.ResetTrigger("Winner");
		animator.ResetTrigger("Nutral");
	}
}
