using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCannon : MonoBehaviour {

	[SerializeField]
	private GameObject redCannon;

	[SerializeField]
	private GameObject greenCannon;

	private GameObject player;

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.name.IndexOf("BigBull") == -1) {
			switch(other.gameObject.tag) {
				case "Player":

					player = other.gameObject;
					var parentOne = player.transform;
					Instantiate(redCannon, player.transform.position, player.transform.rotation, parentOne);
					Destroy(this.gameObject);
					break;

				case "Player2":

					player = other.gameObject;
					var parentTwo = player.transform;
					Instantiate(greenCannon, player.transform.position, player.transform.rotation, parentTwo);
					Destroy(this.gameObject);
					break;
			}
		}
	}

}
