using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMinion : MonoBehaviour {

	/// <summary>
	/// Minionのプレファブ
	/// </summary>
	[SerializeField]
	private GameObject minion;

	/// <summary>
	/// 出現したMinion
	/// </summary>
	private GameObject summonedMinion;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag.IndexOf ("Player") == 0) {
			summonedMinion = Instantiate (minion, minion.transform.position, Quaternion.identity) as GameObject;
			summonedMinion.GetComponent<MinionControl> ().playerIndex = PlayerScore.PlayerIndexMap [other.gameObject.tag];
			Destroy (this.gameObject);
		}

	}

}
