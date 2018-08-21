using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveSnow : MonoBehaviour {
	/// <summary>
	/// Player1のスコア
	/// SnowShrinkの雪縮小判定とともに加算。
	/// </summary>
	static public float ScoreOne = 0;

	/// <summary>
	/// Player2のスコア.
	/// SnowShrinkの雪縮小判定とともに加算。
	/// </summary>
	static public float ScoreTwo = 0;

	/// <summary>
	/// スコア取得できるかどうか。
	/// 車操作のスクリプトから書き換え。
	/// </summary>
	[SerializeField]
	public bool isGetScore = true;

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
		
	void Update(){
		if (BattleGameMaster.IsStarted == false) {
			isGetScore = false;
		}
			
	}
}
