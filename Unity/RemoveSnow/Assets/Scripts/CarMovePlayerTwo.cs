﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player2の除雪車にアタッチ。
/// </summary>
public class CarMovePlayerTwo : MonoBehaviour {
	
	/// <summary>
	/// 横方向の入力。(A, D)
	/// </summary>
	public float inputHorizontal;

	/// <summary>
	/// 縦方向の入力。(W, S)
	/// </summary>
	public float inputVertical;

	/// <summary>
	/// 除雪車のRigidBody。
	/// </summary>
	private Rigidbody carRigidbody;

	/// <summary>
	/// 除雪車の進むスピード。
	/// </summary>
	[SerializeField]
	public float moveSpeed = 3f;

	/// <summary>
	/// 除雪車の回転スピード。
	/// </summary>
	[SerializeField]
	private float rotateSpeed = 3f;

	/// <summary>
	/// ジャンプ力ぅ・・・ですかねぇ・・・。
	/// </summary>
	[SerializeField]
	private float jumpPower = 1f;

	/// <summary>
	/// 地面に触れているかのフラグ。
	/// Trueなら触れている。Falseなら触れてない。
	/// </summary>
	private bool isOnGround;

	/// <summary>
	/// 混乱アイテムを取った際のフラグ。
	/// trueなら操作反転。
	/// </summary>
	public bool isReverse;

	/// <summary>
	/// 除雪車の重心。
	/// </summary>
	private Vector3 center = new Vector3 (0f,-3f,0.5f);


	/// <summary>
	/// 除雪車のRigidBodyを取得。
	/// </summary>
	void Start () {
		carRigidbody = this.GetComponent<Rigidbody> ();
		carRigidbody.centerOfMass = center;
	}

	/// <summary>
	/// キー入力を取得。
	/// </summary>
	void Update () {

		if (BattleGameMaster.IsStarted == false) {
			inputVertical = 0f;
			inputHorizontal = 0f;
			return;
		}

		inputHorizontal = Input.GetAxisRaw ("Horizontal2");
		inputVertical = Input.GetAxisRaw ("Vertical2");

		if (isReverse == true) {
			inputHorizontal = -inputHorizontal;
			inputVertical = -inputVertical;
		}
			
		//後ろ方向のキーが入力されている場合、スコア取得できなくする。
		if (inputVertical <= 0) {
			this.GetComponent<RemoveSnow> ().isGetScore = false;
		}

		//前方向のキーが入力されている場合、スコアが取得できるようにする。
		if (inputVertical > 0) {
			this.GetComponent<RemoveSnow> ().isGetScore = true;
		}
	}

	/// <summary>
	/// ジャンプできるかどうかのフラグを管理。
	/// </summary>
	/// <param name="other">Other.</param>
	void OnCollisionStay(Collision other){
		if (other.gameObject.tag == "Ground"|| other.gameObject.tag == "Snow") {
			isOnGround = true;
		}
	}

	/// <summary>
	/// 除雪車の移動及び回転のリセット（横転時の救済処置）。
	/// </summary>
	void FixedUpdate(){

		this.gameObject.transform.Translate (Vector3.forward * inputVertical * moveSpeed,Space.Self);
		this.gameObject.transform.Rotate (Vector3.up * inputHorizontal * rotateSpeed, Space.Self);

		if (Input.GetKeyDown(KeyCode.RightShift)){
			if (this.gameObject.transform.localEulerAngles.z <= 250 && this.gameObject.transform.localEulerAngles.z >= 80) {
				this.gameObject.transform.eulerAngles = new Vector3 (0f, this.gameObject.transform.localEulerAngles.y, 0f);
			}
		}

		if (Input.GetKeyDown (KeyCode.RightAlt)) {
			if (isOnGround == true) {
				carRigidbody.AddForce (Vector3.up * jumpPower, ForceMode.VelocityChange);
				isOnGround = false;
			}
		}

	}
}
