using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HokkaidoItemSpawn : MonoBehaviour {
	/// <summary>
	/// 出現させるアイテム。
	/// </summary>
	[SerializeField]
	private GameObject[] item;

	[SerializeField]
	private int itemNumber;

	private float itemX;
	private float itemZ;

	Vector3 itemPosition;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("ItemGenerate", 10, 10);
	}

	void ItemGenerate(){
		Physics.queriesHitTriggers = false;
		while (true) {
			itemX = Random.Range (-1f, 39f);

			itemZ = Random.Range (-22f, 16f);

			itemNumber = Random.Range (0, item.Length);

			itemPosition = new Vector3 (itemX, 1f, itemZ);
			var parent = this.gameObject.transform;

			if (Physics.Raycast (itemPosition, -Vector3.up) == true) {
				Instantiate (item [itemNumber], itemPosition, Quaternion.identity, parent);
				break;
			}
		}
	}
}
