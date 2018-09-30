using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prefab版の雪玉
/// サバイバルモード専用
/// </summary>
public class SnowBall : MonoBehaviour {

	/// <summary>
	/// 発射速度
	/// </summary>
	[SerializeField]
	private float shootSpeed;

	/// <summary>
	/// この弾を発射したプレイヤーの番号
	/// </summary>
	public int playerIndex;

	/// <summary>
	/// サバイバルモード時：衝突ダメージ量
	/// </summary>
	public const int PenaltyDamage = 30;

	/// <summary>
	/// 雪玉を発射します。
	/// </summary>
	public void Start() {
		this.GetComponent<Rigidbody>().AddForce(this.transform.forward * this.shootSpeed, ForceMode.VelocityChange);
		this.StartCoroutine(this.destroySnowBall());
	}

	/// <summary>
	/// コルーチン：発射後一定時間経過したら消します。
	/// </summary>
	private IEnumerator destroySnowBall() {
		yield return new WaitForSeconds(1f);
		GameObject.Destroy(this.gameObject);
	}

	/// <summary>
	/// プレイヤーに砲弾が当たったときの処理
	/// </summary>
	/// <param name="other">接触したオブジェクト</param>
	public void OnCollisionEnter(Collision other) {
		if(PlayerScore.IsPlayerTag(other.gameObject) == false) {
			return;
		}
		var playerIndex = PlayerScore.PlayerIndexMap[other.gameObject.tag];
		if(playerIndex == this.playerIndex) {
			// 発射した本人への衝突は無効
			return;
		}

		// 衝突SEの再生
		GameObject.Find("BulletPenalty").GetComponent<AudioSource>().Play();

		if(SelectModeScene.BattleMode == SelectModeScene.BattleModes.SnowFight) {
			// サバイバルモード時：ダメージを与える
			PlayerScore.HPs[playerIndex] -= Bullet.PenaltyDamage;
		}
	}

}
