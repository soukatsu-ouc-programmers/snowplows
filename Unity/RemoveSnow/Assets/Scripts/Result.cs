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
			this.GetComponent<Text> ().text = "Player1 Score " + RemoveSnow.ScoreOne.ToString ("0") + "\r\nPlayer2 Score " + RemoveSnow.ScoreTwo.ToString("0");
	}

}
