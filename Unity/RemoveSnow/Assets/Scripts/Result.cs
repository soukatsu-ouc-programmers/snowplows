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
		this.GetComponent<Text> ().text = 
			RemoveSnow.ScoreOne.ToString ("<color=red>0</color>") + "\r\n"
			+ RemoveSnow.ScoreTwo.ToString("<color=green>0</color>");
	}

}
