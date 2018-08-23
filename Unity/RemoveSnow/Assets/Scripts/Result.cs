using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコア表示のTextにアタッチ。
/// </summary>
public class Result : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Text> ().text = "Player1   Score   " + RemoveSnow.ScoreOne.ToString ("<color=red>0</color>")
			+ "\r\nPlayer2  Score   " + RemoveSnow.ScoreTwo.ToString("<color=green>0</color>");
	}

}
