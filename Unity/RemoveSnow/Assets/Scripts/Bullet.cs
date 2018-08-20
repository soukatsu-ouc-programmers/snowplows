using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private GameObject muzzleOne;
	private GameObject muzzleTwo;

	/// <summary>
	/// 弾のスピード。
	/// </summary>
	[SerializeField]
	private float speed = 100f;

	// Use this for initialization
	void Start () {
		this.gameObject.transform.parent = null;
		StartCoroutine ("DestroyBullets");
		this.GetComponent<Rigidbody> ().AddForce (transform.forward * speed, ForceMode.VelocityChange);
	}

	IEnumerator DestroyBullets(){
		yield return new WaitForSeconds (1f);
		Destroy (this.gameObject);
	}
}
