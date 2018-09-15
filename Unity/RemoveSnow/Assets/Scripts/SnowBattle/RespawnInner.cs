using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤー除雪車が落下したときに所定の位置で復帰させる処理
/// 落下判定のCubeにアタッチ（ステージ全体を覆い尽くすColider付きのオブジェクトから抜けたときに発動）
/// </summary>
public class RespawnInner : RespawnBase {
	
	/// <summary>
	/// このオブジェクトの外側に除雪車がはみ出たときに発動します。
	/// </summary>
	/// <param name="other">接触したオブジェクト</param>
	void OnTriggerExit(Collider other) {
		this.doRespawn(other);
	}

}
