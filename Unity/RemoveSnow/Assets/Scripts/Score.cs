using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	private float playerOneScore;

	private float playerTwoScore;

	static public bool isHideScore;

	void Start() {
		isHideScore = false;
	}

	void Update() {
		playerOneScore = RemoveSnow.ScoreOne;
		playerTwoScore = RemoveSnow.ScoreTwo;

		if(this.gameObject.name == "Text Score Player1") {
			if(isHideScore == false) {
				this.GetComponent<Text>().text = playerOneScore.ToString("0");
			} else {
				this.GetComponent<Text>().text = "????";
			}
		}

		if(this.gameObject.name == "Text Score Player2") {
			if(isHideScore == false) {
				this.GetComponent<Text>().text = playerTwoScore.ToString("0");
			} else {
				this.GetComponent<Text>().text = "????";
			}
		}

		// if (isHideScore == true) {
		// this.GetComponent<Text> ().CrossFadeAlpha(0,0,true);
		// }
	}
}
