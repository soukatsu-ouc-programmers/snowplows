using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour {

	private Transform player;

	/// <summary>
	/// 
	/// </summary>
	[SerializeField]
	private float upSpeed = 0.2f;

	// Use this for initialization
	void Start () {
		//プレイヤーのゲームオブジェクトを親から取得。
		player = this.gameObject.transform.parent;

		//CarMovePlayerのmoveSpeedを書き換え。
		switch (player.gameObject.tag) {
		case "Player":
			player.GetComponent<CarMovePlayerOne> ().moveSpeed = upSpeed;
			break;
		case "Player2":
			player.GetComponent<CarMovePlayerTwo> ().moveSpeed = upSpeed;
			break;
		default:
			break;
		}

		StartCoroutine ("ResetSpeed");
	}

	IEnumerator ResetSpeed(){
		yield return new WaitForSeconds (5f);
		switch (player.gameObject.tag) {
		case "Player":
			player.GetComponent<CarMovePlayerOne> ().moveSpeed = 0.1f;
			break;
		case "Player2":
			player.GetComponent<CarMovePlayerTwo> ().moveSpeed = 0.1f;
			break;
		default:
			break;
		}
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
