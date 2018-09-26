using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverHP : MonoBehaviour {

	private const int recoveryAmount = 100;

	void OnTriggerEnter(Collider other){
		var playerIndex = PlayerScore.PlayerIndexMap [other.gameObject.tag];
		PlayerScore.HPs [playerIndex] += recoveryAmount;
		Object.Destroy(this.gameObject);
	}

}
