using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Muzzleにアタッチ
/// </summary>
public class AutoAim : MonoBehaviour {

	private Transform target;

	private Vector3 targetPosition;

	private Quaternion targetRotation;

	void Start(){
		switch (this.gameObject.transform.parent.tag) {
		case "Player":
			target = GameObject.FindGameObjectWithTag ("Player2").transform;
			break;
		case "Player2":
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			break;
		default:
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		targetPosition = target.transform.position;
		targetRotation = Quaternion.LookRotation (targetPosition - this.gameObject.transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation,Time.deltaTime * 3);
		}
}
