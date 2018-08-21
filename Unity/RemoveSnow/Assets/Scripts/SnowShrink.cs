using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowShrink : MonoBehaviour {

	/// <summary>
	/// 縮んだ後のサイズ。
	/// </summary>
	private float shrinkExtend= 0.7f;

	/// <summary>
	/// 雪が小さくなるのが無限ループしないためのフラグ。
	/// </summary>
	private bool isShrink = true;

	/// <summary>
	/// RemoveSnowのIsGetSocoreを参照。
	/// </summary>
	private bool scoreflug;

	void OnCollisionStay(Collision other){

		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2") {
			
			scoreflug = other.gameObject.GetComponent<RemoveSnow> ().isGetScore;

			if (isShrink == true && scoreflug == true) {
				
				this.gameObject.transform.localScale = new Vector3 (1f, shrinkExtend, 1f);

				//衝突したPlayerのScoreを加算。
				switch (other.gameObject.tag) {

				case "Player":
					RemoveSnow.ScoreOne++;
					break;
				case "Player2":
					RemoveSnow.ScoreTwo++;
					break;

				}

				isShrink = false;
				scoreflug = false;
				shrinkExtend -= 0.1f;
			}
		}
	}

	void OnCollisionExit(Collision other){
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2") {

			isShrink = true;
			scoreflug = true;
		}
	}

	void Update(){
		if (shrinkExtend <= 0.1f) {
			Destroy (this.gameObject);
		}
	}

	
}
