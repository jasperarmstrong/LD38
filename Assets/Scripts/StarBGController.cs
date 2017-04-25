using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBGController : MonoBehaviour {
	private float size;
	private float doubleSize;
	
	private ObjectPool op;
	private List<Vector2> positions;

	public void Initialize (ObjectPool _op, List<Vector2> _positions) {
		op = _op;
		positions = _positions;

		size = GetComponent<SpriteRenderer>().size.x;
		doubleSize = size * 2;
	}
	
	void Reset () {
		
	}

	void Update() {
		if (GameManager.pc == null || op == null) {
			return;
		}

		if (
			GameManager.pc.transform.position.x > transform.position.x + doubleSize ||
			GameManager.pc.transform.position.x < transform.position.x - doubleSize ||
			GameManager.pc.transform.position.y > transform.position.y + doubleSize ||
			GameManager.pc.transform.position.y < transform.position.y - doubleSize
		) {
			positions.Remove(transform.position / size);
			op.DeactivateObject(gameObject);
		}
	}
}
