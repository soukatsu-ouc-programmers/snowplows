using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		switch (other.gameObject.tag) {
		case "Player":
			other.gameObject.transform.position = new Vector3 (4f, 2f, 2f);
			other.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			other.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			other.gameObject.transform.rotation = new Quaternion (0, 0, 0, 0);
			if (RemoveSnow.ScoreOne >= 100) {
				RemoveSnow.ScoreOne = RemoveSnow.ScoreOne - 100;
			} else {
				RemoveSnow.ScoreOne = 0;
			}
			break;
		case "Player2":
			other.gameObject.transform.position = new Vector3 (21f, 2f, 2f);
			other.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			other.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			other.gameObject.transform.rotation = new Quaternion (0, 0, 0, 0);
			if (RemoveSnow.ScoreTwo >= 100) {
				RemoveSnow.ScoreTwo = RemoveSnow.ScoreTwo - 100;
			} else {
				RemoveSnow.ScoreTwo = 0;
			}

			break;
		}
	}
}
