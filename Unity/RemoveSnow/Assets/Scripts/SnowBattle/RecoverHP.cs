using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム：回復
/// サバイバルモード専用
/// </summary>
public class RecoverHP : MonoBehaviour {

	/// <summary>
	/// 回復量
	/// </summary>
	public const int RecoveryAmount = 250;

	/// <summary>
	/// アイテムを取得したときの処理
	/// </summary>
	/// <param name="other">接したオブジェクト</param>
	public void OnTriggerEnter(Collider other) {
		if(PlayerScore.IsPlayerTag(other.gameObject) == false) {
			return;
		}

		var playerIndex = PlayerScore.PlayerIndexMap[other.gameObject.tag];
		PlayerScore.HPs[playerIndex] += RecoverHP.RecoveryAmount;
		if(PlayerScore.HPs[playerIndex] > PlayerScore.MaxHP) {
			// HPカンスト処理
			PlayerScore.HPs[playerIndex] = PlayerScore.MaxHP;
		}

		Object.Destroy(this.gameObject);
	}

}
