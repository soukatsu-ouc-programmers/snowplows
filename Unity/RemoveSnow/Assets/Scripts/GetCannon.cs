using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCannon : MonoBehaviour {
	
	[SerializeField]
	private GameObject redCannon;

	[SerializeField]
	private GameObject greenCannon;

	private GameObject player;

	private AudioSource source;
	[SerializeField]
	private AudioClip itemSound;

	void Start(){
		source = this.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.name.IndexOf("BigBull") == -1) {
			switch (other.gameObject.tag) {
			case "Player":

				source.PlayOneShot (itemSound);
				player = other.gameObject;
				var parentOne = player.transform;
				Instantiate (redCannon, player.transform.position, player.transform.rotation, parentOne);
				Destroy (this.gameObject);
				break;

			case "Player2":
			
				source.PlayOneShot (itemSound);
				player = other.gameObject;
				var parentTwo = player.transform;
				Instantiate (greenCannon, player.transform.position, player.transform.rotation, parentTwo);
				Destroy (this.gameObject);
				break;

			default:
				break;
			}
		}
	}

}
