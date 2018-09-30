using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤー除雪車が落下したときに所定の位置で復帰させる処理の原型
/// </summary>
public abstract class RespawnBase : MonoBehaviour {

	/// <summary>
	/// 落下ペナルティとして減点するスコア量
	/// </summary>
	public const int PenaltyScore = 300;

	/// <summary>
	/// 復帰時の高さオフセット
	/// </summary>
	public const float RespawnHeightOffset = 3.0f;

	/// <summary>
	/// プレイヤーごとの初期位置と初期状態の向きを定義した空のゲームオブジェクト群
	/// </summary>
	[SerializeField]
	protected GameObject[] respawnPositions;

	/// <summary>
	/// プレイヤーごとのペナルティテキストオブジェクト
	/// </summary>
	[SerializeField]
	protected GameObject[] playerPenaltyTexts;

	/// <summary>
	/// このオブジェクトに除雪車が触れたときに発動します。
	/// </summary>
	/// <param name="other">接触したオブジェクト</param>
	protected void doRespawn(Collider other) {
		// プレイヤーのインデックスを取得
		int index = -1;
		if(PlayerScore.PlayerIndexMap.ContainsKey(other.gameObject.tag) == false) {
			return;
		}
		index = PlayerScore.PlayerIndexMap[other.gameObject.tag];

		// SE再生
		GameObject.Find("FallPenalty").GetComponent<AudioSource>().Play();

		// 所定の位置に移動して体勢を立て直す
		other.gameObject.transform.rotation = this.respawnPositions[index].transform.rotation;
		other.gameObject.transform.position = this.respawnPositions[index].transform.position
			+ Vector3.up * RespawnBase.RespawnHeightOffset;
		other.GetComponent<Rigidbody>().velocity = Vector3.zero;
		other.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

		// 得点ペナルティを与える
		switch(SelectModeScene.BattleMode) {
			case SelectModeScene.BattleModes.ShavedIce:
				// 除雪モードは減点
				if(PlayerScore.Scores[index] >= RespawnBase.PenaltyScore) {
					PlayerScore.Scores[index] -= RespawnBase.PenaltyScore;
				} else {
					PlayerScore.Scores[index] = 0;
				}
				break;

			case SelectModeScene.BattleModes.SnowFight:
				// サバイバルモードはHP減算
				if(PlayerScore.HPs[index] >= RespawnBase.PenaltyScore) {
					PlayerScore.HPs[index] -= RespawnBase.PenaltyScore;
				} else {
					PlayerScore.HPs[index] = 0;
				}
				break;
		}

		// アニメーション表示
		this.playerPenaltyTexts[index].GetComponent<Text>().text = "-" + RespawnBase.PenaltyScore;
		this.playerPenaltyTexts[index].GetComponent<Animator>().ResetTrigger("DoPenalty");
		this.playerPenaltyTexts[index].GetComponent<Animator>().SetTrigger("DoPenalty");
	}

}
