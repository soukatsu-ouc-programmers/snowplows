using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雪玉発射装置
/// サバイバルモード専用
/// </summary>
public class ShootSnow : MonoBehaviour {

	/// <summary>
	/// 雪玉
	/// </summary>
	// [SerializeField]
	// private GameObject snowBall;

	/// <summary>
	/// 雪玉を発射するプレイヤー
	/// </summary>
	[SerializeField]
	private int playerIndex;

	/// <summary>
	/// 銃口
	/// </summary>
	private GameObject muzzle;

	/// <summary>
	/// 雪玉のパーティクル
	/// </summary>
	private ParticleSystem snowParticles;

	/// <summary>
	/// 初期処理：必要なオブジェクトの取得
	/// </summary>
	public void Start() {
		this.muzzle = this.transform.Find("Muzzle").gameObject;
		this.snowParticles = this.muzzle.transform.Find("SnowBalls").GetComponent<ParticleSystem>();
	}

	/// <summary>
	/// スコアが加算されたときに雪玉を発射します。
	/// </summary>
	public void FixedUpdate() {
		if(PlayerScore.Scores[this.playerIndex] <= 0) {
			// スコアに変動がない状態のときは何もしない
			return;
		}

		// 雪玉（Prefabバージョン）
		// var parent = this.muzzle.transform;
		// Object.Instantiate(this.snowBall, this.muzzle.transform.position, this.muzzle.transform.rotation, parent);
		// this.snowBall.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
		// PlayerScore.Scores[this.playerIndex]--;

		// 雪玉（Particleバージョン）
		this.snowParticles.Emit(1);

		// 発射するたびにスコアをゼロ方向に戻していく
		PlayerScore.Scores[this.playerIndex]--;
	}

}
