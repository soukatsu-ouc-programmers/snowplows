using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム：大砲
/// 大砲にアタッチ
/// </summary>
public class ShootSnow: MonoBehaviour {

	/// <summary>
	/// 砲弾
	/// </summary>
	[SerializeField]
	private GameObject snowBall;

	private int remainSnow;

	/// <summary>
	/// 銃口
	/// </summary>
	private GameObject muzzle;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Start() {
		this.muzzle = this.transform.Find("Muzzle").gameObject;
	}

	/// <summary>
	/// 射撃
	/// </summary>
	public void FixedUpdate() {

		remainSnow = PlayerScore.Scores [0];

		if (remainSnow <= 0) {
			return;
		}

		if (remainSnow >= 0) {
			var parent = this.muzzle.transform;
			Object.Instantiate (this.snowBall, this.muzzle.transform.position, this.muzzle.transform.rotation,parent);
			this.snowBall.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
			PlayerScore.Scores [0]--;
		}

	}
}
