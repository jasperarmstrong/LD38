using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour {
	private ObjectPool op;

	void Start () {
		op = GetComponent<ObjectPool>();	
	}
	
	public void SpawnExplosion(Vector3 pos, float scale) {
		ExplosionController ec = op.InstantiatePooled(
			pos,
			Quaternion.identity,
			false
		).GetComponent<ExplosionController>();
		ec.Initialize(op, scale);
		StartCoroutine(ec.Explode());
	}
}
