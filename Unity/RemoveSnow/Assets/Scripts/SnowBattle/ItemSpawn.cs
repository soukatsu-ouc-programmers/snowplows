using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// アイテム生成
/// </summary>
public class ItemSpawn : MonoBehaviour {

	/// <summary>
	/// 初回配置をするまでの秒数（通常モード）
	/// Ready-Goの時間を含みません。
	/// </summary>
	public const float FirstDelayTimeSecondsNormal = 5.0f;

	/// <summary>
	/// 初回配置をするまでの秒数（カオスモード）
	/// Ready-Goの時間を含みません。
	/// </summary>
	public const float FirstDelayTimeSecondsChaos = 2.0f;

	/// <summary>
	/// 次の配置をするまでの秒数（通常モード）
	/// </summary>
	public const float NextDelayTimeSecondsNormal = 10.0f;

	/// <summary>
	/// 次の配置をするまでの秒数（カオスモード）
	/// </summary>
	public const float NextDelayTimeSecondsChaos = 2.0f;

	/// <summary>
	/// 出現させるアイテム群：ノーマルモード
	/// </summary>
	[SerializeField]
	private GameObject[] itemsNormal;

	/// <summary>
	/// 出現させるアイテム群：サバイバルモード
	/// </summary>
	[SerializeField]
	private GameObject[] itemsSurvival;

	/// <summary>
	/// 現在のモードに対応した出現させるアイテム群
	/// </summary>
	private GameObject[] items;

	/// <summary>
	/// 出現範囲のX座標最小値（ローカル座標系）
	/// </summary>
	[SerializeField]
	private float rangeXMin;

	/// <summary>
	/// 出現範囲のX座標最大値（ローカル座標系）
	/// </summary>
	[SerializeField]
	private float rangeXMax;

	/// <summary>
	/// 出現最大Y座標（ローカル座標系）
	/// </summary>
	[SerializeField]
	private float positionY;

	/// <summary>
	/// 出現Y座標オフセット
	/// </summary>
	[SerializeField]
	private float positionYOffset;

	/// <summary>
	/// 出現範囲のZ座標最小値（ローカル座標系）
	/// </summary>
	[SerializeField]
	private float rangeZMin;

	/// <summary>
	/// 出現範囲のZ座標最大値（ローカル座標系）
	/// </summary>
	[SerializeField]
	private float rangeZMax;

	/// <summary>
	/// アイテム自動生成をスタートさせたかどうか
	/// </summary>
	private bool isStarted;

	/// <summary>
	/// 初期処理
	/// </summary>
	public void Start() {
		this.isStarted = false;
	}

	/// <summary>
	/// 毎フレーム：アイテム自動生成を開始させる判定を行います。
	/// </summary>
	public void Update() {
		if(SnowBattleScene.IsStarted == true && this.isStarted == false) {
			this.init();
		}
	}

	/// <summary>
	/// アイテム自動生成をスタートさせます。
	/// </summary>
	private void init() {
		this.isStarted = true;

		// アイテムモードに応じて生成間隔を変える
		switch(SelectModeScene.ItemMode) {
			case SelectModeScene.ItemModes.Normal:
				this.InvokeRepeating("itemGenerate", ItemSpawn.FirstDelayTimeSecondsNormal, ItemSpawn.NextDelayTimeSecondsNormal);
				break;

			case SelectModeScene.ItemModes.Chaos:
				this.InvokeRepeating("itemGenerate", ItemSpawn.FirstDelayTimeSecondsChaos, ItemSpawn.NextDelayTimeSecondsChaos);
				break;

			case SelectModeScene.ItemModes.None:
				this.items = null;
				return;
		}

		// バトルモードに応じて生成するアイテムのパターンを変える
		switch(SelectModeScene.BattleMode) {
			case SelectModeScene.BattleModes.ShavedIce:
				this.items = this.itemsNormal;
				break;

			case SelectModeScene.BattleModes.SnowFight:
				this.items = this.itemsSurvival;
				break;
		}
	}

	/// <summary>
	/// ランダムな位置にランダムなアイテムを１つ生成します。
	/// </summary>
	private void itemGenerate() {
		Physics.queriesHitTriggers = false;

		// 配置できるまでやり直し続ける: 配置可能範囲の初期デバッグ時はフリーズに注意！
		while(true) {
			var itemX = Random.Range(this.rangeXMin, this.rangeXMax);
			var itemZ = Random.Range(this.rangeZMin, this.rangeZMax);
			var itemNumber = Random.Range(0, this.items.Length);
			var parent = this.gameObject.transform;

			// 配置可能な場所＝所定のY座標から下に着地点があること
			RaycastHit hit;
			Ray ray = new Ray(parent.TransformPoint(new Vector3(itemX, this.positionY, itemZ)), Vector3.down);
			if(Physics.Raycast(ray, out hit, Mathf.Abs(this.positionY) * 2.0f, -1, QueryTriggerInteraction.Ignore) == false) {
				// 配置できない場所なので座標を決め直す
				continue;
			}

			// 配置可能な場所なら配置して終了する
			var topObject = hit.transform;
			if(topObject.gameObject.tag != "Snow") {
				// 雪の上以外には配置しない
				continue;
			}

			// アイテム生成
			var newObject = Object.Instantiate(this.items[itemNumber], Vector3.zero, Quaternion.identity, parent);

			// Local座標系でいじる
			var itemPosition = new Vector3(
				itemX,
				topObject.transform.localPosition.y + this.positionYOffset,
				itemZ
			);

			// 当たり判定の検査を可視化
			// Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 100f);  
			// Debug.Log("アイテム配置先: " + itemPosition);

			newObject.transform.localPosition = itemPosition;
			break;
		}
	}

}
