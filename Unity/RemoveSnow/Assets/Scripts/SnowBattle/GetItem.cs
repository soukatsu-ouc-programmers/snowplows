using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーに付加する持続的なアイテムを取得したときの一般処理
/// </summary>
public class GetItem : MonoBehaviour {

	/// <summary>
	/// 取得するアイテムのプレハブ
	/// </summary>
	[SerializeField]
	private GameObject item;

	/// <summary>
	/// アイテムを取得したときの処理
	/// </summary>
	/// <param name="other">接したオブジェクト</param>
	public void OnTriggerEnter(Collider other) {
		if(other.gameObject.name.IndexOf("BigBull") != -1
		|| PlayerScore.IsPlayerTag(other.gameObject) == false) {
			// BigBullについてしまうとアイテムの効果が正しく付加できなくなる
			return;
		}

		var player = other.gameObject;
		var parent = player.transform;

		// 取得したプレイヤーにアイテム効果を付加
		Object.Instantiate(this.item, player.transform.position, player.transform.rotation, parent);
		Object.Destroy(this.gameObject);
	}

}
