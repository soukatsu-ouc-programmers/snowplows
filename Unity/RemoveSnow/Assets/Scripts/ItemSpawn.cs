using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Itemsにアタッチ。
/// </summary>
public class ItemSpawn : MonoBehaviour {
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
		itemX = Random.Range (-139f, -97f);

		itemZ = Random.Range (-32f, -26f);

		itemNumber = Random.Range (0, item.Length);

		itemPosition = new Vector3 (itemX, 10f, itemZ);
		var parent = this.gameObject.transform;
		Instantiate (item [itemNumber], itemPosition, Quaternion.identity, parent);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
