using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大砲アイテムを取得したときの処理
/// </summary>
public class GetCannon : MonoBehaviour {

	/// <summary>
	/// プレイヤーごとの大砲プレハブ
	/// </summary>
	[SerializeField]
	private GameObject[] playerCannons;

	/// <summary>
	/// アイテムを取得したときの処理
	/// </summary>
	/// <param name="other">接したオブジェクト</param>
	public void OnTriggerEnter(Collider other) {
		if(other.gameObject.name.IndexOf("BigBull") == -1 && PlayerScore.IsPlayerTag(other.gameObject) == true) {
			var player = other.gameObject;
			var parent = player.transform;
			var playerIndex = PlayerScore.PlayerIndexMap[other.gameObject.tag];

			// プレイヤーの所定の場所に大砲をくっつける
			player = other.gameObject;
			Object.Instantiate (this.playerCannons [playerIndex], player.transform.position, player.transform.rotation, parent);
			Object.Destroy(this.gameObject);
		}
	}

}
