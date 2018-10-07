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
	public int playerIndex;

	/// <summary>
	/// ターゲットとの距離
	/// </summary>
	private float distance;

	/// <summary>
	/// 追いかけるかどうか
	/// </summary>
	private bool isChase = true;

	/// <summary>
	/// 狙う相手の位置
	/// </summary>
	private Vector3 targetPosition;

	/// <summary>
	/// 攻撃アニメーション
	/// </summary>
	private Animator minionAnim;

	/// <summary>
	/// Minionがぶつかったときのダメージ
	/// </summary>
	private int minionDamege = 50;

	/// <summary>
	/// 除雪モードか否か
	/// </summary>
	private bool isShaved;

	/// <summary>
	/// SnowBlockを探す半径
	/// </summary>
	private float searchRadias = 0.03f;

	/// <summary>
	/// 雪を探すかどうか
	/// </summary>
	private bool isSearch = true;

	/// <summary>
	/// ターゲットとなる雪
	/// </summary>
	private GameObject targetSnow;

	private float shrinkExtend;


	/// <summary>
	/// Tagをもとに狙い先を決めます。
	/// </summary>
	public void Start() {

		this.minionAnim = this.GetComponent<Animator> ();

		//サバイバルモードの場合、ターゲットを設定して攻撃
		if(SelectModeScene.BattleMode == SelectModeScene.BattleModes.SnowFight){
		
			switch (this.playerIndex) {
			case 0:
				this.target = GameObject.FindGameObjectWithTag ("Player2").transform;
				break;
			case 1:
				this.target = GameObject.FindGameObjectWithTag ("Player").transform;
				break;
			}
			isShaved = false;
		}

		//除雪モードの場合、一緒に除雪
		if (SelectModeScene.BattleMode == SelectModeScene.BattleModes.ShavedIce) {
			isShaved = true;
			//最初のターゲットを設定
			this.SearchSnowBlock();
		}

		StartCoroutine ("Destroy");

	}

	/// <summary>
	/// 毎フレームで向きを補正します。
	/// </summary>
	private void Update() {
		//サバイバルモードの時は相手を追いかける
		if (isShaved == false) {
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

		//除雪する時は一番近い雪に向かって動く
		if (isShaved == true) {
			//ターゲットが見つかってない場合は探す
			if(targetSnow == null){
				this.SearchSnowBlock ();
			}
			//ターゲットの方向を向き、進む
			var targetSnowRotation = Quaternion.LookRotation (this.gameObject.transform.position - targetSnow.transform.position);
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, targetSnowRotation,Time.deltaTime * 3);
			this.transform.Translate (Vector3.forward * -1 * minionSpeed * Time.deltaTime);
		}
	}

	private void OnCollisionEnter(Collision other){
		
		//サバイバルモードの時は攻撃
		if (isShaved == false) {

			if (PlayerScore.IsPlayerTag (other.gameObject) == false) {
				return;
			}

			var playerIndex = PlayerScore.PlayerIndexMap [other.gameObject.tag];

			if (playerIndex == this.playerIndex) {
				// 召喚した本人への衝突は無効
				return;
			}

			PlayerScore.HPs [playerIndex] -= this.minionDamege;

		}

		//除雪モードの時は除雪
		if (isShaved == true) {
			if (other.gameObject.tag == "Snow") {
				shrinkExtend = other.gameObject.GetComponent<SnowShrink> ().ShrinkExtend;
				minionAnim.SetTrigger ("RemoveSnow");
				other.transform.localScale = new Vector3(1f, shrinkExtend, 1f);
				shrinkExtend -= SnowShrink.OnceShrink;
				other.gameObject.GetComponent<SnowShrink> ().ShrinkExtend = shrinkExtend;
				// 除雪したプレイヤーのスコアを加算
				PlayerScore.Scores[playerIndex]++;
				//次のターゲットを設定
				this.SearchSnowBlock();
			}
		}
	}
	/// <summary>
	/// ターゲットとなる雪を探す
	/// </summary>
	void SearchSnowBlock(){
		
		var snows = Physics.SphereCastAll (this.transform.position, searchRadias, this.transform.forward, 1f);
		var minTargetDistance = float.MaxValue;

		foreach (var snow in snows) {
			float targetDistance = Vector3.Distance (this.transform.position, snow.transform.position);
			if (targetDistance <= minTargetDistance) {
				targetDistance = minTargetDistance;
				if (snow.transform.gameObject.tag == "Snow") {
					targetSnow = snow.transform.gameObject;
				}
			}
		}

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
