using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeParticle : MonoBehaviour {

	/// <summary>
	/// このパーティクルがついているPlayer
	/// </summary>
	private int playerIndex;

	/// <summary>
	/// 瀕死とするHP
	/// </summary>
	private int dying = 200;

	/// <summary>
	/// Smokeパーティクル
	/// </summary>
	private ParticleSystem smoke;

	// Use this for initialization
	void Start () {
		playerIndex = PlayerScore.PlayerIndexMap [this.transform.parent.gameObject.tag];
		smoke = this.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerScore.HPs [playerIndex] <= dying) {

			if (smoke.isPlaying == false) {
				smoke.Play ();
			}

		}
	}
}
