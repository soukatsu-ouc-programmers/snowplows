using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 除雪車から煙を出す演出
/// </summary>
public class SmokeParticle : MonoBehaviour {

	/// <summary>
	/// このパーティクルがついているプレイヤーのインデックス
	/// </summary>
	private int playerIndex;

	/// <summary>
	/// 瀕死とするHP割合
	/// </summary>
	public const float dyingRate = 0.2f;

	/// <summary>
	/// Smokeパーティクル
	/// </summary>
	private ParticleSystem smoke;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Start() {
		this.playerIndex = PlayerScore.PlayerIndexMap[this.transform.parent.gameObject.tag];
		this.smoke = this.GetComponent<ParticleSystem>();
	}

	/// <summary>
	/// このプレイヤーが瀕死になったら煙を出します。
	/// 逆に、瀕死状態から回復したら煙を消します。
	/// </summary>
	public void Update() {
		if(PlayerScore.HPs[this.playerIndex] <= PlayerScore.MaxHP * SmokeParticle.dyingRate) {
			if(this.smoke.isPlaying == false) {
				this.smoke.Play();
			}
		} else {
			if(this.smoke.isPlaying == true) {
				this.smoke.Stop();
			}
		}
	}

}
