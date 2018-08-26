using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム：混乱
/// </summary>
public class PuzzlePlayer : MonoBehaviour {

	/// <summary>
	/// 使用可能な時間秒数
	/// </summary>
	public const float AvailableTimeSeconds = 10.0f;

	/// <summary>
	/// 取得したプレイヤーの除雪車
	/// </summary>
	private GameObject player;

	/// <summary>
	/// アイテム取得時の初回処理
	/// </summary>
	public void Start() {
		// プレイヤーのゲームオブジェクトを取得
		this.player = this.transform.parent.gameObject;

		if(PlayerScore.IsPlayerTag(this.player) == true) {
			// 操作を反転させるフラグを立てる
			this.player.GetComponent<CarMovePlayer>().isReverse = true;
			this.StartCoroutine(this.offReverse());
		}
	}

	/// <summary>
	/// コルーチン：一定時間経過したら解除します。
	/// </summary>
	private IEnumerator offReverse() {
		yield return new WaitForSeconds(PuzzlePlayer.AvailableTimeSeconds);

		if(PlayerScore.IsPlayerTag(this.player) == true) {
			// 操作を反転させるフラグを解除
			this.player.GetComponent<CarMovePlayer>().isReverse = false;
		}

		Object.Destroy(this.gameObject);
	}

}
