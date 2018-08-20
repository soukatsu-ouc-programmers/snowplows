using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBigBull : MonoBehaviour {
	
	/// <summary>
	/// 取得するアイテム効果のプレハブを入れる。
	/// </summary>
	[SerializeField]
	private GameObject items;

	private GameObject player;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2") {
			player = other.gameObject;
			var parent = player.transform;
			Instantiate (items, player.transform.position, player.transform.rotation, parent);
			Destroy (this.gameObject);
		}
	}
}
