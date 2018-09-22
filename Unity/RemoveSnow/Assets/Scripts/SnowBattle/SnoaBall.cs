using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnoaBall : MonoBehaviour {

	[SerializeField]
	private float shootSpeed;

	// Use this for initialization
	void Start () {
		this.GetComponent<Rigidbody> ().AddForce (transform.forward * shootSpeed, ForceMode.VelocityChange);
		StartCoroutine ("destroySnowBall");

	}

	IEnumerator destroySnowBall(){
		yield return new WaitForSeconds (1f);
		Destroy (this.gameObject);
	}
	
}
