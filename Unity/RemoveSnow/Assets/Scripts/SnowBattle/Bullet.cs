using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム付随品：砲弾
/// </summary>
public class Bullet : MonoBehaviour {

	/// <summary>
	/// 有効時間秒数
	/// </summary
	public const float AvailableTimeSeconds = 2.0f;

	/// <summary>
	/// サバイバルモード時：衝突ダメージ量
	/// </summary>
	public const int PenaltyDamage = 150;

	/// <summary>
	/// 弾のスピード
	/// </summary>
	[SerializeField]
	private float speed = 100f;

	/// <summary>
	/// この弾を発射したプレイヤーの番号
	/// </summary>
	public int playerIndex;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Start() {
		this.playerIndex = PlayerScore.PlayerIndexMap[this.transform.parent.parent.parent.gameObject.tag];
		this.StartCoroutine(this.destroyBullets());
		this.GetComponent<Rigidbody>().AddForce(this.transform.forward * this.speed, ForceMode.VelocityChange);
		this.gameObject.transform.parent = null;
	}

	/// <summary>
	/// コルーチン：一定時間経過後に自身を削除する
	/// </summary>
	private IEnumerator destroyBullets() {
		yield return new WaitForSeconds(Bullet.AvailableTimeSeconds);
		Object.Destroy(this.gameObject);
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
