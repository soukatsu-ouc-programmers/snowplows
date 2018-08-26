using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 車のガタつきを抑える処理
/// ＊逆に常時細かいガタつきが付いてしまうため廃止
/// </summary>
public class RotationControl : MonoBehaviour {

	/// <summary>
	/// 対象のプレイヤー除雪車
	/// </summary>
	private GameObject snowplow;

	/// <summary>
	/// ガタつきを自動的に抑える力の大きさ
	/// </summary>
	[SerializeField]
	private float pressurePower = 5;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Start() {
		this.snowplow = this.gameObject;
	}

	/// <summary>
	/// ガタつきを抑える
	/// </summary>
	public void Update() {
		if(this.snowplow.transform.localEulerAngles.z != 0) {
			this.snowplow.GetComponent<Rigidbody>().AddForce(-this.transform.up * this.pressurePower, ForceMode.VelocityChange);
		}
	}

}