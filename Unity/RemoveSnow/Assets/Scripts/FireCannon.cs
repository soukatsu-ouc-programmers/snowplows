using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を発射する。
/// 大砲にアタッチ。
/// </summary>
public class FireCannon : MonoBehaviour {

	[SerializeField]
	private GameObject bullet;

	private GameObject muzzle;

	/// <summary>
	/// 打てる上限。
	/// Bulletも参照。
	/// </summary>
	public int remainBullet = 5;

	/// <summary>
	/// 連続発射制御。
	/// </summary>
	private bool isFireBullet = true;

	// Use this for initialization
	void Start () {
		muzzle = this.transform.Find("Muzzle").gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isFireBullet == true) {
			StartCoroutine ("Fire");
			var parent = muzzle.transform;
			Instantiate (bullet, muzzle.transform.position, muzzle.transform.rotation, parent);
			bullet.transform.localScale =new Vector3 (10f, 10f, 10f);

			if (remainBullet == 0) {
				Destroy (this.gameObject);
			}
			remainBullet--;
			isFireBullet = false;
		}
	}

	IEnumerator Fire(){
		yield return new WaitForSeconds (1f);
		isFireBullet = true;
	}
}
