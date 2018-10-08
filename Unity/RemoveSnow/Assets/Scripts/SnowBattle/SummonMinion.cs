using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム：ミニオン（分身）
/// </summary>
public class SummonMinion : MonoBehaviour {

	/// <summary>
	/// Minionのプレハブ
	/// </summary>
	[SerializeField]
	private GameObject minion;

	/// <summary>
	/// アイテム取得処理
	/// </summary>
	/// <param name="other">接したオブジェクト</param>
	public void OnTriggerEnter(Collider other) {
		if(other.gameObject.name.IndexOf("BigBull") != -1
		|| PlayerScore.IsPlayerTag(other.gameObject) == false) {
			// BigBullについてしまうとアイテムの効果が正しく付加できなくなる
			return;
		}

		var summonedMinion = Object.Instantiate(this.minion, this.transform.position, other.transform.rotation) as GameObject;
		summonedMinion.GetComponent<MinionControl>().PlayerIndex = PlayerScore.PlayerIndexMap[other.gameObject.tag];
		Object.Destroy(this.gameObject);
	}

}
