using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコアの優勢、劣勢をわかりやすくする。
/// BattleCanvasにアタッチ。
/// </summary>
public class WinnerImage : MonoBehaviour {

	[SerializeField]
	private GameObject playerOneImage;
	[SerializeField]
	private GameObject playerTwoImage;

	// Update is called once per frame
	void Update () {
		if (RemoveSnow.ScoreOne >= RemoveSnow.ScoreTwo) {
			playerOneImage.SetActive (true);
			playerTwoImage.SetActive (false);
		}
		if (RemoveSnow.ScoreOne <= RemoveSnow.ScoreTwo) {
			playerOneImage.SetActive (false);
			playerTwoImage.SetActive (true);
		}
		if (RemoveSnow.ScoreOne == RemoveSnow.ScoreTwo) {
			playerOneImage.SetActive (false);
			playerTwoImage.SetActive (false);
		}

		if (Score.isHideScore == true) {
			playerOneImage.SetActive (false);
			playerTwoImage.SetActive (false);
		}
	}
}
