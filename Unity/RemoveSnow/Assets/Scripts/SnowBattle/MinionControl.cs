using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム：ミニオン（分身）の動作制御
/// </summary>
public class MinionControl : MonoBehaviour {

	/// <summary>
	/// Minionのスピード
	/// </summary>
	[SerializeField]
	private float minionSpeed;

	/// <summary>
	/// これを召喚したPlayerの番号
	/// （Minionアイテム取得時にSummonMinionのメソッドからセットされます）
	/// </summary>
	public int PlayerIndex {
		get; set;
	}

	/// <summary>
	/// 狙い先
	/// </summary>
	private Transform target;

	/// <summary>
	/// 攻撃アニメーター
	/// </summary>
	private Animator minionAnimator;

	/// <summary>
	/// ジャンプ力
	/// </summary>
	public const float JumpPower = 2.0f;

	/// <summary>
	/// 持続時間秒
	/// </summary>
	public const float LiveTimeSeconds = 15.0f;

	/// <summary>
	/// 除雪モードのみ: SnowBlockを探す半径
	/// </summary>
	public const float SearchRadias = 0.03f;

	/// <summary>
	/// 除雪モードのみ: このミニオンが雪を削ったときに得られるスコア
	/// </summary>
	public const int BonusScore = 5;

	/// <summary>
	/// 除雪モードのみ: ターゲットとなる雪
	/// </summary>
	private GameObject targetSnow;

	/// <summary>
	/// 除雪モードのみ: 雪の削れ具合
	/// </summary>
	private float shrinkExtend;

	/// <summary>
	/// 除雪モードのみ: ジャンプしているかどうか
	/// </summary>
	private bool isJumping;

	/// <summary>
	/// サバイバルモードのみ: 追いかけるかどうか
	/// </summary>
	private bool isChase = true;

	/// <summary>
	/// サバイバルモードのみ: Minionがぶつかったときのダメージ
	/// </summary>
	private int minionDamege = 50;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Start() {
		this.minionAnimator = this.GetComponent<Animator>();

		// モードに応じて挙動を変える
		switch(SelectModeScene.BattleMode) {
			case SelectModeScene.BattleModes.ShavedIce:
				// プレイヤーと一緒に除雪
				this.isJumping = false;
				if(SelectModeScene.BattleMode == SelectModeScene.BattleModes.ShavedIce) {
					// 最初のターゲットとなる雪を設定
					this.searchSnowBlock();
				}
				break;

			case SelectModeScene.BattleModes.SnowFight:
				// ターゲットを設定して攻撃
				// TODO: プレイヤー人数が３人以上に対応するならどうする？
				this.isChase = true;
				switch(this.PlayerIndex) {
					case 0:
						this.target = GameObject.FindGameObjectWithTag("Player2").transform;
						break;
					case 1:
						this.target = GameObject.FindGameObjectWithTag("Player").transform;
						break;
				}
				break;
		}

		// 効果に関わらず一定時間で消去する
		this.StartCoroutine(this.destroy(MinionControl.LiveTimeSeconds));
	}

