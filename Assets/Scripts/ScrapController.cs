using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapController : MonoBehaviour {
	private ObjectPool op;

	void Initialize(ObjectPool _op) {
		op = _op;
	}

	void Reset() {
		
	}

	public void Deactivate() {
		op.DeactivateObject(gameObject);
	}
}
