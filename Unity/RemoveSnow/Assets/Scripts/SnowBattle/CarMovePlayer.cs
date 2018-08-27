using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー移動
/// 除雪車にアタッチ
/// </summary>
public class CarMovePlayer : MonoBehaviour {

	/// <summary>
	/// コントローラー１つ分のKeyCodeのオフセット
	/// </summary>
	public const int JoypadButtonOffset = KeyCode.Joystick2Button0 - KeyCode.Joystick1Button0;

	/// <summary>
	/// 除雪車の重心
	/// </summary>
	static private readonly Vector3 CenterGravity = new Vector3(0f, -10f, 0f);

	/// <summary>
	/// プレイヤーインデックス
	/// </summary>
	[SerializeField]
	private int playerIndex;

	/// <summary>
	/// 横方向の移動量
	/// </summary>
	[SerializeField]
	private float inputHorizontal;

	/// <summary>
	/// 縦方向の移動量
	/// </summary>
	[SerializeField]
	private float inputVertical;

	/// <summary>
	/// 除雪車の進むスピード
	/// </summary>
	public float MoveSpeed;

	/// <summary>
	/// 除雪車の回転スピード
	/// </summary>
	[SerializeField]
	private float rotateSpeed;

	/// <summary>
	/// ジャンプ力ぅ・・・ですかねぇ・・・。
	/// </summary>
	[SerializeField]
	private float jumpPower;

	/// <summary>
	/// 地面に触れているかどうか
	/// </summary>
	[SerializeField]
	private bool isOnGround;

	/// <summary>
	/// 混乱中であるかどうか
	/// </summary>
	[SerializeField]
	public bool isReverse;

	/// <summary>
	/// アイテム取得時に表示させるテキスト群
	/// </summary>
	[SerializeField]
	private GameObject[] effectText;

	/// <summary>
	/// スコアを加算できるかどうか
	/// </summary>
	public bool canAddScore {
		get; set;
	}

	/// <summary>
	/// 入力軸：X軸方向
	/// Start関数内で初期化するか、インスペクター上で設定して下さい。
	/// </summary>
	[SerializeField]
	protected string inputAxisHorizontal;

	/// <summary>
	/// 入力軸：Y軸方向
	/// Start関数内で初期化するか、インスペクター上で設定して下さい。
	/// </summary>
	[SerializeField]
	protected string inputAxisVertical;

	/// <summary>
	/// 横転復帰のキーコード
	/// Start関数内で初期化するか、インスペクター上で設定して下さい。
	/// </summary>
	[SerializeField]
	protected KeyCode keyCodeRestoreFromRollover;

	/// <summary>
	/// ジャンプのキーコード
	/// Start関数内で初期化するか、インスペクター上で設定して下さい。
	/// </summary>
	[SerializeField]
	protected KeyCode keyCodeJump;

	/// <summary>
	/// 除雪車のRigidBody
	/// </summary>
	private Rigidbody carRigidbody;

	/// <summary>
	/// 初期化処理
	/// </summary>
	protected void Start() {
		// 除雪車の重心を設定
		this.carRigidbody = this.GetComponent<Rigidbody>();
		this.carRigidbody.centerOfMass = CarMovePlayer.CenterGravity;
	}

	/// <summary>
	/// 移動に関わるキー入力判定を行います。
	/// </summary>
	protected void Update() {
		if(SnowBattleScene.IsStarted == false) {
			// ゲーム開始前は操作できないようにする
			this.inputVertical = 0f;
			this.inputHorizontal = 0f;
			return;
		}

		if(string.IsNullOrEmpty(this.inputAxisHorizontal) == false
		&& string.IsNullOrEmpty(this.inputAxisVertical) == false) {

			// それぞれの軸の移動量を取得
			this.inputHorizontal = Input.GetAxisRaw(this.inputAxisHorizontal);
			this.inputVertical = Input.GetAxisRaw(this.inputAxisVertical);

			if(this.isReverse == true) {
				// 混乱中は方向操作を逆にする
				this.inputHorizontal = -this.inputHorizontal;
				this.inputVertical = -this.inputVertical;
			}

			if(this.inputVertical <= 0) {
				// 後ろ方向のキーが入力されている場合、スコア設定できなくする
				this.canAddScore = false;
			} else {
				// 前方向のキーが入力されている場合、スコアが設定できるようにする
				this.canAddScore = true;
			}

		}
	}

