using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雪発射のパーティクルにアタッチ。
/// </summary>

public class SnowCollision : MonoBehaviour {

	/// <summary>
	/// この弾を発射したプレイヤーの番号
	/// </summary>
	public int playerIndex;

	/// <summary>
	/// 雪玉のダメージ量
	/// </summary>
	public const int SnowDamege = 10;

	/// <summary>
	/// プレイヤーに雪玉が当たったときの処理
	/// </summary>
	/// <param name="other">接触したオブジェクト</param>
	void OnParticleCollision(GameObject other){
		if(PlayerScore.IsPlayerTag(other.gameObject) == false) {
			return;
		}
		var playerIndex = PlayerScore.PlayerIndexMap[other.gameObject.tag];
		if(playerIndex == this.playerIndex) {
			// 発射した本人への衝突は無効
			return;
		}

			// ダメージを与える
		PlayerScore.HPs[playerIndex] -= SnowCollision.SnowDamege;



	}

}
