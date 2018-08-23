using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItems : MonoBehaviour {

	/// <summary>
	/// 取得するアイテム効果のプレハブを入れる。
	/// </summary>
	[SerializeField]
	private GameObject items;
	private GameObject player;

	private AudioSource source;
	[SerializeField]
	private AudioClip itemSound;

	void Start(){
		source = this.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.name.IndexOf("BigBull") == -1) {
			if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2") {
				source.PlayOneShot (itemSound);
				player = other.gameObject;
				var parent = player.transform;
				Instantiate (items, player.transform.position, player.transform.rotation, parent);
				Destroy (this.gameObject);
			}
		}
	}
}
