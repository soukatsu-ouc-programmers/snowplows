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
	/// ミニオンを出現させる場所
	/// </summary>
	private Vector3 summonPosition;

	/// <summary>
	/// 出現したMinion
	/// </summary>
	private GameObject summonedMinion;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag.IndexOf ("Player") == 0) {
			summonedMinion = Instantiate (minion, this.transform.position, other.transform.rotation) as GameObject;
			summonedMinion.GetComponent<MinionControl> ().playerIndex = PlayerScore.PlayerIndexMap [other.gameObject.tag];
			Destroy (this.gameObject);
		}

	}

}
