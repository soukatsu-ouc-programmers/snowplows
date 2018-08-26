using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム：大砲
/// 大砲にアタッチ
/// </summary>
public class FireCannon : MonoBehaviour {

	/// <summary>
	/// 砲弾
	/// </summary>
	[SerializeField]
	private GameObject bullet;

	/// <summary>
	/// 銃口
	/// </summary>
	private GameObject muzzle;

	/// <summary>
	/// 使用可能回数
	/// </summary>
	public int remainBullet = 5;

	/// <summary>
	/// 連続発射制御
	/// </summary>
	private bool isFireBullet = true;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Start() {
		this.muzzle = this.transform.Find("Muzzle").gameObject;
	}

	/// <summary>
	/// 初期化処理
	/// </summary>
	public void FixedUpdate() {
		if(this.isFireBullet == true) {
			// 砲弾の生成
			var parent = this.muzzle.transform;
			Object.Instantiate(this.bullet, this.muzzle.transform.position, this.muzzle.transform.rotation, parent);
			this.bullet.transform.localScale = new Vector3(10f, 10f, 10f);

			if(this.remainBullet == 0) {
				// 残弾が残っていない場合は終了
				Object.Destroy(this.gameObject);
			}
			this.remainBullet--;
			this.isFireBullet = false;

			// 一定時間が経過後に次の発車を行う
			this.StartCoroutine(this.Fire());
		}
	}

	/// <summary>
	/// コルーチン：砲弾が消えたら次の発射を行う
	/// </summary>
	private IEnumerator Fire() {
		yield return new WaitForSeconds(Bullet.AvailableTimeSeconds);
		this.isFireBullet = true;
	}

}
