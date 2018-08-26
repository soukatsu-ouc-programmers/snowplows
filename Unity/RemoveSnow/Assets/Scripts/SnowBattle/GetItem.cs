using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムを取得したときの処理
/// ただし、大砲は別処理とします。
/// </summary>
public class GetItem : MonoBehaviour {

	/// <summary>
	/// 取得するアイテムのプレハブ
	/// </summary>
	[SerializeField]
	private GameObject items;

	/// <summary>
	/// アイテムを取得したときの処理
	/// </summary>
	/// <param name="other">接したオブジェクト</param>
	public void OnTriggerEnter(Collider other) {
		if(other.gameObject.name.IndexOf("BigBull") == -1 && PlayerScore.IsPlayerTag(other.gameObject) == true) {
			var player = other.gameObject;
			var parent = player.transform;

			// 所定の場所にアイテム効果を付加
			Object.Instantiate(this.items, player.transform.position, player.transform.rotation, parent);
			Object.Destroy(this.gameObject);
		}
	}
}
