using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト画面の勝敗表示
/// </summary>
public class WinnerDisplay : MonoBehaviour {

	void Start() {
		if(RemoveSnow.ScoreOne > RemoveSnow.ScoreTwo) {

			this.GetComponent<Text>().text = "Player 1\r\nWin !";
			this.GetComponent<Text>().color = new Color(255, 0, 0, 255);
			GameObject.Find("GreenShavedIce").SetActive(false);

		} else if(RemoveSnow.ScoreOne == RemoveSnow.ScoreTwo) {

			this.GetComponent<Text>().text = "Draw !";
			this.GetComponent<Text>().color = new Color(0, 0, 255, 255);

		} else {

			this.GetComponent<Text>().text = "Player 2\r\nWin !";
			this.GetComponent<Text>().color = new Color(0, 255, 0, 255);
			GameObject.Find("RedShavedIce").SetActive(false);

		}
	}
}
