using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 砲弾に当たった雪だるまが壊れる演出
/// </summary>
public class ApartSnowman : MonoBehaviour {

	/// <summary>
	/// 雪だるまの原型サイズ
	/// </summary>
	[SerializeField]
	private Vector3 snowmanScale;

	/// <summary>
	/// 雪だるまプレハブ
	/// </summary>
	[SerializeField]
	private GameObject apartSnowman;

	/// <summary>
	/// 砲弾に当たったときに壊れる演出を行います。
	/// </summary>
	/// <param name="other">ぶつかった対象のオブジェクト</param>
	public void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Bullet") {
			// 元々あった合体済み雪だるまをRigidbody付きの雪だるまに差し替えて自然とバラバラになるようにする
			var fixedSnowman = this.gameObject.transform.parent;
			var apart = Object.Instantiate(
				this.apartSnowman,
				fixedSnowman.position,
				Quaternion.identity,
				GameObject.FindGameObjectWithTag("Ground").transform
			);

			// 原型を削除
			Object.Destroy(this.transform.parent.gameObject);

			// バラバラ雪だるまのスケールを合わせる
			apart.transform.localScale = fixedSnowman.localScale;
		}
	}

}
