using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム：巨大ブレード
/// </summary>
public class BigBull : MonoBehaviour {

	/// <summary>
	/// 使用可能な時間秒数
	/// </summary>
	private const float AvailableTimeSeconds = 5.0f;

	/// <summary>
	/// 巨大ブレードを装着しているプレイヤーの除雪車
	/// </summary>
	private GameObject parent;

	/// <summary>
	/// 装着時の初回処理
	/// </summary>
	public void Start() {
		this.parent = this.gameObject.transform.parent.parent.gameObject;
		this.gameObject.tag = this.parent.tag;
		this.GetComponent<FixedJoint>().connectedBody = this.transform.parent.parent.GetComponent<Rigidbody>();

		// 一定時間経過後に自動的に解除される
		this.StartCoroutine(this.destroyBull());
	}

	/// <summary>
	/// コルーチン：一定時間経過後に自動で解除します。
	/// </summary>
	private IEnumerator destroyBull() {
		yield return new WaitForSeconds(BigBull.AvailableTimeSeconds);
		Object.Destroy(this.transform.parent.gameObject);
	}

}
