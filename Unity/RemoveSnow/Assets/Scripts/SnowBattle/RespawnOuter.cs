using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤー除雪車が落下したときに所定の位置で復帰させる処理
/// 落下判定のPlaneにアタッチ（ステージ外側に貼り合わせたPlaneに触れたときに発動）
/// </summary>
public class RespawnOuter : RespawnBase {
	
	/// <summary>
	/// このオブジェクトに除雪車が触れたときに発動します。
	/// </summary>
	/// <param name="other">接触したオブジェクト</param>
	void OnTriggerEnter(Collider other) {
		this.doRespawn(other);
	}

}
