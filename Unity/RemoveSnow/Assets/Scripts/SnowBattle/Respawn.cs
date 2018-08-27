using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤー除雪車が落下したときに所定の位置で復帰させる処理
/// 落下判定のPlaneにアタッチ
/// </summary>
public class Respawn : MonoBehaviour {

	/// <summary>
	/// 落下ペナルティとして減点するスコア量
	/// </summary>
	public const int PenaltyScore = 300;

	/// <summary>
	/// プレイヤーごとの復帰位置
	/// </summary>
	[SerializeField]
	private Vector3[] respawnPositions;

	/// <summary>
	/// プレイヤーごとのペナルティテキストオブジェクト
	/// </summary>
	[SerializeField]
	private GameObject[] playerPenaltyTexts;

	/// <summary>
	/// このオブジェクトに除雪車が触れたときに発動します。
	/// </summary>
	/// <param name="other">接触したオブジェクト</param>
	void OnTriggerEnter(Collider other) {
		// プレイヤーのインデックスを取得
		int index = -1;
		if(PlayerScore.PlayerIndexMap.ContainsKey(other.gameObject.tag) == false) {
			return;
		}
		index = PlayerScore.PlayerIndexMap[other.gameObject.tag];

		// SE再生
		GameObject.Find("FallPenalty").GetComponent<AudioSource>().Play();

		// 所定の位置に移動して体勢を立て直す
		other.gameObject.transform.position = this.respawnPositions[index];
		other.GetComponent<Rigidbody>().velocity = Vector3.zero;
		other.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		other.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);

		// 得点ペナルティを与える
		if(PlayerScore.Scores[index] >= Respawn.PenaltyScore) {
			PlayerScore.Scores[index] -= Respawn.PenaltyScore;
		} else {
			PlayerScore.Scores[index] = 0;
		}
		this.playerPenaltyTexts[index].GetComponent<Text>().text = "-" + Respawn.PenaltyScore;
		this.playerPenaltyTexts[index].GetComponent<Animator>().ResetTrigger("DoPenalty");
		this.playerPenaltyTexts[index].GetComponent<Animator>().SetTrigger("DoPenalty");
	}
}
