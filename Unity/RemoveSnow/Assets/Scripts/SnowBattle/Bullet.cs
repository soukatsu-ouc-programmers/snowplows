using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム付随品：砲弾
/// </summary>
public class Bullet : MonoBehaviour {

	/// <summary>
	/// 有効時間秒数
	/// </summary>
	public const float AvailableTimeSeconds = 1.0f;

	/// <summary>
	/// 弾のスピード
	/// </summary>
	[SerializeField]
	private float speed = 100f;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Start() {
		this.gameObject.transform.parent = null;
		this.StartCoroutine(this.destroyBullets());
		this.GetComponent<Rigidbody>().AddForce(this.transform.forward * this.speed, ForceMode.VelocityChange);
	}

	/// <summary>
	/// コルーチン：一定時間経過後に自身を削除する
	/// </summary>
	private IEnumerator destroyBullets() {
		yield return new WaitForSeconds(Bullet.AvailableTimeSeconds);
		Object.Destroy(this.gameObject);
	}

}
