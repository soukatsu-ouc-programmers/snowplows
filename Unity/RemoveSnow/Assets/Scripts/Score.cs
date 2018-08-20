using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	private float playerOneScore;

	private float playerTwoScore;

	void Update (){

		playerOneScore = RemoveSnow.ScoreOne;

		playerTwoScore = RemoveSnow.ScoreTwo;

		if (this.gameObject.name == "Text Score Player1") {
			this.GetComponent<Text> ().text = playerOneScore.ToString ("0");
		}

		if (this.gameObject.name == "Text Score Player2") {
			this.GetComponent<Text> ().text = playerTwoScore.ToString ("0");
		}
	}
}
