using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム生成
/// </summary>
public class ItemSpawn : MonoBehaviour {

	/// <summary>
	/// 初回配置をするまでの秒数
	/// Ready-Goの時間も含まれます。
	/// </summary>
	public const float FirstDelayTimeSeconds = 10.0f;

	/// <summary>
	/// 次の配置をするまでの秒数
	/// </summary>
	public const float NextDelayTimeSeconds = 10.0f;

	/// <summary>
	/// 出現させるアイテム。
	/// </summary>
	[SerializeField]
	private GameObject[] items;

	/// <summary>
	/// 出現範囲のX座標最小値
	/// </summary>
	[SerializeField]
	private float rangeXMin;

	/// <summary>
	/// 出現範囲のX座標最大値
	/// </summary>
	[SerializeField]
	private float rangeXMax;

	/// <summary>
	/// 出現Y座標
	/// </summary>
	[SerializeField]
	private float positionY;

	/// <summary>
	/// 出現範囲のZ座標最小値
	/// </summary>
	[SerializeField]
	private float rangeZMin;

	/// <summary>
	/// 出現範囲のZ座標最大値
	/// </summary>
	[SerializeField]
	private float rangeZMax;
	
	/// <summary>
	/// アイテム自動生成をスタート
	/// </summary>
	public void Start() {
		this.InvokeRepeating("itemGenerate", ItemSpawn.FirstDelayTimeSeconds, ItemSpawn.NextDelayTimeSeconds);
	}

	/// <summary>
	/// ランダムな位置にランダムなアイテムを１つ生成します。
	/// </summary>
	private void itemGenerate() {
		Physics.queriesHitTriggers = false;

		// 配置できるまでやり直し続ける
		while(true) {
			var itemX = Random.Range(this.rangeXMin, this.rangeXMax);
			var itemZ = Random.Range(this.rangeZMin, this.rangeZMax);
			var itemNumber = Random.Range(0, this.items.Length);
			var itemPosition = new Vector3(itemX, this.positionY, itemZ);
			var parent = this.gameObject.transform;

			if(Physics.Raycast(itemPosition, -Vector3.up) == true) {
				// 配置可能な場所なら配置して終了する
				Object.Instantiate(this.items[itemNumber], itemPosition, Quaternion.identity, parent);
				break;
			}
		}
	}

}