	/// <summary>
	/// 毎フレームで向きを補正し、進むべき方向へ進みます。
	/// </summary>
	private void Update() {
		if(this.gameObject == null) {
			// 既に効果が切れている場合は無視
			return;
		}

		switch(SelectModeScene.BattleMode) {
			case SelectModeScene.BattleModes.ShavedIce:
				// 一番近い雪に向かって動く
				if(this.targetSnow == null) {
					// ターゲットが見つかってない場合は探す
					this.searchSnowBlock();

					if(this.targetSnow == null) {
						if(this.isJumping == false) {
							// 近くに雪がなかった場合は前方に向かってジャンプする
							this.StartCoroutine(this.jump());
						}
						return;
					}
				}

				// ターゲットの方向を向き、進む
				var targetSnowRotation = Quaternion.LookRotation(this.gameObject.transform.position - this.targetSnow.transform.position);
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetSnowRotation, Time.deltaTime * 3);
				//this.transform.Translate(Vector3.forward * -1 * this.minionSpeed * Time.deltaTime);
				this.GetComponent<Rigidbody>().AddForce(Vector3.forward * -1 * this.minionSpeed * Time.deltaTime);
				break;

			case SelectModeScene.BattleModes.SnowFight:
				// 相手との距離を測る
				var targetPosition = this.target.position;
				var distance = Vector3.Distance(this.transform.position, targetPosition);

				if(distance <= 1f) {
					// 最接近したら攻撃モーションを発動させる
					//this.isChase = false;
					this.minionAnimator.SetTrigger("Attack");
				}

				if(this.isChase == true) {
					// 相手を追いかける
					var targetRotation = Quaternion.LookRotation(this.gameObject.transform.position - targetPosition);
					this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * 3);
					this.transform.Translate(Vector3.forward * -1 * this.minionSpeed * Time.deltaTime);
					//this.GetComponent<Rigidbody>().AddForce(Vector3.forward * +1 * this.minionSpeed * Time.deltaTime);
				}
				break;
		}
	}

	/// <summary>
	/// このミニオンと別のオブジェクトが接触したとき
	/// </summary>
	/// <param name="other">接したオブジェクト</param>
	private void OnCollisionEnter(Collision other) {
		switch(SelectModeScene.BattleMode) {
			case SelectModeScene.BattleModes.ShavedIce:
				if(other.gameObject.tag != "Snow") {
					return;
				}

				// 雪と接触したら除雪する
				this.shrinkExtend = other.gameObject.GetComponent<SnowShrink>().ShrinkExtend;
				this.minionAnimator.SetTrigger("RemoveSnow");
				other.transform.localScale = new Vector3(1f, this.shrinkExtend, 1f);
				this.shrinkExtend -= SnowShrink.OnceShrink;
				other.gameObject.GetComponent<SnowShrink>().ShrinkExtend = this.shrinkExtend;

				// 除雪したプレイヤーのスコアを加算
				PlayerScore.Scores[this.PlayerIndex] += MinionControl.BonusScore;

				// 次のターゲットを設定
				this.searchSnowBlock();
				break;

			case SelectModeScene.BattleModes.SnowFight:
				if(PlayerScore.IsPlayerTag(other.gameObject) == false) {
					return;
				}

				var playerIndex = PlayerScore.PlayerIndexMap[other.gameObject.tag];
				if(playerIndex == this.PlayerIndex) {
					// 召喚した本人への衝突は無効
					return;
				}

				// 衝突SEの再生
				GameObject.Find("BulletPenalty").GetComponent<AudioSource>().Play();

				// ダメージを与える
				PlayerScore.HPs[playerIndex] -= this.minionDamege;
				break;
		}
	}

	/// <summary>
	/// 除雪モード専用: ターゲットとなる雪を探します。
	/// </summary>
	private void searchSnowBlock() {
		var snows = Physics.SphereCastAll(this.transform.position, MinionControl.SearchRadias, this.transform.forward, 1f);
		var minTargetDistance = float.MaxValue;

		foreach(var snow in snows) {
			float targetDistance = Vector3.Distance(this.transform.position, snow.transform.position);
			if(targetDistance <= minTargetDistance) {
				targetDistance = minTargetDistance;
				if(snow.transform.gameObject.tag == "Snow") {
					// 最も近い雪をターゲットにする
					this.targetSnow = snow.transform.gameObject;
				}
			}
		}
	}

	/// <summary>
	/// コルーチン：一定時間経過後に、このミニオン（分身）の追跡を再起動します。
	/// </summary>
	private IEnumerator chaseRestart() {
		yield return new WaitForSeconds(1f);
		this.isChase = true;
	}

	/// <summary>
	/// コルーチン：一定時間経過後に、このミニオン（分身）を削除します。
	/// これはアイテムの効力が切れるのと同義です。
	/// </summary>
	private IEnumerator destroy(float timeSeconds) {
		yield return new WaitForSeconds(timeSeconds);
		if(this.gameObject != null) {
			Object.Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// コルーチン：即座にジャンプします。
	/// </summary>
	private IEnumerator jump() {
		yield return new WaitForEndOfFrame();
		this.isJumping = true;

		this.GetComponent<Rigidbody>().AddForce(Vector3.up * MinionControl.JumpPower, ForceMode.VelocityChange);
		this.GetComponent<Rigidbody>().AddForce(Vector3.forward * MinionControl.JumpPower, ForceMode.VelocityChange);

		yield return new WaitForSeconds(3.0f);

		this.isJumping = false;
	}

}