	/// <summary>
	/// 接地している間はジャンプできるようにします。
	/// </summary>
	/// <param name="other">接しているオブジェクト</param>
	public void OnCollisionStay(Collision other) {
		if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Snow") {
			this.isOnGround = true;
		}
	}

	/// <summary>
	/// 接地していない間はジャンプできないようにします。
	/// </summary>
	/// <param name="other">接していたオブジェクト</param>
	public void OnCollisionExit(Collision other) {
		if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Snow") {
			this.isOnGround = false;
		}
	}

	/// <summary>
	/// 特殊操作のキー入力
	/// </summary>
	public void FixedUpdate() {
		if(SnowBattleScene.IsStarted == false) {
			// ゲーム開始前は操作できないようにする
			this.inputVertical = 0f;
			this.inputHorizontal = 0f;
			return;
		}

		//除雪車を前方or後方に移動。キー入力に応じてPositionを変更
		this.gameObject.transform.Translate(Vector3.forward * this.inputVertical * this.MoveSpeed, Space.Self);
		//除雪車を回転。キー入力に応じて、オブジェクトのRotationを変更
		this.gameObject.transform.Rotate(Vector3.up * this.inputHorizontal * this.rotateSpeed, Space.Self);

		if(Input.GetKeyDown(this.keyCodeRestoreFromRollover) == true
		|| Input.GetKeyDown((KeyCode)(KeyCode.Joystick1Button0 + this.playerIndex * JoypadButtonOffset)) == true) {
			// 一定の閾値を超えたときに横転を復帰する
			if(this.gameObject.transform.localEulerAngles.z <= 250
			&& this.gameObject.transform.localEulerAngles.z >= 80) {
				this.gameObject.transform.eulerAngles = new Vector3(0f, this.gameObject.transform.localEulerAngles.y, 0f);
				this.carRigidbody.angularVelocity = Vector3.zero;
			}
		}

		if(Input.GetKeyDown(this.keyCodeJump) == true
		|| Input.GetKeyDown((KeyCode)(KeyCode.Joystick1Button1 + this.playerIndex * JoypadButtonOffset)) == true) {
			// ジャンプする
			if(this.isOnGround == true) {
				this.carRigidbody.AddForce(Vector3.up * this.jumpPower, ForceMode.VelocityChange);
				this.isOnGround = false;
			}
		}
	}

	/// <summary>
	/// アイテムを取得したときにテキストを表示します。
	/// </summary>
	/// <param name="other">触れたオブジェクト</param>
	public void OnTriggerEnter(Collider other) {
		if(SnowBattleScene.IsStarted == false) {
			return;
		}

		int effectTextIndex = -1;
		switch(other.gameObject.tag) {
			case "Turbo":
				effectTextIndex = 0;
				GameObject.Find("SEGroup").transform.Find("Item").GetComponent<AudioSource>().Play();
				break;

			case "Cannon":
				effectTextIndex = 1;
				GameObject.Find("SEGroup").transform.Find("Item").GetComponent<AudioSource>().Play();
				break;

			case "BigBull":
				effectTextIndex = 2;
				GameObject.Find("SEGroup").transform.Find("Item").GetComponent<AudioSource>().Play();
				break;

			case "Puzzle":
				effectTextIndex = 3;
				GameObject.Find("SEGroup").transform.Find("ItemPuzzle").GetComponent<AudioSource>().Play();
				break;
		}

		if(effectTextIndex != -1) {
			// 該当のテキストを生成
			Object.Instantiate(this.effectText[effectTextIndex], this.transform.position, this.transform.rotation, this.transform);
		}
	}

}
