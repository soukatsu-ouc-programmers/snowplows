using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePlayer : MonoBehaviour {

	private GameObject player;

	void Start(){
		player = this.transform.parent.gameObject;

		if (player.gameObject.tag == "Player") {
			player.GetComponent<CarMovePlayerOne> ().isReverse = true;
		}
		if (player.gameObject.tag == "Player2") {
			player.GetComponent<CarMovePlayerTwo> ().isReverse = true;
		}

		StartCoroutine ("OffReverse");

	}

	IEnumerator OffReverse(){
		yield return new WaitForSeconds (10f);
		if (player.gameObject.tag == "Player") {
			player.GetComponent<CarMovePlayerOne> ().isReverse = false;
		}
		if (player.gameObject.tag == "Player2") {
			player.GetComponent<CarMovePlayerTwo> ().isReverse = false;
		}

	}
}
