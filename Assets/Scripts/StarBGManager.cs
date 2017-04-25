using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBGManager : MonoBehaviour {
	private Vector2 curPos = Vector3.zero;
	[SerializeField] private float size = 100;
	private float halfSize, quarterSize;

	private ObjectPool op;
	private List<Vector2> positions;

	void Start () {
		op = GetComponent<ObjectPool>();
		positions = new List<Vector2>();
		positions.Add(curPos);

		halfSize = size / 2;
		quarterSize = halfSize / 2;
	}

	public void Reset() {
		curPos = Vector3.zero;
		if (positions != null) {
			positions.Clear();
			positions.Add(curPos);
		}
	}
	
	void Update () {
		if (GameManager.pc == null) {
			return;
		}

		curPos = new Vector2(
			(int)GameManager.pc.transform.position.x % size,
			(int)GameManager.pc.transform.position.y % size
		);

		Debug.Log(curPos);
		Debug.Log(transform.position);

		foreach(Vector2 vec in new Vector2[9] {
			curPos,
			new Vector2(curPos.x + 1, curPos.y),
			new Vector2(curPos.x + 1, curPos.y + 1),
			new Vector2(curPos.x + 1, curPos.y - 1),
			new Vector2(curPos.x - 1, curPos.y),
			new Vector2(curPos.x - 1, curPos.y + 1),
			new Vector2(curPos.x - 1, curPos.y - 1),
			new Vector2(curPos.x, curPos.y + 1),
			new Vector2(curPos.x, curPos.y - 1),
		}) {
			if (!positions.Contains(vec)) {
				positions.Add(vec);
				GameObject obj = op.InstantiatePooled(
					(Vector3)vec * size,
					Quaternion.identity,
					false
				);
				obj.GetComponent<StarBGController>().Initialize(op, positions);
			}
		}
	}
}
