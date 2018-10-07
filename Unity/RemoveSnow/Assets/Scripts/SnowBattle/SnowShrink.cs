using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雪が除雪されて縮んでいく演出とスコア加算
/// </summary>
public class SnowShrink : MonoBehaviour {

	/// <summary>
	/// 完全になくなった扱いにするサイズ
	/// </summary>
	public const float RemoveCompleteThreshold = 0.1f;

	/// <summary>
	/// 一回の除雪ごとに縮めるサイズ
	/// </summary>
	public const float OnceShrink = 0.1f;

	/// <summary>
	/// 縮んだ後のサイズ
	/// </summary>
	public float ShrinkExtend = 0.7f;

	/// <summary>
	/// 雪が小さくなるのが無限ループしないようにするためのフラグ
	/// </summary>
	private bool isShrink = true;

	/// <summary>
	/// 除雪車の先端ブレードが接したときに除雪されます。
	/// </summary>
	/// <param name="other">接触しているオブジェクト</param>
	public void OnCollisionStay(Collision other) {
		if(SnowBattleScene.IsStarted == false) {
			return;
		}

		if(PlayerScore.IsPlayerTag(other.gameObject) == true) {
			bool canAddScore;
			if(other.gameObject.name.IndexOf("BigBull") == 0) {
				// 巨大ブレードの先端が接しているときは親の除雪車の設定を見る
				canAddScore = other.transform.parent.parent.GetComponent<CarMovePlayer>().canAddScore;
			} else {
				canAddScore = other.gameObject.GetComponent<CarMovePlayer>().canAddScore;
			}

			if(this.isShrink == true && canAddScore == true) {
				// 雪を縮める
				this.gameObject.transform.localScale = new Vector3(1f, this.ShrinkExtend, 1f);
				this.ShrinkExtend -= SnowShrink.OnceShrink;

				// 除雪したプレイヤーのスコアを加算
				int playerIndex = PlayerScore.PlayerIndexMap[other.gameObject.tag];
				PlayerScore.Scores[playerIndex]++;

				// この雪から離れるまで除雪できないようにする
				this.isShrink = false;
			}
		}
	}

	/// <summary>
	/// 除雪車がこの雪から外れたとき、再度除雪できるようにします。
	/// </summary>
	/// <param name="other">接触していたオブジェクト</param>
	public void OnCollisionExit(Collision other) {
		if(PlayerScore.IsPlayerTag(other.gameObject) == true) {
			this.isShrink = true;
		}
	}

	/// <summary>
	/// 雪の高さが閾値を下回ったら完全に除雪します。
	/// </summary>
	public void Update() {
		if(this.ShrinkExtend <= SnowShrink.RemoveCompleteThreshold) {
			Object.Destroy(this.gameObject);
		}
	}

}
