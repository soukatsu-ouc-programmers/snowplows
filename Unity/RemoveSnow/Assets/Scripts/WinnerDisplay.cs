using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerDisplay : MonoBehaviour {

	void Start () {

		if (RemoveSnow.ScoreOne > RemoveSnow.ScoreTwo) {
			
			this.GetComponent<Text> ().text = "Player1\r\nWin";
			this.GetComponent<Text> ().color = new Color (255, 0, 0, 255);
			GameObject.Find ("GreenShavedIce").SetActive (false);

		} else {
			this.GetComponent<Text> ().text = "Player2\r\nWin";
			this.GetComponent<Text> ().color = new Color (0, 255, 0, 255);
			GameObject.Find ("RedShavedIce").SetActive (false);
		}
		if (RemoveSnow.ScoreOne == RemoveSnow.ScoreTwo) {
			this.GetComponent<Text>().text = "Draw!";
			this.GetComponent<Text> ().color = new Color (0, 0, 255, 255);
		}
	}
}
