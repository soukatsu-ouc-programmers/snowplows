using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム：スピードアップ
/// </summary>
public class SpeedUp : MonoBehaviour {

	/// <summary>
	/// 使用可能な時間秒数
	/// </summary>
	public const float AvailableTimeSeconds = 5.0f;

	/// <summary>
	/// 取得したプレイヤーの除雪車
	/// </summary>
	private Transform player;

	/// <summary>
	/// 上昇後の移動速度
	/// </summary>
	[SerializeField]
	private float upSpeed = 0.2f;

	/// <summary>
	/// アイテム取得時の初回処理
	/// </summary>
	public void Start() {
		// プレイヤーのゲームオブジェクトを取得
		this.player = this.gameObject.transform.parent;

		// 取得したプレイヤーの除雪車の移動速度を更新
		if(PlayerScore.IsPlayerTag(this.player.gameObject) == true) {
			this.player.GetComponent<CarMovePlayer>().MoveSpeed = this.upSpeed;
			this.StartCoroutine(this.resetSpeed());
		}
	}

	/// <summary>
	/// コルーチン：一定時間経過したら解除します。
	/// </summary>
	private IEnumerator resetSpeed() {
		yield return new WaitForSeconds(SpeedUp.AvailableTimeSeconds);

		if(PlayerScore.IsPlayerTag(this.player.gameObject) == true) {
			this.player.GetComponent<CarMovePlayer>().MoveSpeed = 0.1f;
		}

		Object.Destroy(this.gameObject);
	}

}
