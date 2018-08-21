using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartSnowman : MonoBehaviour {

	[SerializeField]
	private GameObject apartSnowman;

	void OnCollisionEnter(Collision other){

		if (other.gameObject.tag == "Bullet") {
			Instantiate (apartSnowman, this.gameObject.transform.position, Quaternion.identity,GameObject.Find("DosankoCity").transform);
			Destroy (this.transform.parent.gameObject);
		}

	}

}
