using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		this.GetComponent<Rigidbody> ().AddForce (transform.forward * shootSpeed, ForceMode.VelocityChange);
		StartCoroutine ("destroySnowBall");

	}

	IEnumerator destroySnowBall(){
		yield return new WaitForSeconds (1f);
		Destroy (this.gameObject);
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
