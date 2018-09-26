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
	/// Ready-Goの時間も含まれます。
	/// </summary>
	public const float FirstDelayTimeSecondsNormal = 10.0f;

	/// <summary>
	/// 初回配置をするまでの秒数（カオスモード）
	/// Ready-Goの時間も含まれます。
	/// </summary>
	public const float FirstDelayTimeSecondsChaos = 1.0f;

	/// <summary>
	/// 次の配置をするまでの秒数（通常モード）
	/// </summary>
	public const float NextDelayTimeSecondsNormal = 10.0f;

	/// <summary>
	/// 次の配置をするまでの秒数（カオスモード）
	/// </summary>
	public const float NextDelayTimeSecondsChaos = 1.0f;

	/// <summary>
	/// 出現させるアイテム。
	/// 一番最後にBigBullを入れてください。
	/// </summary>
	[SerializeField]
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
	/// アイテム自動生成をスタート
	/// </summary>
	public void Start() {
		// モードに応じて生成間隔を変える
		switch(SelectModeScene.ItemMode) {
			case SelectModeScene.ItemModes.Normal:
				this.InvokeRepeating("itemGenerate", ItemSpawn.FirstDelayTimeSecondsNormal, ItemSpawn.NextDelayTimeSecondsNormal);
				break;

			case SelectModeScene.ItemModes.Chaos:
				this.InvokeRepeating("itemGenerate", ItemSpawn.FirstDelayTimeSecondsChaos, ItemSpawn.NextDelayTimeSecondsChaos);
				break;
		}
	}

	/// <summary>
	/// ランダムな位置にランダムなアイテムを１つ生成します。
	/// </summary>
	private void itemGenerate() {
		Physics.queriesHitTriggers = false;

		// 配置できるまでやり直し続ける <- 2018.09.15: 限度有りにしました
		for(int i = 0; i < int.MaxValue; i++) {
			var itemX = Random.Range(this.rangeXMin, this.rangeXMax);
			var itemZ = Random.Range(this.rangeZMin, this.rangeZMax);
			var itemNumber = Random.Range (0, this.items.Length);

			//サバイバルモードの場合、BigBullが出ないようにする
			if (SelectModeScene.BattleMode == SelectModeScene.BattleModes.SnowFight) {
				itemNumber = Random.Range (0, this.items.Length-1);
			}

			var parent = this.gameObject.transform;

			// 配置可能な場所＝所定のY座標から下に着地点があること
			RaycastHit hit;
			Ray ray = new Ray(parent.TransformPoint(new Vector3(itemX, this.positionY, itemZ)), Vector3.down);
			if(Physics.Raycast(ray, out hit, Mathf.Abs(this.positionY) * 2.0f, -1, QueryTriggerInteraction.Ignore) == true) {
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
				Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 100f);  // 当たり判定の検査を可視化
				// Debug.Log("アイテム配置先: " + itemPosition);
				newObject.transform.localPosition = itemPosition;
				break;
			}
		}
	}

}
