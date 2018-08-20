using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 車のがたつきを抑える。
/// </summary>

public class RotationControl : MonoBehaviour {

	private GameObject snowplow;

	[SerializeField]
	private float pressurePower = 5;

	// Use this for initialization
	void Start () {
		snowplow = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (snowplow.transform.localEulerAngles.z != 0) {
			snowplow.GetComponent<Rigidbody> ().AddForce (-transform.up * pressurePower, ForceMode.VelocityChange);
		}
	}
}