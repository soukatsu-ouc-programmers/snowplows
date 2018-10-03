using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionShavel : MonoBehaviour {

	/// <summary>
	/// これを召喚したPlayerの番号
	/// (Minion取得時に送信される)
	/// </summary>
	public int playerIndex = 1;

	/// <summary>
	/// 狙い先
	/// </summary>
	private Transform target;

	/// <summary>
	/// Shavelが当たった時のダメージ
	/// </summary>
	private int minionDamege = 100;

	/// <summary>
	/// Tagをもとに狙い先を決めます。
	/// </summary>
	public void Start() {

		switch(this.playerIndex) {
		case 0:
			this.target = GameObject.FindGameObjectWithTag("Player2").transform;
			break;
		case 1:
			this.target = GameObject.FindGameObjectWithTag("Player").transform;
			break;
		}

		StartCoroutine ("Destroy");

	}

	private void OnCollisionEnter(Collision other){

		if(PlayerScore.IsPlayerTag(other.gameObject) == false) {
			return;
		}

		var playerIndex = PlayerScore.PlayerIndexMap[other.gameObject.tag];

		if(playerIndex == this.playerIndex) {
			// 召喚した本人への衝突は無効
			return;
		}

		PlayerScore.HPs[playerIndex] -= this.minionDamege;
	}
}
