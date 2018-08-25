using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBull : MonoBehaviour {

	private GameObject parent;

	// Use this for initialization
	void Start() {
		parent = this.gameObject.transform.parent.parent.gameObject;
		this.gameObject.tag = parent.tag;
		this.GetComponent<FixedJoint>().connectedBody = this.transform.parent.parent.GetComponent<Rigidbody>();
		StartCoroutine("DestroyBull");
	}

	void Update() {
		if(parent.GetComponent<RemoveSnow>().isGetScore == true) {
			this.GetComponent<RemoveSnow>().isGetScore = true;
		}
	}

	IEnumerator DestroyBull() {
		yield return new WaitForSeconds(5f);
		Destroy(this.transform.parent.gameObject);
	}
}
