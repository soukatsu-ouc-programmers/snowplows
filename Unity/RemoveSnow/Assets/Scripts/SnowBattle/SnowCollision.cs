using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サバイバルモード専用：発射した雪の当たり判定
/// ＊雪発射のパーティクルにアタッチ
/// </summary>
public class SnowCollision : MonoBehaviour {

	/// <summary>
	/// 雪玉のダメージ量
	/// </summary>
	public const int SnowDamege = 10;

	/// <summary>
	/// この雪を発射したプレイヤーの番号
	/// </summary>
	public int playerIndex;

	/// <summary>
	/// 衝突SE
	/// </summary>
	private AudioSource snowballPenaltySE;

	/// <summary>
	/// 初期処理
	/// </summary>
	public void Start() {
		var seObject = GameObject.Find("SnowBallPenalty");
		if(seObject != null) {
			this.snowballPenaltySE = GameObject.Find("SnowBallPenalty").GetComponent<AudioSource>();
		}
	}

	/// <summary>
	/// プレイヤーに雪玉が当たったときの処理
	/// </summary>
	/// <param name="other">接触したオブジェクト</param>
	public void OnParticleCollision(GameObject other) {
		if(PlayerScore.IsPlayerTag(other.gameObject) == false) {
			return;
		}
		var playerIndex = PlayerScore.PlayerIndexMap[other.gameObject.tag];
		if(playerIndex == this.playerIndex) {
			// 発射した本人への衝突は無効
			return;
		}

		// 衝突SEの再生
		this.snowballPenaltySE.Play();

		// ダメージを与える
		PlayerScore.HPs[playerIndex] -= SnowCollision.SnowDamege;
	}

}
