using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveSnow : MonoBehaviour {
	/// <summary>
	/// Player1のスコア
	/// </summary>
	static public float ScoreOne = 0;

	/// <summary>
	/// Player2のスコア
	/// </summary>
	static public float ScoreTwo = 0;

	/// <summary>
	/// スコア取得できるかどうか。
	/// 車操作のスクリプトから書き換え。
	/// </summary>
	[SerializeField]
	public bool isGetScore = false;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private AudioClip snowSound;

	/// <summary>
	/// スコアを初期化
	/// </summary>
	void Start(){
		ScoreOne = 0;
		ScoreTwo = 0;
	}

	/// <summary>
	/// スコアを取得する処理。
	/// Snowタグがついているオブジェクトに触り、isGetSocreフラグがTrueならスコア取得。
	/// </summary>
	/// <param name="other">Other.</param>
	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Snow") {
			if (this.gameObject.tag == "Player" && isGetScore == true) {
				
				ScoreOne++;
				source.PlayOneShot (snowSound);

			}
			if (this.gameObject.tag == "Player2" && isGetScore == true) {

				ScoreTwo++;
				source.PlayOneShot (snowSound);

			}
		}
	}

	void Update(){
		if (BattleGameMaster.IsStarted == false) {
			isGetScore = false;
		}
	}
}
