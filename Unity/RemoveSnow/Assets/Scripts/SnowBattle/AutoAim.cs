using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大砲の向きを自動で相手に向ける
/// Muzzle({Color}Cannonと名の付くプレハブ)にアタッチ
/// </summary>
public class AutoAim : MonoBehaviour {

	/// <summary>
	/// 狙い先
	/// </summary>
	private Transform target;

	/// <summary>
	/// Tagをもとに狙い先を決めます。
	/// </summary>
	public void Start() {
		if(SelectModeScene.Players <= 1) {
			// 一人用ではただの飾りと化す
			return;
		}
		
		// TODO: 複数プレイヤーになったときに誰をターゲットにするか？
		switch(this.gameObject.transform.parent.tag) {
			case "Player":
				this.target = GameObject.FindGameObjectWithTag("Player2").transform;
				break;
			case "Player2":
				this.target = GameObject.FindGameObjectWithTag("Player").transform;
				break;
		}
	}

	/// <summary>
	/// 毎フレームで向きを補正します。
	/// </summary>
	public void Update() {
		if(SelectModeScene.Players <= 1) {
			// 一人用ではただの飾りと化す
			return;
		}

		var targetPosition = this.target.transform.position;
		var targetRotation = Quaternion.LookRotation(targetPosition - this.gameObject.transform.position);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * 3);
	}

}
