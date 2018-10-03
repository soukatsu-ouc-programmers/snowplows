using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionControl : MonoBehaviour {

	/// <summary>
	/// 狙い先
	/// </summary>
	private Transform target;

	/// <summary>
	/// Minionのスピード
	/// </summary>
	[SerializeField]
	private float minionSpeed;

	/// <summary>
	/// これを召喚したPlayerの番号
	/// (Minion取得時に送信される)
	/// </summary>
	public int playerIndex = 1;

	/// <summary>
	/// ターゲットとの距離
	/// </summary>
	private float distance;

	private bool isChase = true;

	private Vector3 targetPosition;

	private Animator minionAnim;

	/// <summary>
	/// Minionがぶつかったときのダメージ
	/// </summary>
	private int minionDamege = 50;

	/// <summary>
	/// Tagをもとに狙い先を決めます。
	/// </summary>
	public void Start() {

		this.minionAnim = this.GetComponent<Animator> ();
		
		switch(this.playerIndex) {
		case 0:
			this.target = GameObject.FindGameObjectWithTag("Player2").transform;
			break;
		case 1:
			this.target = GameObject.FindGameObjectWithTag("Player").transform;
			break;
		}

		StartCoroutine ("Destroy");

	}

	/// <summary>
	/// 毎フレームで向きを補正します。
	/// </summary>
	private void Update() {

		distance = Vector3.Distance (this.transform.position, targetPosition);
		
		if (distance <= 1f) {
			isChase = false;
			minionAnim.SetTrigger ("Attack");
			StartCoroutine ("ChaseRestart");
		}

		if (isChase == true) {
			targetPosition = this.target.position;
			var targetRotation = Quaternion.LookRotation (this.gameObject.transform.position - targetPosition);
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, targetRotation, Time.deltaTime * 3);
			this.transform.Translate (Vector3.forward * -1 * minionSpeed * Time.deltaTime);
		}

	}

	private void OnCollisionEnter(Collision other){
		
		if(PlayerScore.IsPlayerTag(other.gameObject) == false) {
			return;
		}

		var playerIndex = PlayerScore.PlayerIndexMap[other.gameObject.tag];

		if(playerIndex == this.playerIndex) {
			// 召喚した本人への衝突は無効
			return;
		}

		PlayerScore.HPs[playerIndex] -= this.minionDamege;
	}

	IEnumerator ChaseRestart(){
		yield return new WaitForSeconds (1f);
		isChase = true;
	}

	IEnumerator Destroy(){
		yield return new WaitForSeconds (10f);
		Destroy (this.gameObject);
	}

}
