﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーとスコアの管理
/// </summary>
public class PlayerScore : MonoBehaviour {

	/// <summary>
	/// サバイバルモードでの最大HP
	/// </summary>
	public const int MaxHP = 1000;

	/// <summary>
	/// プレイヤーのスコア
	/// SnowShrinkの雪縮小判定とともに加算されます。
	/// </summary>
	static public int[] Scores = new int[0];

	/// <summary>
	/// プレイヤーのHP
	/// サバイバルモードの体力判定に使用し、いずれかのプレイヤーがゼロになったらゲーム終了となります。
	/// </summary>
	static public int[] HPs = new int[0];

	/// <summary>
	/// プレイヤーのスコアを ???? 表示にするかどうか
	/// </summary>
	static public bool IsScoreHidden;

	/// <summary>
	/// 前回までのハイスコアの記録
	/// </summary>
	static public int HighScore;

	/// <summary>
	/// プレイヤーのTagとインデックスを対応付けます。
	/// </summary>
	static public readonly Dictionary<string, int> PlayerIndexMap = new Dictionary<string, int>() {
		{ "Player", 0 },
		{ "Player2", 1 },
	};

	/// <summary>
	/// プレイヤーカラー名
	/// </summary>
	static public readonly List<string> PlayerColorNames = new List<string>() {
		"red",
		"green",
	};

	/// <summary>
	/// プレイヤーカラー
	/// </summary>
	static public readonly List<Color> PlayerColors = new List<Color>() {
		Color.red,
		Color.green,
	};

	/// <summary>
	/// すべてのプレイヤースコアを初期化します。
	/// </summary>
	/// <param name="playerCount">プレイヤーの人数</param>
	static public void Init(int playerCount) {
		PlayerScore.IsScoreHidden = false;
		PlayerScore.Scores = new int[playerCount];
		PlayerScore.HPs = new int[playerCount];
		for(int i = 0; i < PlayerScore.HPs.Length; i++) {
			PlayerScore.HPs[i] = PlayerScore.MaxHP;
		}

		// 前回までのハイスコアの記録を読み込む
		PlayerScore.HighScore = PlayerPrefs.GetInt("HighScore-" + SelectModeScene.TimeMinutes + ":" + SelectModeScene.TimeSeconds, 0);
	}

	/// <summary>
	/// 指定したオブジェクトのタグがプレイヤーであるかどうか判定します。
	/// </summary>
	/// <param name="obj">検査対象のゲームオブジェクト</param>
	/// <returns>プレイヤー除雪車であるかどうか</returns>
	static public bool IsPlayerTag(GameObject obj) {
		return PlayerScore.PlayerIndexMap.ContainsKey(obj.tag);
	}

}
